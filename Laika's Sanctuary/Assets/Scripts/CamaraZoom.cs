using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;  // ✅ NECESARIO PARA Image

public class CamaraZoom : MonoBehaviour
{
    [Header("REFERENCIAS")]
    public Camera camara;
    public Transform planeta;
    
    [Header("ZOOM")]
    public float velocidadZoom = 2f;
    public float zoomFinal = 3f;
    
    [Header("TIEMPOS")]
    public float esperaInicial = 1f;
    public float tiempoQuieto = 2f;
    public float tiempoFadeOut = 1.5f;
    
    [Header("TRANSICIÓN")]
    public Image pantallaNegra;          // 🔴 AHORA SÍ LO RECONOCE
    public string siguienteEscena = "RestArea";
    
    private float timer;
    private bool zoomCompleto = false;
    private bool transicionIniciada = false;

    void Start()
    {
        if (camara == null)
            camara = Camera.main;
        
        timer = esperaInicial;
        
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayMusic(AudioManager.Instance.musicaExoPlaneta, "ExoPlaneta");
        
        if (pantallaNegra != null)
        {
            Color c = pantallaNegra.color;
            c.a = 0;
            pantallaNegra.color = c;
        }
    }

    void Update()
    {
        if (timer > 0 && !zoomCompleto)
        {
            timer -= Time.deltaTime;
            return;
        }
        
        if (!zoomCompleto)
        {
            if (camara.orthographicSize > zoomFinal)
            {
                camara.orthographicSize -= velocidadZoom * Time.deltaTime;
                
                if (camara.orthographicSize <= zoomFinal)
                {
                    camara.orthographicSize = zoomFinal;
                    zoomCompleto = true;
                    timer = tiempoQuieto;
                    Debug.Log("Planeta alcanzado");
                }
            }
        }
        else if (!transicionIniciada)
        {
            timer -= Time.deltaTime;
            
            if (timer <= 0)
            {
                transicionIniciada = true;
                StartCoroutine(IrARestArea());
            }
        }
    }
    
    IEnumerator IrARestArea()
    {
        Debug.Log("Cargando " + siguienteEscena);
        
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayMusic(AudioManager.Instance.musicaRestArea, "Rest Area");
        
        if (pantallaNegra != null)
        {
            float tiempo = 0;
            Color c = pantallaNegra.color;
            c.a = 0;
            pantallaNegra.color = c;
            
            while (tiempo < tiempoFadeOut)
            {
                tiempo += Time.deltaTime;
                c.a = Mathf.Lerp(0, 1, tiempo / tiempoFadeOut);
                pantallaNegra.color = c;
                yield return null;
            }
            
            c.a = 1;
            pantallaNegra.color = c;
        }
        
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(siguienteEscena);
    }
}