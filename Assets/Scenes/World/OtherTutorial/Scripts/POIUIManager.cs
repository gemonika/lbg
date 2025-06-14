using UnityEngine;

public class POIUIManager : MonoBehaviour
{
    [SerializeField] private GameObject userNearPanel; // Reference to the POI UI GameObject
    [SerializeField] private GameObject userFarPanel;  // Reference to the POI UI GameObject
    bool isUIPanelActive = false; // Flag to track if the near panel is active
    int tempObjectID; // Temporary variable to store the object ID

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayNearPanel(int eventID)
    {
        if (!isUIPanelActive)
        {
            tempObjectID = eventID; // Store the object ID for later use
            userNearPanel.SetActive(true); // Show the near panel
            isUIPanelActive = true; // Reset the flag
        }
    }

    public void OnObjectClicked()
    {
        Debug.Log("Object " + tempObjectID + " clicked!"); // Log the click event
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
