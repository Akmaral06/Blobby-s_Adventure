using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Hazard[] prefabs;
    public float firstDelay = 1f;
    public float interval = 2f;
    public float minX = -8f;      
    public float maxX = 8f;
    public float spawnY = 6f;

    void Start()
    {
        InvokeRepeating(nameof(Spawn), firstDelay, interval);
    }

    void Spawn()
    {
        if (prefabs.Length == 0) return;

        Hazard pick = prefabs[Random.Range(0, prefabs.Length)];

        float x = Random.Range(minX, maxX);
        Hazard h = Instantiate(pick, new Vector3(x, spawnY, 0f), Quaternion.identity);

        Destroy(h.gameObject, 10f);
    }

    public void StopSpawning()
    {
        CancelInvoke();
    }
}