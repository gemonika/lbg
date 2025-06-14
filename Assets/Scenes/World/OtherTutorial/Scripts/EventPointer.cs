using UnityEngine.InputSystem; // Import the new Input System namespace
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Xml.Serialization;

//Mapbox
using Mapbox.Examples;
using Mapbox.Utils;

public class EventPointer : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 50f; // Speed of rotation for the pointer icon in degrees per second
    [SerializeField] float amplitude = 2.0f; // Amplitude of the bobbing effect
    [SerializeField] float frequency = 0.5f; // Frequency of the movement
    [SerializeField] float height = 7; // Frequency of the movement

    LocationStatus playerLocation;
    public Vector2d eventPosition;
    POIUIManager poiUIManager; // Reference to the POI UI manager
    public int eventID; // Unique identifier for the event

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        poiUIManager = GameObject.Find("Status").GetComponent<POIUIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        FloatAndRotatePointer();
    }

    // Method to rotate the pointer icon
    void FloatAndRotatePointer()
    {
        // Smoothly rotates the pointer icon around its vertical (Y) axis at the specified speed (degrees per second)
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        // Calculates the vertical offset using a sine wave to create a smooth up-and-down bobbing motion
        float newY = Mathf.Sin(Time.time * frequency) * amplitude + height;

        // Applies the calculated vertical position to the pointer, keeping X and Z unchanged for a floating effect
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    private void OnMouseDown()
    {
        playerLocation = GameObject.Find("Status").GetComponent<LocationStatus>();
        var currentPlayerLocation = new GeoCoordinatePortable.GeoCoordinate(playerLocation.GetLocationLatitude(), playerLocation.GetLocationLongitude());
        var eventLocation = new GeoCoordinatePortable.GeoCoordinate(eventPosition[0], eventPosition[1]);
        var distance = currentPlayerLocation.GetDistanceTo(eventLocation); // Calculate the distance in meters

        if (distance < 50) // Check if the distance is less than 50 meters
        {
            poiUIManager.DisplayNearPanel(eventID); // Show the near panel if close enough
        }
        else
        {
            poiUIManager.DisplayFarPanel(); // Show the far panel if too far
        }

        Debug.Log("Distance to event: " + distance);
        // This method is called when the pointer is clicked with the mouse
        Debug.Log("Pointer clicked with mouse!");
    }
}
