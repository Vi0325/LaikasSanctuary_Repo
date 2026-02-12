using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class MenuVideoBackground : MonoBehaviour
{
    [Header("Configuración de Video")]
    public VideoClip videoClip;
    public bool playOnStart = true;
    public bool loopVideo = true;
    
    [Header("Configuración UI")]
    public RawImage rawImage;
    public Vector2Int resolution = new Vector2Int(1920, 1080);
    
    private VideoPlayer videoPlayer;
    private RenderTexture renderTexture;

    void Awake()
    {
        SetupVideoPlayer();
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
        
        // 🎯 CORRECCIÓN: Respetar FPS del video
        videoPlayer.skipOnDrop = false;     // ✅ NO saltar frames
        videoPlayer.playbackSpeed = 1.0f;   // ✅ Velocidad normal
        
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