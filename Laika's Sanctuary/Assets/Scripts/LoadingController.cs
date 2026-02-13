using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public Image pantallaNegra;
    public string sceneToLoad = "MainMenu";
    
    [Header("Transiciones")]
    public float duracionEntrada = 1.5f;  // Entrada suave del video
    public float duracionSalida = 1.5f;    // Salida suave
    public float esperaPostVideo = 1.5f;

    void Start()
    {
        StartCoroutine(PlayLoading());
    }

    IEnumerator PlayLoading()
    {
        // ===========================================
        // 1. NEGRO TOTAL (asegurar)
        // ===========================================
        Color colorNegro = pantallaNegra.color;
        colorNegro.a = 1f;
        pantallaNegra.color = colorNegro;
        
        // ===========================================
        // 2. PREPARAR VIDEO (mientras está en negro)
        // ===========================================
        videoPlayer.Prepare();
        yield return new WaitUntil(() => videoPlayer.isPrepared);
        
        // Pequeña pausa en negro
        yield return new WaitForSeconds(0.5f);
        
        // ===========================================
        // 3. 🎬 ENTRADA SUAVE DEL VIDEO
        // ===========================================
        videoPlayer.Play();
        
        float tiempo = 0;
        Color colorVideo = pantallaNegra.color;
        colorVideo.a = 1f;
        pantallaNegra.color = colorVideo;
        
        // Fade OUT del panel negro (aparece video gradualmente)
        while (tiempo < duracionEntrada)
        {
            tiempo += Time.deltaTime;
            colorVideo.a = Mathf.Lerp(1, 0, tiempo / duracionEntrada);
            pantallaNegra.color = colorVideo;
            yield return null;
        }
        
        colorVideo.a = 0;
        pantallaNegra.color = colorVideo;
        
        // ===========================================
        // 4. ESPERAR QUE TERMINE EL VIDEO
        // ===========================================
        yield return new WaitForSeconds((float)videoPlayer.length);
        
        // ===========================================
        // 5. ESPERA ADICIONAL
        // ===========================================
        yield return new WaitForSeconds(esperaPostVideo);
        
        // ===========================================
        // 6. SALIDA SUAVE
        // ===========================================
        tiempo = 0;
        colorVideo = pantallaNegra.color;
        colorVideo.a = 0;
        pantallaNegra.color = colorVideo;
        
        while (tiempo < duracionSalida)
        {
            tiempo += Time.deltaTime;
            colorVideo.a = Mathf.Lerp(0, 1, tiempo / duracionSalida);
            pantallaNegra.color = colorVideo;
            yield return null;
        }
        
        colorVideo.a = 1;
        pantallaNegra.color = colorVideo;
        
        yield return new WaitForSeconds(0.3f);
        
        // ===========================================
        // 7. CARGAR ESCENA
        // ===========================================
        SceneManager.LoadScene(sceneToLoad);
    }
}