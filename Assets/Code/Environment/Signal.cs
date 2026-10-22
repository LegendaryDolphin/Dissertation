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
    aspectSignalYellow,
    aspectDoubleYellow,
    aspectDoubleGreen
}

public struct SignalData
{
    //Signal data goes here
    SignalType signalType;
    SignalAspect aspect;
}

public interface ISignal
{
    SignalData GetSignalData();
}

public class Signal : NodeBase, ISignal
{
    private SignalData signalData;

    public SignalData GetSignalData()
    {
        return signalData;
    }
}
