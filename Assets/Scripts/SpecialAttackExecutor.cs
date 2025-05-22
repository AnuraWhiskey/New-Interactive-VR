
/**
 * 특수 공격을 실행한다.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttackExecutor : MonoBehaviour
{
    [SerializeField]
    private PlayerManager playerManager;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("SpecialAttckSensor")) { return; }
        
        if (playerManager == null) { Debug.Log("PlayerManager is null."); return; }

        if (playerManager.SpecialEnergy < playerManager.maxEnergy) { return; }

        // TODO: Do special attck
    }
}