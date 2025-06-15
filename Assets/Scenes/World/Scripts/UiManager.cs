using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using TMPro; // Using TMPro for TextMesh Pro support, if needed

public class UiManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI xpText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject status;

    [SerializeField] private AudioClip menuButtonSound;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        Assert.IsNotNull(audioSource, "AudioSource component is not assigned to the UiManager GameObject.");
        Assert.IsNotNull(xpText, "XP Text is not assigned in the inspector.");
        Assert.IsNotNull(levelText, "Level Text is not assigned in the inspector.");
        Assert.IsNotNull(menu, "Menu GameObject is not assigned in the inspector.");
        Assert.IsNotNull(menuButtonSound, "Menu Button Sound is not assigned in the inspector.");

        Assert.IsNotNull(status, "Status GameObject is not assigned in the inspector.");
    }

    private void Update()
    {
        updateLevel();
        updateXp();
    }

    public void updateLevel()
    {
        levelText.text = GameManager.Instance.CurrentPlayer.Level.ToString();
    }

    public void updateXp()
    {
        int currentXP = GameManager.Instance.CurrentPlayer.CurrentXP;
        int requiredXP = GameManager.Instance.CurrentPlayer.RequiredXP;
        xpText.text = currentXP.ToString() + " / " + requiredXP.ToString();
    }

    public void menuButtonClicked()
    {
        audioSource.PlayOneShot(menuButtonSound); // Play the menu button sound
        toggleMenu(); // Toggle the menu visibility
    }

    public static bool IsMenuOpen { get; private set; }
    private void toggleMenu()
    {
        // Only close panels if the menu is currently open
        if (!menu.activeSelf)
        {
            // Get the POIUIManager component from the status GameObject and close its panel
            var poiManager = status.GetComponent<POIUIManager>();
            if (poiManager != null) { poiManager.ClosePanel(); }
        }

        bool showMenu = !menu.activeSelf;
        menu.SetActive(showMenu);
        IsMenuOpen = showMenu;
    }
}
