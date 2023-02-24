using UnityEngine;

public class PipeSpawner : MonoBehaviour
{
    public float timeToSpawn;
    public float minYPosition;
    public float maxYPosition;
    public GameObject pipePrefab;

    private float timer;

    // Update is called once per frame
    private void Update()
    {
        if (timer <= 0)
        {
            timer = timeToSpawn;
            GameObject pipe = Instantiate(pipePrefab, transform.position, Quaternion.identity);
            float rand = Random.Range(minYPosition, maxYPosition);
            pipe.transform.position = new Vector2(pipe.transform.position.x, rand);
            
            Destroy(pipe, 10);
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }
}
