using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TileManager[] tileManager; 
    public static GameManager instance;
    public static Action onGameStarted;
    private bool isGameStarted;
    private float currentTimeScale;
    private float score;
    private int money;
    private float localTIme;
    private int lastLoadedLevel;
    private int timeForRound;
    private TileManager lastLoadedTileManager;
    private void Awake()
    {
        timeForRound = 60+PlayerPrefs.GetInt("TimeBonus");
        instance = this;
        currentTimeScale = Time.timeScale;
        if (PlayerPrefs.HasKey("Money"))
        {
            money = PlayerPrefs.GetInt("Money");
        }
        else
        {
            PlayerPrefs.SetInt("Money", 0);
            PlayerPrefs.Save();
        }
        
    }
    private void Start()
    {
        UIManager.instance.ShowMoney(money.ToString());
    }
    private void FixedUpdate()
    {
        if (isGameStarted)
        {
            localTIme += Time.fixedDeltaTime;
            UIManager.instance.ShowScore(60 - localTIme);
            score += Time.fixedDeltaTime;
            if (localTIme>60)
            {
                EndGame(false);
            }
            
        }
    }
    public void StartGame(int level=0)
    {
        timeForRound = 30 + PlayerPrefs.GetInt("TimeBonus");
        isGameStarted = true;
        onGameStarted?.Invoke();
        Time.timeScale = 1f;
        UIManager.instance.ShowScore(60 -localTIme) ;
        if (level==0)
        {
            Destroy(lastLoadedTileManager.gameObject);
            lastLoadedTileManager=Instantiate(tileManager[lastLoadedLevel - 1]);
        }
        else
        {
            lastLoadedTileManager = Instantiate(tileManager[level - 1]);
            lastLoadedLevel = level;
        }
        
    }
    public void StartNextLevel()
    {
        timeForRound = 30 + PlayerPrefs.GetInt("TimeBonus");
        Destroy(lastLoadedTileManager.gameObject);
        isGameStarted = true;
        lastLoadedTileManager = Instantiate(tileManager[lastLoadedLevel]);
        lastLoadedLevel += 1;
        Time.timeScale = 1f;
    }
    public void PauseGame()
    {
        isGameStarted = false;
        Time.timeScale = 0f;
    }
    public void UnPauseGame()
    {
        isGameStarted = true;
        Time.timeScale = currentTimeScale;
    }
    public void EndGame(bool isWin = false)
    {
        isGameStarted = false;
        CheckBestScore();
        UIManager.instance.EndGame(isWin);
        UIManager.instance.ShowBestScore(Mathf.RoundToInt(localTIme).ToString());
    }
    private void CheckBestScore()
    {
        if (PlayerPrefs.HasKey("BestScore"))
        {
            int tempBestScore = PlayerPrefs.GetInt("BestScore");
            if (tempBestScore > score)
            {
                UIManager.instance.ShowBestScore(tempBestScore.ToString());
            }
            else
            {
                UIManager.instance.ShowBestScore(score.ToString());
                //PlayerPrefs.SetInt("BestScore", score);
                //PlayerPrefs.Save();
            }
        }
        else
        {
            UIManager.instance.ShowBestScore(score.ToString());
            //PlayerPrefs.SetInt("BestScore", score);
            //PlayerPrefs.Save();
        }
    }
    public bool IsGameStarted()
    {
        return isGameStarted;
    }
}
