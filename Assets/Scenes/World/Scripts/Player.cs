using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int level = 1; // Player's level
    [SerializeField] private int levelBase = 100; // Base XP required for level 1
    [SerializeField] private int currentXP = 0; // Current XP of the player
    [SerializeField] private int requiredXP = 100; // XP required to level up
    [SerializeField] private List<GameObject> droids = new List<GameObject>();

    private string path; // Path to the player data file, if needed for saving/loading

    public int Level { get { return level; } }
    public int LevelBase { get { return levelBase; } }
    public int CurrentXP { get { return currentXP; } }
    public int RequiredXP { get { return requiredXP; } }
    public List<GameObject> Droids { get { return droids; } }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        path = Application.persistentDataPath + "/player.dat"; // Set the path for player data
        Load(); // Load player data from file if it exists
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddXP(int xp)
    {
        this.currentXP += Mathf.Max(0, xp); // Ensure currentXP is not negative
        InitLevelData(); // Recalculate level and required XP after adding XP
        Save(); // Save the player data after updating XP
    }
    public void AddDroids(GameObject droid)
    {
        if (droid)
        droids.Add(droid);
        Save(); // Save the player data after adding a droid
    }

    private void InitLevelData()
    {
        //level = (currentXP / levelBase) + 1; // Calculate level based on current XP and level base
        if (currentXP == requiredXP)
        {
            level++; // Increment level if current XP equals required XP
            requiredXP += levelBase * level; // Calculate required XP for the next level
        }
        print($"Level: {level}, Current XP: {currentXP}, Required XP: {requiredXP}"); // Debug log for level and XP
    }
    private void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(path);
        PlayerData data = new PlayerData(this); // Create a PlayerData object with the current player state
        bf.Serialize(file, data); // Serialize the PlayerData object to the file
        file.Close(); // Close the file stream
    }

    private void Load()
    {
        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file); // Deserialize the PlayerData object from the file
            file.Close(); // Close the file stream
            // Update player state with loaded data
            currentXP = data.XP;
            requiredXP = data.RequiredXP;
            levelBase = data.LevelBase;
            level = data.Level;

            // Import player's droids
        }
        else
        {
            InitLevelData(); // If no save file exists, initialize level data
        }
    }
}
