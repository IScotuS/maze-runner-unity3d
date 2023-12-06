using UnityEngine;
using UnityEngine.UI;

public class PlayerUISliders : MonoBehaviour
{
    public Slider staminaSlider;
    public Slider hungerSlider;

    // Reference to the PlayerMovement script
    private PlayerMovement playerMovement;

    private void Start()
    {
        // Find the sliders in the scene if not assigned
        if (staminaSlider == null)
            staminaSlider = GameObject.Find("StaminaSlider").GetComponent<Slider>();

        if (hungerSlider == null)
            hungerSlider = GameObject.Find("HungerSlider").GetComponent<Slider>();

        // Find the PlayerMovement script in the scene
        playerMovement = GameObject.FindObjectOfType<PlayerMovement>();
    }

    private void Update()
    {
        // Update the sliders based on the player's stamina and hunger values
        if (playerMovement != null)
        {
            UpdateStaminaSlider(playerMovement.GetCurrentStamina(), playerMovement.GetMaxStamina());
            UpdateHungerSlider(playerMovement.GetCurrentHunger(), playerMovement.GetMaxHunger());
        }
    }

    private void UpdateStaminaSlider(float currentStamina, float maxStamina)
    {
        if (staminaSlider != null)
            staminaSlider.value = currentStamina / maxStamina;
    }

    private void UpdateHungerSlider(float currentHunger, float maxHunger)
    {
        if (hungerSlider != null)
            hungerSlider.value = currentHunger / maxHunger;
    }
}
