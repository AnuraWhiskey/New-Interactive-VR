
/**
 * 몬스터가 데미지를 받는 OnCollision 함수를 실행한다.
 * 
 * 충격량이 MonsterManager에서 설정한 최소 충격량보다 작으면 실행하지 않는다.
 * MonsterManager의 Damaged(string) 함수를 호출한다.
 */

using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] private MonsterManager monsterManager;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Monster impulse: {collision.impulse.magnitude} from {collision.collider.tag}");   // 디버그: 몬스터가 받은 충격량.

        if (monsterManager == null) { Debug.Log("MonsterManager is null."); return; }

        if (collision.impulse.magnitude < monsterManager.MinimumImpulse) { return; }

        if (collision.collider.CompareTag("Punch"))
        {
            monsterManager.Damaged("Punch");
            return;
        }

        if (collision.collider.CompareTag("Interactable"))
        {
            monsterManager.Damaged("Interactable");
            return;
        }
    }
}