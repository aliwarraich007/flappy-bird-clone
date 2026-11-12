using UnityEngine;

public class Spawner : MonoBehaviour
{
   [SerializeField] private GameObject prefabs;
   [SerializeField] private float spawnRate = 1f;
   [SerializeField] private float minHeight = -1f;
   [SerializeField] private float maxHeight = 1f;


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
