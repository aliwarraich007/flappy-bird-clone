using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Vector3 direction;
    [SerializeField] float gravity = -9.8f;

    [SerializeField] float floatStrength = 5f;

    SpriteRenderer spriteRenderer;

    [SerializeField] Sprite[] sprites;

    [SerializeField] GameHandler gameHandler;

    int spriteIndex = 0;

    // calls first time when this game object is activated
    void Awake()
    {
       spriteRenderer =  GetComponent<SpriteRenderer>();
    }


    // calls first time when game object is activated

    void Start()
    {
        InvokeRepeating("animateBird", 0.15f, 0.15f);
    }


    private void animateBird() {
        spriteIndex++;

        if (spriteIndex >= sprites.Length) spriteIndex = 0;

        spriteRenderer.sprite = sprites[spriteIndex];
    }


    // calls every frame
    void Update() {
    
        if(Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            direction = Vector3.up * floatStrength;
        }

        if(Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            direction = Vector3.up * floatStrength;

        }

        direction.y += gravity * Time.deltaTime;
        transform.position += direction * Time.deltaTime;
    }

    private void OnTriggerEnter2D (Collider2D other)
    {
        Debug.Log("Collision enter");
        if(other.gameObject.CompareTag("Obstacle"))  { gameHandler.GameOver(); }
        else if(other.gameObject.CompareTag("Score")) { gameHandler.IncScore(); }
    }

}
