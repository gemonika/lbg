using UnityEngine.InputSystem; // Import the new Input System namespace
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Xml.Serialization;

//Mapbox
using Mapbox.Examples;
using Mapbox.Utils;

public class ObjectPin : MonoBehaviour
{
    [SerializeField] int maxDistance = 50; // Maximum distance to consider the event "near"

    LocationStatus playerLocation;
    POIUIManager poiUIManager; // Reference to the POI UI manager

    public int objectID; // Unique identifier for the event
    public string objectTitle; // Title of the event
    public string objectDescription;
    public Vector2d objectPosition;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        poiUIManager = GameObject.Find("Status").GetComponent<POIUIManager>();
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    private void OnMouseDown()
    {
        if (UiManager.IsMenuOpen) return; // Prevent interaction if the menu is open

        // Always set the associated pin before showing the panel
        poiUIManager.SetAssociatedPin(gameObject);

        var distance = calculateDistance(); // Calculate the distance in meters
        // Check if the distance is less than 50 meters
        if (distance < maxDistance) { poiUIManager.DisplayNearPanel(objectTitle, objectDescription); }
        else { poiUIManager.DisplayFarPanel(); }

        Debug.Log("Distance to event: " + distance);
    }

    private double calculateDistance()
    {
        playerLocation = GameObject.Find("Status").GetComponent<LocationStatus>();
        var currentPlayerLocation = new GeoCoordinatePortable.GeoCoordinate(playerLocation.GetLocationLatitude(), playerLocation.GetLocationLongitude());
        var objectLocation = new GeoCoordinatePortable.GeoCoordinate(objectPosition[0], objectPosition[1]);
        return currentPlayerLocation.GetDistanceTo(objectLocation); // Calculate the distance in meters
    }
}
