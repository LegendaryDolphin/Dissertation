using UnityEngine;

//A struct to hold station data, such as the station name.
public struct StationData
{
    public string stationName;
    public int platform;
    public NodeOperatingDirection platformPlacementSide;
}

//A routing table struct will be used to help reduce time complexity for path finding algorithms.
public struct RoutingTableData
{
    public string[] upStationNames;
    public string[] downStationNames;
}

//The interface which the station class must implement.
public interface IStation
{
    StationData GetStationData();
    void SetStationData(StationData data);
    bool CheckForStation(string stationName, NodeOperatingDirection operatingDirection);
}

/// <summary>
/// A station class that represents a station in the environment. It inherits from NodeBase and implements the IStation interface. 
/// The class contains methods to set and get station data, as well as check for the existence of a station in the routing table based on the operating direction.
/// </summary>
public class Station : NodeBase, IStation
{
    private StationVisualComponent stationVisualComponent;
    private GameObject stationGO;
    private StationData stationData;
    private RoutingTableData routingTable;

    /// <summary>
    /// Sets the stations default visuals.
    /// An override of the base implementation.
    /// </summary>
    protected override void SetNodeVisualComponent()
    {
        GameObject stationPrefab = Resources.Load<GameObject>("Components/Station");

        stationGO = Instantiate(stationPrefab);
        stationGO.transform.SetParent(this.nodeGO.transform, false);

        stationVisualComponent = stationGO.GetComponent<StationVisualComponent>();

        stationVisualComponent.SetNodeVisualComponent();
        stationVisualComponent.SetNodeText(data);
        stationVisualComponent.SetPlatformPlacement(stationData);
    }

    /// <summary>
    /// Sets the station data for this station instance.
    /// </summary>
    /// <param name="data">The new data.</param>
    public void SetStationData(StationData data)
    {
        this.stationData = data;
    }

    /// <summary>
    /// Returns the station data for this station instance.
    /// </summary>
    /// <returns>The station data.</returns>
    public StationData GetStationData()
    {
        return stationData;
    }

    /// <summary>
    /// Checks whether a station with the given name exists in the routing table for the specified operating direction (up or down).
    /// </summary>
    /// <param name="stationName">The stations name to check.</param>
    /// <param name="operatingDirection">The operating direction to check.</param>
    /// <returns>A true false for whether the station exists.</returns>
    public bool CheckForStation(string stationName, NodeOperatingDirection operatingDirection)
    {
        if(operatingDirection == NodeOperatingDirection.up)
        {
            for(int i = 0; i < routingTable.upStationNames.Length; i++)
            {
                if (routingTable.upStationNames[i] == stationName)
                {
                    return true;
                }
            }
        }
        else
        {
            for(int i = 0; i < routingTable.downStationNames.Length; i++)
            {
                if(routingTable.downStationNames[i] == stationName)
                {
                    return true;
                }
            }
        }

        return false;
    }
}