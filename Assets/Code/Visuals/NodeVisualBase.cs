using TMPro;
using UnityEngine;

//The interface which the node visual base class must implement.
interface INodeVisualBase
{
    void SetNodeVisualComponent();
    void SetNodeText(NodeData data);
}

/// <summary>
/// A component which is attacthed to game objects to show node instances visually.
/// Is a super class for other node visual components.
/// </summary>
public class NodeVisualBase : MonoBehaviour
{
    protected TextMeshPro textAsset;
    protected SpriteRenderer nodeMarkerRenderer;
    protected GameObject textObject;
    protected GameObject markerObject;

    /// <summary>
    /// Sets all default values of the visual component. Can be overridden.
    /// Note: base must be called first during any additional implementation if overriding.
    /// </summary>
    public virtual void SetNodeVisualComponent()
    {
        textObject = new GameObject("NodeText");
        textObject.transform.SetParent(transform, false);

        textAsset = textObject.AddComponent<TextMeshPro>();
        textAsset.transform.position += new Vector3(0f, 0.55f, 0f);

        markerObject = new GameObject("NodeMarker");
        markerObject.transform.SetParent(transform, false);

        nodeMarkerRenderer = markerObject.AddComponent<SpriteRenderer>();
        nodeMarkerRenderer.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
        nodeMarkerRenderer.sprite = Resources.Load<Sprite>("Sprites/NodeMarkerSprite");
    }

    /// <summary>
    /// Sets the node text from a NodeData object.
    /// </summary>
    /// <param name="data">The data to present.</param>
    public virtual void SetNodeText(NodeData data)
    {
        textAsset.fontSize = 0.6f;
        textAsset.alignment = TextAlignmentOptions.Center;
        textAsset.text = $"{data.id} - M:{data.milage} C:{data.chain}\n{data.description}";
    }

    /// <summary>
    /// Makes text visually present.
    /// </summary>
    public void EnableText()
    {
        textAsset.enabled = true;
    }

    /// <summary>
    /// Makes text hidden.
    /// </summary>
    public void DisableText()
    {
        textAsset.enabled = false;
    }
}