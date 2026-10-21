using TMPro;
using UnityEngine;

public class NodeVisualComponent : MonoBehaviour
{
    private TextMeshPro textAsset;

    public void SetNodeVisualComponent()
    {
        textAsset = this.gameObject.GetComponentInChildren<TextMeshPro>();
    }

    public void SetNodeText(NodeData data)
    {
        textAsset.text = $"{data.name}\n{data.description}\nType: {data.type}\nMilage: {data.milage}\nChain: {data.chain}";
    }
}