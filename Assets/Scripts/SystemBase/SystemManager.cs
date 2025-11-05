using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemManager : MonoBehaviour {
    // システムオブジェクト配列
    [SerializeField]
    private SystemObject[] _systemObjectList = null;

    private void Start() {
        // フレームレートの設定
        Application.targetFrameRate = 60;
        // 初期化
        UniTask task = Initialize();
    }
    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <returns></returns>
    private async UniTask Initialize() {
        // システムオブジェクトの生成、初期化
        for (int i = 0, max = _systemObjectList.Length; i < max; i++) {
            if(_systemObjectList[i] == null) continue;

            SystemObject systemObject = Instantiate(_systemObjectList[i], transform);
            await systemObject.Initialize();
        }
        // パートの遷移
        UniTask task = PartManager.instance.TransitionPart(eGamePart.Stanby);
    }
}
