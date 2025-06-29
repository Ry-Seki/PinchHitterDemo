using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static GameConst;
using static CommonModule;

public class EnemyManager : MonoBehaviour {
    //自身のインスタンス
    public static EnemyManager instance { get; private set; } = null;
    //使用中キャラクターオブジェクトの親オブジェクト
    [SerializeField]
    private Transform useObjectRoot = null;
    //未使用中キャラクターオブジェクトの親オブジェクト
    [SerializeField]
    private Transform unuseObjectRoot = null;
    [SerializeField]
    private EnemyBase originEnemy = null;

    //使用中の敵のリスト
    [SerializeField]
    private List<EnemyBase> useEnemyList = null;
    //未使用中の敵のリスト
    [SerializeField]
    private List<EnemyBase> unuseEnemyList = null;

    public void Initialize() {
        instance = this;
        //敵のカメラ情報の初期化
        EnemyBase.Initialize();
        //リストの初期化
        useEnemyList = new List<EnemyBase>(INIT_FLOOR_ENEMY);
        unuseEnemyList = new List<EnemyBase>(INIT_FLOOR_ENEMY);
        //敵を必要数生成して未使用状態にする
        for (int i = 0; i < INIT_FLOOR_ENEMY; i++) {
            EnemyBase enemy = Instantiate(originEnemy, unuseObjectRoot);
            unuseEnemyList.Add(enemy);
        }
    }
    /// <summary>
    /// 敵を使用状態にする
    /// </summary>
    public void UseEnemy() {
        EnemyBase enemy;
        //未使用リストが空なら新たに生成
        if(IsEmpty(unuseEnemyList)) {
            enemy = Instantiate(originEnemy, useObjectRoot);
        } else {
            //未使用リストから使う
            enemy = unuseEnemyList[0];
            unuseEnemyList.RemoveAt(0);
            enemy.transform.SetParent(useObjectRoot);
        }
        //敵の準備
        useEnemyList.Add(enemy);
        enemy.Setup();
    }
    /// <summary>
    /// 敵を未使用状態にする
    /// </summary>
    public void UnuseEnemy(EnemyBase unuseEnemy) {
        if(unuseEnemy == null) return;

        //未使用リストに加える
        unuseEnemyList.Add(unuseEnemy);
        //片付け処理を呼ぶ
        unuseEnemy.Teardown();
        //使用リストから削除する
        useEnemyList.Remove(unuseEnemy);
        //親オブジェクトの移動
        unuseEnemy.transform.SetParent(unuseObjectRoot);
    }
    public void SpawnEnemy(int spawnCount) {
        //敵の生成数の制御
        if(spawnCount >= MAX_FLOOR_ENEMY) spawnCount = MAX_FLOOR_ENEMY;
        //敵の生成
        for (int i = 0; i < spawnCount; i++) {
            UseEnemy();
        }
    }
    public int GetEnemyCount() {
        return useEnemyList.Count;
    }
}
