using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITween : MonoBehaviour
{
    [SerializeField] private GameObject title, logo, button, buttonEdge, roulette;
    private void Start()
    {
        LeanTween.scale(title, new Vector3(1f, 1f, 1f), 2).setDelay(.5f).setEase(LeanTweenType.easeOutElastic);
   
        RawImage rawImage = logo.GetComponent<RawImage>();
        Image image2 = button.GetComponent<Image>();
        RawImage rawImage3 = buttonEdge.GetComponent<RawImage>();
        
        List<Graphic> imagesToAnimate = new List<Graphic> { rawImage, image2, rawImage3 };

        LeanTween.value(0f, 1f, 2f)
            .setOnUpdate((float alpha) =>
            {
                foreach (Graphic image in imagesToAnimate)
                {
                    Color currentColor = image.color;
                    currentColor.a = alpha;
                    image.color = currentColor;
                }
            })
            .setDelay(2f);
    }
    
    public void ComecarButton()
    {
        List<GameObject> objectsToExitTheScreen = new List<GameObject> { logo, button, buttonEdge };
        List<GameObject> objectsToEnterTheScreen = new List<GameObject> { roulette };

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
    }
    private void MoveObject(GameObject obj, float targetX, float duration, LeanTweenType easeType, bool callCameraAnchorScript, float delay = 0f)
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
}
