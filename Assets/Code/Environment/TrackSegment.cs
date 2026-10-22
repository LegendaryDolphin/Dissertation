using System.Threading;
using UnityEngine;

public enum TrackDirection
{
    up,
    down,
    bidirectional
}

public enum TrackAllocationStatus
{
    free,
    reserved,
    allocated
}

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

public interface ITrackSegment
{
    public TrackData GetTrackData();
}


public class TrackSegment : MonoBehaviour, ITrackSegment
{
    private GameObject trackSegmentGO;
    private TrackSegmentVisualComponent visualComponent;
    private TrackData data;

    public void InitializeTrackSegment(TrackData data)
    {
        trackSegmentGO = this.gameObject;
        this.data = data;

        SetPosition();
    }

    public void InitializeVisuals()
    {
        SetTrackVisualComponent();
    }

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

    private void SetTrackVisualComponent()
    {
        visualComponent = this.gameObject.AddComponent<TrackSegmentVisualComponent>();
        visualComponent.SetTrackSegmentVisualComponent(data);
    }

    public TrackData GetTrackData()
    {
        return data;
    }

    public void UpdateTrackData(TrackData newData)
    {
        data = newData;
    }

    public bool IsFree()
    {
        return data.allocation == TrackAllocationStatus.free;
    }
}