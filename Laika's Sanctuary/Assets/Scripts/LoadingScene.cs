using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingScene : MonoBehaviour
{
    [Header("Configuración de Video")]
    public VideoPlayer videoPlayer;
    public RawImage rawImage;
    public string sceneToLoad = "MainMenu";
    
    [Header("Transición")]
    public Image pantallaNegra;
    public float duracionFade = 1.5f;
    public float esperaPostVideo = 1.5f;
    public float esperaNegroInicial = 0.5f;
    
    private RenderTexture renderTexture;

    void Start()
    {
        ConfigurarVideo();
        StartCoroutine(ReproducirConTransicion());
    }

    void ConfigurarVideo()
    {
        renderTexture = new RenderTexture(1920, 1080, 16);
        renderTexture.Create();
        
        videoPlayer.targetTexture = renderTexture;
        videoPlayer.isLooping = false;
        videoPlayer.playOnAwake = false;
        videoPlayer.skipOnDrop = false;
        videoPlayer.playbackSpeed = 1.0f;
        
        if (rawImage != null)
        {
            rawImage.texture = renderTexture;
            rawImage.color = Color.black;  // 🔴 EMPIEZA EN NEGRO
        }
        
        videoPlayer.Prepare();
    }

    IEnumerator ReproducirConTransicion()
    {
        // ===================================================
        // PASO 0: PANTALLA TOTALMENTE NEGRA
        // ===================================================
        if (pantallaNegra != null)
        {
            Color c = pantallaNegra.color;
            c.a = 1f;
            pantallaNegra.color = c;
        }
        
        yield return new WaitForSeconds(esperaNegroInicial);
        
        // ===================================================
        // PASO 1: ESPERAR VIDEO PREPARADO
        // ===================================================
        yield return new WaitUntil(() => videoPlayer.isPrepared);
        
        // ===================================================
        // PASO 2: ACTIVAR RAWMAGE (YA NO HAY AZUL)
        // ===================================================
        if (rawImage != null)
        {
            rawImage.color = Color.white;  // ✅ RECIÉN AHORA SE VE EL VIDEO
        }
        
        // ===================================================
        // PASO 3: FADE IN
        // ===================================================
        if (pantallaNegra != null)
        {
            float tiempo = 0;
            Color c = pantallaNegra.color;
            
            while (tiempo < duracionFade)
            {
                tiempo += Time.deltaTime;
                c.a = Mathf.Lerp(1, 0, tiempo / duracionFade);
                pantallaNegra.color = c;
                yield return null;
            }
            
            c.a = 0;
            pantallaNegra.color = c;
        }
        
        // ===================================================
        // PASO 4: REPRODUCIR VIDEO
        // ===================================================
        videoPlayer.Play();
        Debug.Log($"🎬 Video iniciado - Duración: {videoPlayer.length}s");
        
        // ===================================================
        // PASO 5: ESPERAR QUE TERMINE
        // ===================================================
        float tiempoInicio = Time.time;
        float duracionVideo = (float)videoPlayer.length;
        
        while (Time.time - tiempoInicio < duracionVideo - 0.1f)
        {
            yield return null;
        }
        
        Debug.Log("🎬 Video terminado");
        
        // ===================================================
        // PASO 6: ESPERA ADICIONAL
        // ===================================================
        yield return new WaitForSeconds(esperaPostVideo);
        
        // ===================================================
        // PASO 7: FADE OUT
        // ===================================================
        if (pantallaNegra != null)
        {
            float tiempo = 0;
            Color c = pantallaNegra.color;
            c.a = 0;
            pantallaNegra.color = c;
            
            while (tiempo < duracionFade)
            {
                tiempo += Time.deltaTime;
                c.a = Mathf.Lerp(0, 1, tiempo / duracionFade);
                pantallaNegra.color = c;
                yield return null;
            }
            
            c.a = 1;
            pantallaNegra.color = c;
        }
        
        yield return new WaitForSeconds(0.3f);
        
        // ===================================================
        // PASO 8: CARGAR ESCENA
        // ===================================================
        Debug.Log($"🎮 Cargando escena: {sceneToLoad}");
        
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);
        asyncLoad.allowSceneActivation = false;
        
        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(0.2f);
        asyncLoad.allowSceneActivation = true;
    }
    
    void OnDestroy()
    {
        if (renderTexture != null)
        {
            renderTexture.Release();
            Destroy(renderTexture);
        }
    }
}