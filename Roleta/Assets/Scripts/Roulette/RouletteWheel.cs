using System.Collections;
using UnityEngine;

public class RouletteWheel : Subject
{
    // Serialized fields for inspector tweaking
    [Header("Roulette Settings")]
    [SerializeField] private int numberOfColors = 4;
    [SerializeField] private float timeOfRotation;
    [SerializeField] private float numberOfCircleRotations;
    [SerializeField] private AnimationCurve rotateCurve;

    // Constants and variables for internal calculations
    private const float Circle = 360.0f;
    private float _angleOfOneColor;
    private float _currentTime;

    // Reference to the RectTransform component of the roulette wheel
    private RectTransform _rouletteRectTransform;

    // Enumeration representing possible colors
    public enum ColorEnum { Red, Green, Yellow, Blue, Purple, Orange, Pink, Brown, Cyan, Gray }

    // Array for color mappings
    [Header("Color Mapping")]
    public ColorMapping[] colorMappings;

    
    // Serializable class to define a mapping between ColorEnum, actual Color, and associated message.
    // This allows easy association of colors with specific meanings or labels in Unity Inspector.
    [System.Serializable]
    public class ColorMapping
    {
        public ColorEnum colorEnum;
        public Color color;
        public string message;
    }
    
    private void Start()
    {
        _angleOfOneColor = Circle / numberOfColors;
        _rouletteRectTransform = GetComponent<RectTransform>();
    }

    // Coroutine for rotating the roulette wheel
    IEnumerator RotateRoulette()
    {
        float startAngle = transform.eulerAngles.z;
        _currentTime = 0;

        // Select a random color index
        int indexColorRandom = Random.Range(0, numberOfColors);
        ColorEnum colorMap = (ColorEnum)indexColorRandom;

        // Retrieve color and message from the mappings
        Color color = GetColorFromMapping(colorMap);
        string finalMessage = GetMessageFromMapping(colorMap);

        // Calculate the final angle for rotation
        float finalAngle = (numberOfCircleRotations * Circle) + _angleOfOneColor * indexColorRandom - startAngle;

        var wait = new WaitForEndOfFrame();
        while (_currentTime < timeOfRotation)
        {
            // Update the rotation angle over time using AnimationCurve
            _currentTime += Time.deltaTime;
            float currentAngle = finalAngle * rotateCurve.Evaluate(_currentTime / timeOfRotation);
            _rouletteRectTransform.localEulerAngles = new Vector3(0, 0, currentAngle + startAngle);
            yield return wait;
        }

        // Notify observers with the selected color and message
        NotifyObservers(color, finalMessage);
    }

    // Method to trigger the roulette rotation
    public void RotateNow()
    {
        StartCoroutine(RotateRoulette());
    }

    // Helper method to get color from the color mappings
    private Color GetColorFromMapping(ColorEnum colorEnum)
    {
        foreach (var mapping in colorMappings)
        {
            if (mapping.colorEnum == colorEnum)
            {
                return mapping.color;
            }
        }
        return Color.white;
    }

    // Helper method to get message from the color mappings
    private string GetMessageFromMapping(ColorEnum colorEnum)
    {
        foreach (var mapping in colorMappings)
        {
            if (mapping.colorEnum == colorEnum)
            {
                return mapping.message;
            }
        }
        return string.Empty;
    }
}
