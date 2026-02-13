using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SalidaRestArea : MonoBehaviour
{
    [Header("Configuración")]
    public string escenaDestino = "EndMenu";
    public string mensajeDialogo = "¿Has completado tu misión?";
    
    [Header("UI - Arrastrar desde Canvas")]
    public GameObject iconoE;
    public GameObject panelDialogo;
    public TextMeshProUGUI textoDialogo;
    public GameObject botonSi;
    public GameObject botonNo;
    
    private bool jugadorCerca = false;
    
    void Start()
    {
        if (iconoE != null) iconoE.SetActive(false);
        if (panelDialogo != null) panelDialogo.SetActive(false);
    }
    
    void Update()
    {
        if (jugadorCerca && Input.GetKeyDown(KeyCode.E) && panelDialogo != null && !panelDialogo.activeSelf)
        {
            MostrarDialogo();
        }
    }
    
    void MostrarDialogo()
    {
        if (panelDialogo != null)
        {
            panelDialogo.SetActive(true);
            if (textoDialogo != null)
                textoDialogo.text = mensajeDialogo;
        }
        
        if (iconoE != null) 
            iconoE.SetActive(false);
    }
    
    public void Confirmar()
    {
        // Volver al menú principal (EndMenu)
        if (GameManager.Instance != null)
        {
            GameManager.Instance.VolverAlMenu(); // Esto va a MainMenu
        }
        else
        {
            SceneManager.LoadScene(escenaDestino);
        }
    }
    
    public void Cancelar()
    {
        if (panelDialogo != null) 
            panelDialogo.SetActive(false);
        
        if (jugadorCerca && iconoE != null)
            iconoE.SetActive(true);
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
            if (iconoE != null && (panelDialogo == null || !panelDialogo.activeSelf))
                iconoE.SetActive(true);
        }
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
            if (iconoE != null) 
                iconoE.SetActive(false);
            if (panelDialogo != null) 
                panelDialogo.SetActive(false);
        }
    }
}