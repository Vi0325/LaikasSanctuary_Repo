using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator anim;
    private float idleTimer = 0f;
    private bool yaTieneAnimacionPausa = false;  // Controla que solo UNA vez por parada

    [Header("Tiempo para activar pausa")]
    public float tiempoParaPausa = 5f;  // Segundos quieto hasta que sale dormir/lamer

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // MOVIMIENTO CON WASD
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        Vector2 movimiento = new Vector2(moveX, moveY).normalized;

        // APLICAR MOVIMIENTO
        transform.Translate(movimiento * 3f * Time.deltaTime);

        // ---------------------------------------- //
        // CONTROL DE ANIMACIONES
        // ---------------------------------------- //

        if (movimiento != Vector2.zero)
        {
            // ✅ SE ESTÁ MOVIENDO
            anim.SetBool("isWalking", true);
            anim.SetBool("isSleeping", false);
            anim.SetBool("isLicking", false);
            
            // Resetea todo porque se movió
            idleTimer = 0f;
            yaTieneAnimacionPausa = false;
        }
        else
        {
            // ❌ ESTÁ QUIETO
            anim.SetBool("isWalking", false);
            
            // Si ya tiene una animación de pausa, no hace nada más
            if (yaTieneAnimacionPausa)
                return;
            
            // Aumenta el tiempo quieto
            idleTimer += Time.deltaTime;

            // ¿Lleva suficiente tiempo quieto Y no tiene animación de pausa?
            if (idleTimer >= tiempoParaPausa && !yaTieneAnimacionPausa)
            {
                // 🎲 ELIGE ALEATORIAMENTE: 50% Sleeping, 50% Licking
                if (Random.value > 0.5f)
                {
                    anim.SetBool("isSleeping", true);
                    anim.SetBool("isLicking", false);
                }
                else
                {
                    anim.SetBool("isLicking", true);
                    anim.SetBool("isSleeping", false);
                }

                // Marca que YA tiene animación de pausa
                yaTieneAnimacionPausa = true;
                
                // 🛑 NO reseteamos el timer, no necesitamos más
            }
        }
    }
}