using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

using static CommonModule;
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
    }
    //アニメーションの再生
    public async UniTask PlayEffectAnimation() {
        int animMax = animationSpriteList.Length;
        if (animIndex != 0) ResetAnimation();
        gameObject.SetActive(true);
        Sprite[] currentAnimSpriteList = animationSpriteList[0];
        while (true) {
            if(!IsEnableIndex(currentAnimSpriteList, animIndex)) break;
            characterSprite.sprite = currentAnimSpriteList[animIndex];
            //規定ミリ秒待ち
            await UniTask.Delay(ANIMATION_DELAY_MILLI_SEC);
            animIndex++;
        }
        gameObject.SetActive(false);
    }
    public void ResetAnimation() {
        animIndex = 0;
    }
}
