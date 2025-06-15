using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider; // Slider to control volume
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        volumeSlider.value = AudioListener.volume; // Set the slider value to the current volume
    }

    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value; // Set the volume based on the slider value
        Save(); // Save the volume setting
    }

    public static void LoadVolumeSettings()
    {
        float savedVolume = PlayerPrefs.GetFloat("Volume", 0.5f);
        AudioListener.volume = savedVolume;
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("Volume", volumeSlider.value); // Save the current volume to PlayerPrefs
        PlayerPrefs.Save(); // Ensure the changes are saved
    }
}
