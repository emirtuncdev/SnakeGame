// yem.cs
using UnityEngine;

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

    public void RandomizeSprite()
    {
        if (fruitSprites != null && fruitSprites.Length > 0)
        {
            int index = Random.Range(0, fruitSprites.Length);
            spiriteRenderer.sprite = fruitSprites[index];
        }
        else
        {
            Debug.LogWarning("fruitSprites dizisi boş! Rastgele sprite atayamadık.");
        }
    }

    public void RandomizePosition()
    {
        Bounds bounds = gridArea.bounds;
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);
        transform.position = new Vector3(x, y, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            RandomizePosition();
            RandomizeSprite();
        }
    }
}

