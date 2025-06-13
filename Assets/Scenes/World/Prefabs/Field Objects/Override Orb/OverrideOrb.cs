using NUnit.Framework;
using UnityEngine;

public class OverrideOrb : MonoBehaviour
{
    [SerializeField] private float throwSpeed = 30.0f; // Speed at which the orb is thrown
    [SerializeField] private float collisionStallTime = 2.0f; // Time to stall on collision
    [SerializeField] private float stallTime = 5.0f;
    [SerializeField] private AudioClip dropSound; // Sound played when the orb is dropped
    [SerializeField] private AudioClip successSound; // Sound played when the orb is thrown
    [SerializeField] private AudioClip throwSound; // Sound played when the orb fails to hit a target

    private float lastX;
    private float lastY;
    private bool released;
    private bool holding;
    private bool trackingCollisions = false;
    private Rigidbody rigidbody;
    private AudioSource audioSource;
    private InputStatus inputStatus; // Assuming InputStatus is a class that handles input states

    private enum InputStatus
    {
        Grabbing,
        Holding,
        Releasing,
        None
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rigidbody = GetComponent<Rigidbody>();

        Assert.IsNotNull(audioSource, "AudioSource component is missing on OverrideOrb.");
        Assert.IsNotNull(rigidbody, "Rigidbody component is missing on OverrideOrb.");
        Assert.IsNotNull(dropSound, "Drop sound is not assigned in OverrideOrb.");
        Assert.IsNotNull(successSound, "Success sound is not assigned in OverrideOrb.");
        Assert.IsNotNull(throwSound, "Throw sound is not assigned in OverrideOrb.");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (released) return;

        if (holding) FollowInput();

        UpdateInputStatus();

        switch (inputStatus)
        {
            case InputStatus.Grabbing:
                Grab();
                break;
            case InputStatus.Holding:
                Drag();
                break;
            case InputStatus.Releasing:
                Release();
                break;
            case InputStatus.None:
                // No input detected
                break;
        }
    }


    private void UpdateInputStatus()
    {
        #if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                inputStatus = InputStatus.Grabbing;
            }
            else if (Input.GetMouseButton(0))
            {
                inputStatus = InputStatus.Holding;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                inputStatus = InputStatus.Releasing;
            }
            else
            {
                inputStatus = InputStatus.None;
            }
        #endif
        #if NOT_UNITY_EDITOR
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                inputStatus = InputStatus.Grabbing;
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                inputStatus = InputStatus.Releasing;
            }
            else if (Input.touchCount == 1)
            {
                inputStatus = InputStatus.Holding;
            }
            else
            {
                inputStatus = InputStatus.None;
            }
          #endif


    }

    private void FollowInput()
    {
        Vector3 inputPosition = GetInputPosition(); // Assuming this method retrieves the current input position
        inputPosition.z = Camera.main.nearClipPlane * 7.5f; // Set z position to the camera's near clip plane
        Vector3 position = Camera.main.ScreenToWorldPoint(inputPosition);

        transform.localPosition = Vector3.Lerp(transform.localPosition, position, 50.0f * Time.deltaTime);
    }
    private void Grab()
    {
        Ray ray = Camera.main.ScreenPointToRay(GetInputPosition()); // Get the ray from the camera to the input position
        RaycastHit point;

        if (Physics.Raycast(ray, out point, 100.0f) && point.transform == transform) // Cast a ray to detect the orb
        {
            holding = true; 
            transform.parent = null; // Detach from any parent to allow free movement
        }
    }

    private void Drag()
    {
        lastX = GetInputPosition().x; // Get the current input position
        lastY = GetInputPosition().y; // Get the current input position
    }

    private void Release()
    {
        if (lastY < GetInputPosition().y) // Check if the input position has moved up
        {
            Throw(GetInputPosition());
        }
    }

    private Vector2 GetInputPosition()
    {
        Vector2 result = new Vector2();

        #if UNITY_EDITOR
            result = Input.mousePosition; // Get the mouse position in the editor
        #endif
        #if NOT_UNITY_EDITOR
            result = Input.GetTouch(0).position; // Get the touch position on mobile devices
        #endif

        return result;
    }

    private void Throw(Vector2 targetPosition)
    {
        rigidbody.useGravity = true; // Enable gravity for the orb
        trackingCollisions = true; // Start tracking collisions

        // Where the player is going and the velocity
        float yDiff = (targetPosition.y - lastY) / Screen.height * 100; // Calculate the vertical distance moved
        float speed = throwSpeed + yDiff; // Adjust the throw speed based on the vertical distance

        // Is the player goinf to the left or right? At which angle?
        float x = (targetPosition.x / Screen.width) - (lastX / Screen.width); // Calculate the horizontal distance moved
        x = Mathf.Abs(GetInputPosition().x - lastX) / Screen.width * 100 * x; // Normalize the horizontal distance

        // Where direction is in game world space?
        Vector3 direction = new Vector3(x, 0.0f, 1.0f); // Create a direction vector based on the input
        direction = Camera.main.transform.TransformDirection(direction); // Transform the direction to world space

        // Added force to the direction
        //rigidbody.AddForce((direction * speed / 2.0f) + Vector3.up * speed); // Apply force to the orb
        rigidbody.AddForce(direction * speed / 2.0f, ForceMode.Impulse); // Use Impulse for instant

        audioSource.PlayOneShot(throwSound); // Play the throw sound

        released = true; // Mark the orb as released
        holding = false; // Reset the holding state

        Invoke("PowerDown", stallTime); // Schedule power down after a delay
    }

    private void PowerDown()
    {
        CaptureSceneManager manager = FindFirstObjectByType<CaptureSceneManager>(); // Find the CaptureSceneManager in the scene
        if (manager != null)
        {
            manager.OrbDestroyerd(); // Notify the manager that the orb has been destroyed
        }
        Destroy(gameObject); // Destroy the orb when power is down
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!trackingCollisions) return; // If not tracking collisions, exit

        trackingCollisions = false; // Stop tracking collisions
        if (collision.gameObject.CompareTag(PocketDroidsConstants.TAG_DROID)) // Check if the orb collides with a Droid
        {
            audioSource.PlayOneShot(successSound); // Play the success sound
        }
        else
        {
            audioSource.PlayOneShot(dropSound); // Play the success sound
        }

        Invoke("PowerDown", collisionStallTime); // Schedule power down after a delay
    }
}
