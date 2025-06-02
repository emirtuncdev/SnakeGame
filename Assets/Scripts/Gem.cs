using UnityEngine;

public class Gem : MonoBehaviour
{
    public int scoreValue = 10; // Gem'in puan deðeri
    public AudioClip gemSound;
    AudioSource audioSource;
    private bool isCollected = false; // Gem'in toplanýp toplanmadýðýný kontrol etmek için
    private Snake snake;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
       

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isCollected) return;
        if (!other.CompareTag("Player")) return;

        isCollected = true; // Gem toplandý
        ScoreManager.Instance.AddScore(scoreValue); // Puan ekle
        Snake snake = other.GetComponent<Snake>();
        if (snake != null)
        {
            // Örneðin: Gem yendiðinde 3 segment eklensin
            snake.GrowMultiple(5);
        }


        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false; // Gem'in collider'ýný devre dýþý býrak
        }
        if (audioSource != null && gemSound != null)
        {
            audioSource.PlayOneShot(gemSound); // Ses çal
        }

        Destroy(gameObject, gemSound.length); // Gem'i yok et, sesin bitmesi için kýsa bir süre bekle
    }

}