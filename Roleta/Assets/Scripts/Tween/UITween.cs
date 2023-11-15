using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITween : Subject
{
    [SerializeField]
    private GameObject title, logo, comecarButton, comecarButtonEdge, roulette, arrow, girarButton, girarButtonEdge;

    private void Start()
    {
        NotifyObservers(ScreenState.InitialScreen);
        LeanTween.scale(title, new Vector3(1f, 1f, 1f), 2).setDelay(.5f).setEase(LeanTweenType.easeOutElastic);

        RawImage rawImage = logo.GetComponent<RawImage>();
        Image image2 = comecarButton.GetComponent<Image>();
        RawImage rawImage3 = comecarButtonEdge.GetComponent<RawImage>();

        List<Graphic> imagesToAnimate = new List<Graphic> { rawImage, image2, rawImage3 };

        AlphaObjectChange(imagesToAnimate, 0f, 1f, 2f, 2f);
    }

    public void ComecarButton()
    {
        NotifyObservers(ScreenState.RouletteScreen);
        StartRouleteScreen();
    }


    public void GirarButton()
    {
        List<Graphic> imagesToAnimate = GetGirarButtonImages();

        LeanTween.rotateAround(roulette, Vector3.forward, -360, 2f).setLoopClamp();
        AlphaObjectChange(imagesToAnimate, 1f, 0f, 1f);
    }

    private void StartRouleteScreen()
    {
        List<GameObject> objectsToExitTheScreen = new List<GameObject> { logo, comecarButton, comecarButtonEdge };
        List<GameObject> objectsToEnterTheScreen = new List<GameObject> { roulette, arrow };

        float exitTargetX = -1200f;
        float enterTargetX = 0f;

        foreach (var obj in objectsToExitTheScreen)
        {
            MoveObject(obj, exitTargetX, .7f, LeanTweenType.easeInBack, true);
        }

        foreach (var obj in objectsToEnterTheScreen)
        {
            MoveObject(obj, enterTargetX, 1f, LeanTweenType.easeOutBack, false, .5f);
        }

        List<Graphic> imagesToAnimate = GetGirarButtonImages();
        AlphaObjectChange(imagesToAnimate, 0f, 1f, 2f, 1.5f);
    }

    private void StartEndingScreen()
    {
        NotifyObservers(ScreenState.EdningScreen);
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
}