using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

using static CommonModule;
using static EnemyDeadEffectUtility;

public class EnemyDeadEffect : MonoBehaviour {
    //画像読み込み用変数
    private static StringBuilder spriteNameBuilder = new StringBuilder();
    private static readonly string CHARACTER_SPRITE_PATH = "Design/Effect/DeadEffect/";
    private static readonly string[] ANIMATION_SPRITE_NAME =
        new string[] { "FX001_01", "FX001_02", "FX001_03", "FX001_04", "FX001_05" };
    //アニメーションで画像が切り替わる時間(固定)
    private static readonly int ANIMATION_DELAY_MILLI_SEC = 150;

    [SerializeField]
    private SpriteRenderer characterSprite = null;
    private Sprite[][] animationSpriteList = null;
    private int animIndex = -1;

    /// <summary>
    /// 使用前準備
    /// </summary>
    public void Setup() {
        int animMax = ANIMATION_SPRITE_NAME.Length;
        animationSpriteList = new Sprite[1][];
        animationSpriteList[0] = new Sprite[animMax];
        for (int i = 0; i < animMax; i++) {
            spriteNameBuilder.Append(CHARACTER_SPRITE_PATH);
            spriteNameBuilder.Append(ANIMATION_SPRITE_NAME[i]);
            animationSpriteList[0][i] = Resources.Load<Sprite>(spriteNameBuilder.ToString());
            spriteNameBuilder.Clear();
        }
        animIndex = 0;
    }
    /// <summary>
    /// アニメーションの再生
    /// </summary>
    /// <param name="targetEnemy"></param>
    /// <returns></returns>
    public async UniTask PlayEffectAnimation(EnemyBase targetEnemy) {
        int animMax = animationSpriteList.Length;
        transform.position = targetEnemy.transform.position;
        Sprite[] currentAnimSpriteList = animationSpriteList[0];
        while (true) {
            if(!IsEnableIndex(currentAnimSpriteList, animIndex)) break;
            characterSprite.sprite = currentAnimSpriteList[animIndex];
            //規定ミリ秒待ち
            await UniTask.Delay(ANIMATION_DELAY_MILLI_SEC);
            animIndex++;
        }
        UnuseEffect(this);
    }
    /// <summary>
    /// 片付け処理
    /// </summary>
    public void Teardown() {
        animIndex = 0;
        transform.position = Vector3.zero;
        gameObject.SetActive(false);
    }
}
