using System.Collections;
using UnityEngine;

public class MysteryBox : MonoBehaviour
{
    [Header("Animasyon & Ses")]
    public Animator animasyon;           // Inspector’dan atanacak Animator
    public AudioClip openBoxSound;       // (Opsiyonel) açılma sesi
    public float openAnimationDuration = 0.7f; // “Boxx_Open” animasyonu süresi

    public float gemLifetime = 5f;

    [Header("Timeout Ayarı")]
    [Tooltip("Yılan dokunmazsa bu süre sonra kutu yok olacak")]
    public float idleLifetime = 10f;     // Kaç saniye bekleyip kendiliğinden yok olacak

    [Header("Spawner Referansı")]
    [Tooltip("Spawner referansını, SpawnRoutine() içinde atanacak")]
    [HideInInspector] public MysteryBoxSpawner spawner;

    [Header("İçinden Çıkacak Obje")]
    public GameObject gemPrefab;         // Açılınca çıkacak mücevher prefab’ı

    // İçeride coroutine referansı
    private Coroutine lifeCoroutine;
    private AudioSource audioSource;
    private bool isOpen = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        // Oyun başladığında kesin “Boxx_Close” state’inde başla:
        if (animasyon != null)
        {
            animasyon.Play("Boxx_Close", 0, 0f);
            animasyon.Update(0f);
        }
    }

    private void OnEnable()
    {
        // Her etkinleştiğinde (spawn edildiği anda) bir "idle timeout" coroutine’i başlat
        lifeCoroutine = StartCoroutine(IdleTimeoutRoutine());
    }

    private void OnDisable()
    {
        // Disable olduğunda coroutine varsa durdur
        if (lifeCoroutine != null)
        {
            StopCoroutine(lifeCoroutine);
            lifeCoroutine = null;
        }
    }

    private IEnumerator IdleTimeoutRoutine()
    {
        // 1) idleLifetime kadar bekle
        yield return new WaitForSeconds(idleLifetime);

        // 2) Süre doldu—eğer hala açılmamışsa, kendini yok et ve spawner’a haber ver:
        if (!isOpen)
        {
            if (spawner != null)
            {
                spawner.SpawnAfterDelay();
            }
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Eğer zaten açıldıysa veya dokunan “Player” değilse, çık.
        if (isOpen) return;
        if (!other.CompareTag("Player")) return;

        isOpen = true;

        // 1) Eğer hala beklemekte olan idle coroutine’i varsa durdur
        if (lifeCoroutine != null)
        {
            StopCoroutine(lifeCoroutine);
            lifeCoroutine = null;
        }

        // 2) “Open” trigger’ını ver (Boxx_Close → Boxx_Open)
        if (animasyon != null)
            animasyon.SetTrigger("Open");

        // 3) Açılma sesini çal (opsiyonel)
        if (openBoxSound != null && audioSource != null)
            audioSource.PlayOneShot(openBoxSound);

        // 4) Açılma animasyonu bittiğinde içinden gem çıkacak ve sandık yok olacak
        StartCoroutine(HandleOpenSpawnGemAndDestroy());
    }

    private IEnumerator HandleOpenSpawnGemAndDestroy()
    {
        // 1) Açılma animasyonu süresi kadar bekle
        yield return new WaitForSeconds(openAnimationDuration);
        yield return new WaitForSeconds(0.2f);

        // 2) Kutunun şu anki pozisyonunu al
        Vector3 spawnPos = transform.position;

        // 3) Eğer gemPrefab atanmışsa, o pozisyonda instantiate et
        if (gemPrefab != null)
        {
            GameObject gemInstance = Instantiate(gemPrefab, spawnPos, Quaternion.identity);
            Destroy(gemInstance, gemLifetime); // Burada sahnedeki gemInstance’ı yok et
        }

        // 4) Kutu yok olsun
        Destroy(gameObject);

        // 5) Spawner’a haber ver
        if (spawner != null)
            spawner.SpawnAfterDelay();
    }

}
