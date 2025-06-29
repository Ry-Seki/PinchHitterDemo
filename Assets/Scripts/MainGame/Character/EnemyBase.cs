using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class EnemyBase : MonoBehaviour {
    [SerializeField]
    protected Slider enemyHPSlider = null;
    public int maxHP { get; protected set; } = -1;
    public int HP { get; protected set; } = -1;
    protected bool isCameraHit = false;
    protected bool damageCoolTime = false;
    protected const float MIN_DAMAGE_NORM = 1.0f;
    protected const float MAX_DAMAGE_NORM = 2.0f;
    protected const float MIN_DAMAGE_PERCENTAGE = 90.0f;
    protected const float MAX_DAMAGE_PERCENTAGE = 100.0f;
    protected readonly int ADD_SCORE = 200;
    protected static CameraController mainCamera { get; private set; } = null;

    public static void Initialize() {
        if(mainCamera != null) return;

        //メインカメラの登録
        mainCamera = Camera.main.GetComponent<CameraController>();
    }
    public virtual void Setup() {
        gameObject.SetActive(true);
        //HPゲージの初期化
        enemyHPSlider.value = 1.0f;
    }
    public virtual void Teardown() {
        gameObject.SetActive(false);
        isCameraHit = false;
        damageCoolTime = false;
    }
    protected virtual void OnWillRenderObject() {
#if UNITY_EDITOR

        if (Camera.current.name == "SceneCamera" || Camera.current.name == "Preview Camera") return;

#endif
            if (PartMainGame.isStart && mainCamera.IsHitter() && !damageCoolTime) {
                isCameraHit = true;
            } else {
                isCameraHit = false;
            }
    }
    /// <summary>
    /// 死亡判定付きのダメージ付与
    /// </summary>
    protected abstract UniTask Damage();
    /// <summary>
    /// ダメージ正規補正
    /// </summary>
    /// <param name="setValue"></param>
    /// <returns></returns>
    protected float DamageNormScaling(float setValue) {
        return MIN_DAMAGE_NORM + 
            ((setValue - MIN_DAMAGE_PERCENTAGE) / (MAX_DAMAGE_PERCENTAGE - MIN_DAMAGE_PERCENTAGE)) * 
            (MAX_DAMAGE_NORM - MIN_DAMAGE_NORM);
    }
    /// <summary>
    /// 攻撃クールタイム
    /// </summary>
    /// <returns></returns>
    public async UniTask DamageCoolTime() {
        damageCoolTime = true;
        float elapsedTime = 0.0f;
        float duration = mainCamera.GetRawAttackInterval();

        while (elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            await UniTask.DelayFrame(1);
        }
        damageCoolTime = false;
    }
}
