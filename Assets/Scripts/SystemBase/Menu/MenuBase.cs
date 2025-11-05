using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBase : MonoBehaviour {
    // メニューオブジェクト
    [SerializeField]
    protected GameObject menuRoot = null;

    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <returns></returns>
    public virtual async UniTask Initialize() {
        await UniTask.CompletedTask;
    }
    /// <summary>
    /// 開く処理
    /// </summary>
    /// <returns></returns>
    public virtual async UniTask Open() {
        //メニューを表示する
        menuRoot?.SetActive(true);
        await UniTask.CompletedTask;
    }
    /// <summary>
    /// 閉じる処理
    /// </summary>
    /// <returns></returns>
    public virtual async UniTask Close() {
        menuRoot?.SetActive(false);
        await UniTask.CompletedTask;
    }
}
