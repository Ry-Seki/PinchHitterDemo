using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

using static CommonModule;

public class PartManager : SystemObject {
    public static PartManager instance { get; private set; } = null;
    // オリジナルパートリスト
    [SerializeField]
    private PartBase[] _originPartList = null;
    // パートリスト
    private PartBase[] _partList = null;
    // 現在のパート
    private PartBase currentPart = null;

    public override async UniTask Initialize() {
        instance = this;
        // パートオブジェクトの生成、初期化
        int partMax = (int)eGamePart.Max;
        _partList = new PartBase[partMax];
        
        // 非同期処理用リスト
        List<UniTask> taskList = new List<UniTask>(partMax);
        for (int i = 0; i < partMax; i++) {
            if (_originPartList[i] == null) continue;

            _partList[i] = Instantiate(_originPartList[i], transform);
            taskList.Add(_partList[i].Initialize());
        }
        // 初期化が終了するまで待つ
        await WaitTask(taskList);
    }
    /// <summary>
    /// パートの切り替え
    /// </summary>
    /// <param name="nextPart"></param>
    /// <returns></returns>
    public async UniTask TransitionPart(eGamePart nextPart) {
        // 現在のパートの片付け
        if (currentPart != null) await currentPart.Teardown();

        // パートの切り替え
        currentPart = _partList[(int)nextPart];
        await currentPart.Setup();
        // 次のパートの実行
        UniTask task = currentPart.Execute();
    }
}
