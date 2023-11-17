using Color = UnityEngine.Color;

// Interface defining the contract for an observer in the observer pattern.
public interface IObserver
{
    // Method called by a subject to notify the observer about a change in state, providing color and a final message.
    void OnNotify(Color color, string finalMessage);
}

