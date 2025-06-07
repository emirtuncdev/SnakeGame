using UnityEngine;

public class Gem : MonoBehaviour
{
    public int scoreValue = 0;
    public AudioClip gemSound;
    AudioSource audioSource;
    private bool isCollected = false;
    

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();


    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isCollected) return;
        if (!other.CompareTag("Player")) return;

        isCollected = true;
        ScoreManager.Instance.AddScore(scoreValue);
        Snake snake = other.GetComponent<Snake>();
        if (snake != null)
        {
            snake.GrowMultiple(5);
        }


        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false;
        }
        if (audioSource != null && gemSound != null)
        {
            audioSource.PlayOneShot(gemSound);
        }

        Destroy(gameObject, gemSound.length);
    }

}