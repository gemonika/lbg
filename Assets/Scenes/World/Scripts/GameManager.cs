using NUnit.Framework;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private Player currentPlayer;

    public Player CurrentPlayer
    {
        get { 
            if (currentPlayer == null) { 
                currentPlayer = gameObject.AddComponent<Player>(); 
            }
            return currentPlayer; 
        }
    }

    private void Awake()
    {
        SoundManager.LoadVolumeSettings();
    }

}
