using UnityEngine;

public class Parallex : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    [SerializeField] private float scrollFactor = 1f;

    // Update is called once per frame
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }
    void Update()
    {
        meshRenderer.material.mainTextureOffset += new Vector2(scrollFactor * Time.deltaTime, 0);
    }
}
