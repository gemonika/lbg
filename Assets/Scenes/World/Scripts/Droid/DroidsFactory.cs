using NUnit.Compatibility;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroidsFactory : Singleton<DroidsFactory>
{
    [SerializeField] private Droids[] availableDroids;
    [SerializeField] private float waitTime = 180.0f;
    [SerializeField] private int startingDroids = 5;
    [SerializeField] private float minRange = 5.0f;
    [SerializeField] private float maxRange = 50.0f;

    private List<Droids> liveDroids = new List<Droids>();
    private Droids selectedDroid;
    private Player player;

    public List<Droids> LiveDroids { get { return liveDroids; } }
    public Droids SelectedDroid { get { return selectedDroid; } }

    private void Awake()
    {
        Assert.IsNotNull(availableDroids, "Available Droids array is not assigned in the inspector.");

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameManager.Instance.CurrentPlayer;
        Assert.IsNotNull(player, "Player is not assigned in the GameManager instance.");

        for (int i = 0; i < startingDroids; i++)
        {
            InstantiateDroid();
        }

        StartCoroutine(GenerateDroids());
    }

    public void SelectDroid(Droids droid)
    {
        selectedDroid = droid;
    }

    private IEnumerator GenerateDroids()
    {
        while (true)
        {
            InstantiateDroid();
            yield return new WaitForSeconds(waitTime);
        }
    }

    private void InstantiateDroid()
    {
        int index = Random.Range(0, availableDroids.Length);
        float x = player.transform.position.x + GenerateRange();
        float y = player.transform.position.y;
        float z = player.transform.position.z + GenerateRange();

        liveDroids.Add(Instantiate(availableDroids[index], new Vector3(x, y, z), Quaternion.identity));
    }

    private float GenerateRange()
    {
        float randomNum = Random.Range(minRange, maxRange);
        bool isPositive = Random.Range(0, 10) < 5; // 50% chance to be positive or negative
        return randomNum * (isPositive ? 1 : -1); // Randomly choose the sign
    }
}
