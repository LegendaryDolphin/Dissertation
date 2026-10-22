using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class NodeVisualComponent : MonoBehaviour
{
    private TextMeshPro textAsset;
    private SpriteRenderer nodeMarker;

    public void SetNodeVisualComponent()
    {
        GameObject textObject = new GameObject("NodeText");
        textObject.transform.SetParent(transform, false);

        textAsset = textObject.AddComponent<TextMeshPro>();
        textAsset.transform.position += new Vector3(0f, 0.5f, 0f);

        GameObject markerObject = new GameObject("NodeMarker");
        markerObject.transform.SetParent(transform, false);

        nodeMarker = markerObject.AddComponent<SpriteRenderer>();
        nodeMarker.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
        nodeMarker.sprite = Resources.Load<Sprite>("Sprites/NodeMarkerSprite");
    }

    public void SetNodeText(NodeData data)
    {
        textAsset.fontSize = 0.6f;
        textAsset.alignment = TextAlignmentOptions.Center;
        textAsset.text = $"{data.id}\n{data.description}\nType: {data.type}\nMilage: {data.milage}\nChain: {data.chain}";
    }

    public void DisableText()
    {

    }

    public void EnableText()
    {

    }
}