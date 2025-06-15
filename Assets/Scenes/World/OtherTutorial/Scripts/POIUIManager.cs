using TMPro;
using UnityEngine;

public class POIUIManager : MonoBehaviour
{
    [SerializeField] private GameObject userNearPanel; // Reference to the POI UI GameObject
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;

    [SerializeField] private GameObject userFarPanel;  // Reference to the POI UI GameObject
    bool isUIPanelActive = false; // Flag to track if the near panel is active

    private GameObject _associatedPin;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayNearPanel(string title, string description)
    {
        if (!isUIPanelActive)
        {
            userNearPanel.SetActive(true); // Show the near panel
            isUIPanelActive = true; // Reset the flag

            // Set the title and description text in the near panel
            titleText.text = title; // Set the title text
            descriptionText.text = description; // Set the description text
        }
    }

    // Add this method to POIUIManager
    public void SetAssociatedPin(GameObject pin)
    {
        _associatedPin = pin;
    }

    public void OnObjectClicked()
    {
        Debug.Log("Object clicked!"); // Log the click event
        GameManager.Instance.CurrentPlayer.AddXP(100); // Add XP to the player

        if (_associatedPin != null)
        {
            _associatedPin.SetActive(false); // Hide the associated pin instead of destroying it
            _associatedPin = null;
        }

        ClosePanel();
    }

    public void DisplayFarPanel()
    {
        if (!isUIPanelActive)
        {
            userFarPanel.SetActive(true); // Show the far panel
            isUIPanelActive = true; // Reset the flag
        }
    }

    public void ClosePanel()
    {
        userNearPanel.SetActive(false); // Hide the near panel
        userFarPanel.SetActive(false);  // Hide the far panel
        isUIPanelActive = false; // Reset the flag
    }
}
