using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RouletteWheel : Subject
{
    public int numberOfColors = 4;
    public float timeOfRotation;
    public float numberOfCircleRotations;

    private const float circle = 360.0f;
    private float angleOfOneColor;

    private float currentTime;

    public AnimationCurve rotateCurve;
    private RectTransform rectTransform;


    private void Start()
    {
        angleOfOneColor = circle / numberOfColors;
        rectTransform = GetComponent<RectTransform>();
    }

    public enum ColorEnum
    {
        Red = 0,
        Green = 1,
        Yellow = 2,
        Blue = 3
    }

    Dictionary<ColorEnum, string> colorStringMapping = new Dictionary<ColorEnum, string>
    {
        { ColorEnum.Red, "Parabéns, você vai se tornar uma milionário" },
        { ColorEnum.Green, "Você ganhará um presente de alguem que menos espera" },
        { ColorEnum.Yellow, "Em breve, algo surpreendente acontecerá em sua vida, fique atento!" },
        { ColorEnum.Blue, "A cor azul traz tranquilidade, prepare-se para momentos serenos e inspiradores!" }
    };

    Dictionary<ColorEnum, Color> colorMapping = new Dictionary<ColorEnum, Color>
    {
        { ColorEnum.Red, Color.red },
        { ColorEnum.Green, Color.green },
        { ColorEnum.Yellow, Color.yellow },
        { ColorEnum.Blue, Color.blue }
    };

    IEnumerator RotateRoulette()
    {
        float startAngle = transform.eulerAngles.z;
        currentTime = 0;
        int indexColorRandom = Random.Range(0, numberOfColors);
        ColorEnum colorMap = (ColorEnum)indexColorRandom;
        Color color = colorMapping[colorMap];
        string finalMessage = colorStringMapping[colorMap];


        float finalAngle = (numberOfCircleRotations * circle) + angleOfOneColor * indexColorRandom - startAngle;

        var wait = new WaitForEndOfFrame();
        while (currentTime < timeOfRotation)
        {
            currentTime += Time.deltaTime;

            float currentAngle = finalAngle * rotateCurve.Evaluate(currentTime / timeOfRotation);
            rectTransform.localEulerAngles = new Vector3(0, 0, currentAngle + startAngle);
            yield return wait;
        }

        NotifyObservers(color, finalMessage);
    }

    public void rotateNow()
    {
        StartCoroutine(RotateRoulette());
    }
}