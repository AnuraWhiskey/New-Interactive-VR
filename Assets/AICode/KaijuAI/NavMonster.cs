using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMonster : MonoBehaviour
{
    public Transform player;  // 플레이어 위치
    public float detectionRange = 150f;  // 감지 거리
    public float fieldOfViewAngle = 180f;  // 시야각
    public float memoryDuration = 5f; // 마지막 위치 기억 시간 (초)

    public float buildingAttackRange = 15f;  // 건물 공격 범위
    public float meleeAttackRange = 15f;  // 근접 공격 범위
    public float meleeAttackCooldown = 5f; // 근접 공격 쿨타임

    [Header("Breath Attack")]
    public float breathTriggerDistance = 100f;
    public float breathCooldown = 25f;
    public GameObject breathEffectPrefab;
    public Transform breathSpawnPoint;

    private float lastBreathTime = -Mathf.Infinity;
    private bool isBreathing = false;

    private float lastMeleeAttackTime = -Mathf.Infinity;

    private NavMeshAgent _agent;
    private bool LockOn = false;  // 플레이어 감지 여부
    private Vector3 lastKnownPosition;  // 마지막 감지 위치
    private float lastSeenTime; // 마지막 감지 시간

    private Transform targetBuilding; // 타겟 건물
    private bool isAttackingBuilding = false;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        lastKnownPosition = transform.position;  // 초기 위치 설정
    }

    void Update()
    {
        DetectPlayer();

        if (isBreathing) return;

        if (LockOn)
        {
            float distance = Vector3.Distance(transform.position, player.position);

            // 브레스 발사 조건: 추적 범위 이내 && 브레스 범위 이상 && 쿨타임 완료
            if (distance <= detectionRange && distance > breathTriggerDistance && Time.time > lastBreathTime + breathCooldown)
            {
                StartCoroutine(BreathAttack());
                return;
            }
            else if (distance <= meleeAttackRange && Time.time > lastMeleeAttackTime + meleeAttackCooldown)
            {
                MeleeAttack(player);
                return;
            }

            _agent.isStopped = false;
            _agent.SetDestination(player.position);
            lastKnownPosition = player.position;
            lastSeenTime = Time.time;
            targetBuilding = null;
            isAttackingBuilding = false;
        }
        else if (Time.time - lastSeenTime <= memoryDuration)
        {
            _agent.isStopped = false;
            _agent.SetDestination(lastKnownPosition);
            targetBuilding = null;
            isAttackingBuilding = false;
        }
        else
        {
            if (targetBuilding == null)
            {
                targetBuilding = FindNearestBuilding();
                if (targetBuilding != null)
                {
                    _agent.SetDestination(targetBuilding.position);
                }
            }
            else
            {
                float dist = Vector3.Distance(transform.position, targetBuilding.position);
                if (dist <= meleeAttackRange && Time.time > lastMeleeAttackTime + meleeAttackCooldown)
                {
                    _agent.ResetPath();
                    isAttackingBuilding = true;
                    MeleeAttack(targetBuilding);
                    return;
                }
                else if (dist > meleeAttackRange)
                {
                    _agent.SetDestination(targetBuilding.position);
                }
            }
        }

        if (isAttackingBuilding && targetBuilding != null && !LockOn)
        {
            transform.LookAt(new Vector3(targetBuilding.position.x, transform.position.y, targetBuilding.position.z));
        }
    }

    void DetectPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            directionToPlayer.y = 0;
            Vector3 forward = transform.forward;
            forward.y = 0;

            float angleToPlayer = Vector3.Angle(forward, directionToPlayer);

            if (angleToPlayer <= fieldOfViewAngle / 2)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position + Vector3.up * 1.0f, directionToPlayer, out hit, detectionRange))
                {
                    if (hit.transform == player)
                    {
                        LockOn = true;
                        return;
                    }
                }
            }
        }

        LockOn = false;
    }

    Transform FindNearestBuilding()
    {
        GameObject[] buildings = GameObject.FindGameObjectsWithTag("Wall");
        Transform nearest = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject building in buildings)
        {
            float dist = Vector3.Distance(transform.position, building.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                nearest = building.transform;
            }
        }

        return nearest;
    }

    private IEnumerator BreathAttack()
    {
        isBreathing = true;
        _agent.isStopped = true;
        lastBreathTime = Time.time;

        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        float rotateDuration = 0.5f;
        float elapsed = 0f;

        while (elapsed < rotateDuration)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, elapsed / rotateDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (breathEffectPrefab != null && breathSpawnPoint != null)
        {
            GameObject breath = Instantiate(breathEffectPrefab, breathSpawnPoint.position, transform.rotation);
            Destroy(breath, 5f);
        }

        Debug.Log("브레스 발사!");

        yield return new WaitForSeconds(5f);

        _agent.isStopped = false;
        isBreathing = false;
    }

    private void MeleeAttack(Transform target)
    {
        lastMeleeAttackTime = Time.time;
        transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
        Debug.Log($"{target.name}에게 근접 공격!");
        // 여기에 데미지 적용 로직 추가 가능
    }
}
