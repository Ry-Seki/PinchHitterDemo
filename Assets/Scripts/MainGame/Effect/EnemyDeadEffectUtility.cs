using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadEffectUtility {
    /// <summary>
    /// エフェクトの使用
    /// </summary>
    /// <param name="targetEnemy"></param>
    public static void UseEffect(EnemyBase targetEnemy) {
        EnemyDeadEffectManager.instance.UseEffect(targetEnemy);
    }
    /// <summary>
    /// エフェクトを未使用状態にする
    /// </summary>
    /// <param name="unuseEffect"></param>
    public static void UnuseEffect(EnemyDeadEffect unuseEffect) {
        EnemyDeadEffectManager.instance.UnuseEffect(unuseEffect);
    }
}
