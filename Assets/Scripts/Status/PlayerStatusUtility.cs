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
    /// UŒ‚—Íİ’è
    /// </summary>
    /// <param name="setLevel"></param>
    public static void SetRawAttack(int setValue) {
        PlayerStatusManager.instance.SetRawAttack(setValue);
    }
    /// <summary>
    /// ƒŒƒxƒ‹w’è‚ÌUŒ‚—Í‘‰Á
    /// </summary>
    /// <param name="setLevel"></param>
    public static void SetAttackStatusLv(int setLevel) {
        PlayerStatusDataManager.instance.SetAttackLv(setLevel);
        PlayerStatusManager.instance.SetAttackStatusLv(setLevel);
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
    public static void SetRawInterval(int setValue) {
        PlayerStatusManager.instance.SetRawInterval(setValue);
    }
    /// <summary>
    /// ƒŒƒxƒ‹w’è‚ÌUŒ‚ŠÔŠu’Zk
    /// </summary>
    /// <param name="setLevel"></param>
    public static void SetIntervalStatusLv(int setLevel) {
        PlayerStatusDataManager.instance.SetIntervalLv(setLevel);
        PlayerStatusManager.instance.SetIntervalStatusLv(setLevel);
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
    public static void SetRawPercentage(int setValue) {
        PlayerStatusManager.instance.SetRawPercentage(setValue);
    }
    /// <summary>
    /// ƒŒƒxƒ‹w’è‚ÌUŒ‚‰Â”\Šgk—¦Šg‘å
    /// </summary>
    /// <param name="setLevel"></param>
    public static void SetPercentageStatusLv(int setLevel) {
        PlayerStatusDataManager.instance.SetPercentageLv(setLevel);
        PlayerStatusManager.instance.SetPercentageStatusLv(setLevel);
    }
    /// <summary>
    /// §ŒÀŠÔ‚Ìæ“¾
    /// </summary>
    /// <returns></returns>
    public static int GetRawLimitTime() {
        return PlayerStatusManager.instance.GetRawLimitTime();
    }
    /// <summary>
    /// §ŒÀŠÔ‚Ìİ’è
    /// </summary>
    /// <param name="setValue"></param>
    public static void SetRawLimitTime(int setValue) {
        PlayerStatusManager.instance.SetRawLimitTme(setValue);
    }
    /// <summary>
    /// ƒŒƒxƒ‹w’è‚Ì§ŒÀŠÔ‚Ìİ’è
    /// </summary>
    /// <param name="setLevel"></param>
    public static void SetLimitTimeStatusLv(int setLevel) {
        PlayerStatusDataManager.instance.SetLimitTimeLv(setLevel);
        PlayerStatusManager.instance.SetLimitTimeStatusLv(setLevel);
    }
    public static void InitStatus() {
        PlayerStatusManager.instance.InitStatus();
        PlayerStatusDataManager.instance.InitLevel();
    }
}
