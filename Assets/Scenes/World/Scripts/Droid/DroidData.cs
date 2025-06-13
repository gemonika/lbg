using System;
using UnityEngine;

[Serializable]
public class DroidData
{
    private float spawnRate; // Rate at which droids spawn
    private float catchRate; // Rate at which droids catch up
    private int attack;
    private int defense;
    private int hp;
    private string droidSound; // Sound played when a droid is tapped


    public float SpawnRate { get { return spawnRate; } }
    public float CatchRate { get { return catchRate; } }
    public int Attack { get { return attack; } }
    public int Defense { get { return defense; } }
    public int HP { get { return hp; } }
    public string DroidSound { get { return droidSound; } }

    public DroidData(Droids droid)
    {
        spawnRate = droid.SpawnRate;
        catchRate = droid.CatchRate;
        attack = droid.Attack;
        defense = droid.Defense;
        hp = droid.HP;
        droidSound = droid.DroidSound.name; // Assuming the sound is set in the AudioSource component
    }
}
