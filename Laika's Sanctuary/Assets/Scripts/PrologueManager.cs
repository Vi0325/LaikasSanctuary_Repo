using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PrologueManager : MonoBehaviour
{
    [Header("Video")]
    public VideoPlayer videoPlayer;
    public VideoClip prologueClip;
    
    [Header("UI")]
    public RawImage videoDisplay;
    public Button skipButton;
    public Image fadePanel; 

    [Header("Configuración")]
    public string nextScene = "Level1";
    public float fadeDuration = 1f;
    public float skipButtonDelay = 3f;
    public float startDelay = 1.5f;
    
    private enum PrologueState { Playing, Skipping, Finished }
    private PrologueState currentState;
    
    void Start()
    {
        InitializeVideo();
        SetupUI();
        
        videoPlayer.time = 0;
        videoPlayer.frame = 0;
        videoPlayer.Prepare();
        
        // Empezar con FADE IN desde negro
        StartCoroutine(FadeFromBlack());
        
        // Esperar y empezar video
        Invoke(nameof(StartPrologue), startDelay);
    }
    
    void InitializeVideo()
    {
        if (videoPlayer == null)
            videoPlayer = GetComponent<VideoPlayer>();
            
        videoPlayer.clip = prologueClip;
        videoPlayer.loopPointReached += OnVideoEnd;
        videoPlayer.targetTexture = new RenderTexture(1920, 1080, 24);
        videoDisplay.texture = videoPlayer.targetTexture;
        videoPlayer.playOnAwake = false;
        videoPlayer.Pause();
        
        // RawImage visible desde el principio
        videoDisplay.color = Color.white;
    }
    
    void SetupUI()
    {
        if (skipButton != null)
        {
            skipButton.gameObject.SetActive(false);
            skipButton.onClick.AddListener(SkipPrologue);
        }
        
        // FADE PANEL empieza NEGRO y ACTIVO
        if (fadePanel != null)
        {
            fadePanel.gameObject.SetActive(true);
            fadePanel.color = Color.black;
            fadePanel.raycastTarget = false;
        }
    }
    
    IEnumerator FadeFromBlack()
    {
        float timer = 0;
        while (timer < fadeDuration)
        {
            fadePanel.color = Color.Lerp(Color.black, Color.clear, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        
        fadePanel.color = Color.clear;
    }
    
    void StartPrologue()
    {
        currentState = PrologueState.Playing;
        
        videoPlayer.time = 0;
        videoPlayer.frame = 0;
        videoPlayer.Play();
        
        Invoke(nameof(ShowSkipButton), skipButtonDelay);
    }
    
    void ShowSkipButton()
    {
        if (skipButton != null && currentState == PrologueState.Playing)
        {
            skipButton.gameObject.SetActive(true);
        }
    }
    
    public void SkipPrologue()
    {
        if (currentState == PrologueState.Playing)
        {
            currentState = PrologueState.Skipping;
            StartCoroutine(TransitionToGame());
        }
    }
    
    void OnVideoEnd(VideoPlayer vp)
    {
        if (currentState == PrologueState.Playing)
        {
            currentState = PrologueState.Finished;
            StartCoroutine(TransitionToGame());
        }
    }
    
    IEnumerator TransitionToGame()
    {
        yield return StartCoroutine(FadeToBlack());
        yield return new WaitForSeconds(0.2f);
        
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartGame(nextScene);
        }
        else
        {
            SceneManager.LoadScene(nextScene);
        }
    }
    
    IEnumerator FadeToBlack()
    {
        fadePanel.color = Color.clear;
        fadePanel.gameObject.SetActive(true);
        
        float timer = 0;
        while (timer < fadeDuration)
        {
            fadePanel.color = Color.Lerp(Color.clear, Color.black, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        
        fadePanel.color = Color.black;
    }
}