using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

using static CommonModule;

public abstract class EnemyBase : MonoBehaviour {
    protected static StringBuilder spriteNameBuilder = new StringBuilder();

    [SerializeField]
    protected SpriteRenderer enemySprite = null;
    [SerializeField]
    protected Sprite[][] animSpriteList = null;
    protected int animIndex = -1;
    protected eEnemyAnimation currentAnim = eEnemyAnimation.Invalid;
    protected UniTask animTask;
    private static readonly int _ANIMATION_DELAY_MILLI_SEC = 150;

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
    /// <summary>
    /// アニメーション用の画像切り替えタスク
    /// </summary>
    /// <returns></returns>
    protected async UniTask PlayAnimationTask() {
        while (true) {
            //現在のアニメーション取得
            int currentAnimIndex = (int)currentAnim;
            if (!IsEnableIndex(animSpriteList, currentAnimIndex)) {
                //無効なアニメーションなら終了
                await UniTask.DelayFrame(1);
                return;
            }
            Sprite[] currentAnimSpriteList = animSpriteList[currentAnimIndex];
            //ループ判定、処理
            if (!IsEnableIndex(currentAnimSpriteList, animIndex))
                AnimationLoopProcess();
            //画像の設定
            enemySprite.sprite = currentAnimSpriteList[animIndex];
            //規定ミリ秒待ち、インデックス増加
            await UniTask.Delay(_ANIMATION_DELAY_MILLI_SEC);
            animIndex++;
        }
    }
    /// <summary>
    /// アニメーションのループ処理
    /// </summary>
    private void AnimationLoopProcess() {
        //待機と歩行は_animIndexを0にする
        animIndex = 0;
    }

    /// <summary>
    /// アニメーションの再生
    /// </summary>
    /// <param name="setAnim"></param>
    public void SetAnimation(eEnemyAnimation setAnim) {
        //現在と同じアニメーションなら処理しない
        if (currentAnim == setAnim) return;

        currentAnim = setAnim;
        animIndex = 0;
    }

}
