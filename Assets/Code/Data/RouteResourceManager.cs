using System;
using System.IO;
using UnityEngine;

/// <summary>
/// Manages the collection of route types.
/// </summary>
public class RouteResourceManager : MonoBehaviour
{
    private RouteData[] routes;
    [SerializeField] private string fileName = "Timetables.json";
    private static RouteResourceManager instance;

    //Singleton
    private void Awake()
    {
        if(instance == null)
        {
            RouteResourceManager.instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    /// <summary>
    /// To be called by global manager only.
    /// Loads routes from file into route resource manager.
    /// </summary>
    public void InitaliseRouteResources()
    {
        LoadRoutesIntoRouteManager("TestMap");
    }

    /// <summary>
    /// Gets route data from a json file. Located in streaming assets
    /// </summary>
    /// <param name="mapName">The name of the map which has a collection of routes.</param>
    /// <exception cref="FileNotFoundException">Thrown when the path given is incorrect for the timetable load.</exception>
    /// <exception cref="ArgumentException">Thrown when the mapName cannot be found in the JSON.</exception>
    private void LoadRoutesIntoRouteManager(string mapName)
    {
        string path = Path.Combine(Application.streamingAssetsPath, fileName);

        if (!File.Exists(path))
        {
            Debug.LogError($"Timetable file not found: {path}");
            throw new FileNotFoundException($"Timetable file not found: {path}");
        }

        //get all data.
        string json = File.ReadAllText(path);
        RouteFileData fileData = JsonUtility.FromJson<RouteFileData>(json);

        MapJSONData map = null;

        //Search for map
        foreach(MapJSONData mapData in fileData.Maps)
        {
            if(mapData.Name == mapName)
            {
                map = mapData;
                break;
            }
        }

        //Throw expection when map couldnt be found.
        if(map == null)
        {
            Debug.LogError($"The map with the name {mapName} could not be found. Check your JSON and LoadRoutes call!");
            throw new ArgumentException($"The map with the name {mapName} could not be found. Check your JSON and LoadRoutes call!");
        }

        routes = new RouteData[map.Routes.Length];

        for(int i = 0; i < routes.Length; i++)
        {
            routes[i] = ConvertJSONRouteToData(map.Routes[i]);
        }

        Debug.Log($"Loaded map with name: {mapName}, with {routes.Length} routes.");
    }

    /// <summary>
    /// Takes the JSON data of a route and converts it to a struct.
    /// </summary>
    /// <param name="routeJSONData">The JSON data to be converted</param>
    /// <returns>A route data struct.</returns>
    private RouteData ConvertJSONRouteToData(RouteJSONData routeJSONData)
    {
        return new RouteData
        {
            routeName = routeJSONData.RouteName,
            forwardRoute = ConvertJSONRoutePatternToData(routeJSONData.Forward),
            reverseRoute = ConvertJSONRoutePatternToData(routeJSONData.Reverse),
            maxTrains = routeJSONData.MaxTrains,
        };
    }

    /// <summary>
    /// Converts a JSON route pattern to a RoutePatternData instance.
    /// </summary>
    /// <param name="routePatternJSON">The JSON data representing the route pattern.</param>
    /// <returns>A RoutePatternData object containing the converted route pattern information.</returns>
    private RoutePatternData ConvertJSONRoutePatternToData(RoutePatternJSONData routePatternJSON)
    {
        TimetableEntryData[] entries = new TimetableEntryData[routePatternJSON.Entries.Length];

        for(int i = 0; i < entries.Length; i++)
        {
            entries[i] = ConvertJSONTimetableEntryToData(routePatternJSON.Entries[i]);
        }

        TimeSlotData[] dailyServiceTimes = new TimeSlotData[routePatternJSON.DailyTimes.Length];

        for(int i = 0; i < dailyServiceTimes.Length; i++)
        {
            dailyServiceTimes[i] = ConvertJSONTimeToData(routePatternJSON.DailyTimes[i].Time);
        }

        return new RoutePatternData
        {
            origin = routePatternJSON.Origin,
            destination = routePatternJSON.Destination,
            entries = entries,
            dailyServiceTimes = dailyServiceTimes,
        };
    }

    /// <summary>
    /// Converts a timetable entry from JSON format to a TimetableEntryData instance.
    /// </summary>
    /// <param name="entryJSON">The JSON representation of a timetable entry.</param>
    /// <returns>A TimetableEntryData instance populated with values from the JSON data.</returns>
    private TimetableEntryData ConvertJSONTimetableEntryToData(TimetableEntryJSONData entryJSON)
    {
        return new TimetableEntryData
        {
            station = entryJSON.Station,
            platform = entryJSON.Platform,
            arrival = ConvertJSONTimeToData(entryJSON.Arrival),
            departure = ConvertJSONTimeToData(entryJSON.Departure),
            pass = ConvertJSONTimeToData(entryJSON.Pass),
        };
    }
    
    /// <summary>
    /// Converts a stringed time to its hrs, mins and seconds and returns it in struct form.
    /// WARNING! If no entry exists for a time in the JSON, the time returned will be 99:99:99!
    /// </summary>
    /// <param name="dailyTimeJSON"></param>
    /// <returns>TimeSlotData struct of the time.</returns>
    /// <exception cref="ArgumentException">Thrown when parsing an illegal time.</exception>
    private TimeSlotData ConvertJSONTimeToData(string dailyTimeJSON)
    {
        if(dailyTimeJSON == "")
        {
            //No entry exists.
            return new TimeSlotData
            {
                hrs = 99,
                mins = 99,
                secs = 99,
                slotCondition = TimeSlotCondition.emptyTime
            };
        }

        try
        {
            string[] hms = dailyTimeJSON.Split(':');

            return new TimeSlotData
            {
                hrs = Convert.ToInt32(hms[0]),
                mins = Convert.ToInt32(hms[1]),
                secs = Convert.ToInt32(hms[2]),
                slotCondition = TimeSlotCondition.loggedTime
            };
        }
        catch(Exception e)
        {
            Debug.LogError($"The method attempted to convert a string time to individual components. Check the JSON file for colons. ERR: {e}");
            throw new ArgumentException($"The method attempted to convert a string time to individual components. Check the JSON file for colons. ERR: {e}");
        }
    }

    public RouteData[] GetAllRoutes()
    {
        return routes;
    }

    public static RouteResourceManager GetInstance()
    {
        return RouteResourceManager.instance;
    }
}