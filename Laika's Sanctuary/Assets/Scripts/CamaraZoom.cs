using UnityEngine;

public class CamaraZoom : MonoBehaviour
{
    [Header("REFERENCIAS")]
    public Camera camara;
    public Transform planeta;
    
    [Header("ZOOM")]
    public float velocidadZoom = 2f;
    public float zoomFinal = 3f;
    
    [Header("TIEMPO")]
    public float esperaInicial = 1f;
    
    private float timer;

    void Start()
    {
        // Asignar cámara si no se puso
        if (camara == null)
            camara = Camera.main;
        
        timer = esperaInicial;
        
        // 🎵 Activar música de espacio
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayMusic(AudioManager.Instance.musicaEspacio, "🌌 ExoPlaneta");
    }

    void Update()
    {
        // Esperar antes de empezar
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            return;
        }
        
        // Zoom in (acercarse al planeta)
        if (camara.orthographicSize > zoomFinal)
        {
            camara.orthographicSize -= velocidadZoom * Time.deltaTime;
            
            // Limitar al zoom final
            if (camara.orthographicSize < zoomFinal)
                camara.orthographicSize = zoomFinal;
        }
    }
}