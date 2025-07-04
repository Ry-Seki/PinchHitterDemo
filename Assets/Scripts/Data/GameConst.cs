using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConst {
    //カメラの拡縮度(最大値、最小値反転)
    public static readonly float MAX_EXPANSION = 5.0f;
    public static readonly float MIN_EXPANSION = 105.0f;

    //正規化の倍率
    public static readonly float MAX_NORM = 0.8f;
    public static readonly float MIN_NORM = 0.02f;

    //最大百分率
    public static readonly float MAX_PERCENTAGE = 100.0f;

    //初期化時の敵の個数
    public static readonly int INIT_FLOOR_ENEMY = 16;
    //想定される敵の最大数
    public static readonly int MAX_FLOOR_ENEMY = 128;

    //最大ステータスレベル
    public static readonly int MAX_STATUS_LEVEL = 50;
}
