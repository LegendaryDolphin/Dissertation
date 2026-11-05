using UnityEngine;

//An enum containing the junctions state.
public enum JunctionState
{
    straight,
    diversion
}

//A struct representing the junction data.
public struct JunctionData
{
    public JunctionState state;
    public NodeOperatingDirection junctionDirection;
    public int straightNodeId;
    public int diversionNodeId;
}

//The interface which the Junction class must implement.
public interface IJunction
{
    JunctionData GetJunctionData();
}

/// <summary>
/// A type of NodeBase with extended functionality of a Junction.
/// Note: This class extends NodeBase which is a monobehaviour, therefore you cannot create a new instance with the 'new' keyword.
/// </summary>
public class Junction : NodeBase, IJunction
{
    private JunctionVisualComponent junctionVisualComponent;
    private GameObject junctionGO;
    private JunctionData junctionData;

    /// <summary>
    /// Sets the junctions default visuals.
    /// An override of the base implementation.
    /// </summary>
    protected override void SetNodeVisualComponent()
    {
        GameObject junctionPrefab = Resources.Load<GameObject>("Components/Junction");

        junctionGO = Instantiate(junctionPrefab);
        junctionGO.transform.SetParent(this.nodeGO.transform, false);

        junctionVisualComponent = junctionGO.GetComponent<JunctionVisualComponent>();

        SetNodeStraightAndDiversion();
        junctionVisualComponent.SetNodeVisualComponent();
        junctionVisualComponent.SetJunctionPositions(this.junctionData);
        junctionVisualComponent.SetJunction(this.junctionData);
    }

    /// <summary>
    /// Sets the straight and diversion node ids in the junction data. Should be called once on init!
    /// </summary>
    public void SetNodeStraightAndDiversion()
    {
        JunctionData newJunctionData = junctionData;

        foreach(int connection in this.GetNodeData().connections)
        {
            //Must find connections which are not entries.
            if((junctionData.junctionDirection == NodeOperatingDirection.up && connection > this.GetNodeData().id) || (junctionData.junctionDirection == NodeOperatingDirection.down && connection < this.GetNodeData().id))
            {
                //Get the position of the connection and compare the y values to see if it is a straight connection or diversion
                Vector2 connectionPosition = ResourceRegistry.GetInstance().GetNodeByID(connection).GetPosition();
                if(Mathf.Round(connectionPosition.y) == this.GetPosition().y)
                {
                    newJunctionData.straightNodeId = connection;
                }
                else
                {
                    newJunctionData.diversionNodeId = connection;
                }
            }
        }

        this.junctionData = newJunctionData;
    }

    /// <summary>
    /// Sets the junction direction when given a new junction state.
    /// </summary>
    /// <param name="state">The new state (direction).</param>
    public void SetJunctionDirection(JunctionState state)
    {
        JunctionData junctionData = this.junctionData;
        junctionData.state = state;
        this.junctionData = junctionData;
        junctionVisualComponent.SetJunction(this.junctionData);
    }

    /// <summary>
    /// Sets the junction data of this instance given new data.
    /// </summary>
    /// <param name="data">The new data.</param>
    public void SetJunctionData(JunctionData data)
    {
        this.junctionData = data;
    }

    /// <summary>
    /// Returns the junction data of this instance.
    /// </summary>
    /// <returns></returns>
    public JunctionData GetJunctionData()
    {
        return junctionData;
    }
}