using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MonsterManager : MonoBehaviour
{
    [SerializeField]
    private Slider MonsterHPBar;
    public int MonsterHP;

    private TMP_Text hpText;
    private int maxHp;

    private void Awake()
    {
        maxHp = MonsterHP;
    }

    private void Start()
    {
        if (MonsterHPBar == null) { Debug.Log("MonsterHPBar is null."); return; }

        MonsterHPBar.maxValue = maxHp;

        hpText = MonsterHPBar.GetComponentInChildren<TMP_Text>();
        if (hpText == null) { Debug.Log("hpText is null."); }
    }

    private void Update()
    {
        Damaged();

        // HP bar updates
        if (MonsterHPBar == null) { Debug.Log("MonsterHPBar is null."); return; }

        MonsterHPBar.value = MonsterHP;

        if (hpText == null) { Debug.Log("hpText is null."); return; }

        hpText.text = $"{MonsterHP} / {maxHp}";
    }

    private void Damaged()
    {
        // TODO: Damage to monster HP

        
    }
}