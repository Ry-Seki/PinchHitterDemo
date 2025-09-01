using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

public class SettingStatusDataManager : SystemObject {
    public static SettingStatusDataManager instance { get; private set; } = null;
    public SettingStatusData saveData { get; private set; } = null;
    private string saveFileName = "/Data.P";
    private string filePath = null;
    //定数
    private const int INIT_BGM_VOLUME = 10;
    private const int INIT_SE_VOLUME = 10;

    public override async UniTask Initialize() {
        instance = this;
        //StringBuilderの宣言
        StringBuilder fileNameBuilder = new StringBuilder();
        //StringBuilderを使った文字列連結
        fileNameBuilder.Append(Application.persistentDataPath);
        fileNameBuilder.Append(saveFileName);
        filePath = fileNameBuilder.ToString();
        //セーブデータの宣言
        LoadData();
        await UniTask.CompletedTask;
    }
    public void SaveData() {
        Debug.Log("Save");
        SaveDataToFile(saveData);
    }

    public void LoadData() {
        saveData = LoadDataFromFile();
    }
    /// <summary>
    /// セーブデータをファイルに渡す
    /// </summary>
    /// <param name="setData"></param>
    /// <param name="fileName"></param>
    private void SaveDataToFile(SettingStatusData setData) {
        //FileSteamの宣言
        FileStream fileStream = new FileStream(filePath, FileMode.Create);
        //BinaryFormatterの宣言
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fileStream, setData);
        //ファイルを閉じる
        fileStream.Close();
    }
    /// <summary>
    /// ファイルの中身をセーブデータに渡す
    /// </summary>
    /// <returns></returns>
    private SettingStatusData LoadDataFromFile() {
        if (File.Exists(filePath)) {
            FileInfo fileInfo = new FileInfo(filePath);
            if(fileInfo.Length == 0) {
                return new SettingStatusData();
            }
            try { 
                //FileSteamの宣言
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open)) {
                    //BinaryFormatterの宣言
                    BinaryFormatter bf = new BinaryFormatter();
                    return (SettingStatusData)bf.Deserialize(fileStream);
                }
            } catch {
                return new SettingStatusData();
            }
        } else {
            return new SettingStatusData(); 
        }
    }
    /// <summary>
    /// Unityが終了した時に呼び出される処理(スマホ版)
    /// </summary>
    /// <param name="pauseStatus"></param>
    private void OnApplicationPause(bool pauseStatus) {
        if (pauseStatus) {
            SaveData();
        }
    }
    /// <summary>
    /// Unityが終了した時に呼び出される処理(スマホ版)
    /// </summary>
    /// <param name="pauseStatus"></param>
    private void OnApplicationFocus(bool hasFocus) {
        if (!hasFocus) {
            SaveData();
        }
    }
    /// <summary>
    /// Unityが終了した時に呼び出される処理(PC版)
    /// </summary>
    private void OnApplicationQuit() {
        //セーブ
        SaveData();
    }
    /// <summary>
    /// BGMの音量設定
    /// </summary>
    public void SetBGMVolume(int setValue) {
        saveData.bgmVolume = setValue;

    }
    /// <summary>
    /// SEの音量設定
    /// </summary>
    /// <param name="setValue"></param>
    public void SetSEVolume(int setValue) {
        saveData.seVolume = setValue;
    }
    /// <summary>
    /// 感度設定
    /// </summary>
    /// <param name="setValue"></param>
    public void SetMoveSensitivity(int setValue) {
        saveData.moveSensitivity = setValue;
    }
}
