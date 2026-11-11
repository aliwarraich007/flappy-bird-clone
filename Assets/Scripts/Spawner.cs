using UnityEngine;

public class Spawner : MonoBehaviour
{
    private GameObject prefabs;
    private float spawnRate = 1f;
    private float minHeight = -1f;
    private float maxHeight = 1f;


    private void OnEnable() {

        InvokeRepeating(nameof(Spawn), spawnRate, spawnRate);

    }

    private void OnDestroy()
    {
        CancelInvoke(nameof(Spawn));
    }


    private void Spawn() {

        GameObject pipes = Instantiate(prefabs, transform.position, Quaternion.identity);
        pipes.transform.position += Vector3.up * Random.Range(minHeight, maxHeight);
    
    }


}
