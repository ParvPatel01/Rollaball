using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public AudioSource dashAudioSource;  // Reference to the Audio Source for dash sound
    public AudioClip dashSound;
    public AudioClip backgroundMusic;
    public AudioClip pickUpSound;
    public AudioClip damageSound;
    public AudioClip breakableWallSound;
    public AudioClip movementSound;
    public AudioClip healingSound;
    private Rigidbody rb;
    private float movementX;
    private float movementY;
    public float speed = 5f;

    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    private bool isDashing = false;
    private bool canDash = true;
    private TrailRenderer trailRenderer;

    // Health system
    public int maxHealth = 100;
    private int currentHealth;

    public GameObject gameOverScreen; // Reference to the game over screen UI (optional)
    public Slider healthBar;

    public Button restartButton;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        trailRenderer = GetComponent<TrailRenderer>();
        trailRenderer.emitting = false;
        // Initialize health
        currentHealth = maxHealth;

        // Set the health bar and health text to match current health at the start
        UpdateHealthUI();
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
        dashAudioSource.PlayOneShot(movementSound);
    }

    private void FixedUpdate()
    {
        if (!isDashing)
        {
            // Regular movement
            Vector3 movement = new Vector3(movementX, 0.0f, movementY);
            float adjustedSpeed = speed / rb.transform.localScale.x;  // Adjust speed based on size
            rb.AddForce(movement * adjustedSpeed);
        }
    }

    void Update()
    {
        // Detect dash input and trigger dash if possible
        if (Keyboard.current.spaceKey.wasPressedThisFrame && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        // Initiate dash
        isDashing = true;
        canDash = false;
        trailRenderer.emitting = true;

        dashAudioSource.PlayOneShot(dashSound);

        // Calculate dash direction based on player's movement input
        Vector3 dashDirection = new Vector3(movementX, 0.0f, movementY).normalized;

        // Apply force for dash (Impulse only once)
        rb.AddForce(dashDirection * dashSpeed, ForceMode.Impulse);

        // Wait for dash duration
        yield return new WaitForSeconds(dashDuration);

        // End the dash
        isDashing = false;
        trailRenderer.emitting = false;

        // Wait for cooldown before dash can be triggered again
        yield return new WaitForSeconds(dashCooldown);

        canDash = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        float scaleFactor = 0.5f;  // Gradual scaling
        if (other.gameObject.CompareTag("upscalePickUp"))
        {
            rb.transform.localScale += new Vector3(scaleFactor, scaleFactor, scaleFactor);
        }
        else if (other.gameObject.CompareTag("downscalePickUp"))
        {
            if (rb.transform.localScale.x > 0.5f)  // Minimum size
            {
                rb.transform.localScale -= new Vector3(scaleFactor, scaleFactor, scaleFactor);
            }
        }
        dashAudioSource.PlayOneShot(pickUpSound);

        other.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isDashing && collision.gameObject.CompareTag("BreakableWall"))
        {
            dashAudioSource.PlayOneShot(breakableWallSound);
            Destroy(collision.gameObject);
        }

        // Apply damage when colliding with damaging objects
        if (collision.gameObject.CompareTag("DamagingObject"))
        {
            Debug.Log("Player collided with damaging object! " + currentHealth);
            TakeDamage(20); // Example damage value
        }

        if(collision.gameObject.CompareTag("HealingObject"))
        {
            Debug.Log("Player collided with healing object! " + currentHealth);
            TakeHeal(20); // Example heal value
        }
    }

    private void UpdateHealthUI()
    {
        if (healthBar != null && currentHealth >= 0)
        {
            healthBar.value = currentHealth;
        }

    }

    public void SetHealth(int health)
    {
        currentHealth = health;

        UpdateHealthUI();
    }

    public void TakeHeal(int health)
    {
        currentHealth += health;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        dashAudioSource.PlayOneShot(healingSound);
        UpdateHealthUI();
    }

    // Method to take damage
    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0)
        {
            return;
        }
        currentHealth -= damage;
        Debug.Log("Player took damage! Current health: " + currentHealth);
        dashAudioSource.PlayOneShot(damageSound);

        // Update health bar and text
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player has died!");
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
            restartButton.onClick.AddListener(RestartGame);
        }
    }

    public void RestartGame()
    {
        // Reload the scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
