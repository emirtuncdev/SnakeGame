using UnityEditor;
using UnityEngine;
using TMPro;

public class yem : MonoBehaviour
{
    public BoxCollider2D gridArea;
    public Sprite[] fruitSprites;
    private SpriteRenderer spiriteRenderer;
    private void Awake()
    {
        spiriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        RandomizePosition();
        RandomizeSprite();
    }
    private void RandomizeSprite()
    {
        if (fruitSprites != null && fruitSprites.Length > 0)
        {
            // 0 ... fruitSprites.Length - 1 arasýnda rastgele indeks seç
            int index = Random.Range(0, fruitSprites.Length);
            // Seçilen sprite'ý SpriteRenderer'ýna uygula
            spiriteRenderer.sprite = fruitSprites[index];
        }
        else
        {
            Debug.LogWarning("fruitSprites dizisi boþ! Rastgele sprite atayamadýk.");
        }
    }

    private void RandomizePosition()
    {
        Bounds bounds = this.gridArea.bounds;

        float x=Random.Range(bounds.min.x,bounds.max.x);
        float y= Random.Range(bounds.min.y, bounds.max.y);  

        this.transform.position=new Vector3(x,y,0);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {

            RandomizePosition();
            RandomizeSprite();
        }

    }
}

