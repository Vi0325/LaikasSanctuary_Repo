using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections;

public class MenuVideoBackground : MonoBehaviour
{
    [Header("Configuración de Video")]
    public VideoClip videoClip;
    public bool playOnStart = true;
    public bool loopVideo = true;
    
    [Header("Configuración UI")]
    public RawImage rawImage;
    public Vector2Int resolution = new Vector2Int(1920, 1080);
    
    [Header("Transición de Entrada")]
    public Image pantallaNegra;      // Arrastra el Panel negro aquí
    public float duracionFadeIn = 1.5f;  // Duración de la entrada suave
    
    private VideoPlayer videoPlayer;
    private RenderTexture renderTexture;

    void Awake()
    {
        SetupVideoPlayer();
    }

    void Start()
    {
        // 🎬 EMPEZAR CON FADE IN
        if (pantallaNegra != null)
        {
            StartCoroutine(FadeIn());
        }
    }

    void SetupVideoPlayer()
    {
        videoPlayer = gameObject.GetComponent<VideoPlayer>();
        if (videoPlayer == null)
            videoPlayer = gameObject.AddComponent<VideoPlayer>();
        
        // Configurar VideoPlayer
        videoPlayer.clip = videoClip;
        videoPlayer.isLooping = loopVideo;
        videoPlayer.playOnAwake = playOnStart;
        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        videoPlayer.audioOutputMode = VideoAudioOutputMode.None;
        
        // 🎯 Respetar FPS del video
        videoPlayer.skipOnDrop = false;
        videoPlayer.playbackSpeed = 1.0f;
        
        renderTexture = new RenderTexture(resolution.x, resolution.y, 16);
        renderTexture.Create();
        
        videoPlayer.targetTexture = renderTexture;
        
        if (rawImage != null)
        {
            rawImage.texture = renderTexture;
            rawImage.color = Color.white;
        }
        
        videoPlayer.prepareCompleted += OnVideoPrepared;
        
        if (playOnStart)
            videoPlayer.Prepare();
    }

    void OnVideoPrepared(VideoPlayer source)
    {
        videoPlayer.Play();
        Debug.Log($"Video iniciado en: {gameObject.scene.name} - FPS: {videoPlayer.frameRate}");
    }

    // 🎬 FADE IN SUAVE
    IEnumerator FadeIn()
    {
        // Asegurar que empieza en NEGRO TOTAL
        Color colorNegro = pantallaNegra.color;
        colorNegro.a = 1f;
        pantallaNegra.color = colorNegro;
        
        // Esperar 0.2 segundos para asegurar que el video ya se ve
        yield return new WaitForSeconds(0.2f);
        
        // Fade In gradual
        float tiempo = 0;
        Color color = pantallaNegra.color;
        
        while (tiempo < duracionFadeIn)
        {
            tiempo += Time.deltaTime;
            color.a = Mathf.Lerp(1, 0, tiempo / duracionFadeIn);
            pantallaNegra.color = color;
            yield return null;
        }
        
        // Asegurar que queda transparente
        color.a = 0;
        pantallaNegra.color = color;
        
        Debug.Log("🎬 Fade In completado");
    }

    void OnDestroy()
    {
        if (renderTexture != null)
            renderTexture.Release();
        
        if (videoPlayer != null)
            videoPlayer.prepareCompleted -= OnVideoPrepared;
    }

    public void ChangeVideo(VideoClip newClip)
    {
        videoPlayer.clip = newClip;
        videoPlayer.Prepare();
    }
}