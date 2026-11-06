using TMPro;
using UnityEngine;

/// <summary>
/// A station which extends from a node visual base component.
/// </summary>
public class StationVisualComponent : NodeVisualBase
{
    [SerializeField] private GameObject platformGameObject;

    /// <summary>
    /// Override base implementation to set the node text to include the station's description, id, milage, and chain.
    /// </summary>
    /// <param name="data">The nodes base data.</param>
    public override void SetNodeText(NodeData data)
    {
        base.SetNodeText(data);

        textAsset.text = $"{data.description}\n {data.id} - M:{data.milage} C:{data.chain}";
    }

    /// <summary>
    /// Sets the platform game object placement based on the station data's platform placement side.
    /// </summary>
    /// <param name="stationData">The station instances data.</param>
    public void SetPlatformPlacement(StationData stationData)
    {
        if(stationData.platformPlacementSide == NodeOperatingDirection.up)
        {
            platformGameObject.transform.localPosition = new Vector3(0f, 0.15f, 0f); //Top placement
        }
        else if(stationData.platformPlacementSide == NodeOperatingDirection.down)
        {
            platformGameObject.transform.localPosition = new Vector3(0f, -0.15f, 0f); //Bottom placement
        }
        else
        {
            platformGameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
            Debug.LogWarning($"Station {stationData.stationName} has a bidirectional platform placement, which is not supported. Defaulting to center placement.");
        }
    }
}