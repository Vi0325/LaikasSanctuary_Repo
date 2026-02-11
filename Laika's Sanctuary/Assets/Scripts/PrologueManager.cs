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
    public Button skipButton;      // ← Botón de UI, no GameObject
    public Image fadePanel;
    
    [Header("Configuración")]
    public string nextScene = "Level1";
    public float fadeDuration = 1f;
    public float skipButtonDelay = 3f; // Segundos hasta que aparezca el botón
    
    private enum PrologueState { Playing, Skipping, Finished }
    private PrologueState currentState;
    
    void Start()
    {
        InitializeVideo();
        SetupUI();
        StartPrologue();
    }
    
    void InitializeVideo()
    {
        if (videoPlayer == null)
            videoPlayer = GetComponent<VideoPlayer>();
            
        videoPlayer.clip = prologueClip;
        videoPlayer.loopPointReached += OnVideoEnd;
        videoPlayer.targetTexture = new RenderTexture(1920, 1080, 24);
        videoDisplay.texture = videoPlayer.targetTexture;
    }
    
    void SetupUI()
    {
        // Fade Panel
        if (fadePanel != null)
        {
            fadePanel.gameObject.SetActive(false);
            fadePanel.color = Color.black;
        }
        
        // Botón Skip - empieza OCULTO
        if (skipButton != null)
        {
            skipButton.gameObject.SetActive(false);
            skipButton.onClick.AddListener(SkipPrologue); // Conectar el botón
        }
        
        StartCoroutine(FadeFromBlack());
    }
    
    void StartPrologue()
    {
        currentState = PrologueState.Playing;
        videoPlayer.Play();
        
        // Mostrar botón SKIP después de X segundos
        Invoke(nameof(ShowSkipButton), skipButtonDelay);
    }
    
    void ShowSkipButton()
    {
        if (skipButton != null && currentState == PrologueState.Playing)
        {
            skipButton.gameObject.SetActive(true);
        }
    }
    
    public void SkipPrologue() // ← Público para el botón
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
        SceneManager.LoadScene(nextScene);
    }
    
    IEnumerator FadeFromBlack()
    {
        fadePanel.gameObject.SetActive(true);
        fadePanel.color = Color.black;
        
        float timer = 0;
        while (timer < fadeDuration)
        {
            fadePanel.color = Color.Lerp(Color.black, Color.clear, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        
        fadePanel.color = Color.clear;
        fadePanel.gameObject.SetActive(false);
    }
    
    IEnumerator FadeToBlack()
    {
        fadePanel.gameObject.SetActive(true);
        fadePanel.color = Color.clear;
        
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