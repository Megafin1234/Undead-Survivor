using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject mainMenuPanel;  
    public GameObject tutorialPanel;  
    public Button storyButton;    
    public Button storyEndButton;  
    public Button goBackButton;    
    public GameObject nightPhaseText; 
    public GameObject dayPhaseText;    
    public Image fadeImage;     
    public CanvasGroup fadeCanvasGroup;   
    public AnimationCurve fadeCurve;
    public Image nightEffect;
    public GameObject clickEffectA;
    public GameObject clickEffectB;
    public GameObject clickEffectC;
    void Awake()
    {
        instance = this;
        fadeCanvasGroup.alpha = 0;
    }

    public void GameStart()  //작동안함. 수동으로 설정중
    {
        storyButton.onClick.AddListener(ShowTutorial);
        storyEndButton.onClick.AddListener(TutorialEnd);
        goBackButton.onClick.AddListener(ShowMainMenu);
        nightPhaseText.gameObject.SetActive(false);
        dayPhaseText.gameObject.SetActive(false);
        fadeImage.gameObject.SetActive(false);  
        nightEffect.gameObject.SetActive(false); 
    } 


    public void ShowMainMenu()
    {
        clickEffectB.SetActive(true);
        StartCoroutine(BackDelay());
    }
    private IEnumerator BackDelay(){
        yield return new WaitForSeconds(0.5f); 
        mainMenuPanel.SetActive(true);
        tutorialPanel.SetActive(false);
        clickEffectB.SetActive(false);
    }
    public void ShowTutorial()
    {
        clickEffectA.SetActive(true);
        StartCoroutine(TutDelay());
    }

    private IEnumerator TutDelay()
    {
        yield return new WaitForSeconds(0.5f); 
        mainMenuPanel.SetActive(false);
        tutorialPanel.SetActive(true);
        clickEffectA.SetActive(false);
    }

    public void TutorialEnd()
    {
        clickEffectC.SetActive(true);
        StartCoroutine(EndDelay());
    }
        private IEnumerator EndDelay()
    {
        yield return new WaitForSeconds(0.5f); 
        mainMenuPanel.SetActive(false);
        tutorialPanel.SetActive(false);
        clickEffectC.SetActive(false);
    }

    public void FadeOut(System.Action callback)
    {
        StartCoroutine(Fade(0, 1, 4f, callback)); 
    }

    public void FadeIn(System.Action callback)
    {
        StartCoroutine(Fade(1, 0, 1f, callback)); 
    }



    private System.Collections.IEnumerator Fade(float startAlpha, float endAlpha, float duration, System.Action callback)
    {
        float fadeTime = 0;

        while (fadeTime < duration)
        {
            fadeTime += Time.deltaTime;
            float t = fadeTime / duration;
            float curveValue = fadeCurve.Evaluate(t);
            fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, curveValue); 
            yield return null;
        }

        fadeCanvasGroup.alpha = endAlpha; 

        if (endAlpha == 0)
        {
            fadeCanvasGroup.blocksRaycasts = true; 
        }
        else
        {
            fadeCanvasGroup.blocksRaycasts = false; 
        }

        callback?.Invoke(); 
    } 



    public void ShowDayPhaseText()
    {
        dayPhaseText.gameObject.SetActive(true);
        Invoke("HidePhaseText", 5f); 
    }

    public void ShowNightPhaseText()
    {
        nightPhaseText.gameObject.SetActive(true);
        Invoke("HidePhaseText", 5f);  
    }
    public void HidePhaseText()
    {
        nightPhaseText.gameObject.SetActive(false);  
        dayPhaseText.gameObject.SetActive(false);
    }

        public void NightEffect()
    {
        nightEffect.gameObject.SetActive(true);
    }
            public void DayEffect()
    {
        nightEffect.gameObject.SetActive(false);
    }

}

