using TMPro;
using UnityEngine;

interface INodeVisualBase
{
    void SetNodeVisualComponent();
    void SetNodeText(NodeData data);
}

public class NodeVisualBase : MonoBehaviour
{
    protected TextMeshPro textAsset;
    protected SpriteRenderer nodeMarkerRenderer;
    protected GameObject textObject;
    protected GameObject markerObject;

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

    public virtual void SetNodeText(NodeData data)
    {
        textAsset.fontSize = 0.5f;
        textAsset.alignment = TextAlignmentOptions.Center;
        textAsset.text = $"{data.id} - M:{data.milage} C:{data.chain}\n{data.description}";
    }

    public void EnableText()
    {
        textAsset.enabled = true;
    }

    public void DisableText()
    {
        textAsset.enabled = false;
    }
}