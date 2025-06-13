using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBase : MonoBehaviour {
    [SerializeField]
    private GameObject _menuRoot = null;

    public virtual async UniTask Initialize() {
        await UniTask.CompletedTask;
    }
    //開く
    public virtual async UniTask Open() {
        //メニューを表示する
        _menuRoot?.SetActive(true);
        await UniTask.CompletedTask;
    }

    public virtual async UniTask Close() {
        _menuRoot?.SetActive(false);
        await UniTask.CompletedTask;
    }
}
