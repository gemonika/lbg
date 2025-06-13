using NUnit.Framework;
using TMPro;
using UnityEngine;

public class CaptureSceneUIManager : MonoBehaviour
{
    [SerializeField] private CaptureSceneManager manager;
    [SerializeField] private GameObject successScreen;
    [SerializeField] private GameObject failScreen;
    [SerializeField] private GameObject gameScreen;
    [SerializeField] private TextMeshProUGUI orbCountText;

    private void Awake()
    {
        Assert.IsNotNull(manager, "CaptureSceneManager instance is null.");
        Assert.IsNotNull(successScreen, "Success screen is not assigned in CaptureSceneUIManager.");
        Assert.IsNotNull(failScreen, "Fail screen is not assigned in CaptureSceneUIManager.");
        Assert.IsNotNull(gameScreen, "Game screen is not assigned in CaptureSceneUIManager.");

       
    }

    // Update is called once per frame
    void Update()
    {
        switch (manager.Status)
        {
            case CaptureSceneStatus.InProgress:
                HandleInProggress();
                break;
            case CaptureSceneStatus.Successful:
                HandleSuccess();
                break;
            case CaptureSceneStatus.Failed:
                HandleFailure();
                break;
            default:
                break;
        }
    }

    private void HandleInProggress()
    {
        UpdateVisibleScreen();
        orbCountText.text = manager.CurrentThrowAttempts.ToString();
    }

    private void HandleSuccess()
    {
        UpdateVisibleScreen();
    }

    private void HandleFailure()
    {
        UpdateVisibleScreen();
    }

    private void UpdateVisibleScreen()
    {
        successScreen.SetActive(manager.Status == CaptureSceneStatus.Successful);
        failScreen.SetActive(manager.Status == CaptureSceneStatus.Failed);
        gameScreen.SetActive(manager.Status == CaptureSceneStatus.InProgress);
    }
}
