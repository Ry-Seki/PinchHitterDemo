using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

using static GameConst;
using static CommonModule;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour {
    //拡縮率を表すテキスト
    private PinchExpansionText pinchText = null;
    //カメラ
    private Camera mainCamera = null;
    //InputAction
    private PinchHitterDemo cameraInput = null;
    private float mousePinchExpansion = -1;
    private float touchPinchExpansion = -1;
    public static float pinchPercentage { get; private set; } = -1;
    //攻撃力
    private int rawAttack = -1;
    //初期攻撃力
    private const int INIT_RAW_ATTACK = 10;
    //想定される最大の攻撃力
    private const int MAX_RAW_ATTACK = 1000000;
    //素の攻撃間隔
    private float attackIntervalSec = -1;
    //初期攻撃間隔
    private const float INIT_ATTACK_INTERVAL_SEC = 0.5f;
    //攻撃フラグ
    public bool isEnableAttack { get; private set; } = false;
    // ２本指のタッチ情報
    private TouchState _touchState0;
    private TouchState _touchState1;

    public async UniTask Initialize() {
        //カメラのキャスト
        mainCamera = Camera.main;
        //初期攻撃力設定
        SetRawAttack(INIT_RAW_ATTACK);
        //初期攻撃間隔設定
        SetRawAttackInterval(INIT_ATTACK_INTERVAL_SEC);
        //InputActionを取得
        cameraInput = InputSystemManager.instance.input;
        //CameraInputActionの登録
        cameraInput.Camera.MouseWheel.performed += OnMouseWheel;
        cameraInput.Camera.MouseWheel.canceled += EndMouseWheel;
        cameraInput.Camera.Touch_0.performed += OnTouch0;
        cameraInput.Camera.Touch_1.performed += OnTouch1;

        pinchText.gameObject.SetActive(true);
        await UniTask.CompletedTask;
    }
    public async UniTask Setup(float duration) {
        mousePinchExpansion = 5.0f;
        touchPinchExpansion = 0.2f;
        pinchPercentage = MAX_PERCENTAGE;
        float elapsedTime = 0.0f;
        //カメラの演出
        while (elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            float animationTime = Mathf.Lerp(MAX_EXPANSION, MIN_EXPANSION, t);
            mainCamera.orthographicSize = animationTime;
            pinchPercentage = Mathf.Lerp(MAX_PERCENTAGE, 0, t);
            pinchText.VisiblePinchExpansion(pinchPercentage);
            await UniTask.DelayFrame(1);
        }
        await pinchText.PinchTextFade();
        //InputActionの有効化
        cameraInput.Enable();
        //攻撃フラグOn
        isEnableAttack = true;
    }
    private void Update() {
        if(!PartMainGame.isStart || !Input.GetMouseButton(0)) return;

        //マウスドラッグでのカメラ移動
        float cameraX = Input.GetAxis("Mouse X") * ReverseNormScaling(pinchPercentage, 100, 0);
        float cameraY = Input.GetAxis("Mouse Y") * ReverseNormScaling(pinchPercentage, 100, 0);
        mainCamera.transform.position -= new Vector3(cameraX, cameraY, 0.0f);
    }
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
        UniTask task = pinchText.PinchTextFade();
        //移動量
        Vector2 delta = setTouch.delta * ReverseNormScaling(pinchPercentage, MAX_PERCENTAGE, 0.0f);

        mainCamera.transform.position -= new Vector3(delta.x, delta.y, 0.0f); 
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

        // 距離の変化量をログ出力
        Debug.Log($"ピンチ操作量 : {pinchDelta}");

        //カメラの拡縮に反映
        SetCameraGraphicSize(-pinchDelta);

        //一定の操作量の時フェード実行
        if( pinchDelta < 0.1f) return;

        UniTask task = pinchText.PinchTextFade();
    }
    /// <summary>
    /// ホイール操作
    /// </summary>
    /// <param name="context"></param>
    public void OnMouseWheel(InputAction.CallbackContext context) {
        //ホイールを取得して、代入
        float scroll = Input.mouseScrollDelta.y * mousePinchExpansion;
        //カメラの拡縮に反映
        SetCameraGraphicSize(scroll);
    }
    public void EndMouseWheel(InputAction.CallbackContext context) {
        UniTask task = pinchText.PinchTextFade();
    }
    /// <summary>
    /// カメラの拡縮率の変更
    /// </summary>
    /// <param name="setSize"></param>
    private void SetCameraGraphicSize(float setSize) {
        //カメラの拡縮を取得
        float cameraScale = mainCamera.orthographicSize;
        float scroll = setSize;
        //最大拡大率の判定
        if (cameraScale + scroll < MAX_EXPANSION) {
            scroll = MAX_EXPANSION;
            pinchPercentage = 100;
            cameraScale = scroll;
            //最小拡大率の判定
        } else if (cameraScale + scroll > MIN_EXPANSION) {
            scroll = MIN_EXPANSION;
            pinchPercentage = 0;
            cameraScale = scroll;
            //残りは値を足す
        } else {
            cameraScale += scroll * touchPinchExpansion;
            pinchPercentage = ReversePercentageScaling(cameraScale, MIN_EXPANSION, MAX_EXPANSION);
        }
        //カメラに拡縮を反映
        mainCamera.orthographicSize = cameraScale;
        //テキストスクリプトに値を渡す
        pinchText.VisiblePinchExpansion(pinchPercentage);
    }
    /// <summary>
    /// 素の攻撃力の取得
    /// </summary>
    /// <returns></returns>
    public int GetRawAttack() {
        return rawAttack;
    }
    /// <summary>
    /// 素の攻撃間隔の取得
    /// </summary>
    /// <returns></returns>
    public float GetRawAttackInterval() {
        return attackIntervalSec;
    }
    /// <summary>
    /// 素の攻撃間隔の設定
    /// </summary>
    /// <param name="setValue"></param>
    public void SetRawAttackInterval(float setValue) {
        attackIntervalSec = Mathf.Clamp(setValue, 0.1f, 2.0f);
    }
    /// <summary>
    /// 素の攻撃間隔の短縮
    /// </summary>
    /// <param name="setValue"></param>
    public void FasterRawAttackInterval(float setValue) {
        SetRawAttackInterval(attackIntervalSec + setValue);
    }
    /// <summary>
    /// 素の攻撃力の設定
    /// </summary>
    /// <param name="setValue"></param>
    /// <returns></returns>
    public void SetRawAttack(int setValue) {
        rawAttack = Mathf.Clamp(setValue, 0, MAX_RAW_ATTACK);
    }
    /// <summary>
    /// 素の攻撃力増加
    /// </summary>
    /// <param name="setValue"></param>
    public void AddRawAttack(int setValue) {
        SetRawAttack(rawAttack + setValue);
    }
    /// <summary>
    /// 特定の拡縮率かの判定
    /// </summary>
    /// <returns></returns>
    public bool IsHitter() {
        return pinchPercentage >= 90.0f;
    }
    /// <summary>
    /// 拡縮率テキストの設定
    /// </summary>
    /// <param name="setPinchText"></param>
    public void SetPinchText(PinchExpansionText setPinchText) {
        pinchText = setPinchText;
    }
    public void Teardown() {
        cameraInput.Disable();
        mainCamera.transform.position = new Vector3(0, 0, -10);
        mainCamera.orthographicSize = MAX_EXPANSION;
        pinchPercentage = MAX_PERCENTAGE;
        pinchText.VisiblePinchExpansion(pinchPercentage);
    }
}
