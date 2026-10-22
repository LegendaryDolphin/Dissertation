using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class TrackSegmentVisualComponent : MonoBehaviour
{
    private SpriteRenderer trackSegmentRenderer;
    private BoxCollider2D trackSegmentCollider;
    private TextMeshPro textAsset;

    public void SetTrackSegmentVisualComponent(TrackData data)
    {
        GameObject textObject = new GameObject("TrackText");
        textObject.transform.SetParent(transform, false);

        textAsset = textObject.AddComponent<TextMeshPro>();
        textAsset.transform.position += new Vector3(0f, 0.2f, 0f);
        textAsset.color = Color.yellow;
        textAsset.enabled = false;

        GameObject trackSegmentObject = new GameObject("TrackSegment");
        trackSegmentObject.transform.SetParent(transform, false);

        trackSegmentRenderer = trackSegmentObject.AddComponent<SpriteRenderer>();
        trackSegmentRenderer.sprite = Resources.Load<Sprite>("Sprites/TrackSegmentSprite");

        trackSegmentCollider = this.gameObject.AddComponent<BoxCollider2D>();
        trackSegmentCollider.enabled = true;
        trackSegmentCollider.isTrigger = true;

        SetTrackLength(data);
        SetTrackText(data);
    }

    private void SetTrackText(TrackData data)
    {
        string text;

        if(data.direction == TrackDirection.up)
        {
            text = $"< {data.upSpeed} |";
        }
        else if(data.direction == TrackDirection.down)
        {
            text = $"| {data.downSpeed} >";
        }
        else
        {
            text = $"< {data.upSpeed} | {data.downSpeed} >";
        }

        textAsset.fontSize = 0.8f;
        textAsset.alignment = TextAlignmentOptions.Center;
        textAsset.text = text;
    }

    private void SetTrackLength(TrackData data)
    {
        ResourceRegistry resourceRegistry = ResourceRegistry.GetInstance();

        Vector2 startPos = resourceRegistry.GetNodeByID(data.aID).GetPosition();
        Vector2 endPos = resourceRegistry.GetNodeByID(data.bID).GetPosition();

        float length = Vector2.Distance(startPos, endPos);

        trackSegmentRenderer.transform.localScale = new Vector3(length, 0.05f, 1f);
        trackSegmentCollider.size = new Vector2(length, 0.1f);
    }

    private void OnMouseEnter()
    {
        textAsset.enabled = true;
        trackSegmentRenderer.color = Color.yellow;
    }

    private void OnMouseExit()
    {
        textAsset.enabled = false;
        trackSegmentRenderer.color = Color.white;
    }
}