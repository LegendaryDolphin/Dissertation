using UnityEngine;

public enum NodeType
{
    Default,
    Junction,
    Signal,
    Station
}

public struct NodeData
{
    public string name;
    public string description; //can be empty
    public NodeType type;

    public int milage;
    public int chain; //(0 - 79)
}

public interface INode
{
    public NodeData GetData();
}

public abstract class NodeBase: MonoBehaviour, INode
{
    private NodeData data;

    public NodeData GetData()
    {
        return data;
    }
}
