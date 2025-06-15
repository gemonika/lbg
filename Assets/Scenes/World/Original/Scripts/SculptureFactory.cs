using Mapbox.Examples;
using Mapbox.Json.Linq;
using Mapbox.Map;
using Mapbox.Unity.Map;
using Mapbox.Utils;
using NUnit.Compatibility;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


[System.Serializable]
public class PointOfInterest
{
    public string ID;
    public string title;
    public string description;
    public string latitude_coordinate;
    public string longitude_coordinate;
}

public class SculptureFactory : MonoBehaviour
{
    [SerializeField] private TextAsset jsonFile;
    [SerializeField] private SpawnOnMap spawnOnMap;
    [SerializeField] private AbstractMap map;

    public List<PointOfInterest> pointsOfInterest;

    void Awake()
    {
        ReadPointsOfInterest();

        // Build the string[] for SpawnOnMap
        var locationStrings = new List<string>();
        foreach (var poi in pointsOfInterest)
        {
            locationStrings.Add($"{poi.latitude_coordinate},{poi.longitude_coordinate}");
        }
        spawnOnMap.GetType()
            .GetField("_locationStrings", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(spawnOnMap, locationStrings.ToArray());
    }

    void Start()
    {
        map.OnInitialized += OnMapReady;
    }

    private void OnMapReady()
    {
        // Enable SpawnOnMap after map is initialized to ensure correct pin placement
        spawnOnMap.enabled = true;

        // Optionally, assign POI data to pins after they are spawned
        StartCoroutine(AssignPOIDataToPinsNextFrame());
    }

    void ReadPointsOfInterest()
    {
        if (jsonFile == null)
        {
            Debug.LogError("JSON file not assigned in the Inspector.");
            return;
        }

        string json = jsonFile.text;

        // Parse as JArray
        JArray rootArray = JArray.Parse(json);

        // Find the table object
        foreach (var obj in rootArray)
        {
            if ((string)obj["type"] == "table" && (string)obj["name"] == "points_of_interest")
            {
                var dataArray = obj["data"];
                pointsOfInterest = dataArray.ToObject<List<PointOfInterest>>();
                break;
            }
        }

        if (pointsOfInterest == null) { Debug.LogError("Failed to parse points of interest JSON."); }
    }

    private IEnumerator AssignPOIDataToPinsNextFrame()
    {
        yield return null; // Wait one frame

        var pins = FindObjectsByType<ObjectPin>(FindObjectsSortMode.None);

        foreach (var pin in pins)
        {
            // Get the pin's lat/lon (assume it's stored in pin.objectPosition or pin.eventPosition)
            Vector2d pinPos = pin.objectPosition; // or pin.eventPosition

            // Find the matching POI by coordinates (with a small tolerance for floating point)
            PointOfInterest matchedPOI = null;
            foreach (var poi in pointsOfInterest)
            {
                double lat = double.Parse(poi.latitude_coordinate, System.Globalization.CultureInfo.InvariantCulture);
                double lon = double.Parse(poi.longitude_coordinate, System.Globalization.CultureInfo.InvariantCulture);

                if (Mathf.Abs((float)(lat - pinPos.x)) < 0.0001f && Mathf.Abs((float)(lon - pinPos.y)) < 0.0001f)
                {
                    matchedPOI = poi;
                    break;
                }
            }

            if (matchedPOI != null)
            {
                pin.objectID = int.Parse(matchedPOI.ID);
                pin.objectTitle = matchedPOI.title;
                pin.objectDescription = matchedPOI.description;
            }
            else
            {
                Debug.LogWarning("No matching POI found for pin at: " + pinPos);
            }
        }
    }
}

//public class SculptureFactory : Singleton<SculptureFactory>
//{


//    [SerializeField] private Droids[] availableDroids;
//    [SerializeField] private float waitTime = 180.0f;
//    [SerializeField] private int startingDroids = 5;
//    [SerializeField] private float minRange = 5.0f;
//    [SerializeField] private float maxRange = 50.0f;

//    private List<Droids> liveDroids = new List<Droids>();
//    private Droids selectedDroid;
//    private Player player;

//    public List<Droids> LiveDroids { get { return liveDroids; } }
//    public Droids SelectedDroid { get { return selectedDroid; } }

//    private void Awake()
//    {
//        Assert.IsNotNull(availableDroids, "Available Droids array is not assigned in the inspector.");

//    }

//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//    void Start()
//    {
//        player = GameManager.Instance.CurrentPlayer;
//        Assert.IsNotNull(player, "Player is not assigned in the GameManager instance.");

//        for (int i = 0; i < startingDroids; i++)
//        {
//            InstantiateDroid();
//        }

//        StartCoroutine(GenerateDroids());
//    }

//    public void SelectDroid(Droids droid)
//    {
//        selectedDroid = droid;
//    }

//    private IEnumerator GenerateDroids()
//    {
//        while (true)
//        {
//            InstantiateDroid();
//            yield return new WaitForSeconds(waitTime);
//        }
//    }

//    private void InstantiateDroid()
//    {
//        int index = Random.Range(0, availableDroids.Length);
//        float x = player.transform.position.x + GenerateRange();
//        float y = player.transform.position.y;
//        float z = player.transform.position.z + GenerateRange();

//        liveDroids.Add(Instantiate(availableDroids[index], new Vector3(x, y, z), Quaternion.identity));
//    }

//    private float GenerateRange()
//    {
//        float randomNum = Random.Range(minRange, maxRange);
//        bool isPositive = Random.Range(0, 10) < 5; // 50% chance to be positive or negative
//        return randomNum * (isPositive ? 1 : -1); // Randomly choose the sign
//    }
//}
