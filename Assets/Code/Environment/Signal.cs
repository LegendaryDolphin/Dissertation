using UnityEditor.Experimental.GraphView;
using UnityEditor.Tilemaps;
using UnityEngine;

public enum SignalType
{
    aspect2,
    aspect3,
    aspect4
}

public enum SignalAspect
{
    aspectRed,
    aspectSingalYellow,
    aspectDoubleYellow,
    aspectGreen
}

public struct SignalData
{
    //Signal data goes here
    public SignalType signalType;
    public SignalAspect aspect;
    public NodeOperatingDirection signalDirection;
}

public interface ISignal
{
    SignalData GetSignalData();
}

public class Signal : NodeBase, ISignal
{
    private SignalData signalData;
    private SignalVisualComponent signalVisualComponent;
    private GameObject signalGO;

    public override void InitializeNode(NodeData data, int layoutRowNumber, int layoutColumn)
    {
        base.InitializeNode(data, layoutRowNumber, layoutColumn);
    }

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
                break;
        }

        signalGO = Instantiate(signalPrefab);
        signalGO.transform.SetParent(this.nodeGO.transform, false);

        signalVisualComponent = signalGO.GetComponent<SignalVisualComponent>();

        signalVisualComponent.SetNodeVisualComponent();
        signalVisualComponent.SetNodeText(data);
        signalVisualComponent.SetArrow(signalData);
        signalVisualComponent.SetAspect(signalData.aspect);
    }

    public SignalData GetSignalData()
    {
        return signalData;
    }

    public void SetSignalData(SignalData signalData)
    {
        this.signalData = signalData;
    }

    public void ChangeAspect(SignalAspect aspect)
    {
        SignalData data = GetSignalData();
        data.aspect = aspect;
        SetSignalData(data);
    }
}
