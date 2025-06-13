using UnityEngine;

public class XPBonus : MonoBehaviour
{
    [SerializeField] private int bonus = 10; // Amount of XP to give when collected

    private void OnMouseDown()
    {
        GameManager.Instance.CurrentPlayer.AddXP(bonus); // Add XP to the player
        Destroy(gameObject); // Destroy the XP bonus object
    }
}
