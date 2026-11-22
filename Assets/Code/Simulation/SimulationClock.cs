using System;
using Unity.VisualScripting;
using UnityEngine;

public enum ClockMultiplier
{
    none = 1,
    two = 2,
    five = 5,
    ten = 10,
    fifteen = 15,
    twenty = 20,
}

public class SimulationClock : MonoBehaviour
{
    private static SimulationClock instance;

    private int hours;
    private int minutes;
    private int seconds;
    private float milliseconds = 0f;
    [SerializeField] private ClockMultiplier clockMultiplier = ClockMultiplier.none;

    [SerializeField] private bool isRunning = false;

    [SerializeField] private int simulationStartHours = 4;
    [SerializeField] private int simulationStartMinutes = 0;
    [SerializeField] private int simulationStartSeconds = 0;

    public class ClockUpdateEventArgs : EventArgs
    {
        public int seconds;
        public int minutes;
        public int hours;
        public string clockAsString;
    }
    public EventHandler<ClockUpdateEventArgs> clockUpdateEvent;

    public class ClockRunningStateChangeEventArgs : EventArgs
    {
        public bool clockRunningState;
    }
    public EventHandler<ClockRunningStateChangeEventArgs> clockRunningStateChangeEvent;

    public class ClockMultiplerUpdateEventArgs : EventArgs
    {
        public ClockMultiplier multiplierOld;
        public ClockMultiplier multiplierNew;
    }
    public EventHandler<ClockMultiplerUpdateEventArgs> clockMultiplierUpdateEvent;

    private void Awake()
    {
        if (instance == null)
        {
            SimulationClock.instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        hours = simulationStartHours;
        minutes = simulationStartMinutes;
        seconds = simulationStartSeconds;

        clockUpdateEvent?.Invoke(this, new ClockUpdateEventArgs
        {
            seconds = this.seconds,
            minutes = this.minutes,
            hours = this.hours,
            clockAsString = GetClockAsString()
        });

        clockRunningStateChangeEvent?.Invoke(this, new ClockRunningStateChangeEventArgs
        {
            clockRunningState = this.isRunning,
        });

        clockMultiplierUpdateEvent?.Invoke(this, new ClockMultiplerUpdateEventArgs
        {
            multiplierOld = this.clockMultiplier,
            multiplierNew = this.clockMultiplier
        });
    }

    private void Update()
    {
        UpdateClockValues();
    }

    /// <summary>
    /// Updates the instance seconds, mins and hours is the clock instance is running.
    /// </summary>
    private void UpdateClockValues()
    {
        if (!isRunning)
        {
            return;
        }

        milliseconds += Time.deltaTime * 1000f * (float)clockMultiplier;

        if (milliseconds > 1000)
        {
            milliseconds = milliseconds - 1000;

            seconds += 1;

            if (seconds > 59)
            {
                seconds = 0;

                minutes += 1;

                if (minutes > 59)
                {
                    minutes = 0;

                    hours += 1;

                    if (hours > 23)
                    {
                        hours = 0;
                    }
                }
            }

            clockUpdateEvent?.Invoke(this, new ClockUpdateEventArgs
            {
                seconds = this.seconds,
                minutes = this.minutes,
                hours = this.hours,
                clockAsString = GetClockAsString()
            });
        }
    }

    /// <summary>
    /// Concatenates the instances seconds, minutes and hours into a single string.
    /// </summary>
    /// <returns>A string in the form XX:XX:XX</returns>
    public string GetClockAsString()
    {
        string secs;
        string mins;
        string hrs;

        if(this.seconds < 10)
        {
            secs = "0" + this.seconds.ToString();
        }
        else
        {
            secs = this.seconds.ToString();
        }

        if(this.minutes < 10)
        {
            mins = "0" + this.minutes.ToString();
        }
        else
        {
            mins = this.minutes.ToString();
        }

        if(this.hours < 10)
        {
            hrs = "0" + this.hours.ToString();
        }
        else
        {
            hrs = this.hours.ToString();
        }

        return $"{hrs}:{mins}:{secs}";
    }

    /// <summary>
    /// Gets the instance of the singleton system clock.
    /// </summary>
    /// <returns>The system clock isntance</returns>
    /// <exception cref="System.NullReferenceException"">Thrown when the singleton does not exist.</exception>
    public static SimulationClock GetInstance()
    {
        if(SimulationClock.instance == null)
        {
            throw new System.NullReferenceException("The system clock instance is null!");
        }

        return instance;
    }

    /// <summary>
    /// Takes an enum to update the clock multiplier field. This speeds up or slows down the time being elapsed.
    /// </summary>
    /// <param name="clockMultiplier">The new clock multiplier</param>
    public void ChangeClockSpeed(ClockMultiplier clockMultiplier)
    {
        ClockMultiplerUpdateEventArgs e = new ClockMultiplerUpdateEventArgs()
        {
            multiplierOld = this.clockMultiplier
        };

        this.clockMultiplier = clockMultiplier; //Update multipler here

        e.multiplierNew = this.clockMultiplier;

        clockMultiplierUpdateEvent?.Invoke(this, e);
    }

    /// <summary>
    /// Stops the running clock.
    /// </summary>
    public void PauseClock()
    {
        if (this.isRunning)
        {
            this.isRunning = false;

            clockRunningStateChangeEvent?.Invoke(this, new ClockRunningStateChangeEventArgs
            {
                clockRunningState = this.isRunning
            });
        }
    }

    /// <summary>
    /// Resumes the clock if it is not already running.
    /// </summary>
    public void ResumeClock()
    {
        if (!this.isRunning)
        {
            this.isRunning = true;

            clockRunningStateChangeEvent?.Invoke(this, new ClockRunningStateChangeEventArgs
            {
                clockRunningState = this.isRunning
            });
        }
    }

    //UI CAN CALL THESE FUNCTIONS
    public void SetClockSpeed1() { ChangeClockSpeed(ClockMultiplier.none); }
    public void SetClockSpeed2() { ChangeClockSpeed(ClockMultiplier.two); }
    public void SetClockSpeed5() { ChangeClockSpeed(ClockMultiplier.five); }
    public void SetClockSpeed10() { ChangeClockSpeed(ClockMultiplier.ten); }
    public void SetClockSpeed15() { ChangeClockSpeed(ClockMultiplier.fifteen); }
    public void SetClockSpeed20() { ChangeClockSpeed(ClockMultiplier.twenty); }
}