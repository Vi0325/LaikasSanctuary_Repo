using TMPro;
using UnityEngine;

public class VidasAsterioides : MonoBehaviour
{
    [Header("UI")]
    public int Vidas = 3;
    public TextMeshProUGUI textovidas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Asteroide"))
        {
            Vidas--;
            textovidas.text = "Vidas: " + Vidas;
            
        }
    }
}
