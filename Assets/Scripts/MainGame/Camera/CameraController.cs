using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class CameraController : MonoBehaviour {
    private Camera mainCamera;

    // Start is called before the first frame update
    void Start() {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update() {
        
    }
    //public void OnMove(InputAction.CallbackContext context) {
        
    //}

    public void OnMouseWheel() {
        //ホイールを取得して、均しのためにtime.deltaTimeをかけておく
        float scroll = Input.mouseScrollDelta.y * Time.deltaTime * 500;
        //Debug.Log(scroll);

        //mainCam.orthographicSizeは0だとエラー出るっぽいので回避策
        if (mainCamera.orthographicSize > 0) {
            mainCamera.orthographicSize += scroll;
        } else {
            mainCamera.orthographicSize -= scroll;
        }
    }
}
