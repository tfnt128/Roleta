using UnityEngine;

public class ScreenChanges : MonoBehaviour, IObserver
{
    [SerializeField] private Subject _screenSubject;
    public void OnNotify(ScreenState state)
    {
        switch (state)
        {
            case ScreenState.InitialScreen:
                Debug.Log("Initial Screen Started");
                break;

            case ScreenState.RouletteScreen:
                Debug.Log("Roulette Screen Started");
                break;

            case ScreenState.EdningScreen:
                Debug.Log("Ending Screen Started");
                break;
        }
    }

    private void OnEnable()
    {
        _screenSubject.AddObserver(this);
    }

    private void OnDisable()
    {
        
        _screenSubject.RemoveObserver(this);
    }
}
