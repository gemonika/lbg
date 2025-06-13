using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 50f; // Speed of rotation
    [SerializeField] private float floatAmplitude = 2.0f; // Amplitude of floating effect
    [SerializeField] private float floatFrequency = 0.5f; // Frequency of floating effect

    private Vector3 startPosition;

    void Start()
    {
        // Store the initial position of the object
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime); // Rotate the object around the Y-axis
        Vector3 tempPos = startPosition;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * floatFrequency) * floatAmplitude + 10; // Calculate the floating effect
        transform.position = tempPos; // Apply the new position to the object
    }
}
