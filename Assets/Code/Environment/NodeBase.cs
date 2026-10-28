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

//Used for junctions and signals
public enum NodeOperatingDirection
{
    up,
    down,
    bidirectional
}

public interface INode
{
    void InitializeNode(NodeData data, int layoutRow, int layoutColumn);
    void InitializeVisuals();
}

public class NodeBase: MonoBehaviour, INode
{
    private const float LAYOUT_SCALING_VALUE_X = 1f; //This value is used to scale the layout row number to world position. Adjust as needed.
    private const float LAYOUT_SCALING_VALUE_Y = 1f; //This value is used to scale the layout row number to world position. Adjust as needed.

    private const float LAYOUT_REALISTIC_SCALING_X = 12f;

    protected Vector2 worldPosition;
    protected GameObject nodeGO;
    protected NodeVisualBase visualComponent;
    protected NodeData data;

    public virtual void InitializeNode(NodeData data, int layoutRowNumber, int layoutColumn)
    {
        nodeGO = this.gameObject;
        this.data = data;

        SetPosition(data.milage, data.chain, layoutRowNumber, layoutColumn);
    }

    public void InitializeVisuals()
    {
        SetNodeVisualComponent();
    }

    protected virtual void SetNodeVisualComponent()
    {
        visualComponent = nodeGO.AddComponent<NodeVisualBase>();
        visualComponent.SetNodeVisualComponent();
        visualComponent.SetNodeText(this.data);
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
