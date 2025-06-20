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
    [SerializeField]
    private PinchExpansionText pinchText = null;
    //カメラ
    private Camera mainCamera = null;
    //InputAction
    private PinchHitterDemo cameraInput = null;
    private bool isMove = false;
    Vector2 swipeDelta = Vector2.zero;
    private int pinchExpansion = -1;
    private float pinchPercentage = -1;

    // ２本指のタッチ情報
    private TouchState _touchState0;
    private TouchState _touchState1;

    public async UniTask Initialize() {
        mainCamera = Camera.main;
        //InputActionを取得
        cameraInput = InputSystemManager.instance.input;
        //InputActionの登録
        cameraInput.Camera.MouseWheel.performed += OnMouseWheel;
        cameraInput.Camera.MouseWheel.canceled += EndMouseWheel;
        cameraInput.Camera.Touch_0.performed += OnTouch0;
        cameraInput.Camera.Touch_1.performed += OnTouch1;

        cameraInput.Enable();
        await UniTask.CompletedTask;
    }
    public async UniTask Setup(float duration) {
        pinchExpansion = 5;
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
    }
    private void Update() {
        if(!PartMainGame.isStart || !Input.GetMouseButton(0)) return;

        //マウスドラッグでのカメラ移動
        float cameraX = Input.GetAxis("Mouse X") * ReverseNormScaling(pinchPercentage, 100, 0);
        float cameraY = Input.GetAxis("Mouse Y") * ReverseNormScaling(pinchPercentage, 100, 0);
        transform.position -= new Vector3(cameraX, cameraY, 0.0f);
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
        //タッチ位置
        Vector2 currentPos = setTouch.position;
        //移動量
        Vector2 delta = setTouch.delta;
        //移動前の位置
        Vector2 prevPos = currentPos - delta;

        float distance = Vector3.Distance(currentPos, prevPos);
        transform.position -= new Vector3(currentPos.x + distance, currentPos.y + distance, 0.0f); 
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
        float pinchDelta = Vector3.Distance(currentPos0, currentPos1) - Vector3.Distance(prevPos0, prevPos1) * pinchExpansion;

        //カメラの拡縮に反映
        SetCameraGraphicSize(pinchDelta);
        // 距離の変化量をログ出力
        Debug.Log($"ピンチ操作量 : {pinchDelta}");
    }
    /// <summary>
    /// ホイール操作
    /// </summary>
    /// <param name="context"></param>
    public void OnMouseWheel(InputAction.CallbackContext context) {
        //ホイールを取得して、代入
        float scroll = Input.mouseScrollDelta.y * pinchExpansion;
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
            cameraScale += scroll;
            pinchPercentage = ReversePercentageScaling(cameraScale, MIN_EXPANSION, MAX_EXPANSION);
        }
        //カメラに拡縮を反映
        mainCamera.orthographicSize = cameraScale;
        //テキストスクリプトに値を渡す
        pinchText.VisiblePinchExpansion(pinchPercentage);
    }
    /// <summary>
    /// 特定の拡縮率かの判定
    /// </summary>
    /// <returns></returns>
    public bool IsHitter() {
        return pinchPercentage >= 90.0f;
    }
}
