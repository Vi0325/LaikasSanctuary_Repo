using UnityEngine;
using UnityEngine.SceneManagement;

public class FinRecorrido : MonoBehaviour
{
    [Header("Configuración")]
    public string nombreEscenaDestino = "RestArea"; // O la escena que toque
    public bool usarGameManager = true;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (usarGameManager && GameManager.Instance != null)
                GameManager.Instance.StartGame(nombreEscenaDestino);
            else
                SceneManager.LoadScene(nombreEscenaDestino);
        }
    }
}