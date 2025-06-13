using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using TMPro; // Using TMPro for TextMesh Pro support, if needed

public class UiManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI xpText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private GameObject menu;

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

    private void toggleMenu()
    {
        // If the menu is active, deactivate it; if it's inactive, activate it.
        menu.SetActive(!menu.activeSelf);
    }


    //public void updateLevel(int level)
    //{
    //    levelText.text = "Level: " + level.ToString();
    //}

    //public void updateXp(int currentXP, int requiredXP)
    //{
    //    xpText.text = "XP: " + currentXP.ToString() + " / " + requiredXP.ToString();
    //}


    //// Start is called once before the first execution of Update after the MonoBehaviour is created
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}
}
