using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PrologueManager : MonoBehaviour
{
    // Referencias en Inspector
    [Header("Video")]
    public VideoPlayer videoPlayer;
    public VideoClip prologueClip;
    
    [Header("UI")]
    public RawImage videoDisplay;
    public Text subtitleText;
    public GameObject skipPrompt;
    public Image fadePanel;
    
    [Header("Configuración")]
    public string nextScene = "Level1";
    public float fadeDuration = 1f;
    
    // Estados
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
        videoPlayer.clip = prologueClip;
        videoPlayer.loopPointReached += OnVideoEnd;
        videoPlayer.targetTexture = new RenderTexture(1920, 1080, 24);
        videoDisplay.texture = videoPlayer.targetTexture;
    }
    
    void SetupUI()
    {
        skipPrompt.SetActive(false);
        subtitleText.text = "";
        fadePanel.color = Color.black;
        
        // Fade in
        StartCoroutine(FadeFromBlack());
    }
    
    void StartPrologue()
    {
        currentState = PrologueState.Playing;
        videoPlayer.Play();
        
        // Mostrar opción de skip después de 5 segundos
        Invoke("ShowSkipOption", 5f);
    }
    
    void Update()
    {
        if (currentState == PrologueState.Playing)
        {
            HandleInput();
            UpdateSubtitles(); // Lógica de subtítulos aquí
        }
    }
    
    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            SkipPrologue();
        }
    }
    
    void ShowSkipOption()
    {
        skipPrompt.SetActive(true);
    }
    
    void SkipPrologue()
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
    
    System.Collections.IEnumerator TransitionToGame()
    {
        // Fade out
        yield return StartCoroutine(FadeToBlack());
        
        // Cargar siguiente escena
        SceneManager.LoadScene(nextScene);
    }
    
    System.Collections.IEnumerator FadeFromBlack()
    {
        float timer = 0;
        Color startColor = Color.black;
        Color endColor = new Color(0, 0, 0, 0);
        
        while (timer < fadeDuration)
        {
            fadePanel.color = Color.Lerp(startColor, endColor, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        
        fadePanel.color = endColor;
    }
    
    System.Collections.IEnumerator FadeToBlack()
    {
        float timer = 0;
        Color startColor = new Color(0, 0, 0, 0);
        Color endColor = Color.black;
        
        while (timer < fadeDuration)
        {
            fadePanel.color = Color.Lerp(startColor, endColor, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        
        fadePanel.color = endColor;
    }
    
    // Método para subtítulos (simplificado)
    void UpdateSubtitles()
    {
        // Tu lógica de subtítulos aquí
        // Puedes usar videoPlayer.time para sincronizar
    }
}