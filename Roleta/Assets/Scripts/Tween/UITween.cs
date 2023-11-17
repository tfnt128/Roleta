using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UITween : MonoBehaviour, IObserver
{
    // Serialized fields for inspector tweaking
    [SerializeField] private GameObject titleRoulette, logoFirstScreen, startButtonObj, startButtonEdge, rouletteObj, rouletteArrow, restartButtonArrow, spinButtonObj, spinButtonEdge, logoLastScreen, finalMessagePos;
    [SerializeField] private Button startButton, spinButton, restartButton;
    [SerializeField] private List<string> roulettePhrases;
    [SerializeField] private TextMeshProUGUI finalMessage, rouletteMessage, buttonSpinTxt;
    [SerializeField] private UnityEvent onRouletteButtonPressed;
    [SerializeField] private RouletteWheel rouletteSubject;

    // Constants for target X position to animate enter or exit the screen
    private readonly float _exitTargetX = -1200f;
    private readonly float _enterTargetX = 0f;
    
    private void Start()
    {
        // Disable buttons initially
        DisableButtons();

        // Check if there are enough phrases in the list
        if (roulettePhrases.Count < 4)
        {
            Debug.LogError("The list of phrases must have at least 4 elements.");
            return;
        }

        // Scale animation for the title
        LeanTween.scale(titleRoulette, Vector3.one, 2).setDelay(0.5f).setEase(LeanTweenType.easeOutElastic);

        // Initial alpha animation for the logo and start button
        List<Graphic> imagesToAnimate = GetImagesToAnimate(logoFirstScreen.GetComponent<Graphic>(), startButtonObj.GetComponent<Graphic>(), startButtonEdge.GetComponent<Graphic>());
        AlphaObjectChange(imagesToAnimate, 0f, 1f, 1f, startButton, 2f);
    }

    // Method triggered by "Start" button click
    public void StartButton()
    {
        // Disable buttons and transition to the roulette screen
        DisableButtons();
        StartRouletteScreen();
    }

    // Method triggered by "Spin" button click
    public void SpinButton()
    {
        // Disable spin button, trigger roulette event, and animate elements
        spinButton.interactable = false;
        onRouletteButtonPressed.Invoke();

        List<Graphic> imagesToAnimate = GetSpinButtonImages();
        AlphaObjectChange(imagesToAnimate, 1f, 0f, .5f);

        // Display a random phrase from the list
        int indexRandomPhrase = Random.Range(0, roulettePhrases.Count);
        string chosenPhrase = roulettePhrases[indexRandomPhrase];
        rouletteMessage.text = chosenPhrase;

        // Animate text and button alpha
        List<Graphic> textToAnimate = new List<Graphic> { rouletteMessage };
        AlphaObjectChange(textToAnimate, 0f, 1f, 1f, null, 1f);

        List<Graphic> buttonTextToAnimate = new List<Graphic> { buttonSpinTxt};
        AlphaObjectChange(buttonTextToAnimate, 1f, 0f, 1f, null);
    }

    // Method triggered by "Restart" button click
    public void ButtonToRestart()
    {
        // Disable restart button, animate elements, and restart the scene
        restartButton.interactable = false;
        List<Graphic> arrowToAnimate = GetImagesToAnimate(restartButtonArrow.GetComponent<Image>(), finalMessage, titleRoulette.GetComponent<RawImage>(), logoLastScreen.GetComponent<RawImage>());
        AlphaObjectChange(arrowToAnimate, 1f, 0f, 1f);
        StartCoroutine(RestartScene());
    }
    
    private void StartRouletteScreen()
    {
        // Move objects for transition to the roulette screen
        MoveObjects(new List<GameObject> { logoFirstScreen, startButtonObj, startButtonEdge }, _exitTargetX, 0.7f, LeanTweenType.easeInBack, true);
        MoveObjects(new List<GameObject> { rouletteObj, rouletteArrow }, _enterTargetX, 1f, LeanTweenType.easeOutBack, false, 0.5f);

        List<Graphic> imagesToAnimate = GetSpinButtonImages();
        AlphaObjectChange(imagesToAnimate, 0f, 1f, 1f, spinButton, 1.5f);
    }

    // Method for ending the screen with color and final message
    private void StartEndingScreen(Color color, string _finalMessage)
    {
        // Move objects for transition to the ending screen
        MoveObjects(new List<GameObject> { rouletteObj, rouletteArrow, spinButtonObj }, _exitTargetX, 0.7f, LeanTweenType.easeInBack, true, 1f);
        MoveObjects(new List<GameObject> { logoLastScreen, finalMessagePos }, _enterTargetX, 1f, LeanTweenType.easeOutBack, false, 1.5f);

        RawImage logoRawImage = logoLastScreen.GetComponent<RawImage>();
        logoRawImage.color = color;
        finalMessage.text = _finalMessage;

        // Animate text and button alpha
        List<Graphic> textToAnimate = new List<Graphic> { rouletteMessage };
        AlphaObjectChange(textToAnimate, 1f, 0f, 1f, null);

        List<Graphic> arrowToAnimate = GetImagesToAnimate(restartButtonArrow.GetComponent<Image>());
        AlphaObjectChange(arrowToAnimate, 0, 1f, 1f, restartButton, 2f);
    }

    // Alpha change animation for a list of graphics
    private void AlphaObjectChange(List<Graphic> imagesToAnimate, float startAlpha, float endAlpha, float duration, Button hasButton = null, float delay = 0f)
    {
        LeanTween.value(startAlpha, endAlpha, duration)
            .setOnUpdate((float alpha) => UpdateAlpha(imagesToAnimate, alpha))
            .setDelay(delay)
            .setOnComplete(() => EnableButton(hasButton));
    }

    // Update alpha of a list of graphics
    private void UpdateAlpha(List<Graphic> imagesToAnimate, float alpha)
    {
        foreach (Graphic image in imagesToAnimate)
        {
            Color currentColor = image.color;
            currentColor.a = alpha;
            image.color = currentColor;
        }
    }

    
    // Move a list of objects to a target X position
    private void MoveObjects(List<GameObject> objects, float targetX, float duration, LeanTweenType easeType, bool callCameraAnchorScript, float delay = 0f)
    {
        foreach (var obj in objects)
        {
            MoveObject(obj, targetX, duration, easeType, callCameraAnchorScript, delay);
        }
    }

    // Move a single object to a target X position
    private void MoveObject(GameObject obj, float targetX, float duration, LeanTweenType easeType, bool callCameraAnchorScript, float delay = 0f)
    {
        RectTransform objTransform = obj.GetComponent<RectTransform>();
        LeanTween.moveX(objTransform, targetX, duration)
            .setEase(easeType)
            .setDelay(delay);
    }

    // Get a list of graphics to animate from an array
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

    // Get a list of graphics to animate for the "Spin" button
    private List<Graphic> GetSpinButtonImages()
    {
        Image image = spinButton.GetComponent<Image>();
        RawImage rawImage2 = spinButtonEdge.GetComponent<RawImage>();

        return GetImagesToAnimate(image, rawImage2);
    }

    // Coroutine to restart the scene after a delay
    private IEnumerator RestartScene()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Observer pattern method: called when the roulette wheel notifies
    public void OnNotify(Color color, string finalMessage)
    {
        StartEndingScreen(color, finalMessage);
    }

    // Subscribe to the roulette wheel as an observer when enabled
    private void OnEnable()
    {
        rouletteSubject.AddObserver(this);
    }

    // Unsubscribe from the roulette wheel when disabled
    private void OnDisable()
    {
        rouletteSubject.RemoveObserver(this);
    }

    // Disable all buttons
    private void DisableButtons()
    {
        startButton.interactable = false;
        spinButton.interactable = false;
        restartButton.interactable = false;
    }
    
    // Enable a button
    private void EnableButton(Button button)
    {
        if (button != null)
        {
            button.interactable = true;
        }
    }

}
