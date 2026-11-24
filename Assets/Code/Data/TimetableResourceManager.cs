using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class for containing and creating all timetable instances. 
/// </summary>
public class TimetableResourceManager : MonoBehaviour
{
    private static TimetableResourceManager instance;

    [SerializeField] private Dictionary<string, List<Timetable>> timetables = new Dictionary<string, List<Timetable>>();

    private void Awake()
    {
        if(instance == null)
        {
            TimetableResourceManager.instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    /// <summary>
    /// To be called by global manager only.
    /// Initialises timetable instance and stores them in this class.
    /// </summary>
    public void InitaliseTimetableResourceManager(RouteData[] routes)
    {
        CreateTimetablesFromRoutes(routes);
    }

    /// <summary>
    /// Creates all timetable instances from an array of route data. Which should be provided from the route resource manager.
    /// </summary>
    /// <param name="routes"></param>
    private void CreateTimetablesFromRoutes(RouteData[] routes)
    {
        //For each route
        foreach(RouteData routeData in routes)
        {
            //Get the forward pattern
            RoutePatternData forwardPattern = routeData.forwardRoute;
            
            //Create new instances and add to dict.
            for(int i = 0;  i < forwardPattern.dailyServiceTimes.Length; i++)
            {
                Timetable t = new Timetable(forwardPattern.origin, forwardPattern.destination, forwardPattern.dailyServiceTimes[i], forwardPattern);

                if (!timetables.ContainsKey(forwardPattern.origin))
                {
                    timetables.Add(forwardPattern.origin, new List<Timetable>());
                }

                timetables[forwardPattern.origin].Add(t);
            }

            //Get the reverse pattern
            RoutePatternData reversePattern = routeData.reverseRoute;

            //Create new instances and add to dict.
            for (int i = 0; i < reversePattern.dailyServiceTimes.Length; i++)
            {
                Timetable t = new Timetable(reversePattern.origin, reversePattern.destination, reversePattern.dailyServiceTimes[i], reversePattern);

                if (!timetables.ContainsKey(reversePattern.origin))
                {
                    timetables.Add(reversePattern.origin, new List<Timetable>());
                }

                timetables[reversePattern.origin].Add(t);
            }
        }
    }

    /// <summary>
    /// Lists every timetable in the debug.
    /// </summary>
    public void DebugTimetables()
    {
        foreach(string k in timetables.Keys)
        {
            Debug.Log($"TIMETABLES FOR STATION ORIGIN: {k}:");
            Debug.Log("------------------------------------");

            for(int i = 0; i < timetables[k].Count; i++)
            {
                timetables[k][i].DebugTimetable();
            }
        }
    }

    public static TimetableResourceManager GetInstance()
    {
        return TimetableResourceManager.instance;
    }
}