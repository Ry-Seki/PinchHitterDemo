using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugDamage : MonoBehaviour {
    [SerializeField]
    private Slider HPSlider;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
    private void OnWillRenderObject() {
#if UNITY_EDITOR

        if (Camera.current.name != "SceneCamera" && Camera.current.name != "Preview Camera")

#endif
            if (PartMainGame.isStart && Camera.main.GetComponent<CameraController>().IsHitter()) {
                HPSlider.value -= 0.001f;
                if (HPSlider.value <= 0) {
                    HPSlider.value = 0;
                    Destroy(gameObject);
                }
            }
    }
}
