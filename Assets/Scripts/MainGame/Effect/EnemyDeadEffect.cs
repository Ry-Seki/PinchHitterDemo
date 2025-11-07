using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using UnityEngine;

using static CommonModule;
using static EnemyDeadEffectUtility;

public class EnemyDeadEffect : MonoBehaviour {
    //画像読み込み用変数
    private static StringBuilder _spriteNameBuilder = new StringBuilder();
    private static readonly string _CHARACTER_SPRITE_PATH = "Design/Effect/DeadEffect/";
    private static readonly string[] _ANIMATION_SPRITE_NAME =
        new string[] { "FX001_01", "FX001_02", "FX001_03", "FX001_04", "FX001_05" };
    //アニメーションで画像が切り替わる時間(固定)
    private static readonly int _ANIMATION_DELAY_MILLI_SEC = 150;
    // エフェクトのスプライトコンポーネント
    [SerializeField]
    private SpriteRenderer _effectSprite = null;
    // アニメーションエフェクトスプライトリスト
    private Sprite[][] _animationSpriteList = null;
    // アニメション番号
    private int _animIndex = -1;

    private CancellationToken _token;    

    /// <summary>
    /// エフェクトスプライトの読み込み
    /// </summary>
    public void LoadSprite() {
        int animMax = _ANIMATION_SPRITE_NAME.Length;
        _animationSpriteList = new Sprite[1][];
        _animationSpriteList[0] = new Sprite[animMax];
        for (int i = 0; i < animMax; i++) {
            _spriteNameBuilder.Append(_CHARACTER_SPRITE_PATH);
            _spriteNameBuilder.Append(_ANIMATION_SPRITE_NAME[i]);
            _animationSpriteList[0][i] = Resources.Load<Sprite>(_spriteNameBuilder.ToString());
            _spriteNameBuilder.Clear();
        }
    }
    /// <summary>
    /// 使用前準備
    /// </summary>
    public void Setup() {
        // エフェクトの読み込み (初回のみ)
        if(IsEmpty(_animationSpriteList)) LoadSprite();
        _animIndex = 0;
    }
    /// <summary>
    /// アニメーションの再生
    /// </summary>
    /// <param name="targetEnemy"></param>
    /// <returns></returns>
    public async UniTask PlayEffectAnimation(EnemyBase targetEnemy) {
        _token = this.GetCancellationTokenOnDestroy();
        int animMax = _animationSpriteList.Length;
        // 位置設定
        transform.position = targetEnemy.transform.position;
        Sprite[] currentAnimSpriteList = _animationSpriteList[0];
        while (true) {
            // 配列分再生終わったら抜ける
            if(!IsEnableIndex(currentAnimSpriteList, _animIndex)) break;
            _effectSprite.sprite = currentAnimSpriteList[_animIndex];
            //規定ミリ秒待ち
            await UniTask.Delay(_ANIMATION_DELAY_MILLI_SEC, false, PlayerLoopTiming.Update, _token);
            _animIndex++;
        }
        // 未使用状態にする
        UnuseEffect(this);
    }
    /// <summary>
    /// 片付け処理
    /// </summary>
    public void Teardown() {
        _animIndex = 0;
        transform.position = Vector3.zero;
    }
}
