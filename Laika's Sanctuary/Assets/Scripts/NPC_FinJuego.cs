using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class NPC_FinJuego : MonoBehaviour
{
    [Header("UI")]
    public GameObject iconoE;
    public GameObject panelDialogo;
    public TextMeshProUGUI textoMensaje;
    public TextMeshProUGUI textoNombre;
    public GameObject botonGO;
    
    [Header("Configuración")]
    public string escenaDestino = "EndMenu";
    public string mensaje = "¿Ir al menú principal?";
    public string nombreNPC = "Laika";
    public float offsetY = 1.2f;
    
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
        
        if (jugadorCerca && iconoE != null && iconoE.activeSelf)
        {
            Vector3 posNPC = transform.position;
            Vector3 posPantalla = Camera.main.WorldToScreenPoint(posNPC + Vector3.up * offsetY);
            iconoE.transform.position = posPantalla;
        }
    }
    
    void MostrarDialogo()
    {
        if (panelDialogo != null)
        {
            panelDialogo.SetActive(true);
            if (textoMensaje != null)
                textoMensaje.text = mensaje;
            if (textoNombre != null)
                textoNombre.text = nombreNPC;
        }
        if (iconoE != null) iconoE.SetActive(false);
    }
    
    public void IrAEndMenu()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.StartGame(escenaDestino);
        else
            SceneManager.LoadScene(escenaDestino);
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
            
            if (iconoE != null)
            {
                Vector3 posNPC = transform.position;
                Vector3 posPantalla = Camera.main.WorldToScreenPoint(posNPC + Vector3.up * offsetY);
                iconoE.transform.position = posPantalla;
                iconoE.SetActive(true);
            }
        }
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
            if (iconoE != null) iconoE.SetActive(false);
            if (panelDialogo != null) panelDialogo.SetActive(false);
        }
    }
}