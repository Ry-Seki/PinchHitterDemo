using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugButton : MonoBehaviour
{
    PinchHitterDemo inputActions;
    private void Start() {
        inputActions = InputSystemManager.instance.input;
        inputActions.UI.Press.started += DebugTap;
        inputActions.Enable();
    }
    public void DebugTap(InputAction.CallbackContext context) {
        Debug.Log("‰Ÿ‚³‚ê‚½");
    }
}
