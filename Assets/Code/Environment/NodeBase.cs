using System.Net;
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
    public int id;
    public string description; //can be empty
    public NodeType type;

    public int milage;
    public int chain; //(0 - 79)

    public int[] connections; //Array of connected node IDs
    public int[] speeds; //Array of speeds corresponding to the connections array.
}

public interface INode
{
    void InitializeNode(NodeData data, int layoutRow, int layoutColumn);
}

public abstract class NodeBase: MonoBehaviour, INode
{
    private const float LAYOUT_SCALING_VALUE_X = 1f; //This value is used to scale the layout row number to world position. Adjust as needed.
    private const float LAYOUT_SCALING_VALUE_Y = 1f; //This value is used to scale the layout row number to world position. Adjust as needed.

    private const float LAYOUT_REALISTIC_SCALING_X = 12f;

    private Vector2 worldPosition;
    private GameObject nodeGO;
    private NodeVisualComponent visualComponent;
    private NodeData data;

    public virtual void InitializeNode(NodeData data, int layoutRowNumber, int layoutColumn)
    {
        nodeGO = this.gameObject;
        this.data = data;

        SetPosition(data.milage, data.chain, layoutRowNumber, layoutColumn);
    }

    public virtual void InitializeVisuals()
    {
        SetNodeVisualComponent(data);
    }

    protected virtual void SetNodeVisualComponent(NodeData data)
    {
        visualComponent = nodeGO.AddComponent<NodeVisualComponent>();
        visualComponent.SetNodeVisualComponent();
        visualComponent.SetNodeText(data);
    }

    private void SetPosition(int milage, int chain, int layoutRow, int layoutColumn)
    {
        //worldPosition.x = (milage + (chain / 79f)) * LAYOUT_REALISTIC_SCALING_X; //For geographically accurate scaling
        //worldPosition.y = -(layoutRow) * 0.5f;

        worldPosition.x = layoutColumn * LAYOUT_SCALING_VALUE_X;
        worldPosition.y = -(layoutRow * LAYOUT_SCALING_VALUE_Y);

        nodeGO.transform.position = worldPosition;
    }

    public Vector2 GetPosition()
    {
        return worldPosition;
    }

    public NodeData GetNodeData()
    {
        return data;
    }

    public int GetNodeID()
    {
        return data.id;
    }
}
