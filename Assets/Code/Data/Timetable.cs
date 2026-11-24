using UnityEngine;

/// <summary>
/// Specifies the possible states of a timetable.
/// </summary>
public enum TimetableState
{
    notActivatd,
    aquiredByAgent,
    finished
}

public class Timetable
{
    private TimetableData data;
    private int delay = 0;
    private TimetableState state = TimetableState.notActivatd;

    /// <summary>
    /// Creates a new timetable instance.
    /// </summary>
    /// <param name="origin">The origin of the timetable.</param>
    /// <param name="destination">The destonation of the timetable. (Terminus).</param>
    /// <param name="originDepartureTime">The departure time from the origin.</param>
    /// <param name="routePatternData">The route pattern to follow. (Schedule).</param>
    public Timetable(string origin, string destination, TimeSlotData originDepartureTime, RoutePatternData routePatternData)
    {
        InitaliseTimetableData(origin, destination, originDepartureTime, routePatternData);
    }

    /// <summary>
    /// Called by the constructor only, used to initialise the timetable data struct. By adding the origin time on top of the pattern time.
    /// </summary>
    /// <param name="origin">The origin of the timetable.</param>
    /// <param name="destination">The destonation of the timetable. (Terminus).</param>
    /// <param name="originDepartureTime">The departure time from the origin.</param>
    /// <param name="routePatternData">The route pattern to follow. (Schedule).</param>
    private void InitaliseTimetableData(string origin, string destination, TimeSlotData originDepartureTime, RoutePatternData routePatternData)
    {
        TimetableEntryData[] scheduledTimes = new TimetableEntryData[routePatternData.entries.Length];
        TimetableEntryData[] actualTimes = new TimetableEntryData[routePatternData.entries.Length];

        for(int i = 0; i < scheduledTimes.Length; i++)
        {
            TimeSlotData newTime = scheduledTimes[i].arrival.AddToTimeSlot(originDepartureTime);
            scheduledTimes[i] = new TimetableEntryData
            {
                station = routePatternData.entries[i].station,
                platform = routePatternData.entries[i].platform,
                arrival = routePatternData.entries[i].arrival.AddToTimeSlot(originDepartureTime),
                departure = routePatternData.entries[i].departure.AddToTimeSlot(originDepartureTime),
                pass = routePatternData.entries[i].pass.AddToTimeSlot(originDepartureTime),
            };
        }

        data = new TimetableData
        {
            scheduled = scheduledTimes,
            actual = actualTimes,
        };
    }

    /// <summary>
    /// Prints out the scheduled timetable in the debug.
    /// </summary>
    public void DebugTimetable()
    {
        Debug.Log("SCHEDULED:");
        Debug.Log($"CURRENT DELAY: {this.delay}");
        Debug.Log("STATION:PLT - ARRIVAL - DEPARTURE - PASS");

        foreach(TimetableEntryData d in data.scheduled)
        {
            string l = $"{d.station}:{d.platform} ";

            if(d.arrival.slotCondition != TimeSlotCondition.emptyTime)
            {
                l += $" - {d.arrival.TimeToString()}";
            }
            else
            {
                l += "- __:__:__";
            }

            if (d.departure.slotCondition != TimeSlotCondition.emptyTime)
            {
                l += $" - {d.departure.TimeToString()}";
            }
            else
            {
                l += " - __:__:__";
            }

            if (d.pass.slotCondition != TimeSlotCondition.emptyTime)
            {
                l += $" - {d.pass.TimeToString()}";
            }
            else
            {
                l += " - __:__:__";
            }

            Debug.Log(l);
        }
    }
}