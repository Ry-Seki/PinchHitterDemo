using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUtility : MonoBehaviour {
    /// <summary>
    /// “G‚ğg—pó‘Ô‚É‚·‚é
    /// </summary>
    /// <param name="setPhase"></param>
    public static void UseEnemy(int setPhase) {
        EnemyManager.instance.UseEnemy(setPhase);
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
    /// <param name="setPhase"></param>
    public static void SpawnEnemy(int spawnCount, int setPhase) {
        EnemyManager.instance.SpawnEnemy(spawnCount, setPhase);
    }
    /// <summary>
    /// ‘S‚Ä‚Ì“G‚ğ”ñ•\¦‚É‚·‚é
    /// </summary>
    public static void UnuseAllEnemy() {
        EnemyManager.instance.UnuseAllEnemy();
    }
    /// <summary>
    /// ‘¶İ‚·‚é“G‚Ì”‚Ìæ“¾
    /// </summary>
    /// <returns></returns>
    public static int GetEnemyCount() { 
        return EnemyManager.instance.GetEnemyCount();
    }

    public static void DeathEnemy(EnemyBase unuseEnemy) {
        UnuseEnemy(unuseEnemy);
        ScoreTextManager.instance.AddEnemyDeathValue();
    }
}
