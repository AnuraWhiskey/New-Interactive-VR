using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMonster : MonoBehaviour
{
    public Transform player;  // 플레이어 위치
    public float detectionRange = 100f;  // 감지 거리
    public float fieldOfViewAngle = 90f;  // 시야각
    public float memoryDuration = 5f; // 마지막 위치 기억 시간 (초)

    private NavMeshAgent _agent;
    private bool LockOn = false;  // 플레이어 감지 여부
    private Vector3 lastKnownPosition;  // 마지막 감지 위치
    private float lastSeenTime; // 마지막 감지 시간

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        lastKnownPosition = transform.position;  // 초기 위치 설정
    }

    void Update()
    {
        DetectPlayer();

        if (LockOn)
        {
            // 플레이어가 감지된 경우, 플레이어 위치로 이동
            _agent.SetDestination(player.position);
            lastKnownPosition = player.position;
            lastSeenTime = Time.time;  // 마지막으로 본 시간 갱신
        }
        else
        {
            // 플레이어가 감지되지 않은 경우, 마지막 위치로 이동
            if (Time.time - lastSeenTime <= memoryDuration)
            {
                _agent.SetDestination(lastKnownPosition);
            }
            else
            {
                _agent.ResetPath();  // 기억 시간 초과 시 추적 중지
            }
        }
    }

    void DetectPlayer()
    {
        // 플레이어와의 거리 계산
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            // 플레이어 방향 벡터 계산 (Y축 고정)
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            directionToPlayer.y = 0;
            Vector3 forward = transform.forward;
            forward.y = 0;

            // 시야각 내에 있는지 확인
            float angleToPlayer = Vector3.Angle(forward, directionToPlayer);

            if (angleToPlayer <= fieldOfViewAngle / 2)
            {
                // Raycast로 장애물 확인
                RaycastHit hit;
                if (Physics.Raycast(transform.position + Vector3.up * 1.0f, directionToPlayer, out hit, detectionRange))
                {
                    if (hit.transform == player)
                    {
                        LockOn = true;  // 플레이어 감지
                        return;
                    }
                }
            }
        }
        LockOn = false;  // 감지되지 않으면 false 설정
    }
}