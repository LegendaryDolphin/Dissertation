using System;

/// <summary>
/// Contains a forward route pattern and reverse route pattern.
/// </summary>
public struct RouteData
{
    public string routeName;
    public RoutePatternData forwardRoute;
    public RoutePatternData reverseRoute;
    public int maxTrains;
}

/// <summary>
/// Contains a single journey pattern.
/// </summary>
public struct RoutePatternData
{
    public string origin;
    public string destination;
    public TimetableEntryData[] entries;
    public TimeSlotData[] dailyServiceTimes;
}

/// <summary>
/// Contains a single timetable entry.
/// </summary>
public struct TimetableEntryData
{
    public string station;
    public string platform;
    public TimeSlotData arrival;
    public TimeSlotData departure;
    public TimeSlotData pass;
}

/// <summary>
/// A collection of hours minutes and seconds all in integer format.
/// </summary>
public struct TimeSlotData
{
    public int hrs;
    public int mins;
    public int secs;

    public TimeSlotData(int hrs, int mins, int secs)
    {
        this.hrs = hrs;
        this.mins = mins;
        this.secs = secs;
    }

    public bool isEmpty()
    {
        return (this.hrs == 99 && this.mins == 99 && this.secs == 99);
    }
}

/// <summary>
/// A struct of scheduled and actual timetable entries.
/// </summary>
public struct Timetable
{
    public TimetableEntryData[] scheduled;
    public TimetableEntryData[] actual;
}

/// <summary>
/// The JSON compatible class of the timetable data.
/// </summary>
[Serializable]
public class RouteFileData
{
    public MapJSONData[] Maps;
}

/// <summary>
/// The JSON compatible class of a map, containing routes for a certain map.
/// </summary>
[Serializable]
public class MapJSONData
{
    public string Name;
    public RouteJSONData[] Routes;
}

/// <summary>
/// The JSON compatible class of a full route including the forward and backward patterns.
/// </summary>
[Serializable]
public class RouteJSONData
{
    public string RouteName;
    public RoutePatternJSONData Forward;
    public RoutePatternJSONData Reverse;
    public int MaxTrains;
}

/// <summary>
/// The JSON compatible class of a single route pattern.
/// </summary>
[Serializable]
public class RoutePatternJSONData
{
    public string Origin;
    public string Destination;
    public TimetableEntryJSONData[] Entries;
    public DailyTimeJSONData[] DailyTimes;
}  

/// <summary>
/// The JSON compatible class of a full timetable entry.
/// </summary>
[Serializable]
public class TimetableEntryJSONData
{
    public string Station;
    public string Platform;
    public string Arrival;
    public string Departure;
    public string Pass;
}

/// <summary>
/// The JSON compatible class of a daily service time.
/// </summary>
[Serializable]
public class DailyTimeJSONData
{
    public string Time;
}

