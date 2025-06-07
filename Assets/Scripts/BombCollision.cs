// BombCollision.cs
// �Bomb� prefab��n�z�n �zerine bu script�i ekleyin. 
// Inspector�dan gerekli alanlar� ayarlay�n:
//   explosionPrefab: Patlama efekti prefab�� (�r. ParticleSystem prefab��).
//   explosionSound: Patlama sesi (AudioClip).
//   segmentsToShrink: Her vuru�ta ka� segment silinecek? (5 yaz�n.)

using UnityEngine;

public class BombCollision : MonoBehaviour
{
    [Header("Patlama Ayarlar�")]
    public GameObject explosionPrefab;      // Patlama efekti prefab��
    public AudioClip explosionSound;        // Patlama sesi
    public int segmentsToShrink = 5;        // Bombaya �arp�nca ka� segment silinecek?

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

        // 1) Y�lan� SHRINK ile 5 segment azalt
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

        // 3) Bomban�n sprite��n� ve collider��n� devre d��� b�rak
        if (spriteRenderer != null) spriteRenderer.enabled = false;
        if (bombCollider != null) bombCollider.enabled = false;

        // 4) Ses uzunlu�u kadar bekleyip bombay� yok et
        float delay = (explosionSound != null) ? explosionSound.length : 0.5f;
        Destroy(gameObject, delay);
    }
}
