using UnityEngine;

//An enum representing a nodes type.
public enum NodeType
{
    Default,
    Junction,
    Signal,
    Station
}

//A struct containing data relevant to all node types.
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

//An enum representing the operating direction of a node.
//Used for junctions and signals
public enum NodeOperatingDirection
{
    up,
    down,
    bidirectional
}

//An interface which NodeBase must inherit.
public interface INode
{
    void InitializeNode(NodeData data, int layoutRow, int layoutColumn);
    void InitializeVisuals();
}

/// <summary>
/// The base for all node types. If a node has no specialised qualities instantiate this.
/// Note: This class extends monobehaviour so cannot be instantiated using the 'new' keyword.
/// </summary>
public class NodeBase: MonoBehaviour, INode
{
    private const float LAYOUT_SCALING_VALUE_X = 1f; //This value is used to scale the layout row number to world position. Adjust as needed.
    private const float LAYOUT_SCALING_VALUE_Y = 1f; //This value is used to scale the layout row number to world position. Adjust as needed.

    private const float LAYOUT_REALISTIC_SCALING_X = 12f;

    protected Vector2 worldPosition;
    protected GameObject nodeGO;
    protected NodeVisualBase visualComponent;
    protected NodeData data;

    /// <summary>
    /// Sets the instances default values for UI position and data. Call once only!
    /// </summary>
    /// <param name="data">The data belonging to the instance.</param>
    /// <param name="layoutRowNumber">The row number of the UI to place the node.</param>
    /// <param name="layoutColumnNumber">The column number of the UI to place the node.</param>
    public virtual void InitializeNode(NodeData data, int layoutRowNumber, int layoutColumnNumber)
    {
        nodeGO = this.gameObject;
        this.data = data;

        SetPosition(data.milage, data.chain, layoutRowNumber, layoutColumnNumber);
    }

    /// <summary>
    /// PUBLIC Sets the instances default visuals.
    /// </summary>
    public void InitializeVisuals()
    {
        SetNodeVisualComponent();
    }

    /// <summary>
    /// PROTECTED Sets the node bases default visuals. This method should be overridden when a new node type is being implemented.
    /// Note: When creating a new implementation, base does not need to be called, as visuals are in their own hierarchy.
    /// </summary>
    protected virtual void SetNodeVisualComponent()
    {
        visualComponent = nodeGO.AddComponent<NodeVisualBase>();
        visualComponent.SetNodeVisualComponent();
        visualComponent.SetNodeText(this.data);
    }

    /// <summary>
    /// Sets the nodes position on the UI, normally based off grid references. However, realistic scaling is possible.
    /// </summary>
    /// <param name="milage"></param>
    /// <param name="chain"></param>
    /// <param name="layoutRow"></param>
    /// <param name="layoutColumn"></param>
    private void SetPosition(int milage, int chain, int layoutRow, int layoutColumn)
    {
        //worldPosition.x = (milage + (chain / 79f)) * LAYOUT_REALISTIC_SCALING_X; //For geographically accurate scaling
        //worldPosition.y = -(layoutRow) * 0.5f;

        worldPosition.x = layoutColumn * LAYOUT_SCALING_VALUE_X;
        worldPosition.y = -(layoutRow * LAYOUT_SCALING_VALUE_Y);

        nodeGO.transform.position = worldPosition;
    }

    /// <summary>
    /// Returns the position of the node in world space.
    /// </summary>
    /// <returns>Position of the node in world space as a 2d vector.</returns>
    public Vector2 GetPosition()
    {
        return worldPosition;
    }

    /// <summary>
    /// Returns the node data of the instance.
    /// </summary>
    /// <returns>The NodeData of the instance.</returns>
    public NodeData GetNodeData()
    {
        return data;
    }

    /// <summary>
    /// Returns the node id of the instance.
    /// </summary>
    /// <returns>The node id as an integer.</returns>
    public int GetNodeID()
    {
        return data.id;
    }
}
