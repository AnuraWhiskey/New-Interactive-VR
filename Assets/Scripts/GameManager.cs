
/**
 * 게임 시작 버튼을 누를 때까지 플레이어의 이동을 무효로 한다.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject XROrigin;

    public bool isGameStarted = false;

    private LocomotionSystem locomotionSystem;
    private XRDirectInteractor[] directInteractors;

    private void Start()
    {
        locomotionSystem = XROrigin.GetComponentInChildren<LocomotionSystem>();
        directInteractors = XROrigin.GetComponentsInChildren<XRDirectInteractor>();

        if (locomotionSystem == null) { Debug.Log("Locomotion System is null."); return; }
        if (directInteractors == null) { Debug.Log("XR Direct Interactors are null."); return; }

        foreach (XRDirectInteractor directInteractor in directInteractors)
            directInteractor.enabled = false;
        locomotionSystem.enabled = false;
    }

    public void GameStart()
    {
        if (locomotionSystem == null) { Debug.Log("Locomotion System is null."); return; }
        if (directInteractors == null) { Debug.Log("XR Direct Interactors are null."); return; }

        foreach (XRDirectInteractor directInteractor in directInteractors)
            directInteractor.enabled = true;
        locomotionSystem.enabled = true;
    }
}