using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

using static GameConst;

public class CameraController : MonoBehaviour {
    private Camera mainCamera;
    const float MOVE_MAX = 100.0f;
    Vector2 previousPos, currentPos;
    // ２本指のタッチ情報
    private TouchState _touchState0;
    private TouchState _touchState1;

    private PinchHitterDemo cameraInput;

    // Start is called before the first frame update
    void Start() {
    }

    public void Initialize() {
        mainCamera = Camera.main;

        cameraInput = InputSystemManager.instance.input;

        cameraInput.Camera.MouseWheel.started += OnMouseWheel;
        cameraInput.Camera.Move.started += OnStartMove;
        cameraInput.Camera.Move.performed += OnCameraMove;
        cameraInput.Enable();
    }

    // Update is called once per frame
    void Update() {
        
    }
    public void OnStartMove(InputAction.CallbackContext context) {
        previousPos = Input.mousePosition;
    }
    public void OnCameraMove(InputAction.CallbackContext context) {
        // スワイプによる移動距離を取得
        currentPos = Input.mousePosition;

        // 次のローカルx座標を設定 ※道の外にでないように
        float newX = Input.GetAxis("Mouse X") * 0.1f;
        float newY = Input.GetAxis("Mouse Y") * 0.1f;
        transform.position -= new Vector3(newX, newY, 0.0f);
    }

    public void OnMouseWheel(InputAction.CallbackContext context) {
        //ホイールを取得して、均しのためにtime.deltaTimeをかけておく
        float scroll = Input.mouseScrollDelta.y * 10;
        //Debug.Log(scroll);
        //最大拡大率を設定
        if (mainCamera.orthographicSize + scroll < MAX_EXPANSION) {
            scroll = MAX_EXPANSION;
            mainCamera.orthographicSize = scroll;
        } else if(mainCamera.orthographicSize + scroll > MIN_EXPANSION){ 
            scroll = MIN_EXPANSION;
            mainCamera.orthographicSize = scroll;
        } else {
            mainCamera.orthographicSize += scroll;
        }
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
        return mainCamera.orthographicSize <= 13;
    }
}
