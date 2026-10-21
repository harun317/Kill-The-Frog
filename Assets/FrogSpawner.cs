using UnityEngine;

public class FrogSpawner : MonoBehaviour
{
    public GameObject frogPrefab;
    public float spawnInterval = 1.2f;
    public float minInterval = 0.5f;
    public float xRange = 7f;
    public float yRange = 3.5f;

    float timer;

    void Update()
    {
        if (!GameManager.Instance.IsPlaying) return;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            Spawn();
            timer = spawnInterval;
            // Game gets harder over time
            spawnInterval = Mathf.Max(minInterval, spawnInterval - 0.02f);
        }
    }

    void Spawn()
    {
        Vector2 pos = new Vector2(Random.Range(-xRange, xRange),
                                  Random.Range(-yRange, yRange));
        Instantiate(frogPrefab, pos, Quaternion.identity);
    }
}