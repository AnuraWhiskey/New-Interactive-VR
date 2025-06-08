
/**
 * 특수 공격을 실행한다.
 * 
 * 센서의 위치가 서로 충분히 가까울 때 실행한다.
 * && 스페셜 에너지가 최대일 때 실행한다.
 */

using System.Collections;
using UnityEngine;

public class SpecialAttackExecutor : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private Transform RightSpecialAttackSensor;

    [SerializeField] private ParticleSystem EnergyBeam;
    [SerializeField] private ParticleSystem EnergyCharge;

    [Space(10f)]
    [SerializeField] private float ChargeDistance;  // 차지하기 위해 센서의 거리가 얼마나 가까워야하는지
    [SerializeField] private float ChargeStartTime; // 차지를 시작하기 위해 콜라이더를 겹치고 있어야하는 시간
    [SerializeField] private float ChargeTime;      // 빔을 쏘기 위해 콜라이더를 겹치고 있어야하는 시간, 1초 단위로 측정
    [SerializeField] private int MinEnergy;         // 빔을 쏘기 위해 필요한 최소 에너지량

    private bool isCharging = false;                // 현재 차지 또는 특수공격 중인지 여부
    private float sensorDistance = 0;               // 센서 사이의 거리

    private void Start()
    {
        if (playerManager == null) { Debug.Log("PlayerManager is null."); return; }
        if (EnergyBeam == null) { Debug.Log("EnergyBeam is null."); return; }
        if (EnergyCharge == null) { Debug.Log("EnergyCharge is null."); return; }

        EnergyBeam.gameObject.SetActive(false);
        EnergyCharge.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!gameManager.isGameStarted) { return; }

        sensorDistance = Vector3.Distance(transform.position, RightSpecialAttackSensor.position);
        if (sensorDistance > ChargeDistance) { return; }
        if (playerManager.SpecialEnergy < MinEnergy) { return; }

        // Do charge
        if (!isCharging)
        {
            StartCoroutine(SpecialAttack());
            isCharging = true;
        }
    }

    private IEnumerator SpecialAttack()
    {
        // Charge

        float _time = 0;

        yield return new WaitForSeconds(ChargeStartTime);
        if (sensorDistance > ChargeDistance) { isCharging = false; yield break; }
        
        EnergyCharge.gameObject.SetActive(true);
        EnergyCharge.Stop();
        EnergyCharge.Play();

        while (_time < ChargeTime)
        {
            yield return new WaitForSeconds(1f);
            _time += 1f;

            if (sensorDistance > ChargeDistance)
            {
                EnergyCharge.gameObject.SetActive(false);
                isCharging = false;
                yield break;
            }

            EnergyCharge.Stop();
            EnergyCharge.Play();
        }

        EnergyCharge.gameObject.SetActive(false);

        // Beam

        EnergyBeam.gameObject.SetActive(true);
        EnergyBeam.Stop();
        EnergyBeam.Play();

        while (playerManager.SpecialEnergy > 0)
        {
            if (sensorDistance > ChargeDistance)
            {
                EnergyBeam.gameObject.SetActive(false);
                isCharging = false;
                yield break;
            }

            playerManager.SpecialEnergy -= (Time.deltaTime * playerManager.EnergyConsumption);
            yield return null;
        }

        EnergyBeam.gameObject.SetActive(false);
        isCharging = false;
    }
}