using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
public class Tile : MonoBehaviour
{
    public int type;
    public List<Tile> neighbours=new List<Tile>();
    public SpriteRenderer sprite;
    public Sprite[] sprites;
    public bool isCanInteract = true;
    private bool isSelected;
    private BoingButton boingButton;
    private void Awake()
    {
        boingButton = GetComponent<BoingButton>();
        SetSprite();
    }
    private void Start()
    {
        Collider2D[] colliders=Physics2D.OverlapCircleAll(new Vector2(transform.position.x,transform.position.y), 0.5f);
        Debug.Log(colliders.Length);
        foreach(Collider2D coll in colliders)
        {
            neighbours.Add(coll.GetComponent<Tile>());
        }
    }
    private void SetSprite()
    {
        sprite.sprite = sprites[type];
    }
    private void LateUpdate()
    {
        if (isCanInteract)
        {
            sprite.color = Color.white;
        }
        else
        {
            sprite.color = Color.grey;
        }
    }
    private void OnMouseDown()
    {
        if (isCanInteract /*&& !isSelected*/)
        {
            isSelected = true;
            //boingButton.UpScaleTile();
            ChooseTile();

            //TileManager.onTileSelected?.Invoke(this);
        }
    }
    public void FlipTile()
    {
        if (transform.rotation.y==0)
        {
            transform.DORotate(transform.rotation.eulerAngles + new Vector3(0, 180, 0), 0.3f);
        }
        else
        {
            transform.DORotate(Vector3.zero, 0.3f).OnComplete(CallToTileManager);
        }
        
    }
    private void CallToTileManager()
    {
        TileManager.onTileSelected?.Invoke(this);
    }
    public void UnSelectTile()
    {
        boingButton.DownScaleTile();
        isSelected = false;
    }
    public void ShowNeighbours()
    {
        foreach (Tile neighbor in neighbours)
        {
            neighbor.isCanInteract = true;
        }
    }
    private void ChooseTile()
    {
        FlipTile();
    }
}
