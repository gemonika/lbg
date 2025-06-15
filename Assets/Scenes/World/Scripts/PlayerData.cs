using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData
{
    private int xp;
    private int requiredXP;
    private int levelBase;
    private int level;
    //private List<DroidData> droids;

    public int XP { get { return xp; } }
    public int RequiredXP { get { return requiredXP; } }
    public int LevelBase { get { return levelBase; } }
    public int Level { get { return level; } }
    //public List<DroidData> Droids { get { return droids; } }

    public PlayerData(Player player)
    {
        xp = player.CurrentXP;
        requiredXP = player.RequiredXP;
        levelBase = player.LevelBase;
        level = player.Level;
        
        //foreach (GameObject droidObject in player.Droids)
        //{
        //    Droids droid = droidObject.GetComponent<Droids>();
        //    if (droid != null)
        //    {
        //        DroidData data = new DroidData(droid);
        //        droids.Add(data);
        //    }
        //}
    }
}
