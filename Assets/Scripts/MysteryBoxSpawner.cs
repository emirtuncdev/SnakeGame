using System.Collections;
using UnityEngine;

public class MysteryBoxSpawner : MonoBehaviour
{
    [Header("Prefab ve Spawn Ayarlarý")]
    public GameObject boxPrefab;       // Inspector’dan: MysteryBox prefab’ýný sürükleyin
    public BoxCollider2D gridArea;     // Inspector’dan: Spawn alanýný tanýmlayan BoxCollider2D
    public float respawnDelay = 700f;    // Kaç saniye sonra yeni bir sandýk spawn edilsin

    private void Start()
    {
        
        SpawnAfterDelay();
        // Ya da direkt SpawnBox() diyebilirsin. Fakat respawnDelay’ýn beklemesini istiyorsan:
        // StartCoroutine( SpawnRoutine() );
    }

    public void SpawnAfterDelay()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        // 1) Belirlenen süre kadar bekle
        yield return new WaitForSeconds(respawnDelay);

        // 2) gridArea.bounds içinde rastgele pozisyon hesapla
        Bounds b = this.gridArea.bounds;
        float x = Random.Range(b.min.x, b.max.x);
        float y = Random.Range(b.min.y, b.max.y);
        Vector3 spawnPos = new Vector3(x, y, 0f);

        // 3) Yeni bir MysteryBox instantiate et
        GameObject newBox = Instantiate(boxPrefab, spawnPos, Quaternion.identity);

        // 4) Yeni kutunun “spawner” referansýný kendimize (this) atayalým
        MysteryBox newBoxScript = newBox.GetComponent<MysteryBox>();
        if (newBoxScript != null)
            newBoxScript.spawner = this;
    }
}
