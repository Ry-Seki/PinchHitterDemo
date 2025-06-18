using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

using static GameConst;

public class CameraController : MonoBehaviour {
    [SerializeField]
    private PinchExpansionText pinchText = null;
    //カメラ
    private Camera mainCamera = null;
    //InputAction
    private PinchHitterDemo cameraInput = null;
    private bool isMove = false;
    Vector2 startMousePos = Vector2.zero;
    Vector2 goalMousePos = Vector2.zero;
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
        cameraInput.Camera.MouseWheel.started += OnMouseWheel;
        cameraInput.Camera.Move.started += OnStartPosition;
        cameraInput.Camera.Move.performed += OnCameraMove;
        cameraInput.Camera.Move.canceled += EndCameraMove;

        cameraInput.Enable();
        await UniTask.CompletedTask;
    }
    public async UniTask Setup(float duration) {
        pinchExpansion = 5;
        pinchPercentage = 100;
        float elapsedTime = 0.0f;
        while (elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            float animationTime = Mathf.Lerp(MAX_EXPANSION, MIN_EXPANSION, t);
            pinchPercentage = Mathf.Lerp(100, 0, t);
            mainCamera.orthographicSize = animationTime;
            pinchText.VisiblePinchExpansion(pinchPercentage);
            await UniTask.DelayFrame(1);
        }
        await UniTask.CompletedTask;
    }
    private void Update() {
        if(!PartMainGame.isStart || !isMove) return;

        float cameraX = Input.GetAxis("Mouse X") * 0.5f;
        float cameraY = Input.GetAxis("Mouse Y") * 0.5f;
        transform.position -= new Vector3(cameraX, cameraY, 0.0f);
    }
    /// <summary>
    /// マウスのスタート座標を取得
    /// </summary>
    /// <param name="context"></param>
    public void OnStartPosition(InputAction.CallbackContext context) {
        startMousePos = Input.mousePosition;
    }
    /// <summary>
    /// マウスの移動
    /// </summary>
    /// <param name="context"></param>
    public void OnCameraMove(InputAction.CallbackContext context) {
        isMove = true;
    }

    public void EndCameraMove(InputAction.CallbackContext context) {
        isMove = false;
    }
    public void OnMouseWheel(InputAction.CallbackContext context) {
        //ホイールを取得して、均しのためにtime.deltaTimeをかけておく
        float scroll = Input.mouseScrollDelta.y * pinchExpansion;
        //Debug.Log(scroll);
        //最大拡大率を設定
        if (mainCamera.orthographicSize + scroll < MAX_EXPANSION) {
            scroll = MAX_EXPANSION;
            pinchPercentage = 100;
            mainCamera.orthographicSize = scroll;
        } else if (mainCamera.orthographicSize + scroll > MIN_EXPANSION) {
            scroll = MIN_EXPANSION;
            pinchPercentage = 0;
            mainCamera.orthographicSize = scroll;
        } else {
            pinchPercentage -= scroll;
            mainCamera.orthographicSize += scroll;
        }
        pinchText.VisiblePinchExpansion(pinchPercentage);
    }

    // Touch #0 入力
    public void OnTouch0(InputAction.CallbackContext context) {
        _touchState0 = context.ReadValue<TouchState>();

        OnPinch();
    }

    // Touch #1 入力
    public void OnTouch1(InputAction.CallbackContext context) {
        _touchState1 = context.ReadValue<TouchState>();

        OnPinch();
    }

    // ピンチ判定処理
    private void OnPinch() {
        // ２本指が移動していなかれば操作なしと判断
        if (!_touchState0.isInProgress || !_touchState1.isInProgress)
            return;

        // タッチ位置（スクリーン座標）
        Vector2 pos0 = _touchState0.position;
        Vector2 pos1 = _touchState1.position;

        // 移動量（スクリーン座標）
        Vector2 delta0 = _touchState0.delta;
        Vector2 delta1 = _touchState1.delta;

        // 移動前の位置（スクリーン座標）
        Vector2 prevPos0 = pos0 - delta0;
        Vector2 prevPos1 = pos1 - delta1;

        // 距離の変化量を求める
        float pinchDelta = Vector3.Distance(pos0, pos1) - Vector3.Distance(prevPos0, prevPos1);

        // 距離の変化量をログ出力
        Debug.Log($"ピンチ操作量 : {pinchDelta}");
    }

    public bool IsHitter() {
        return mainCamera.orthographicSize <= 5;
    }
}
