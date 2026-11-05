using UnityEngine;

//An enum representing the type of signal.
public enum SignalType
{
    aspect2,
    aspect3,
    aspect4
}

//An enum representing the signals state.
public enum SignalAspect
{
    aspectRed,
    aspectSingalYellow,
    aspectDoubleYellow,
    aspectGreen
}

//A struct containing all base data for a signal.
public struct SignalData
{
    public SignalType signalType;
    public SignalAspect aspect;
    public NodeOperatingDirection signalDirection;
}

//The interface which the signal class must implement.
public interface ISignal
{
    SignalData GetSignalData();
}

/// <summary>
/// A type of NodeBase with extended functionality of a signal.
/// Note: This class extends NodeBase which is a monobehaviour, therefore you cannot create a new instance with the 'new' keyword.
/// </summary>
public class Signal : NodeBase, ISignal
{
    private SignalData signalData;
    private SignalVisualComponent signalVisualComponent;
    private GameObject signalGO;

    /// <summary>
    /// Sets the signals default visuals.
    /// An override of the base implementation.
    /// </summary>
    /// <exception cref="System.Exception">Thrown when the signal has an unexpected signal type.</exception>
    protected override void SetNodeVisualComponent()
    {
        GameObject signalPrefab;

        switch (signalData.signalType)
        {
            case SignalType.aspect2:
                signalPrefab = Resources.Load<GameObject>("Components/TwoAspectSignal");
                break;
            case SignalType.aspect3:
                signalPrefab = Resources.Load<GameObject>("Components/ThreeAspectSignal");
                break;
            case SignalType.aspect4:
                signalPrefab = Resources.Load<GameObject>("Components/FourAspectSignal");
                break;
            default:
                signalPrefab = Resources.Load<GameObject>("Components/TwoAspectSignal");
                Debug.LogError("Signal visual instantiation failed. This is because the signal type does not exist. A two aspect was made in replacement.");
                throw new System.Exception("Signal visual instantiation failed. This is because the signal type does not exist. A two aspect was made in replacement.");
        }

        signalGO = Instantiate(signalPrefab);
        signalGO.transform.SetParent(this.nodeGO.transform, false);

        signalVisualComponent = signalGO.GetComponent<SignalVisualComponent>();

        signalVisualComponent.SetNodeVisualComponent();
        signalVisualComponent.SetNodeText(data);
        signalVisualComponent.SetArrow(signalData);
        signalVisualComponent.SetAspect(signalData.aspect);
    }

    /// <summary>
    /// Gets the signal data of the instance.
    /// </summary>
    /// <returns>The signal data of the instance.</returns>
    public SignalData GetSignalData()
    {
        return signalData;
    }

    /// <summary>
    /// Sets the signal data of the instance from a new data object.
    /// </summary>
    /// <param name="signalData">The new data.</param>
    public void SetSignalData(SignalData signalData)
    {
        this.signalData = signalData;
    }

    /// <summary>
    /// Sets the signal aspect to a new aspect given a new aspect.
    /// </summary>
    /// <param name="aspect">The new aspect.</param>
    public void ChangeAspect(SignalAspect aspect)
    {
        SignalData data = GetSignalData();
        data.aspect = aspect;
        SetSignalData(data);
    }
}
