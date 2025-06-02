using Unity.VisualScripting;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [Header("Patlama Ayarlarý")]
    public GameObject explosionPrefab;
    public AudioClip explosionSound;
    public int segmetsToShrink = 5;
    public float speedReductionPerSegment = 1f;


    private AudioSource audioSource;
    private bool isExploding = false;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isExploding) return;
        if (!other.CompareTag("Player")) return;


        isExploding = true;

        Snake snake = other.GetComponent<Snake>();
        if (snake != null)
        {
            snake.Shrink(segmetsToShrink);

        }
        if(explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }
        if (audioSource != null && explosionSound != null)
        {
            audioSource.PlayOneShot(explosionSound);
        }
        float delay = explosionSound != null ? explosionSound.length : 0.5f;
        Destroy(gameObject, delay); 
    }

      

}