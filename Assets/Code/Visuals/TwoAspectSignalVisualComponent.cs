using UnityEngine;
using UnityEngine.Rendering;

public class TwoAspectSignalVisualComponent : SignalVisualComponent
{
    [SerializeField] private SpriteRenderer topBulb;
    [SerializeField] private SpriteRenderer bottomBulb;

    public override void SetNodeVisualComponent()
    {
        base.SetNodeVisualComponent();

        bulbRenderers.transform.position += new Vector3(0f, 0.2f, 0f);
    }

    public override void SetAspect(SignalAspect aspect)
    {
        switch (aspect)
        {
            case SignalAspect.aspectRed:
                topBulb.color = red;
                bottomBulb.color = off;
                break;
            case SignalAspect.aspectGreen:
                topBulb.color = green;
                bottomBulb.color = off;
                break;
            default:
                topBulb.color = error;
                bottomBulb.color = error;
                Debug.LogError("Signal visual recieved an incorrect aspect type");
                break;
        }
    }
}