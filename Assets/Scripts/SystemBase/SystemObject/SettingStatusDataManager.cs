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
        await UniTask.CompletedTask;
        instance = this;
        //セーブデータの宣言
        saveData = new SettingStatusData();
        //StringBuilderの宣言
        StringBuilder fileNameBuilder = new StringBuilder();
        //StringBuilderを使った文字列連結
        fileNameBuilder.Append(Application.persistentDataPath);
        fileNameBuilder.Append(saveFileName);
        filePath = fileNameBuilder.ToString();
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
            //FileSteamの宣言
            FileStream fileStream = new FileStream(filePath, FileMode.Open);
            //BinaryFormatterの宣言
            BinaryFormatter bf = new BinaryFormatter();
            SettingStatusData data = (SettingStatusData)bf.Deserialize(fileStream);
            fileStream.Close();
            Debug.Log("Save data loaded from " + filePath);
            return data;
        } else {
            Debug.Log("Save file not found.");
            SettingStatusData data = new SettingStatusData();
            return data;
        }
    }
    /// <summary>
    /// Unityが終了した時に呼び出される処理
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
}
