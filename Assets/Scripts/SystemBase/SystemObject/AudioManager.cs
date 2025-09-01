using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;

using static CommonModule;
using static GameConst;

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
    /// BGMの音量調整(10段階調整)
    /// </summary>
    /// <param name="setValue"></param>
    public void SetBGMVolume(float setValue) {
        if (setValue <= 0) {
            bgmAudioSource.volume = 0;
        } else {
            bgmAudioSource.volume = setValue / TEN_DEVIDE_VALUE;
        }
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
            if (seAudioSource[i] == null || seAudioSource[i].isPlaying) continue;
            //再生中でないオーディオソースが見つかったので再生
            seAudioSource[i].clip = seAssign.seArray[seID];
            seAudioSource[i].Play();
            //SEの終了待ち
            while (seAudioSource[i].isPlaying) await UniTask.DelayFrame(1, PlayerLoopTiming.Update, token);

            return;
        }
    }
    /// <summary>
    /// SEの音量調整(10段階調整)
    /// </summary>
    /// <param name="setValue"></param>
    public void SetSEVolume(float setValue) {
        for (int i = 0, max = seAudioSource.Length; i < max; i++) {
            if (setValue <= 0) {
                seAudioSource[i].volume = 0;
            } else {
                seAudioSource[i].volume = setValue / TEN_DEVIDE_VALUE;
            }
        }
    }
}
