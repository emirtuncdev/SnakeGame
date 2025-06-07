// BombCollision.cs
// “Bomb” prefab’ýnýzýn üzerine bu script’i ekleyin. 
// Inspector’dan gerekli alanlarý ayarlayýn:
//   explosionPrefab: Patlama efekti prefab’ý (ör. ParticleSystem prefab’ý).
//   explosionSound: Patlama sesi (AudioClip).
//   segmentsToShrink: Her vuruþta kaç segment silinecek? (5 yazýn.)

using UnityEngine;

public class BombCollision : MonoBehaviour
{
    [Header("Patlama Ayarlarý")]
    public GameObject explosionPrefab;      // Patlama efekti prefab’ý
    public AudioClip explosionSound;        // Patlama sesi
    public int segmentsToShrink = 5;        // Bombaya çarpýnca kaç segment silinecek?

    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;
    private Collider2D bombCollider;
    private bool hasExploded = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        bombCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasExploded) return;
        if (!other.CompareTag("Player")) return;

        hasExploded = true;

        // 1) Yýlaný SHRINK ile 5 segment azalt
        Snake snake = other.GetComponent<Snake>();
        if (snake != null)
        {
            snake.Shrink(segmentsToShrink);
        }

        // 2) Patlama efekti ve sesi
        if (explosionPrefab != null)
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        if (audioSource != null && explosionSound != null)
            audioSource.PlayOneShot(explosionSound);

        // 3) Bombanýn sprite’ýný ve collider’ýný devre dýþý býrak
        if (spriteRenderer != null) spriteRenderer.enabled = false;
        if (bombCollider != null) bombCollider.enabled = false;

        // 4) Ses uzunluðu kadar bekleyip bombayý yok et
        float delay = (explosionSound != null) ? explosionSound.length : 0.5f;
        Destroy(gameObject, delay);
    }
}
