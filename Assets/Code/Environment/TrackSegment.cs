using UnityEngine;

//An enum to store the track direction constraint.
public enum TrackDirection
{
    up,
    down,
    bidirectional
}

//An enum to store the track allocation state.
public enum TrackAllocationStatus
{
    free,
    reserved,
    allocated
}

//A struct to store the track segment data.
public struct TrackData
{
    public string trackID;
    public int aID;
    public int bID;
    public int upSpeed;
    public int downSpeed;
    public TrackAllocationStatus allocation;
    public TrackDirection direction;
}

//The interface which TrackSegment must implement.
public interface ITrackSegment
{
    public TrackData GetTrackData();
}


/// <summary>
/// A monobehaviour representing a connection between 2 nodes.
/// Note: Can only be initialised using the unity method.
/// </summary>
public class TrackSegment : MonoBehaviour, ITrackSegment
{
    private GameObject trackSegmentGO;
    private TrackSegmentVisualComponent visualComponent;
    private TrackData data;

    /// <summary>
    /// Sets default values of the track segment. Call once only!
    /// </summary>
    /// <param name="data"></param>
    public void InitializeTrackSegment(TrackData data)
    {
        trackSegmentGO = this.gameObject;
        this.data = data;

        SetPosition();
    }

    /// <summary>
    /// Sets the default visuals of the track segment. Call once only!
    /// </summary>
    public void InitializeVisuals()
    {
        SetTrackVisualComponent();
    }

    /// <summary>
    /// Sets the position of the track segment on the UI.
    /// </summary>
    private void SetPosition()
    {
        ResourceRegistry resourceRegistry = ResourceRegistry.GetInstance();

        Vector2 startPos = resourceRegistry.GetNodeByID(data.aID).GetPosition();
        Vector2 endPos = resourceRegistry.GetNodeByID(data.bID).GetPosition();

        Vector2 midPoint = (startPos + endPos) / 2f;

        float angle = Mathf.Atan2(endPos.y - startPos.y, endPos.x - startPos.x) * Mathf.Rad2Deg;

        trackSegmentGO.transform.position = midPoint;
        trackSegmentGO.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    /// <summary>
    /// Sets the track visual component properties.
    /// </summary>
    private void SetTrackVisualComponent()
    {
        visualComponent = this.gameObject.AddComponent<TrackSegmentVisualComponent>();
        visualComponent.SetTrackSegmentVisualComponent(data);
    }

    /// <summary>
    /// Gets the track data of this track instance.
    /// </summary>
    /// <returns>The track data of this instance.</returns>
    public TrackData GetTrackData()
    {
        return data;
    }

    /// <summary>
    /// Sets the track data from new data.
    /// </summary>
    /// <param name="newData">The new track data.</param>
    public void UpdateTrackData(TrackData newData)
    {
        data = newData;
    }

    /// <summary>
    /// Checks whether the track segment is occupied.
    /// </summary>
    /// <returns>Whether the track segment is free as a bool.</returns>
    public bool IsFree()
    {
        return data.allocation == TrackAllocationStatus.free;
    }
}