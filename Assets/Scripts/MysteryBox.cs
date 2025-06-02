using System.Collections;
using UnityEngine;

public class MysteryBox : MonoBehaviour
{
    [Header("Animasyon & Ses")]
    public Animator animasyon;               // Inspector’dan atanacak Animator
    public AudioClip openBoxSound;           // (Opsiyonel) açılma sesi
    public float openAnimationDuration = 0.7f; // “Boxx_Open” animasyonu süresi

    public float gemLifetime = 5f;

    [Header("Timeout Ayarı")]
    [Tooltip("Yılan dokunmazsa bu süre sonra kutu yok olacak")]
    public float idleLifetime = 10f;         // Spawn olduktan sonra kaç saniye bekleyip yok olacak

    [Header("Spawner Referansı")]
    [HideInInspector] public MysteryBoxSpawner spawner;

    [Header("İçinden Çıkacak Obje")]
    public GameObject gemPrefab;             // Gem prefab’ı
    public GameObject bombPrefab;            // Bomba prefab’ı

    private Coroutine lifeCoroutine;
    private AudioSource audioSource;
    private bool isOpen = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if (animasyon != null)
        {
            animasyon.Play("Boxx_Close", 0, 0f);
            animasyon.Update(0f);
        }
    }

    private void OnEnable()
    {
        lifeCoroutine = StartCoroutine(IdleTimeoutRoutine());
    }

    private void OnDisable()
    {
        if (lifeCoroutine != null)
        {
            StopCoroutine(lifeCoroutine);
            lifeCoroutine = null;
        }
    }

    private IEnumerator IdleTimeoutRoutine()
    {
        yield return new WaitForSeconds(idleLifetime);
        if (!isOpen)
        {
            if (spawner != null)
                spawner.SpawnAfterDelay();
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isOpen) return;
        if (!other.CompareTag("Player")) return;

        isOpen = true;

        if (lifeCoroutine != null)
        {
            StopCoroutine(lifeCoroutine);
            lifeCoroutine = null;
        }

        if (animasyon != null)
            animasyon.SetTrigger("Open");

        if (openBoxSound != null && audioSource != null)
            audioSource.PlayOneShot(openBoxSound);

        StartCoroutine(HandleOpenSpawnGemOrBomb());
    }

    private IEnumerator HandleOpenSpawnGemOrBomb()
    {
        Vector3 spawnPos = transform.position;
        float roll = Random.value;
        bool isBoom = false;
        if (roll < 0.40f && gemPrefab != null)
        {

        }
        else if (bombPrefab != null)
        {
            isBoom = true;

        }


        if (!isBoom && gemPrefab != null)
        {
            yield return new WaitForSeconds(openAnimationDuration);
            yield return new WaitForSeconds(0.2f);
            GameObject gemInstance = Instantiate(gemPrefab, spawnPos, Quaternion.identity);
            Destroy(gemInstance, gemLifetime);
        }
        else if (bombPrefab != null)
        {
            Instantiate(bombPrefab, spawnPos, Quaternion.identity);
        }

        Destroy(gameObject);
        if (spawner != null)
            spawner.SpawnAfterDelay();
    }
}
