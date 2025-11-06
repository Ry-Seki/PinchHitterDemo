/// <summary>
/// ゲームパート
/// </summary>
public enum eGamePart {
    Invalid = -1,
    Stanby,
    Title,
    MainGame,
    Ending,

    Max
}
/// <summary>
/// タイトルメニュー
/// </summary>
public enum eTitleMenu {
    Invalid = -1,
    StartGame,
    Upgrade,
    Setting,
    
    Max
}
/// <summary>
/// ゲームモード
/// </summary>
public enum eGameMode {
    Invalid = -1,
    ScoreAttack,
    Endless,

    Max
}

public enum eFadeState {
    Invalid = -1,
    FadeOut = 1,
    FadeIn = 0,
}

public enum eEnemyAnimation {
    Invalid = -1,
    Wait,
    Max,
}