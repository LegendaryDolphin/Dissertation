using UnityEngine;

public enum JunctionState
{
    straight,
    diversion
}

public struct JunctionData
{
    public JunctionState state;
    public NodeOperatingDirection junctionDirection;
    public int straightNodeId;
    public int diversionNodeId;
}

public interface IJunction
{
    JunctionData GetJunctionData();
}

public class Junction : NodeBase, IJunction
{
    private JunctionVisualComponent junctionVisualComponent;
    private GameObject junctionGO;
    private JunctionData junctionData;

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

    public void SetJunctionDirection(JunctionState state)
    {
        JunctionData junctionData = this.junctionData;
        junctionData.state = state;
        this.junctionData = junctionData;
        junctionVisualComponent.SetJunction(this.junctionData);
    }

    public void SetJunctionData(JunctionData data)
    {
        this.junctionData = data;
    }

    public JunctionData GetJunctionData()
    {
        return junctionData;
    }
}