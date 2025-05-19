using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private Slider SpecialEnergyGauge;
    public int SpecialEnergy;
    public int EnergyRegen;

    private int maxEnergy;
    private float regenCount;

    private void Awake()
    {
        maxEnergy = SpecialEnergy;
        SpecialEnergy = 0;
        regenCount = 0f;
    }

    private void Start()
    {
        if (SpecialEnergyGauge == null) { Debug.Log("SpecialEnergyGauge is null."); return; }

        SpecialEnergyGauge.maxValue = maxEnergy;
    }

    private void Update()
    {


        // Energy regen and gauge updates
        if (SpecialEnergy >= 100) { return; }

        regenCount += Time.deltaTime;
        if (regenCount >= 1f)
        {
            SpecialEnergy += EnergyRegen;
            regenCount = 0f;
        }

        if (SpecialEnergyGauge == null) { Debug.Log("SpecialEnergyGauge is null."); return; }

        SpecialEnergyGauge.value = SpecialEnergy;
    }
}