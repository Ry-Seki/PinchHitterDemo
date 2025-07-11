using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;

using static CommonModule;

public class AudioManager : SystemObject {
    public static AudioManager instance { get; private set; } = null;
    //BGM再生用コンポーネント
    [SerializeField]
    private AudioSource bgmAudioSource = null;
    //SE再生用コンポーネント
    [SerializeField]
    private AudioSource[] seAudioSource = null;
    //BGMのリスト
    [SerializeField]
    private BGMAssign bgmAssign = null;
    [SerializeField]
    private SEAssign seAssign = null;
    //移動のタスクを中断するためのトークン
    private CancellationToken token;

    public override async UniTask Initialize() {
        instance = this;
        await UniTask.CompletedTask;
    }

    /// <summary>
    /// BGM再生
    /// </summary>
    /// <param name="bgmID"></param>
    public void PlayBGM(int bgmID) {
        if (!IsEnableIndex(bgmAssign.bgmArray, bgmID)) return;

        bgmAudioSource.clip = bgmAssign.bgmArray[bgmID];
        bgmAudioSource.Play();
    }
    /// <summary>
    /// BGM停止
    /// </summary>
    public void StopBGM() {
        bgmAudioSource.Stop();
    }
    /// <summary>
    /// SE再生
    /// </summary>
    /// <param name="seID"></param>
    public async UniTask PlaySE(int seID) {
        token = this.GetCancellationTokenOnDestroy();
        if (!IsEnableIndex(seAssign.seArray, seID)) return;
        //再生中でないオーディオソースを探してそれで再生
        for (int i = 0, max = seAudioSource.Length; i < max; i++) {
            AudioSource audioSource = seAudioSource[i];
            if (audioSource == null || audioSource.isPlaying) continue;
            //再生中でないオーディオソースが見つかったので再生
            audioSource.clip = seAssign.seArray[seID];
            audioSource.Play();
            //SEの終了待ち
            while (audioSource.isPlaying) await UniTask.DelayFrame(1, PlayerLoopTiming.Update, token);

            return;
        }
    }
}
