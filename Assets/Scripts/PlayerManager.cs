
/**
 * 플레이어의 특수 에너지를 관리한다.
 * 
 * 에너지의 총량과 에너지 재생량은 인스펙터에서 정의한다.
 * 에너지의 총량까지 매초 EnergyRegen 만큼 재생한다.
 * 에너지 UI에 매초 현재 에너지량을 업데이트한다.
 * GainEnergy() 함수는 몬스터가 데미지를 입었을 때 호출된다.
 */

using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Slider SpecialEnergyGauge;     // HUD의 에너지 게이지 UI

    [Space(10f)]
    public float SpecialEnergy;                       // 에너지의 총량 -> 앱 실행 후엔 현재 에너지 상태
    [SerializeField] private float EnergyRegen;       // 초당 에너지 재생량
    [SerializeField] private float HitEnergyRegen;    // 공격 시 에너지 재생량
    public float EnergyConsumption;                   // 특수공격시 초당 에너지 소비량

    [HideInInspector] public float maxEnergy;
    private float regenCount;

    private void Awake()
    {
        maxEnergy = SpecialEnergy;
        SpecialEnergy = 0f;
        regenCount = 0f;
    }

    private void Start()
    {
        if (SpecialEnergyGauge == null) { Debug.Log("SpecialEnergyGauge is null."); return; }

        SpecialEnergyGauge.maxValue = maxEnergy;
    }

    private void Update()
    {
        // 게임이 시작되지 전엔 실행되지 않음.
        if (!gameManager.isGameStarted) { return; }

        if (SpecialEnergy >= maxEnergy) { SpecialEnergy = maxEnergy; return; }

        regenCount += Time.deltaTime;
        if (regenCount >= 1f)
        {
            SpecialEnergy += EnergyRegen;
            regenCount = 0f;
        }

        SpecialEnergyGauge.value = SpecialEnergy;
    }

    public void GainEnergy()
    {
        if (SpecialEnergy >= maxEnergy) { return; }

        SpecialEnergy += HitEnergyRegen;
    }
}