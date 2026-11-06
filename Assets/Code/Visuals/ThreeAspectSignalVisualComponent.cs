using UnityEngine;

/// <summary>
/// A three aspect signal which extends from a signal visual component.
/// </summary>
public class ThreeAspectSignalVisualComponent : SignalVisualComponent
{
    [SerializeField] private SpriteRenderer topBulb;
    [SerializeField] private SpriteRenderer middleBulb;
    [SerializeField] private SpriteRenderer bottomBulb;

    /// <summary>
    /// Sets node visual component settings for three aspect signals, with base calls.
    /// </summary>
    public override void SetNodeVisualComponent()
    {
        base.SetNodeVisualComponent();

        bulbRenderers.transform.position += new Vector3(0f, 0.2f, 0f);
    }

    /// <summary>
    /// Sets the signal aspect based on its three aspect implementation.
    /// </summary>
    /// <param name="aspect">The new aspect.</param>
    /// <exception cref="System.Exception">Thrown when a signal is given an unsupported aspect.</exception>
    public override void SetAspect(SignalAspect aspect)
    {
        switch (aspect)
        {
            case SignalAspect.aspectRed:
                topBulb.color = off;
                middleBulb.color = off;
                bottomBulb.color = red;
                break;
            case SignalAspect.aspectSingalYellow:
                topBulb.color = off;
                middleBulb.color = yellow;
                bottomBulb.color = off;
                break;
            case SignalAspect.aspectGreen:
                topBulb.color = green;
                middleBulb.color = off;
                bottomBulb.color = off;
                break;
            default:
                topBulb.color = error;
                middleBulb.color = error;
                bottomBulb.color = error;
                Debug.LogError("Signal visual recieved an incorrect aspect type");
                throw new System.Exception("The signal aspect given is not supported by this signal.");
        }
    }
}