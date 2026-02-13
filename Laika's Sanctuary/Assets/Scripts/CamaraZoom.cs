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
        if (camara == null)
            camara = Camera.main;
        
        timer = esperaInicial;
    }

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            return;
        }
        
        if (camara.orthographicSize > zoomFinal)
        {
            camara.orthographicSize -= velocidadZoom * Time.deltaTime;
        }
    }
}