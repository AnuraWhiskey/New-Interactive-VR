
/**
 * 타이틀 UI와 인게임 UI를 전환한다.
 * 
 * 앱 실행 직후 타이틀 UI를 켜고 HUD를 끈다.
 * UI 전환 함수는 타이틀 UI의 버튼이 호출한다.
 */

using UnityEngine;

public class UIManager : MonoBehaviour
{
    public Canvas UITitle;      // 처음 화면 UI 캔버스
    public Canvas HUD;          // 게임 시작 후 HUD 캔버스
    public Canvas GameEnd;      // 게임 끝 캔버스

    private void Start()
    {
        if (UITitle == null) { Debug.Log("UI Title is null."); }
        if (HUD == null) { Debug.Log("UI HUD is null."); }
        if (GameEnd == null) { Debug.Log("GameEnd is null."); }

        if (UITitle != null) { UITitle.gameObject.SetActive(true); }
        if (HUD != null) { HUD.gameObject.SetActive(false); }
        if (GameEnd != null) { GameEnd.gameObject.SetActive(false); }
    }

    public void ActiveGameUI()
    {
        if (UITitle != null) { UITitle.gameObject.SetActive(false); }
        if (HUD != null) { HUD.gameObject.SetActive(true); }
    }

    public void EndGameUI()
    {
        if (HUD != null) { HUD.gameObject.SetActive(false); }
        if (GameEnd != null) { GameEnd.gameObject.SetActive(true); }
    }
}