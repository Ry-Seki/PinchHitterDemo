using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

using static GameConst;
using static CommonModule;
using static PlayerStatusUtility;

public class CameraController : MonoBehaviour {
    public static float pinchPercentage { get; private set; } = -1;
    // 攻撃フラグ
    public bool isEnableAttack { get; private set; } = false;

    // 拡縮率を表すテキスト
    private PinchExpansionText _pinchText = null;
    //カメラ
    private Camera _mainCamera = null;
    //InputAction
    private PinchHitterDemo _cameraInput = null;
    // ２本指のタッチ情報
    private TouchState _touchState0;
    private TouchState _touchState1;

    //操作感度
    private static float _moveSensitivity = -1;
    private float _mousePinchExpansion = -1;

    //初期ピンチ倍率
    private const float _TOUCH_PINCH_SENSITIVITY = 0.01f;
    // 最小感度
    private const float MIN_SENSITIVITY = 0.1f;
    // 最大感度
    private const float MAX_SENSITIVITY = 1.0f;

    /// <summary>
    /// 準備前処理
    /// </summary>
    /// <returns></returns>
    public async UniTask Setup() {
        //カメラのキャスト
        _mainCamera = Camera.main;
        _mainCamera.orthographicSize = MAX_EXPANSION;
        pinchPercentage = MAX_PERCENTAGE;
        //InputActionを取得
        _cameraInput = InputSystemManager.instance.input;
        //CameraInputActionの登録
        _cameraInput.Camera.MouseWheel.performed += OnMouseWheel;
        _cameraInput.Camera.MouseWheel.canceled += EndMouseWheel;
        _cameraInput.Camera.Touch_0.performed += OnTouch0;
        _cameraInput.Camera.Touch_1.performed += OnTouch1;
        if(!_pinchText) return;
        _pinchText.gameObject.SetActive(true);
        await UniTask.CompletedTask;
    }
    /// <summary>
    /// ゲーム開始時の演出
    /// </summary>
    /// <param name="duration"></param>
    /// <returns></returns>
    public async UniTask StartGameCamera(float duration) {
        _mousePinchExpansion = 5.0f;
        pinchPercentage = MAX_PERCENTAGE;
        float elapsedTime = 0.0f;
        //カメラの演出
        while (elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            float animationTime = Mathf.Lerp(MAX_EXPANSION, MIN_EXPANSION, t);
            _mainCamera.orthographicSize = animationTime;
            pinchPercentage = Mathf.Lerp(MAX_PERCENTAGE, 0, t);
            _pinchText.VisiblePinchExpansion(pinchPercentage);
            await UniTask.DelayFrame(1);
        }
        await _pinchText.PinchTextFade();
        //攻撃フラグOn
        isEnableAttack = true;
        //InputActionの有効化
        _cameraInput.Enable();
    }
    /// <summary>
    /// パソコン用更新処理
    /// </summary>
    private void Update() {
        if(!PartMainGame.isStart || !Input.GetMouseButton(0)) return;

        //マウスドラッグでのカメラ移動
        float cameraX = Input.GetAxis("Mouse X") * ReverseNormScaling(pinchPercentage, 100, 0);
        float cameraY = Input.GetAxis("Mouse Y") * ReverseNormScaling(pinchPercentage, 100, 0);
        _mainCamera.transform.position -= new Vector3(cameraX, cameraY, 0.0f);
    }
    /// <summary>
    /// タッチ操作1
    /// </summary>
    /// <param name="context"></param>
    public void OnTouch0(InputAction.CallbackContext context) {
        _touchState0 = context.ReadValue<TouchState>();

        //もし、マルチタップでなければスワイプ移動
        if (!_touchState1.isInProgress) {
            SwipeCameraMove(_touchState0);
            return;
        }
        //ピンチイン、アウトの実行
        OnPinch();
    }
    /// <summary>
    /// タッチ操作2
    /// </summary>
    /// <param name="context"></param>
    public void OnTouch1(InputAction.CallbackContext context) {
        _touchState1 = context.ReadValue<TouchState>();

        //もし、マルチタップでなければスワイプ移動
        if (!_touchState0.isInProgress) {
            SwipeCameraMove(_touchState1);
            return;
        }
        //ピンチイン、アウトの実行
        OnPinch();
    }
    /// <summary>
    /// カメラ視点移動
    /// </summary>
    public void SwipeCameraMove(TouchState setTouch) {
        UniTask task = _pinchText.PinchTextFade();
        //移動量
        Vector3 delta = setTouch.delta * ReverseNormScaling(pinchPercentage, MAX_PERCENTAGE, 0.0f) * _moveSensitivity;
        _mainCamera.transform.position -= new Vector3(delta.x, delta.y, 0.0f);
        Vector3 goalPos = transform.position - delta;
        goalPos.z = -10.0f;
    }
    /// <summary>
    /// ピンチ判定処理
    /// </summary>
    private void OnPinch() {
        // タッチ位置（スクリーン座標）
        Vector2 currentPos0 = _touchState0.position;
        Vector2 currentPos1 = _touchState1.position;

        // 移動量（スクリーン座標）
        Vector2 delta0 = _touchState0.delta;
        Vector2 delta1 = _touchState1.delta;

        // 移動前の位置（スクリーン座標）
        Vector2 prevPos0 = currentPos0 - delta0;
        Vector2 prevPos1 = currentPos1 - delta1;

        // 距離の変化量を求める
        float pinchDelta = Vector3.Distance(currentPos0, currentPos1) - Vector3.Distance(prevPos0, prevPos1);

        //カメラの拡縮に反映
        SetCameraGraphicSize(-pinchDelta * _TOUCH_PINCH_SENSITIVITY);

        //一定の操作量の時フェード実行
        if( pinchDelta < 0.1f) return;

        UniTask task = _pinchText.PinchTextFade();
    }
    /// <summary>
    /// ホイール操作
    /// </summary>
    /// <param name="context"></param>
    public void OnMouseWheel(InputAction.CallbackContext context) {
        //ホイールを取得して、代入
        float scroll = Input.mouseScrollDelta.y * _mousePinchExpansion;
        //カメラの拡縮に反映
        SetCameraGraphicSize(scroll);
    }
    /// <summary>
    /// ホイール操作終了
    /// </summary>
    /// <param name="context"></param>
    public void EndMouseWheel(InputAction.CallbackContext context) {
        UniTask task = _pinchText.PinchTextFade();
    }
    /// <summary>
    /// カメラの拡縮率の変更
    /// </summary>
    /// <param name="setSize"></param>
    private void SetCameraGraphicSize(float setSize) {
        //カメラの拡縮を取得
        float cameraScale = _mainCamera.orthographicSize;
        float scroll = setSize;
        //最大拡大率の判定
        if (cameraScale + scroll <= MAX_EXPANSION) {
            scroll = MAX_EXPANSION;
            pinchPercentage = 100;
            cameraScale = scroll;
            //最小拡大率の判定
        } else if (cameraScale + scroll >= MIN_EXPANSION) {
            scroll = MIN_EXPANSION;
            pinchPercentage = 0;
            cameraScale = scroll;
            //残りは値を足す
        } else {
            cameraScale += scroll;
            pinchPercentage = ReversePercentageScaling(cameraScale, MIN_EXPANSION, MAX_EXPANSION);
        }
        //カメラに拡縮を反映
        _mainCamera.orthographicSize = cameraScale;
        //テキストスクリプトに値を渡す
        _pinchText.VisiblePinchExpansion(pinchPercentage);
    }
    /// <summary>
    /// 特定の拡縮率かの判定
    /// </summary>
    /// <returns></returns>
    public bool IsHitter() {
        return pinchPercentage >= GetRawPercentage();
    }
    /// <summary>
    /// 拡縮率テキストの設定(パートメインゲームの初期化時にセット)
    /// </summary>
    /// <param name="setPinchText"></param>
    public void SetPinchText(PinchExpansionText setPinchText) {
        _pinchText = setPinchText;
    }
    /// <summary>
    /// 片付け処理
    /// </summary>
    public void Teardown() {
        // 入力イベントの破棄処理
        _cameraInput.Camera.MouseWheel.performed -= OnMouseWheel;
        _cameraInput.Camera.MouseWheel.canceled -= EndMouseWheel;
        _cameraInput.Camera.Touch_0.performed -= OnTouch0;
        _cameraInput.Camera.Touch_1.performed -= OnTouch1;
        _cameraInput.Disable();
        // 位置の初期化
        _mainCamera.transform.position = new Vector3(0, 0, -10);
        // カメラの高さのリセット
        _mainCamera.orthographicSize = MAX_EXPANSION;
        // 拡縮率のリセット
        pinchPercentage = MAX_PERCENTAGE;
        // 拡縮率テキストのリセット
        _pinchText.VisiblePinchExpansion(pinchPercentage);
    }
    /// <summary>
    /// 感度の補正関数
    /// </summary>
    /// <param name="setValue"></param>
    public static void SetMoveSensitivity(float setValue) {
        _moveSensitivity = setValue / TEN_DEVIDE_VALUE;
        if(_moveSensitivity < 0) {
            _moveSensitivity = MIN_SENSITIVITY;
        }else if(_moveSensitivity > 1) {
            _moveSensitivity = MAX_SENSITIVITY;
        }
    }
}
