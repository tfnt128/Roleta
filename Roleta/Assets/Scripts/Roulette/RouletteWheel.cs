using System.Collections;
using UnityEngine;

public class RouletteWheel : Subject
{
    [Header("Roulette Settings")]
    [SerializeField] private int numberOfColors = 4;
    [SerializeField] private float timeOfRotation;
    [SerializeField] private float numberOfCircleRotations;
    [SerializeField] private AnimationCurve rotateCurve;

    private const float Circle = 360.0f;
    private float _angleOfOneColor;
    private float _currentTime;
    
    private RectTransform _rouletteRectTransform;

    public enum ColorEnum { Red, Green, Yellow, Blue, Purple, Orange, Pink, Brown, Cyan, Gray }

    [Header("Color Mapping")]
    public ColorMapping[] colorMappings;

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

    IEnumerator RotateRoulette()
    {
        float startAngle = transform.eulerAngles.z;
        _currentTime = 0;
        int indexColorRandom = Random.Range(0, numberOfColors);
        ColorEnum colorMap = (ColorEnum)indexColorRandom;
        Color color = GetColorFromMapping(colorMap);
        string finalMessage = GetMessageFromMapping(colorMap);

        float finalAngle = (numberOfCircleRotations * Circle) + _angleOfOneColor * indexColorRandom - startAngle;

        var wait = new WaitForEndOfFrame();
        while (_currentTime < timeOfRotation)
        {
            _currentTime += Time.deltaTime;

            float currentAngle = finalAngle * rotateCurve.Evaluate(_currentTime / timeOfRotation);
            _rouletteRectTransform.localEulerAngles = new Vector3(0, 0, currentAngle + startAngle);
            yield return wait;
        }

        NotifyObservers(color, finalMessage);
    }

    public void RotateNow()
    {
        StartCoroutine(RotateRoulette());
    }

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
