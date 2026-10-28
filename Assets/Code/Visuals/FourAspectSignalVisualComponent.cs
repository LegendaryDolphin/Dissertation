using UnityEngine;
using UnityEngine.Rendering;

public class FourAspectSignalVisualComponent : SignalVisualComponent
{
    [SerializeField] private SpriteRenderer topBulb;
    [SerializeField] private SpriteRenderer upperMiddleBulb;
    [SerializeField] private SpriteRenderer lowerMiddleBulb;
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
                topBulb.color = off;
                upperMiddleBulb.color = off;
                lowerMiddleBulb.color = off;
                bottomBulb.color = red;
                break;
            case SignalAspect.aspectSingalYellow:
                topBulb.color = off;
                upperMiddleBulb.color = off;
                lowerMiddleBulb.color = yellow;
                bottomBulb.color = off;
                break;
            case SignalAspect.aspectDoubleYellow:
                topBulb.color = yellow;
                upperMiddleBulb.color = off;
                lowerMiddleBulb.color = yellow;
                bottomBulb.color = off;
                break;
            case SignalAspect.aspectGreen:
                topBulb.color = green;
                upperMiddleBulb.color = off;
                lowerMiddleBulb.color = off;
                bottomBulb.color = off;
                break;
            default:
                topBulb.color = error;
                upperMiddleBulb.color = error;
                lowerMiddleBulb.color = error;
                bottomBulb.color = error;
                Debug.LogError("Signal visual recieved an incorrect aspect type");
                break;
        }
    }
}