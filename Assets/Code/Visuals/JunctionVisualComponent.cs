using UnityEngine;

/// <summary>
/// A junction visual component which extends a node visual base.
/// Represents a junction instance visually.
/// </summary>
public class JunctionVisualComponent : NodeVisualBase
{
    [SerializeField] private GameObject junctionObject;

    private Vector2 diversionNodePosition;
    private Vector2 straightNodePosition;

    /// <summary>
    /// When given junction data, set junction pointer positions by accessing the resource registry.
    /// Resource registry must exist.
    /// </summary>
    /// <param name="junctionData"></param>
    public void SetJunctionPositions(JunctionData junctionData)
    {
        diversionNodePosition = ResourceRegistry.GetInstance().GetNodeByID(junctionData.diversionNodeId).GetPosition();
        straightNodePosition = ResourceRegistry.GetInstance().GetNodeByID(junctionData.straightNodeId).GetPosition();
    }

    /// <summary>
    /// Sets a junctions direction given new junction data.
    /// </summary>
    /// <param name="junctionData">The new junction data.</param>
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