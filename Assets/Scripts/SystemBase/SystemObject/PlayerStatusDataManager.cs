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
    // セーブデータ
    public PlayerStatusData saveData { get; private set; } = null;

    // セーブデータ拡張子
    private string saveFileName = "/Data.S";
    // ファイルパス
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
    /// <summary>
    /// データのセーブ
    /// </summary>
    public void SaveData() {
        SaveDataToFile(saveData);
    }
    /// <summary>
    /// データのロード
    /// </summary>
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
    /// ファイルの中身をセーブデータに変換して返す
    /// </summary>
    /// <returns></returns>
    private PlayerStatusData LoadDataFromFile() {
        // ファイルの存在確認
        if (File.Exists(filePath)) {
            FileInfo fileInfo = new FileInfo(filePath);
            // 中身が空なら新たに生成
            if (fileInfo.Length == 0) {
                return new PlayerStatusData();
            }
            try {
                //FileSteamの宣言
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open)) {
                    //BinaryFormatterの宣言
                    BinaryFormatter bf = new BinaryFormatter();
                    return (PlayerStatusData)bf.Deserialize(fileStream);
                }
            } catch {
                // エラーがある場合、新たに生成
                return new PlayerStatusData();
            }
        } else {
            // 新たに生成
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
    /// <summary>
    /// ハイスコア設定
    /// </summary>
    /// <param name="setValue"></param>
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
