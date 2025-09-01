using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStatusDataManager : SystemObject {
    public static PlayerStatusDataManager instance { get; private set; } = null;
    public PlayerStatusData saveData { get; private set; } = null;
    private string saveFileName = "/Data.S";
    private string filePath = null;

    public override async UniTask Initialize() {
        instance = this;
        //StringBuilderの宣言
        StringBuilder fileNameBuilder = new StringBuilder();
        //StringBuilderを使った文字列連結
        fileNameBuilder.Append(Application.persistentDataPath);
        fileNameBuilder.Append(saveFileName);
        //連結したファイルパスを渡す
        filePath = fileNameBuilder.ToString();
        //セーブデータの宣言
        LoadData();
        await UniTask.CompletedTask;
    }

    public void SaveData() {
        SaveDataToFile(saveData);
    }

    public void LoadData() {
        saveData = LoadDataFromFile();
        //マスターデータの読み込み
        MasterDataManager.LoadAllData();
    }
    /// <summary>
    /// セーブデータをファイルに渡す
    /// </summary>
    /// <param name="setData"></param>
    /// <param name="fileName"></param>
    private void SaveDataToFile(PlayerStatusData setData) {
        //FileStreamの宣言
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
    private PlayerStatusData LoadDataFromFile() {
        if (File.Exists(filePath)) {
            FileInfo fileInfo = new FileInfo(filePath);
            if (fileInfo.Length == 0) {
                Debug.LogWarning("ファイルは存在しますが中身が空です。");
                return new PlayerStatusData(); // デフォルトデータを返す
            }
            try {
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open)) {
                    BinaryFormatter bf = new BinaryFormatter();
                    return (PlayerStatusData)bf.Deserialize(fileStream);
                }
            } catch (Exception exeption) {
                Debug.LogError("デシリアライズ中に例外が発生しました: " + exeption.Message);
                return new PlayerStatusData(); // デフォルトデータでリカバリ
            }
        } else {
            Debug.Log("セーブデータが見つからないので、新規作成します。");
            return new PlayerStatusData();
        }
    }
    /// <summary>
    /// Unityが終了した時に呼び出される処理
    /// </summary>
    private void OnApplicationQuit() {
        //セーブ
        SaveData();
    }
    public void SetHighScore(int setValue) {
        saveData.highScore = setValue;
    }
    /// <summary>
    /// 攻撃レベル設定
    /// </summary>
    /// <param name="setLevel"></param>
    public void SetAttackLv(int setLevel) {
        saveData.attackLv = setLevel;
    }
    /// <summary>
    /// 攻撃間隔レベル設定
    /// </summary>
    /// <param name="setLevel"></param>
    public void SetIntervalLv(int setLevel) {
        saveData.atkIntervalLv = setLevel;
    }
    /// <summary>
    /// 攻撃可能拡縮率レベル設定
    /// </summary>
    /// <param name="setLevel"></param>
    public void SetPercentageLv(int setLevel) {
        saveData.atkPercentageLv = setLevel;
    }
    /// <summary>
    /// 制限時間レベルの設定
    /// </summary>
    /// <param name="setLevel"></param>
    public void SetLimitTimeLv(int setLevel) {
        saveData.limitTimeLv = setLevel;
    }
    /// <summary>
    /// ステータスポイント設定
    /// </summary>
    /// <param name="setValue"></param>
    public void SetStatusPoint(int setValue) {
        saveData.statusPoint = setValue;
    }
    /// <summary>
    /// ステータスポイント加算
    /// </summary>
    /// <param name="setValue"></param>
    public void AddStatusPoint(int setValue) {
        saveData.statusPoint += setValue;
    }
    /// <summary>
    /// セーブデータの初期化
    /// </summary>
    public void InitSaveStatus() {
        saveData = new PlayerStatusData();
    }
    /// <summary>
    /// レベルデータの初期化
    /// </summary>
    public void InitLevel() {
        saveData.attackLv = 0;
        saveData.atkIntervalLv = 0;
        saveData.atkPercentageLv = 0;
        saveData.limitTimeLv = 0;
    }
}
