using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUtility : MonoBehaviour {
    /// <summary>
    /// “G‚ğg—pó‘Ô‚É‚·‚é
    /// </summary>
    public static void UseEnemy() {
        EnemyManager.instance.UseEnemy();
    }
    /// <summary>
    /// “G‚ğ–¢g—pó‘Ô‚É‚·‚é
    /// </summary>
    /// <param name="unuseEnemy"></param>
    public static void UnuseEnemy(EnemyBase unuseEnemy) {
        EnemyManager.instance.UnuseEnemy(unuseEnemy);
    }
    /// <summary>
    /// “G‚Ì¶¬
    /// </summary>
    /// <param name="spawnCount"></param>
    public static void SpawnEnemy(int spawnCount) {
        EnemyManager.instance.SpawnEnemy(spawnCount);
    }
    /// <summary>
    /// ‘¶İ‚·‚é“G‚Ì”‚Ìæ“¾
    /// </summary>
    /// <returns></returns>
    public static int GetEnemyCount() { 
        return EnemyManager.instance.GetEnemyCount();
    }
}
