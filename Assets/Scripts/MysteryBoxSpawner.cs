using System.Collections;
using UnityEngine;

public class MysteryBoxSpawner : MonoBehaviour
{
    [Header("Prefab ve Spawn Ayarlarý")]
    public GameObject boxPrefab;       
    public BoxCollider2D gridArea;     
    public float respawnDelay = 700f;    

    private void Start()
    {
        
        SpawnAfterDelay();
      
    }

    public void SpawnAfterDelay()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        
        yield return new WaitForSeconds(respawnDelay);

        Bounds b = this.gridArea.bounds;
        float x = Random.Range(b.min.x, b.max.x);
        float y = Random.Range(b.min.y, b.max.y);
        Vector3 spawnPos = new Vector3(x, y, 0f);

        
        GameObject newBox = Instantiate(boxPrefab, spawnPos, Quaternion.identity);

        
        MysteryBox newBoxScript = newBox.GetComponent<MysteryBox>();
        if (newBoxScript != null)
            newBoxScript.spawner = this;
    }
}
