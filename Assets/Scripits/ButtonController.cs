using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
public class ButtonController : MonoBehaviour
{
    public Button button;
    public GameObject stars;
    public TMP_Text levelNumber;
    private void OnEnable()
    {
        Int32.TryParse(levelNumber.text,out int levelNum);
        Debug.Log(levelNum);
        if (PlayerPrefs.HasKey(("LevelDone") + levelNum))
        {
            stars.SetActive(true);
        }
        if (levelNum==1 || PlayerPrefs.HasKey(("LevelDone") + (levelNum-1)))
        {
            button.interactable = true;
        }
    }
}
