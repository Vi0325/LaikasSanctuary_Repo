using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LogoEscena : MonoBehaviour
{
    [Header("Configuración")]
    public float tiempoEspera = 3f;
    public string siguienteEscena = "LoadingScene";
    
    void Start()
    {
        StartCoroutine(EsperarYCargar());
    }
    
    IEnumerator EsperarYCargar()
    {
        yield return new WaitForSeconds(tiempoEspera);
        
        if (GameManager.Instance != null)
            GameManager.Instance.StartGame(siguienteEscena);
        else
            SceneManager.LoadScene(siguienteEscena);
    }
}