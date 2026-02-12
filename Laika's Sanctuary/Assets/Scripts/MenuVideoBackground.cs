using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class MenuVideoBackground : MonoBehaviour
{
    [Header("Configuración de Video")]
    public VideoClip videoClip;      // Arrastra tu video aquí
    public bool playOnStart = true;
    public bool loopVideo = true;
    
    [Header("Configuración UI")]
    public RawImage rawImage;        // Arrastra tu Raw Image aquí
    public Vector2Int resolution = new Vector2Int(1920, 1080);
    
    private VideoPlayer videoPlayer;
    private RenderTexture renderTexture;

    void Awake()
    {
        SetupVideoPlayer();
    }

    void SetupVideoPlayer()
    {
        // Crear VideoPlayer si no existe
        videoPlayer = gameObject.GetComponent<VideoPlayer>();
        if (videoPlayer == null)
            videoPlayer = gameObject.AddComponent<VideoPlayer>();
        
        // Configurar VideoPlayer
        videoPlayer.clip = videoClip;
        videoPlayer.isLooping = loopVideo;
        videoPlayer.playOnAwake = playOnStart;
        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        videoPlayer.audioOutputMode = VideoAudioOutputMode.None;
        videoPlayer.skipOnDrop = true; // Mejor rendimiento
        
        // Crear RenderTexture
        renderTexture = new RenderTexture(resolution.x, resolution.y, 16);
        renderTexture.Create();
        
        // Asignar al VideoPlayer y RawImage
        videoPlayer.targetTexture = renderTexture;
        
        if (rawImage != null)
        {
            rawImage.texture = renderTexture;
            rawImage.color = Color.white;
        }
        
        // Evento cuando el video está preparado
        videoPlayer.prepareCompleted += OnVideoPrepared;
        
        if (playOnStart)
            videoPlayer.Prepare();
    }

    void OnVideoPrepared(VideoPlayer source)
    {
        videoPlayer.Play();
        Debug.Log($"Video iniciado en: {gameObject.scene.name}");
    }

    void OnDestroy()
    {
        // Limpiar recursos
        if (renderTexture != null)
            renderTexture.Release();
        
        if (videoPlayer != null)
            videoPlayer.prepareCompleted -= OnVideoPrepared;
    }

    // Método público para cambiar video
    public void ChangeVideo(VideoClip newClip)
    {
        videoPlayer.clip = newClip;
        videoPlayer.Prepare();
    }
}