using System.Collections.Generic;
using UnityEngine;


// Abstract class representing a subject in the observer pattern.
// Subjects maintain a list of observers and provide methods to add, remove, and notify them.
public abstract class Subject : MonoBehaviour
{
    private List<IObserver> _observers = new List<IObserver>(); // List to store registered observers.

    // Method to add an observer to the list.
    public void AddObserver(IObserver observer)
    {
        _observers.Add(observer);
    }

    // Method to remove an observer from the list.
    public void RemoveObserver(IObserver observer)
    {
        _observers.Remove(observer);
    }

    // Protected method to notify all registered observers with a color and final message.
    protected void NotifyObservers(Color color, string finalMessage)
    {
        // Iterate through each observer in the list and call their OnNotify method.
        _observers.ForEach((observer) => { observer.OnNotify(color, finalMessage); });
    }
}
