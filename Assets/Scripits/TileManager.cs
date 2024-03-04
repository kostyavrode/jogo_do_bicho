using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TileManager : MonoBehaviour
{
    public static Action<Tile> onTileSelected;
    public int levelNumber;
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private List<Tile> selectedTiles = new List<Tile>();
    [SerializeField] private Tile[] allTiles;
    [SerializeField] private Transform level;
    private bool isAllTilesOpened;
    private void Awake()
    {
        onTileSelected += SelectTile;
    }
    private void OnDisable()
    {
        onTileSelected -= SelectTile;
    }
    private void Start()
    {
        FindAllTiles();
        StartShowTiles();
    }
    public void FindAllTiles()
    {
        allTiles = level.GetComponentsInChildren<Tile>();
        
    }
    private void SelectTile(Tile newTile)
    {
        selectedTiles.Add(newTile);
        CheckTiles();
    }
    private void ClearArray()
    {
        foreach(Tile tile in selectedTiles)
        {
            tile.FlipTile();
        }
        selectedTiles.Clear();
    }
    private void CheckTiles()
    {
        for (int i=0;i<selectedTiles.Count;i++)
        {
            if (selectedTiles[i].GetHashCode()==allTiles[i].GetHashCode())
            {
                if (allTiles.Length==selectedTiles.Count)
                {
                    LevelCompleted();
                }
            }
            else
            {
                ClearArray();
                Debug.Log("ClearArray");
            }
        }
    }
    private void StartShowTiles()
    {
        for (int i=0;i<allTiles.Length;i++)
        {
            StartCoroutine(WaitForShowTiles(i, allTiles[i]));
        }
    }
    private void LevelCompleted()
    {
        Debug.Log("level completed");
        if (!PlayerPrefs.HasKey("LevelDone" + levelNumber))
        {
            PlayerPrefs.SetString("LevelDone" + levelNumber, "true");
            PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + 3);
            PlayerPrefs.Save();
        }
        GameManager.instance.EndGame(true);
    }
    private IEnumerator WaitForUnSelectTile(Tile tile)
    {
        yield return new WaitForSeconds(0.3f);
        tile.UnSelectTile();
    }
    private IEnumerator WaitForShowTiles(int x,Tile tile)
    {
        yield return new WaitForSeconds(0.3f * x);
        tile.FlipTile();
    }
}
