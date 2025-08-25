using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private List<EnemyDeadEffect> useDeadEffect = null;
    //未使用時のエフェクトリスト
    private List<EnemyDeadEffect> unuseDeadEffect = null;
    //初期化時に生成しておくエフェクトの数
    private const int INIT_DEAD_EFFECT_COUNT = 16;

    /// <summary>
    /// 初期化処理
    /// </summary>
    public void Initialize() {
        
    }

    public void UseEffect() {

    }
}
