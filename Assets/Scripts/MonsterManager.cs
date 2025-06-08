
/**
 * 몬스터의 HP를 관리한다.
 * 
 * 몬스터의 HP 총량과 받는 데미지량은 인스펙터에서 정의한다.
 * 매 프레임 몬스터의 HP UI를 업데이트한다.
 * 데미지 함수는 Monster 스크립트에서 호출한다.
 * 몬스터가 데미지를 받으면 플레이어의 스페셜 에너지가 증가한다.
 */

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MonsterManager : MonoBehaviour
{
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private Monster monster;

    [Space (10f)]
    [SerializeField] private Slider MonsterHPBar;   // HUD의 몬스터 체력 바 UI
    public float MonsterHP;                           // 몬스터의 최대 HP -> 앱 실행 후엔 현재 HP 상태

    [Space(10f)]
    public float MinimumImpulse;                        // 몬스터가 데미지를 받는 최소 충격량
    [SerializeField] private float DamageByPunch;         // 플레이어의 주먹에서 받는 데미지
    [SerializeField] private float DamageByInteractable;  // 상호작용 가능한 물건에서 받는 데미지
    [SerializeField] private float DamageBySpecialAttack; // 특수공격에게 초당 받는 데미지

    private TMP_Text hpText;
    private float maxHp;

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
        // HP bar updates
        MonsterHPBar.value = MonsterHP;
        hpText.text = $"{((int)MonsterHP)} / {maxHp}";
    }

    public void Damaged(string _tag)
    {
        switch (_tag)
        {
            case "Punch": MonsterHP -= DamageByPunch; break;
            case "Interactable": MonsterHP -= DamageByInteractable; break;
            default: return;
        }

        playerManager.GainEnergy();

        if (MonsterHP <= 0)
        {
            // TODO: Kill Monster, Game End
        }
    }

    public void DamagedBySpecialAttack()
    {
        MonsterHP -= (DamageBySpecialAttack * Time.deltaTime);
    }
}