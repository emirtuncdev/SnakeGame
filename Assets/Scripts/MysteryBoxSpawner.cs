using System.Collections;
using UnityEngine;

public class MysteryBoxSpawner : MonoBehaviour
{
    [Header("Prefab ve Spawn Ayarlar�")]
    public GameObject boxPrefab;       // Inspector�dan: MysteryBox prefab��n� s�r�kleyin
    public BoxCollider2D gridArea;     // Inspector�dan: Spawn alan�n� tan�mlayan BoxCollider2D
    public float respawnDelay = 700f;    // Ka� saniye sonra yeni bir sand�k spawn edilsin

    private void Start()
    {
        
        SpawnAfterDelay();
        // Ya da direkt SpawnBox() diyebilirsin. Fakat respawnDelay��n beklemesini istiyorsan:
        // StartCoroutine( SpawnRoutine() );
    }

    public void SpawnAfterDelay()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        // 1) Belirlenen s�re kadar bekle
        yield return new WaitForSeconds(respawnDelay);

        // 2) gridArea.bounds i�inde rastgele pozisyon hesapla
        Bounds b = this.gridArea.bounds;
        float x = Random.Range(b.min.x, b.max.x);
        float y = Random.Range(b.min.y, b.max.y);
        Vector3 spawnPos = new Vector3(x, y, 0f);

        // 3) Yeni bir MysteryBox instantiate et
        GameObject newBox = Instantiate(boxPrefab, spawnPos, Quaternion.identity);

        // 4) Yeni kutunun �spawner� referans�n� kendimize (this) atayal�m
        MysteryBox newBoxScript = newBox.GetComponent<MysteryBox>();
        if (newBoxScript != null)
            newBoxScript.spawner = this;
    }
}
