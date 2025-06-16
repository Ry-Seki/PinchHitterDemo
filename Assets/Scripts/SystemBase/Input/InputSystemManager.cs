using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;

public class InputSystemManager : SystemObject
{
    public static InputSystemManager instance { get; private set; } = null;
    public PinchHitterDemo input { get; private set; } = null;

    public override async UniTask Initialize() {
        instance = this;
        input = new PinchHitterDemo();
        await UniTask.CompletedTask;
    }
}
