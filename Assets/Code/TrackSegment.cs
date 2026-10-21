using System.Threading;
using UnityEngine;

public struct TrackData
{
    public NodeData nodeA;
    public NodeData nodeB;
    public int maxSpeed;
    public int allocation; //0 - free, 1 - reserved, 2 - allocated
}

public interface ITrackSegment
{
    public TrackData GetTrackData();
}


public class TrackSegment : MonoBehaviour, ITrackSegment
{
    private TrackData data;

    public TrackData GetTrackData()
    {
        return data;
    }

    public bool EqualTo(NodeData a, NodeData b, out TrackSegment trackSegment)
    {
        if((a.name == data.nodeA.name && b.name == data.nodeB.name) || (a.name == data.nodeB.name && b.name == data.nodeA.name))
        {
            trackSegment = this;
            return true;
        }

        trackSegment = null;
        return false;
    }

    public bool IsFree()
    {
        return data.allocation == 0;
    }
}