 using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    Animator animator; // Reference to the Animator component
    int isWalkingHash; // Cached hash for the "IsWalking" parameter in the Animator

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>(); // Get the Animator component attached to this GameObject
        isWalkingHash = Animator.StringToHash("IsWalking"); // Cache the hash for the "IsWalking" parameter
    }

    // Update is called once per frame
    void Update()
    {
        bool isWalking = animator.GetBool(isWalkingHash); // Get the current value of the "isWalking" parameter
        bool forwardPressed = Input.GetKey("w"); // Check if the 'W' key is pressed

        if (!isWalking && forwardPressed)
        {
            animator.SetBool(isWalkingHash, true); // Set the "isWalking" parameter to true when 'W' is pressed
        }

        if (isWalking && !forwardPressed)
        {
            animator.SetBool(isWalkingHash, false); // Set the "isWalking" parameter to false when 'W' is released
        }
    }
}
