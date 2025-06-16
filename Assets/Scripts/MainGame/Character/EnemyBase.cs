using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBase : MonoBehaviour {
    [SerializeField]
    private Slider enemyHPSlider = null;

    public int maxHp { get; protected set; } = -1;

    public int hp { get; protected set; } = -1;
}
