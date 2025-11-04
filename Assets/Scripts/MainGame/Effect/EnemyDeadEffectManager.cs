using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static CommonModule;

public class EnemyDeadEffectManager : MonoBehaviour {
    //自身の参照
    public static EnemyDeadEffectManager instance { get; private set; } = null;
    //使用中の親オブジェクト
    [SerializeField]
    private Transform useRoot = null;
    //未使用時の親オブジェクト
    [SerializeField]
    private Transform unuseRoot = null;
    //敵死亡時エフェクトの素
    [SerializeField]
    private EnemyDeadEffect originDeadEffect = null;
    //使用中のエフェクトリスト
    private List<EnemyDeadEffect> useEffectList = null;
    //未使用時のエフェクトリスト
    private List<EnemyDeadEffect> unuseEffectList = null;
    //初期化時に生成しておくエフェクトの数
    private const int INIT_DEAD_EFFECT_COUNT = 16;

    /// <summary>
    /// 初期化処理
    /// </summary>
    public void Initialize() {
        instance = this;
        unuseEffectList = new List<EnemyDeadEffect>(INIT_DEAD_EFFECT_COUNT);
        useEffectList = new List<EnemyDeadEffect>(INIT_DEAD_EFFECT_COUNT);
        for (int i = 0; i < INIT_DEAD_EFFECT_COUNT; i++) {
            EnemyDeadEffect createObject = Instantiate(originDeadEffect, unuseRoot);
            unuseEffectList.Add(createObject);
        }
    }
    /// <summary>
    /// 使用状態にする
    /// </summary>
    public void UseEffect(EnemyBase targetEneny) {
        EnemyDeadEffect effect;
        //未使用リストにあるなら未使用リストから使う
        if (IsEmpty(unuseEffectList)) {
            effect = Instantiate(originDeadEffect, useRoot);
        } else {
            effect = unuseEffectList[0];
            unuseEffectList.RemoveAt(0);
            effect.transform.SetParent(useRoot);
        }
        useEffectList.Add(effect);
        effect.Setup();
        UniTask effectTask = effect.PlayEffectAnimation(targetEneny);
    }
    /// <summary>
    /// 未使用状態にする
    /// </summary>
    /// <param name="unuseEffect"></param>
    public void UnuseEffect(EnemyDeadEffect unuseEffect) {
        if(unuseEffect == null) return;

        unuseEffect.Teardown();
        unuseEffectList.Add(unuseEffect);
        useEffectList.Remove(unuseEffect);
        unuseEffect.transform.SetParent(unuseRoot);
    }
}
