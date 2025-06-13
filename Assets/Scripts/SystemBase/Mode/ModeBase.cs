using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ModeBase : MonoBehaviour {
    
    public virtual async UniTask Initialize() {
        gameObject.SetActive(false);
        await UniTask.CompletedTask;
    }
    public virtual async UniTask Setup() {
        gameObject.SetActive(true);
        await UniTask.CompletedTask;
    }
    public abstract UniTask Execute();

    public virtual async UniTask Teardown() {
        gameObject.SetActive(false);
        await UniTask.CompletedTask;
    }
}
