using UnityEngine;

/// <summary>
/// A two aspect signal which extends from a signal visual component.
/// </summary>
public class TwoAspectSignalVisualComponent : SignalVisualComponent
{
    [SerializeField] private SpriteRenderer topBulb;
    [SerializeField] private SpriteRenderer bottomBulb;

    /// <summary>
    /// Sets node visual component settings for two aspect signals, with base calls.
    /// </summary>
    public override void SetNodeVisualComponent()
    {
        base.SetNodeVisualComponent();

        bulbRenderers.transform.position += new Vector3(0f, 0.2f, 0f);
    }

    /// <summary>
    /// Sets the signal aspect based on its two aspect implementation.
    /// </summary>
    /// <param name="aspect">The new aspect.</param>
    /// <exception cref="System.Exception">Thrown when a signal is given an unsupported aspect.</exception>
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
                throw new System.Exception("The signal aspect given is not supported by this signal.");
        }
    }
}