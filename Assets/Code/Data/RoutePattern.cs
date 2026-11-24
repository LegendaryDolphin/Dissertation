using System;
using System.Diagnostics;
using UnityEngine;

/// <summary>
/// Contains a forward route pattern and reverse route pattern.
/// </summary>
public struct RouteData
{
    public string routeName;
    public RoutePatternData forwardRoute;
    public RoutePatternData reverseRoute;
    public int maxTrains;
    public int currentTrains;
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
    public TimeSlotCondition slotCondition;

    public TimeSlotData(int hrs, int mins, int secs, TimeSlotCondition condition)
    {
        this.hrs = hrs;
        this.mins = mins;
        this.secs = secs;
        this.slotCondition = condition;
    }

    /// <summary>
    /// Takes a time slot and its it to this time slot.
    /// If either time slot is empty as its condition, the new slot will return empty.
    /// </summary>
    /// <param name="toAdd">The time to add to the slot.</param>
    /// <returns>A new time slot.</returns>
    public TimeSlotData AddToTimeSlot(TimeSlotData toAdd)
    {
        //If either time slot is empty then return empty.
        if(this.slotCondition == TimeSlotCondition.emptyTime || toAdd.slotCondition == TimeSlotCondition.emptyTime)
        {
            return EmptySlot();
        }

        int newHrs = this.hrs + toAdd.hrs;
        int newMins = this.mins + toAdd.mins;
        int newSecs = this.secs + toAdd.secs;

        int finalHrs;
        int finalMins;
        int finalSecs;

        if(newSecs > 59)
        {
            finalSecs = newSecs - 60;
            newMins += 1;
        }
        else
        {
            finalSecs = newSecs;
        }

        if(newMins > 59)
        {
            finalMins = newMins - 60;
            newHrs += 1;
        }
        else
        {
            finalMins = newMins;
        }

        if(newHrs > 23)
        {
            finalHrs = newHrs - 23;
        }
        else
        {
            finalHrs = newHrs;
        }

        TimeSlotData t =  new TimeSlotData
        {
            hrs = finalHrs,
            mins = finalMins,
            secs = finalSecs,
            slotCondition = TimeSlotCondition.loggedTime
        };

        return t;
    }

    public bool isEmpty()
    {
        return ((this.hrs == 99 && this.mins == 99 && this.secs == 99) || this.slotCondition == TimeSlotCondition.emptyTime);
    }

    /// <summary>
    /// Return an 'empty' time slot.
    /// </summary>
    /// <returns>The empty time slot</returns>
    public TimeSlotData EmptySlot()
    {
        return new TimeSlotData
        {
            hrs = 99,
            mins = 99,
            secs = 99,
            slotCondition = TimeSlotCondition.emptyTime
        };
    }

    /// <summary>
    /// Returns struct as string.
    /// </summary>
    /// <returns>The struct in form XX:XX:XX</returns>
    public string TimeToString()
    {
        string secs;
        string mins;
        string hrs;

        if (this.secs < 10)
        {
            secs = "0" + this.secs.ToString();
        }
        else
        {
            secs = this.secs.ToString();
        }

        if (this.mins < 10)
        {
            mins = "0" + this.mins.ToString();
        }
        else
        {
            mins = this.mins.ToString();
        }

        if (this.hrs < 10)
        {
            hrs = "0" + this.hrs.ToString();
        }
        else
        {
            hrs = this.hrs.ToString();
        }

        return $"{hrs}:{mins}:{secs}";
    }
}

public enum TimeSlotCondition
{
    emptyTime,
    loggedTime
}

/// <summary>
/// A struct of scheduled and actual timetable entries.
/// </summary>
public struct TimetableData
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

