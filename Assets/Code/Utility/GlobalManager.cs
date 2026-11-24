using System.Resources;
using UnityEngine;

/// <summary>
/// A container class for all resource managers and registrys.
/// </summary>
public class GlobalManager : MonoBehaviour
{
    public static GlobalManager instance;

    ResourceRegistry resourceRegistry;
    RouteResourceManager routeResourceManager;
    TimetableResourceManager timetableResourceManager;

    private void Awake()
    {
        if(GlobalManager.instance != null)
        {
            Destroy(GlobalManager.instance);
        }
        else
        {
            GlobalManager.instance = this;
        }
    }

    private void Start()
    {
        this.resourceRegistry = ResourceRegistry.GetInstance();
        this.routeResourceManager = RouteResourceManager.GetInstance();
        this.timetableResourceManager = TimetableResourceManager.GetInstance();

        InitaliseAll();
    }

    /// <summary>
    /// Calls init on all managers and registrys.
    /// </summary>
    private void InitaliseAll()
    {
        resourceRegistry.InitaliseMap();
        routeResourceManager.InitaliseRouteResources();
        timetableResourceManager.InitaliseTimetableResourceManager(routeResourceManager.GetAllRoutes());

        timetableResourceManager.DebugTimetables();
    }
}