using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataUtility : MonoBehaviour {
    public static void SetAttackLv(int setLv) {
        PlayerStatusDataManager.instance.SetAttackLv(setLv);
    }
    public static void SetIntervalLv(int setLv) {
        PlayerStatusDataManager.instance.SetIntervalLv(setLv);
    }
    public static void SetPercentageLv(int setLv) {
        PlayerStatusDataManager.instance.SetPercentageLv(setLv);
    }
    public static void SetStatusPointData(int setValue) {
        PlayerStatusDataManager.instance.SetStatusPoint(setValue);
    }
    public static void AddStatusPointData(int setValue) {
        PlayerStatusDataManager.instance.AddStatusPoint(setValue);
    }
}
