using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UITween : MonoBehaviour, IObserver
{
    [SerializeField] private GameObject title,
        logo,
        comecarButton,
        comecarButtonEdge,
        roulette,
        arrow,
        girarButton,
        girarButtonEdge,
        logoColor,
        finalMessagePos;

    [SerializeField] private TextMeshProUGUI _finalMessage;

    [SerializeField] private UnityEvent onRouletteButtonPressed;

    [SerializeField] private RouletteWheel _rouletteSubject;

    private float _exitTargetX = -1200f;
    private float _enterTargetX = 0f;

    private void Start()
    {
        LeanTween.scale(title, new Vector3(1f, 1f, 1f), 2).setDelay(.5f).setEase(LeanTweenType.easeOutElastic);

        RawImage rawImage = logo.GetComponent<RawImage>();
        Image image2 = comecarButton.GetComponent<Image>();
        RawImage rawImage3 = comecarButtonEdge.GetComponent<RawImage>();

        List<Graphic> imagesToAnimate = new List<Graphic> { rawImage, image2, rawImage3 };

        AlphaObjectChange(imagesToAnimate, 0f, 1f, 2f, 2f);
    }

    public void ComecarButton()
    {
        LeanTween.cancelAll();
        StartRouletteScreen();
    }

    public void GirarButton()
    {
        onRouletteButtonPressed.Invoke();
        LeanTween.cancelAll();
        List<Graphic> imagesToAnimate = GetGirarButtonImages();
        AlphaObjectChange(imagesToAnimate, 1f, 0f, .5f);
    }


    private void StartRouletteScreen()
    {
        List<GameObject> objectsToExitTheScreen = new List<GameObject> { logo, comecarButton, comecarButtonEdge };
        List<GameObject> objectsToEnterTheScreen = new List<GameObject> { roulette, arrow };


        foreach (var obj in objectsToExitTheScreen)
        {
            MoveObject(obj, _exitTargetX, .7f, LeanTweenType.easeInBack, true);
        }

        foreach (var obj in objectsToEnterTheScreen)
        {
            MoveObject(obj, _enterTargetX, 1f, LeanTweenType.easeOutBack, false, .5f);
        }

        List<Graphic> imagesToAnimate = GetGirarButtonImages();
        AlphaObjectChange(imagesToAnimate, 0f, 1f, 2f, 1.5f);
    }

    private void StartEndingScreen(Color color, string finalMessage)
    {
        List<GameObject> objectsToExitTheScreen = new List<GameObject> { roulette, arrow, girarButton };
        List<GameObject> objectsToEnterTheScreen = new List<GameObject> { logoColor, finalMessagePos };
        RawImage logoRawImage = logoColor.GetComponent<RawImage>();
        logoRawImage.color = color;
        _finalMessage.text = finalMessage;


        foreach (var obj in objectsToExitTheScreen)
        {
            MoveObject(obj, _exitTargetX, .7f, LeanTweenType.easeInBack, true, 1f);
        }

        foreach (var obj in objectsToEnterTheScreen)
        {
            MoveObject(obj, _enterTargetX, 1f, LeanTweenType.easeOutBack, false, 1.5f);
        }
    }


    private void AlphaObjectChange(List<Graphic> imagesToAnimate, float startAlpha, float endAlpha, float duration,
        float delay = 0f)
    {
        LeanTween.value(startAlpha, endAlpha, duration)
            .setOnUpdate((float alpha) =>
            {
                foreach (Graphic image in imagesToAnimate)
                {
                    Color currentColor = image.color;
                    currentColor.a = alpha;
                    image.color = currentColor;
                }
            })
            .setDelay(delay);
    }

    private void MoveObject(GameObject obj, float targetX, float duration, LeanTweenType easeType,
        bool callCameraAnchorScript, float delay = 0f)
    {
        RectTransform objTransform = obj.GetComponent<RectTransform>();
        LeanTween.moveX(objTransform, targetX, duration)
            .setEase(easeType)
            .setDelay(delay)
            .setOnComplete(() => CameraAnchorScriptCall(obj, callCameraAnchorScript));
    }

    private void CameraAnchorScriptCall(GameObject obj, bool enable)
    {
        CameraAnchor script = obj.GetComponent<CameraAnchor>();
        if (script != null)
        {
            script.enabled = enable ? false : true;
        }
    }

    private List<Graphic> GetGirarButtonImages()
    {
        Image image = girarButton.GetComponent<Image>();
        RawImage rawImage2 = girarButtonEdge.GetComponent<RawImage>();

        return new List<Graphic> { image, rawImage2 };
    }

    public void OnNotify(Color color, string finalMessage)
    {
        StartEndingScreen(color, finalMessage);
    }

    private void OnEnable()
    {
        _rouletteSubject.AddObserver(this);
    }

    private void OnDisable()
    {
        _rouletteSubject.RemoveObserver(this);
    }
}