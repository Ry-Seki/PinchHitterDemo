using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeScoreAttack : ModeBase {
    [SerializeField]
    private GameObject scoreAttackUI = null;

    public override async UniTask Initialize() {
        await base.Initialize();
        await FadeManager.instance.FadeIn();
        scoreAttackUI.SetActive(true);
    }
    public override async UniTask Execute() {
        await UniTask.CompletedTask;
    }
}
