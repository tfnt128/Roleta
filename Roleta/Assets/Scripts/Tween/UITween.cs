using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UITween : MonoBehaviour, IObserver
{
    [SerializeField] private GameObject titleRoulette, logoFirstScreen, startButtonObj, startButtonEdge, rouletteObj, rouletteArrow, restartButtonArrow, spinButtonObj, spinButtonEdge, logoLastScreen, finalMessagePos;
    [SerializeField] private Button startButton, spinButton, restartButton;
    [SerializeField] private List<string> roulettePhrases;
    [SerializeField] private TextMeshProUGUI finalMessage, rouletteMessage, buttonSpinTxt;
    [SerializeField] private UnityEvent onRouletteButtonPressed;
    [SerializeField] private RouletteWheel rouletteSubject;

    private readonly float _exitTargetX = -1200f;
    private readonly float _enterTargetX = 0f;

    private void Start()
    {
        DisableButtons();

        if (roulettePhrases.Count < 4)
        {
            Debug.LogError("A lista de frases deve ter pelo menos 4 elementos.");
            return;
        }

        LeanTween.scale(titleRoulette, Vector3.one, 2).setDelay(0.5f).setEase(LeanTweenType.easeOutElastic);

        List<Graphic> imagesToAnimate = GetImagesToAnimate(logoFirstScreen.GetComponent<Graphic>(), startButtonObj.GetComponent<Graphic>(), startButtonEdge.GetComponent<Graphic>());
        AlphaObjectChange(imagesToAnimate, 0f, 1f, 1f, startButton, 2f);
    }

    public void ComecarButton()
    {
        DisableButtons();
        StartRouletteScreen();
    }

    public void GirarButton()
    {
        spinButton.interactable = false;
        onRouletteButtonPressed.Invoke();
        
        List<Graphic> imagesToAnimate = GetGirarButtonImages();
        AlphaObjectChange(imagesToAnimate, 1f, 0f, .5f);

        int indexRandomPhrase = Random.Range(0, roulettePhrases.Count);
        string chosenPhrase = roulettePhrases[indexRandomPhrase];
        rouletteMessage.text = chosenPhrase;

        List<Graphic> textToAnimate = new List<Graphic> { rouletteMessage };
        AlphaObjectChange(textToAnimate, 0f, 1f, 1f, null, 1f);
        
        List<Graphic> buttonTextToAnimate = new List<Graphic> { buttonSpinTxt};
        AlphaObjectChange(buttonTextToAnimate, 1f, 0f, 1f, null);
    }

    public void ButtonToRestart()
    {
        restartButton.interactable = false;
        List<Graphic> arrowToAnimate = GetImagesToAnimate(restartButtonArrow.GetComponent<Image>(), finalMessage, titleRoulette.GetComponent<RawImage>(), logoLastScreen.GetComponent<RawImage>());
        AlphaObjectChange(arrowToAnimate, 1f, 0f, 1f);
        StartCoroutine(RestartScene());
    }

    private void StartRouletteScreen()
    {
        MoveObjects(new List<GameObject> { logoFirstScreen, startButtonObj, startButtonEdge }, _exitTargetX, 0.7f, LeanTweenType.easeInBack, true);
        MoveObjects(new List<GameObject> { rouletteObj, rouletteArrow }, _enterTargetX, 1f, LeanTweenType.easeOutBack, false, 0.5f);

        List<Graphic> imagesToAnimate = GetGirarButtonImages();
        AlphaObjectChange(imagesToAnimate, 0f, 1f, 1f, spinButton, 1.5f);
    }

    private void StartEndingScreen(Color color, string _finalMessage)
    {
        MoveObjects(new List<GameObject> { rouletteObj, rouletteArrow, spinButtonObj }, _exitTargetX, 0.7f, LeanTweenType.easeInBack, true, 1f);
        MoveObjects(new List<GameObject> { logoLastScreen, finalMessagePos }, _enterTargetX, 1f, LeanTweenType.easeOutBack, false, 1.5f);

        RawImage logoRawImage = logoLastScreen.GetComponent<RawImage>();
        logoRawImage.color = color;
        finalMessage.text = _finalMessage;

        List<Graphic> textToAnimate = new List<Graphic> { rouletteMessage };
        AlphaObjectChange(textToAnimate, 1f, 0f, 1f, null);

        List<Graphic> arrowToAnimate = GetImagesToAnimate(restartButtonArrow.GetComponent<Image>());
        AlphaObjectChange(arrowToAnimate, 0, 1f, 1f, restartButton, 2f);
    }

    private void AlphaObjectChange(List<Graphic> imagesToAnimate, float startAlpha, float endAlpha, float duration, Button hasButton = null, float delay = 0f)
    {
        LeanTween.value(startAlpha, endAlpha, duration)
            .setOnUpdate((float alpha) => UpdateAlpha(imagesToAnimate, alpha))
            .setDelay(delay)
            .setOnComplete(() => EnableButton(hasButton));
    }

    private void UpdateAlpha(List<Graphic> imagesToAnimate, float alpha)
    {
        foreach (Graphic image in imagesToAnimate)
        {
            Color currentColor = image.color;
            currentColor.a = alpha;
            image.color = currentColor;
        }
    }

    private void EnableButton(Button button)
    {
        if (button != null)
        {
            button.interactable = true;
        }
    }

    private void MoveObjects(List<GameObject> objects, float targetX, float duration, LeanTweenType easeType, bool callCameraAnchorScript, float delay = 0f)
    {
        foreach (var obj in objects)
        {
            MoveObject(obj, targetX, duration, easeType, callCameraAnchorScript, delay);
        }
    }

    private void MoveObject(GameObject obj, float targetX, float duration, LeanTweenType easeType, bool callCameraAnchorScript, float delay = 0f)
    {
        RectTransform objTransform = obj.GetComponent<RectTransform>();
        LeanTween.moveX(objTransform, targetX, duration)
            .setEase(easeType)
            .setDelay(delay);
    }

    private List<Graphic> GetImagesToAnimate(params Graphic[] graphics)
    {
        List<Graphic> images = new List<Graphic>();
        foreach (var graphic in graphics)
        {
            if (graphic != null)
            {
                images.Add(graphic);
            }
        }
        return images;
    }

    private List<Graphic> GetGirarButtonImages()
    {
        Image image = spinButton.GetComponent<Image>();
        RawImage rawImage2 = spinButtonEdge.GetComponent<RawImage>();

        return GetImagesToAnimate(image, rawImage2);
    }

    private IEnumerator RestartScene()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnNotify(Color color, string finalMessage)
    {
        StartEndingScreen(color, finalMessage);
    }

    private void OnEnable()
    {
        rouletteSubject.AddObserver(this);
    }

    private void OnDisable()
    {
        rouletteSubject.RemoveObserver(this);
    }
    
    private void DisableButtons()
    {
        startButton.interactable = false;
        spinButton.interactable = false;
        restartButton.interactable = false;
    }
}
