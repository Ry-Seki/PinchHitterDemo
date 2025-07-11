using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static SaveDataUtility;

public class PlayerStatusUtility : MonoBehaviour {
    /// <summary>
    /// UŒ‚—Íæ“¾
    /// </summary>
    /// <returns></returns>
    public static int GetRawAttack() {
        return PlayerStatusManager.instance.GetRawAttack();
    }
    /// <summary>
    /// ƒŒƒxƒ‹w’è‚ÌUŒ‚—Íİ’è
    /// </summary>
    /// <param name="setLevel"></param>
    public static void SetRawAttack(int setLevel) {
        PlayerStatusManager.instance.EnhanceAttack(setLevel);
    }
    /// <summary>
    /// ƒŒƒxƒ‹w’è‚ÌUŒ‚—Í‘‰Á
    /// </summary>
    /// <param name="setLevel"></param>
    public static void EnhanceAttack(int setLevel) {
        SaveDataManager.instance.SetAttackLv(setLevel);
        PlayerStatusManager.instance.EnhanceAttack(setLevel);
    }
    /// <summary>
    /// UŒ‚ŠÔŠuæ“¾
    /// </summary>
    /// <returns></returns>
    public static float GetRawInterval() {
        return PlayerStatusManager.instance.GetRawInterval();
    }
    /// <summary>
    /// ƒŒƒxƒ‹w’è‚ÌUŒ‚ŠÔŠuİ’è
    /// </summary>
    /// <param name="setLevel"></param>
    public static void SetRawInterval(int setLevel) {
        PlayerStatusManager.instance.ShortInterval(setLevel);
    }
    /// <summary>
    /// ƒŒƒxƒ‹w’è‚ÌUŒ‚ŠÔŠu’Zk
    /// </summary>
    /// <param name="setLevel"></param>
    public static void ShortInterval(int setLevel) {
        SaveDataManager.instance.SetIntervalLv(setLevel);
        PlayerStatusManager.instance.ShortInterval(setLevel);
    }
    /// <summary>
    /// UŒ‚‰Â”\Šgk—¦æ“¾
    /// </summary>
    /// <returns></returns>
    public static float GetRawPercentage() {
        return PlayerStatusManager.instance.GetRawPercentage();
    }
    /// <summary>
    /// ƒŒƒxƒ‹w’è‚ÌUŒ‚‰Â”\Šgk—¦İ’è
    /// </summary>
    /// <param name="setLevel"></param>
    public static void SetRawPercentage(int setLevel) {
        PlayerStatusManager.instance.ExpansionPercentage(setLevel);
    }
    /// <summary>
    /// ƒŒƒxƒ‹w’è‚ÌUŒ‚‰Â”\Šgk—¦Šg‘å
    /// </summary>
    /// <param name="setLevel"></param>
    public static void ExpansionPercentage(int setLevel) {
        SaveDataManager.instance.SetPercentageLv(setLevel);
        PlayerStatusManager.instance.ExpansionPercentage(setLevel);
    }

    public static void InitStatus() {
        PlayerStatusManager.instance.InitStatus();
        SaveDataManager.instance.InitLevel();
    }
}
