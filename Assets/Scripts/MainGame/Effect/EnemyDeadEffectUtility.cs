using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadEffectUtility {
    public static void UseEffect(EnemyBase targetEnemy) {
        EnemyDeadEffectManager.instance.UseEffect(targetEnemy);
    }
    public static void UnuseEffect(EnemyDeadEffect unuseEffect) {
        EnemyDeadEffectManager.instance.UnuseEffect(unuseEffect);
    }
}
