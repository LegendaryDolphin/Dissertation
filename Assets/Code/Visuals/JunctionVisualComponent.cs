using UnityEngine;

public class JunctionVisualComponent : NodeVisualBase
{
    [SerializeField] private GameObject junctionObject;

    private Vector2 diversionNodePosition;
    private Vector2 straightNodePosition;

    public void SetJunctionPositions(JunctionData junctionData)
    {
        diversionNodePosition = ResourceRegistry.GetInstance().GetNodeByID(junctionData.diversionNodeId).GetPosition();
        straightNodePosition = ResourceRegistry.GetInstance().GetNodeByID(junctionData.straightNodeId).GetPosition();

        //if(junctionData.junctionDirection == NodeOperatingDirection.down)
        //{
        //    junctionObject.transform.position += new Vector3(-0.24f, 0f, 0f);
        //}
    }

    public void SetJunction(JunctionData junctionData)
    {
        Vector2 targetPosition;

        if (junctionData.state == JunctionState.diversion)
        {
            targetPosition = diversionNodePosition;
        }
        else
        {
            targetPosition = straightNodePosition;
        }

        // Calculate direction from the junction to the target node.
        Vector2 direction = targetPosition - (Vector2)junctionObject.transform.position;

        if (direction.sqrMagnitude < 0.0001f)
        {
            return;
        }

        // Rotate around the Z axis to point towards the target.
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        junctionObject.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}