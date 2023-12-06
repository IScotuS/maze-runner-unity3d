using UnityEngine;
using UnityEngine.UI;

public class SetupUI : MonoBehaviour
{
    public Canvas canvas;
    public Camera playerCamera;
    public Slider staminaSlider;
    public Slider hungerSlider;
    public Text staminaText;
    public Text hungerText;

    void Start()
    {
        AttachCanvasToCamera();
        SetSliderPositions();
        SetSliderColors();
        DisplayCurrentNumbers();
    }

    private void AttachCanvasToCamera()
    {
        if (playerCamera != null && canvas != null)
        {
            // Attach the Canvas to the player camera
            canvas.worldCamera = playerCamera;
        }
        else
        {
            Debug.LogError("Player camera or Canvas not assigned.");
        }
    }

    private void SetSliderPositions()
    {
        // Set the initial positions of the sliders to the top left corner
        RectTransform staminaRect = staminaSlider.GetComponent<RectTransform>();
        staminaRect.anchorMin = new Vector2(0f, 1f);
        staminaRect.anchorMax = new Vector2(0f, 1f);
        staminaRect.pivot = new Vector2(0f, 1f);
        staminaRect.anchoredPosition = new Vector2(10f, -10f); // Adjust the position as needed

        RectTransform hungerRect = hungerSlider.GetComponent<RectTransform>();
        hungerRect.anchorMin = new Vector2(0f, 1f);
        hungerRect.anchorMax = new Vector2(0f, 1f);
        hungerRect.pivot = new Vector2(0f, 1f);
        hungerRect.anchoredPosition = new Vector2(10f, -staminaRect.sizeDelta.y - 20f); // Adjust the position as needed
    }

    private void SetSliderColors()
    {
        // Set the colors for the sliders
        staminaSlider.fillRect.GetComponent<Image>().color = Color.yellow;
        hungerSlider.fillRect.GetComponent<Image>().color = Color.red;
    }

    private void DisplayCurrentNumbers()
    {
        // Create Text components for displaying current numbers
        staminaText = CreateText("Stamina: ", staminaSlider.transform);
        hungerText = CreateText("Hunger: ", hungerSlider.transform);
    }

    private Text CreateText(string prefix, Transform parent)
    {
        // Create a Text component and set its properties
        GameObject textObject = new GameObject(prefix + "Text");
        textObject.transform.SetParent(parent);
        Text text = textObject.AddComponent<Text>();
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = 14;
        text.color = Color.white;
        text.alignment = TextAnchor.MiddleLeft;
        text.horizontalOverflow = HorizontalWrapMode.Wrap;
        text.verticalOverflow = VerticalWrapMode.Overflow;

        // Set the initial position of the Text
        RectTransform textRect = text.GetComponent<RectTransform>();
        textRect.anchorMin = new Vector2(0f, 1f);
        textRect.anchorMax = new Vector2(0f, 1f);
        textRect.pivot = new Vector2(0f, 1f);

        // Adjust the position as needed
        if (parent == staminaSlider.transform)
            textRect.anchoredPosition = new Vector2(120f, -10f);
        else if (parent == hungerSlider.transform)
            textRect.anchoredPosition = new Vector2(120f, -staminaSlider.GetComponent<RectTransform>().sizeDelta.y - 20f);

        return text;
    }

    void Update()
    {
        // Update the displayed numbers based on current values
        UpdateNumberText(staminaText, Mathf.Round(staminaSlider.value * 100));
        UpdateNumberText(hungerText, Mathf.Round(hungerSlider.value * 100));
    }

    private void UpdateNumberText(Text text, float value)
    {
        // Update the Text component with the current value
        if (text != null)
        {
            text.text = value.ToString();
        }
    }
}
