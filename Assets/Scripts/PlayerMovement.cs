using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))] 

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float lookSpeed = 2f;
    public Transform playerCamera;
    public float minimumVerticalAngle = -80.0f;
    public float maximumVerticalAngle = 80.0f;

    public GameObject gameOver;

    // Stamina settings
    public float maxStamina = 100f;
    public float staminaDecreaseRate = 20f;
    public float staminaRegenRate = 15f;
    private float currentStamina;
    private float initialMaxStamina;
    private float AdjustedMaxStamina => CalculateAdjustedMaxStamina();

    // Hunger settings
    public float maxHunger = 100f;
    public float baseHungerDecreaseRate = 0.2f;
    private float currentbase;
    private float currentHungerDecreaseRate;
    private float currentHunger;

    private CharacterController controller;
    private float rotationX = 0f;

    // Additional variable to store current speed
    private float currentSpeed;

    // Sound
    public AudioClip walkingSound;
    public AudioClip runningSound;
    private AudioSource audioSource;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        initialMaxStamina = maxStamina;
        currentStamina = maxStamina;
        currentHunger = maxHunger;
        currentHungerDecreaseRate = baseHungerDecreaseRate;
        currentSpeed = walkSpeed; // Set initial speed to walk speed

        audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
            Debug.Log("AudioSource component found and assigned.");
    }

    void Update()
    {
        HandleStaminaAndHunger();
        HandleMovementInput();
        HandleMouseLook();
        HandleFootsteps();
    }

    // Getter methods for current stamina and hunger
    public float GetCurrentStamina()
    {
        return currentStamina;
    }

    public float GetMaxStamina()
    {
        return maxStamina;
    }

    public float GetCurrentHunger()
    {
        return currentHunger;
    }

    public float GetMaxHunger()
    {
        return maxHunger;
    }

    void HandleStaminaAndHunger()
    {
       // Decrease stamina when running
        if (Input.GetKey(KeyCode.LeftShift) && currentStamina > 0)
        {
            currentStamina -= staminaDecreaseRate * Time.deltaTime;
            currentHungerDecreaseRate = baseHungerDecreaseRate * 2f; // Double the hunger decrease rate when running
        }
        else
        {
            currentHungerDecreaseRate = baseHungerDecreaseRate;
            // Regenerate stamina when walking
            if (currentStamina < AdjustedMaxStamina)
            {
                currentStamina += staminaRegenRate * Time.deltaTime;
            }
        }

        // Decrease hunger over time
        currentHunger -= currentHungerDecreaseRate * Time.deltaTime;

        // Clamp values to ensure they stay within the defined range
        currentStamina = Mathf.Clamp(currentStamina, 0f, AdjustedMaxStamina);
        currentHunger = Mathf.Clamp(currentHunger, 0f, maxHunger);

        // Adjust movement speed based on stamina
        currentSpeed = currentStamina > 0 ? (Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed) : walkSpeed;
    }

    float CalculateAdjustedMaxStamina()
    {
        float minStamina = 30f; // Adjust this value to set the minimum stamina when hunger is at its maximum
        float sigmoidParam = 0.1f; // Adjust this value to control the curve steepness

        return initialMaxStamina - (initialMaxStamina - minStamina) / (1 + Mathf.Exp(sigmoidParam * (currentHunger - maxHunger / 2)));
    }

    void HandleMovementInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, 0f, vertical).normalized;
        Vector3 moveDirection = playerCamera.TransformDirection(movement);
        moveDirection.y = 0f;

        // Apply movement with the chosen speed
        controller.Move(moveDirection * currentSpeed * Time.deltaTime);
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
        float mouseY = -Input.GetAxis("Mouse Y") * lookSpeed;

        // Rotate the player based on mouse input (for horizontal movement)
        transform.Rotate(Vector3.up * mouseX);

        // Accumulate the vertical rotation without clamping
        rotationX += mouseY;

        // Clamp the accumulated rotation to avoid going beyond the limits
        rotationX = Mathf.Clamp(rotationX, minimumVerticalAngle, maximumVerticalAngle);

        // Apply the vertical rotation to the camera
        playerCamera.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object triggered with is the cube
        if (other.gameObject.tag == "Cube")
        {
            gameOver.SetActive(true);
        }
    }

    void HandleFootsteps()
    {
        if (controller.velocity.magnitude > 0.1f)
        {
            bool isRunning = Input.GetKey(KeyCode.LeftShift);
            AudioClip currentClip = isRunning ? runningSound : walkingSound;
            if (audioSource.clip != currentClip)
            {
                audioSource.clip = currentClip;
                audioSource.Play();
            }
            else if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }
}
