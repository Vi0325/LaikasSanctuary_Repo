using TMPro;
using UnityEngine;

public class OuroborosController : MonoBehaviour
{
    public float speed = 5f; // Speed of the Ouroboros movement
    public Rigidbody2D rb;
    private bool isFacingRight = true;

    [Header("Sistema de Pickups")]
    public int monedas = 0;
    public int vidas = 3;
    public TextMeshProUGUI textoMonedas;
    public TextMeshProUGUI textoVidas;

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        rb.linearVelocity = new Vector2(horizontal * speed, vertical * speed);

        if (horizontal > 0)
        {
            if (!isFacingRight)
            {
                Flip();
            }
        }
        if (horizontal < 0)
        {
            if (isFacingRight)
            {
                Flip();
            }
        }
    }

    void Flip()
    {
        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;
        isFacingRight = !isFacingRight;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // ==== AÑADE ESTE BLOQUE ====
        if (other.CompareTag("PickUp"))
        {
            RecogerPickUp(other.gameObject);
        }

        if (other.CompareTag("Asteroide"))
    {
            vidas--;
            textoVidas.text = "Vidas: " + vidas;
            Destroy(other.gameObject);
            Debug.Log("Vidas: " + vidas);
        }
    }

    private void RecogerPickUp(GameObject pickup)
    {
        // Sumar moneda
        monedas++;

        // Actualizar UI
        if (textoMonedas != null)
        {
            textoMonedas.text = "Monedas: " + monedas;
        }

        Destroy(pickup);

        Debug.Log("Monedas: " + monedas);
    }
}
