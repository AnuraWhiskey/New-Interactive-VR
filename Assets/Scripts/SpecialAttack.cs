
/**
 * 특수공격 콜라이더에 닿은 건물을 파괴한다.
 * 특수공격에 닿은 몬스터에게 데미지를 준다.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttack : MonoBehaviour
{
    [SerializeField] private MonsterManager monsterManager;
    public float MaxDistance;

    private Ray ray;

    private void Start()
    {
        if (monsterManager == null) { Debug.Log("MonsterManager is null."); return; }
    }

    private void Update()
    {

        ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out RaycastHit _hit, MaxDistance))
        {
            if (_hit.collider.CompareTag("Building"))
            {
                RayFire.RayfireRigid _rayfireRigid = _hit.collider.GetComponent<RayFire.RayfireRigid>();
                _rayfireRigid.Demolish();

                return;
            }

            if (_hit.collider.CompareTag("Monster"))
            {
                monsterManager.DamagedBySpecialAttack();

                return;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 _origin = transform.position;
        Vector3 _direction = transform.forward;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(_origin, _origin + _direction * MaxDistance);
    }
}