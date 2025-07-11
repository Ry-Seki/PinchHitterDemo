using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugDamage : MonoBehaviour {
    [SerializeField]
    private Slider HPSlider;
    private const float MIN_DAMAGE_NORM = 0.01f;
    private const float MAX_DAMAGE_NORM = 0.05f;
    private const float MIN_DAMAGE_PERCENTAGE = 90.0f;
    private const float MAX_DAMAGE_PERCENTAGE = 100.0f;
    private const int ADD_SCORE = 200;
    private CameraController mainCamera;
    // Start is called before the first frame update
    void Start() {
        mainCamera = Camera.main.GetComponent<CameraController>();
    }

    // Update is called once per frame
    void Update() {

    }
    private void OnWillRenderObject() {
#if UNITY_EDITOR
        if (Camera.current.name != "SceneCamera" && Camera.current.name != "Preview Camera")

#endif
            if (PartMainGame.isStart && mainCamera.IsHitter()) {
                float damage = (float)DamageNormScaling(CameraController.pinchPercentage);
                HPSlider.value -= damage;
                if (HPSlider.value <= 0) {
                    HPSlider.value = 0;
                    MenuManager.instance.Get<ScoreTextManager>().AddScore(ADD_SCORE);
                    Destroy(gameObject);
                }
            }
    }

    private float DamageNormScaling(float setValue) {
        return MIN_DAMAGE_NORM + ((setValue - MIN_DAMAGE_PERCENTAGE) / (MAX_DAMAGE_PERCENTAGE - MIN_DAMAGE_PERCENTAGE)) * (MAX_DAMAGE_NORM - MIN_DAMAGE_NORM);
    }
}
