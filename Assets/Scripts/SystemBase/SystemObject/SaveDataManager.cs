using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

using static PlayerStatusUtility;

public class SaveDataManager : SystemObject {
    public static SaveDataManager instance { get; private set; } = null;
    public SaveData saveData { get; private set; } = null;
    private string saveFileName = "/Data.S";
    private string filePath = null;

    public override async UniTask Initialize() {
        instance = this;
        //セーブデータの宣言
        saveData = new SaveData();
        //StringBuilderの宣言
        StringBuilder fileNameBuilder = new StringBuilder();
        //StringBuilderを使った文字列連結
        fileNameBuilder.Append(Application.persistentDataPath);
        fileNameBuilder.Append(saveFileName);
        filePath = fileNameBuilder.ToString();
        await UniTask.CompletedTask;
    }

    public void SaveData() {
        if(ScoreText.score > saveData.highScore) 
            saveData.highScore = ScoreText.score;

        Debug.Log("Save");
        SaveDataToFile(saveData);
    }

    public void LoadData() {
        saveData = LoadDataFromFile();
        //デバッグ用
        Debug.Log("saveData.highScore : " + saveData.highScore);
    }
    /// <summary>
    /// セーブデータをファイルに渡す
    /// </summary>
    /// <param name="setData"></param>
    /// <param name="fileName"></param>
    private void SaveDataToFile(SaveData setData) {
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
    private SaveData LoadDataFromFile() {
        if (File.Exists(filePath)) {
            //FileSteamの宣言
            FileStream fileStream = new FileStream(filePath, FileMode.Open);
            //BinaryFormatterの宣言
            BinaryFormatter bf = new BinaryFormatter();
            SaveData data = (SaveData)bf.Deserialize(fileStream);
            fileStream.Close();
            Debug.Log("Save data loaded from " + filePath);
            return data;
        } else {
            Debug.Log("Save file not found.");
            SaveData data = new SaveData();
            InitSaveStatus();
            return data;
        }
    }
    private void OnApplicationQuit() {
        SaveData();
    }
    /// <summary>
    /// 攻撃レベル設定
    /// </summary>
    /// <param name="setValue"></param>
    public void SetAttackLv(int setValue) {
        saveData.rawAttackLv = setValue;
    }
    /// <summary>
    /// 攻撃間隔レベル設定
    /// </summary>
    /// <param name="setValue"></param>
    public void SetIntervalLv(int setValue) {
        saveData.rawAtkIntervalLv = setValue;
    }
    /// <summary>
    /// 攻撃可能拡縮率レベル設定
    /// </summary>
    /// <param name="setValue"></param>
    public void SetPercentageLv(int setValue) {
        saveData.rawAtkPercentageLv = setValue;
    }
    /// <summary>
    /// ステータスポイント設定
    /// </summary>
    /// <param name="setValue"></param>
    public void SetStatusPoint(int setValue) {
        saveData.statusPoint = setValue;
    }
    /// <summary>
    /// セーブデータの初期化
    /// </summary>
    public void InitSaveStatus() {
        saveData.highScore = 0;
        saveData.rawAttackLv = 0;
        saveData.rawAtkIntervalLv = 0;
        saveData.rawAtkPercentageLv = 0;
        saveData.statusPoint = 0;
    }
}
