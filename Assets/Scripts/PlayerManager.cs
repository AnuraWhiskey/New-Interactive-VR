
/**
 * 플레이어의 특수 에너지를 관리한다.
 * 에너지의 총량과 에너지 재생량은 인스펙터에서 정의한다.
 * 에너지의 총량까지 매초 EnergyRegen 만큼 재생한다.
 * 에너지 UI에 매초 현재 에너지량을 업데이트한다.
 * GainEnergy() 함수는 몬스터가 데미지를 입었을 때 호출된다.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private Slider SpecialEnergyGauge;

    [Space (10f)]
    public int SpecialEnergy;
    [SerializeField]
    private int EnergyRegen;
    [SerializeField]
    private int HitEnergyRegen;

    [HideInInspector]
    public int maxEnergy;
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
        if (gameManager.isGameStarted) { return; }

        // Energy regen and gauge updates
        if (SpecialEnergy >= maxEnergy) { SpecialEnergy = maxEnergy; return; }

        regenCount += Time.deltaTime;
        if (regenCount >= 1f)
        {
            SpecialEnergy += EnergyRegen;
            regenCount = 0f;
        }

        if (SpecialEnergyGauge == null) { Debug.Log("SpecialEnergyGauge is null."); return; }

        SpecialEnergyGauge.value = SpecialEnergy;
    }

    public void GainEnergy()
    {
        if (SpecialEnergy >= maxEnergy) { return; }

        SpecialEnergy += HitEnergyRegen;
    }
}