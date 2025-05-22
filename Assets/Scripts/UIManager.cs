
/**
 * 타이틀 UI와 인게임 UI를 전환한다.
 * 전환 함수는 타이틀 UI의 버튼이 호출한다.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Canvas UITitle;
    public Canvas HUD;

    private void Start()
    {
        if (UITitle == null) { Debug.Log("UI Title is null."); }
        if (HUD == null) { Debug.Log("UI HUD is null."); }

        if (UITitle != null) { UITitle.gameObject.SetActive(true); }
        if (HUD != null) { HUD.gameObject.SetActive(false); }
    }

    public void ActiveGameUI()
    {
        if (UITitle != null) { UITitle.gameObject.SetActive(false); }
        if (HUD != null) { HUD.gameObject.SetActive(true); }
    }
}