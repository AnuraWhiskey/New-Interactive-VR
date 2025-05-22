
/**
 * 몬스터가 데미지를 받는 OnCollision 함수를 실행한다.
 * MonsterManager의 Damaged(string) 함수를 호출한다.
 * 충격량이 MonsterManager의 최소 충격량보다 작으면 실행하지 않는다.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField]
    private MonsterManager monsterManager;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Monster impulse: {collision.impulse.magnitude}");
        if (collision.impulse.magnitude < monsterManager.MinimumImpulse) { return; }

        if (collision.collider.CompareTag("Punch"))
        {
            monsterManager.Damaged("Punch");
            return;
        }

        if (collision.collider.CompareTag("Interactable"))
        {
            monsterManager.Damaged("Interactable");
        }
    }
}