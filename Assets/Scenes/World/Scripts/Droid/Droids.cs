using NUnit.Framework;
using UnityEngine;
using UnityEngine.Rendering;

public class Droids : MonoBehaviour
{
    [SerializeField] private float spawnRate = 0.1f; // Rate at which droids spawn
    [SerializeField] private float catchRate = 0.1f; // Rate at which droids catch up
    [SerializeField] private int attack = 0;
    [SerializeField] private int defense = 0;
    [SerializeField] private int hp = 10;

    [SerializeField] private AudioClip droidSound; // Sound played when a droid is tapped
    private AudioSource audioSource; // Audio source to play the droid sound

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        Assert.IsNotNull(audioSource, "AudioSource component is not assigned to the Droids GameObject.");
        Assert.IsNotNull(droidSound, "Droid sound is not assigned in the inspector.");
    }

    public float SpawnRate { get { return spawnRate; } }
    public float CatchRate { get { return catchRate; } }
    public int Attack { get { return attack; } }
    public int Defense { get { return defense; } }
    public int HP { get { return hp; } }

    public AudioClip DroidSound { get { return droidSound; } }


    private void OnMouseDown()
    {
        PocketDroidsSceneManager[] managers = FindObjectsByType<PocketDroidsSceneManager>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        audioSource.PlayOneShot(droidSound); // Play the droid sound when tapped
        foreach (PocketDroidsSceneManager pocketDroidsSceneManager in managers)
        {
            if (pocketDroidsSceneManager.gameObject.activeSelf)
            {
               pocketDroidsSceneManager.droidTapped(this.gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        PocketDroidsSceneManager[] managers = FindObjectsByType<PocketDroidsSceneManager>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        foreach (PocketDroidsSceneManager pocketDroidsSceneManager in managers)
        {
            if (pocketDroidsSceneManager.gameObject.activeSelf)
            {
                pocketDroidsSceneManager.droidCollision(this.gameObject, other);
            }
        }
    }

}
