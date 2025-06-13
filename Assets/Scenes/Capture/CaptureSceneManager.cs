using System.Collections.Generic;
using UnityEngine;

public class CaptureSceneManager : PocketDroidsSceneManager
{
    [SerializeField] private int maxThrowAttempts = 3; // Maximum number of throw attempts allowed
    [SerializeField] private GameObject orb;
    [SerializeField] private Vector3 spawnPoint;

    private int currentThrowAttempts; // Current number of throw attempts made
    private CaptureSceneStatus status = CaptureSceneStatus.InProgress;

    public int MaxThrowAttempts
    {
        get { return maxThrowAttempts; }
    }

    public int CurrentThrowAttempts
    {
        get { return currentThrowAttempts; }
    }

    public CaptureSceneStatus Status
    {
        get { return status; }
    }

    private void Start()
    {
        CalculateMaxThrows();
        currentThrowAttempts = maxThrowAttempts; // Initialize current throw attempts to max
    }

    private void CalculateMaxThrows()
    {
        //maxThrowAttempts += GameManager.Instance.CurrentPlayer.Level / 5; // Increase max throws based on player level
    }

    public void OrbDestroyerd()
    {
        currentThrowAttempts--;

        if (currentThrowAttempts <= 0)
        {
            if (status != CaptureSceneStatus.Successful)
            {
                status = CaptureSceneStatus.Failed;
                Invoke("MoveToWorldScene", 2.0f);
            }
        }
        else
        {
            Instantiate(orb, spawnPoint, Quaternion.identity);
        }
    }

    public override void droidTapped(GameObject droid)
    {
        print("CaptureSceneManager.droidTapped activated");
    }

    public override void playerTapped(GameObject player)
    {
        print("CaptureSceneManager.playerTapped activated");
    }

    public override void droidCollision(GameObject droid, Collision other)
    {
        status = CaptureSceneStatus.Successful; // Set status to failed on collision
        Invoke("MoveToWorldScene", 2.0f); // Delay before moving to the world scene
    }

    private void MoveToWorldScene()
    {
        SceneTransitionManager.Instance.GoToScene(PocketDroidsConstants.SCENE_WORLD, new List<GameObject>());
    }
}
