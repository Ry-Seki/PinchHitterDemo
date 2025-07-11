using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataUtility : MonoBehaviour {
    public static void SetAttackLv(int setLv) {
        SaveDataManager.instance.SetAttackLv(setLv);
    }
    public static void SetIntervalLv(int setLv) {
        SaveDataManager.instance.SetIntervalLv(setLv);
    }
    public static void SetPercentageLv(int setLv) {
        SaveDataManager.instance.SetPercentageLv(setLv);
    }
}
