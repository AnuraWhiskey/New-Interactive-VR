using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMonster : MonoBehaviour
{
    public Transform player;  // �÷��̾� ��ġ
    public float detectionRange = 100f;  // ���� �Ÿ�
    public float fieldOfViewAngle = 90f;  // �þ߰�
    public float memoryDuration = 5f; // ������ ��ġ ��� �ð� (��)

    private NavMeshAgent _agent;
    private bool LockOn = false;  // �÷��̾� ���� ����
    private Vector3 lastKnownPosition;  // ������ ���� ��ġ
    private float lastSeenTime; // ������ ���� �ð�

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        lastKnownPosition = transform.position;  // �ʱ� ��ġ ����
    }

    void Update()
    {
        DetectPlayer();

        if (LockOn)
        {
            // �÷��̾ ������ ���, �÷��̾� ��ġ�� �̵�
            _agent.SetDestination(player.position);
            lastKnownPosition = player.position;
            lastSeenTime = Time.time;  // ���������� �� �ð� ����
        }
        else
        {
            // �÷��̾ �������� ���� ���, ������ ��ġ�� �̵�
            if (Time.time - lastSeenTime <= memoryDuration)
            {
                _agent.SetDestination(lastKnownPosition);
            }
            else
            {
                _agent.ResetPath();  // ��� �ð� �ʰ� �� ���� ����
            }
        }
    }

    void DetectPlayer()
    {
        // �÷��̾���� �Ÿ� ���
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            // �÷��̾� ���� ���� ��� (Y�� ����)
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            directionToPlayer.y = 0;
            Vector3 forward = transform.forward;
            forward.y = 0;

            // �þ߰� ���� �ִ��� Ȯ��
            float angleToPlayer = Vector3.Angle(forward, directionToPlayer);

            if (angleToPlayer <= fieldOfViewAngle / 2)
            {
                // Raycast�� ��ֹ� Ȯ��
                RaycastHit hit;
                if (Physics.Raycast(transform.position + Vector3.up * 1.0f, directionToPlayer, out hit, detectionRange))
                {
                    if (hit.transform == player)
                    {
                        LockOn = true;  // �÷��̾� ����
                        return;
                    }
                }
            }
        }
        LockOn = false;  // �������� ������ false ����
    }
}