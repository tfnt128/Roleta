using Color = UnityEngine.Color;

public interface IObserver
{
    public void OnNotify(Color color, string finalMessage);
}
