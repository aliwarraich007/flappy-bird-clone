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

    [SerializeField] float fallSoundThreshold = -2f;

    [SerializeField] AudioClip die, swoosh, flap, score;
    private AudioSource audioSource;
    private AudioSource swooshSource;

    int spriteIndex = 0;

    // calls first time when this game object is activated
    void Awake()
    {
       spriteRenderer =  GetComponent<SpriteRenderer>();
    }


    // calls first time when game object is activated

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        swooshSource = gameObject.AddComponent<AudioSource>();
        swooshSource.clip = swoosh;
        swooshSource.loop = true;
        swooshSource.playOnAwake = false;
        InvokeRepeating("animateBird", 0.15f, 0.15f);
    }


    private void animateBird() {
        spriteIndex++;

        if (spriteIndex >= sprites.Length) spriteIndex = 0;

        spriteRenderer.sprite = sprites[spriteIndex];
    }


    void Update() {
        if (!gameHandler.IsPlaying) {
            SetSwoosh(false);
            return;
        }

        bool touched = Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame || 
            Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame;

        if (touched)
        {
            PlaySound(flap);
            direction = Vector3.up * floatStrength;
        } 

        direction.y += gravity * Time.deltaTime;
        transform.position += direction * Time.deltaTime;
        SetSwoosh(direction.y < fallSoundThreshold);
    }

    private void OnTriggerEnter2D (Collider2D other)
    {

        if (!gameHandler.IsPlaying) return;

        if(other.gameObject.CompareTag("Obstacle"))  { 
            gameHandler.GameOver();
            PlaySound(die);
        }
        else if(other.gameObject.CompareTag("Score")) { 
            gameHandler.IncScore();
            PlaySound(score);
        }
    }

    private void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
        
    }

    private void SetSwoosh(bool falling) {

        if (falling && !swooshSource.isPlaying) swooshSource.Play();
        else if (!falling && swooshSource.isPlaying) swooshSource.Stop();

    }

}
