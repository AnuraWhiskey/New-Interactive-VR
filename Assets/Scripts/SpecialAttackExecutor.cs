
/**
 * 특수 공격을 실행한다.
 * 
 * 센서가 겹쳤을 때 && 스페셜 에너지가 최대일 때 실행한다.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttackExecutor : MonoBehaviour
{
    [SerializeField] private PlayerManager playerManager;

    [SerializeField] private ParticleSystem EnergyBeam;
    [SerializeField] private ParticleSystem EnergyCharge;

    [Space(10f)]
    [SerializeField] private float ChargeStartTime;  // 차지를 시작하기 위해 콜라이더를 겹치고 있어야하는 시간.
    [SerializeField] private float ChargeTime;      // 빔을 쏘기 위해 콜라이더를 겹치고 있어야하는 시간, 1초 단위로 측정.

    private bool isTriggered = false;               // 현재 콜라이더가 겹쳐져 있는지 여부.

    private void Start()
    {
        if (EnergyBeam == null) { Debug.Log("EnergyBeam is null."); return; }
        if (EnergyCharge == null) { Debug.Log("EnergyCharge is null."); return; }

        EnergyBeam.gameObject.SetActive(false);
        EnergyCharge.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("SpecialAttackSensor")) { return; }
        
        if (playerManager == null) { Debug.Log("PlayerManager is null."); return; }

        //if (playerManager.SpecialEnergy < playerManager.maxEnergy) { return; }

        isTriggered = true;

        StartCoroutine(SpecialAttack());
    }

    private void OnTriggerExit(Collider other)
    {
        isTriggered = false;
    }

    private IEnumerator SpecialAttack()
    {
        // Charge

        float _time = 0;

        yield return new WaitForSeconds(ChargeStartTime);
        if (!isTriggered) { yield break; }
        
        EnergyCharge.gameObject.SetActive(true);
        EnergyCharge.Stop();
        EnergyCharge.Play();

        while (_time < ChargeTime)
        {
            yield return new WaitForSeconds(1f);
            if (!isTriggered)
            {
                EnergyCharge.gameObject.SetActive(false);
                yield break;
            }

            EnergyCharge.Stop();
            EnergyCharge.Play();

            _time += 1f;
        }

        EnergyCharge.gameObject.SetActive(false);

        // Beam

        EnergyBeam.gameObject.SetActive(true);
        EnergyBeam.Stop();
        EnergyBeam.Play();
        // TODO?: Beam Settings

        while (isTriggered)
        {
            yield return null;
        }

        EnergyBeam.gameObject.SetActive(false);
    }
}