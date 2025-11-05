using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMasterUtility : MonoBehaviour {
    /// <summary>
    /// プレイヤーのマスターデータの取得
    /// </summary>
    /// <returns></returns>
    public static Entity_PlayerStatus.Param GetPlayerMaster() {
        var playerMasterList = MasterDataManager.playerStatusData[0];
        if(playerMasterList == null ) return null;

        return playerMasterList[0];
    }
}
