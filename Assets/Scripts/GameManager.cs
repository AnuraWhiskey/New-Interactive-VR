
/**
 * 게임 시작 버튼을 누를 때까지 플레이어의 이동과 상호작용을 무효로 한다.
 * 
 * 게임 시작 함수는 타이틀 UI의 버튼이 호출한다.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private MonsterManager monsterManager;
    [SerializeField] private GameObject XROrigin;

    [Space(10f)]
    [SerializeField] private float PlayerMoveSpeed;
    [SerializeField] private float PlayerTurnSpeed;

    [HideInInspector] public bool isGameStarted = false;
    private ActionBasedContinuousMoveProvider moveProvider;
    private ActionBasedContinuousTurnProvider turnProvider;
    private XRDirectInteractor[] directInteractors;

    private void Start()
    {
        moveProvider = XROrigin.GetComponentInChildren<ActionBasedContinuousMoveProvider>();
        turnProvider = XROrigin.GetComponentInChildren<ActionBasedContinuousTurnProvider>();
        directInteractors = XROrigin.GetComponentsInChildren<XRDirectInteractor>();

        if (moveProvider == null) { Debug.Log("moveProvider is null."); return; }
        if (turnProvider == null) { Debug.Log("turnProvider is null."); return; }
        if (directInteractors == null) { Debug.Log("XR Direct Interactors are null."); return; }

        foreach (XRDirectInteractor directInteractor in directInteractors)
            directInteractor.enabled = false;
        moveProvider.moveSpeed = 0;
        turnProvider.turnSpeed = 0;
    }

    public void GameStart()
    {
        foreach (XRDirectInteractor directInteractor in directInteractors)
            directInteractor.enabled = true;
        moveProvider.moveSpeed = PlayerMoveSpeed;
        turnProvider.turnSpeed = PlayerTurnSpeed;

        isGameStarted = true;
    }

    private void Update()
    {
        // Monster HP check
        if (monsterManager.MonsterHP > 0) { return; }
        if (!isGameStarted) { return; }
        isGameStarted = false;

        // End game
        foreach (XRDirectInteractor directInteractor in directInteractors)
            directInteractor.enabled = false;
        moveProvider.moveSpeed = 0;
        turnProvider.turnSpeed = 0;

        uiManager.EndGameUI();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
}