using UnityEngine;
using System.Collections;

public class Bomba : MonoBehaviour
{
    [Header("Bomba ayarları")]
    public GameObject bombPrefab;
    public BoxCollider2D gridArea;
    [Tooltip("Bomba her x saniyede bir spawn olsun")]
    public float spawnInterval = 5f;
    [Tooltip("Bomba ne kadar süre sonra yok olsun (0 ise kalıcı)")]
    public float bombLifetime = 4f;

    [Header("Seviye Ayarları")]
    [Tooltip("Her seviye arası geçen süre (saniye)")]
    public float levelDuration = 5f;

    private int currentLevel = 1;
    private Coroutine spawnRoutine;
    private Coroutine levelRoutine;

    private void Start()
    {
        if (bombPrefab == null || gridArea == null || spawnInterval <= 0f)
        {
            Debug.LogWarning("Bomba: bombPrefab veya gridArea eksik, ya da spawnInterval ≤ 0.");
            enabled = false;
            return;
        }

       
        spawnRoutine = StartCoroutine(SpawnRoutine());
        levelRoutine = StartCoroutine(LevelRoutine());
    }

 
    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            for (int i = 0; i < currentLevel; i++)
                SpawnBombAtRandomPosition();
        }
    }

   
    private IEnumerator LevelRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(levelDuration);
            currentLevel++;
        }
    }

    private void SpawnBombAtRandomPosition()
    {
        Bounds b = gridArea.bounds;
        float x = Random.Range(b.min.x, b.max.x);
        float y = Random.Range(b.min.y, b.max.y);
        Vector2 spawnPosition = new Vector2(x, y);

        GameObject bomb = Instantiate(bombPrefab, spawnPosition, Quaternion.identity);
        if (bombLifetime > 0f)
            Destroy(bomb, bombLifetime);
    }
}
