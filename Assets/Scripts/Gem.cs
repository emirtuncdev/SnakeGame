using UnityEngine;

public class Gem : MonoBehaviour
{
    public int scoreValue = 10; // Gem'in puan de�eri
    public AudioClip gemSound;
    AudioSource audioSource;
    private bool isCollected = false; // Gem'in toplan�p toplanmad���n� kontrol etmek i�in
    private Snake snake;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
       

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isCollected) return;
        if (!other.CompareTag("Player")) return;

        isCollected = true; // Gem topland�
        ScoreManager.Instance.AddScore(scoreValue); // Puan ekle
        Snake snake = other.GetComponent<Snake>();
        if (snake != null)
        {
            // �rne�in: Gem yendi�inde 3 segment eklensin
            snake.GrowMultiple(5);
        }


        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false; // Gem'in collider'�n� devre d��� b�rak
        }
        if (audioSource != null && gemSound != null)
        {
            audioSource.PlayOneShot(gemSound); // Ses �al
        }

        Destroy(gameObject, gemSound.length); // Gem'i yok et, sesin bitmesi i�in k�sa bir s�re bekle
    }

}