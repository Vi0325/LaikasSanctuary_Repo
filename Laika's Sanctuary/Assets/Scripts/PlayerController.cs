using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Velocidad de movimiento")]
    public float velocidad = 3f;

    [Header("Tiempo para activar pausa (segundos)")]
    public float tiempoParaPausa = 5f;

    [Header("GameObjects del gato y animaciones")]
    public GameObject rigCat;       // Gato principal (caminar + idle)
    public GameObject sleepSprite;  // Animación de dormir
    public GameObject lickSprite;   // Animación de lamer

    private Animator anim;
    private Animator sleepAnim;
    private Animator lickAnim;

    private float idleTimer = 0f;
    private bool yaTieneAnimacionPausa = false;

    void Start()
    {
        // Obtener los animators
        anim = rigCat.GetComponent<Animator>();
        sleepAnim = sleepSprite.GetComponent<Animator>();
        lickAnim = lickSprite.GetComponent<Animator>();

        // Al inicio, solo el gato principal activo
        rigCat.SetActive(true);
        sleepSprite.SetActive(false);
        lickSprite.SetActive(false);
    }

    void Update()
    {
        // --- INPUT ---
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        Vector3 movimiento = new Vector3(moveX, moveY, 0f).normalized;

        // --- MOVIMIENTO ---
        transform.position += movimiento * velocidad * Time.deltaTime;

        // --- FLIP DEL GATO ---
        if (moveX > 0.01f)
            rigCat.transform.localScale = new Vector3(-1, 1, 1); // Mira derecha
        else if (moveX < -0.01f)
            rigCat.transform.localScale = new Vector3(1, 1, 1);  // Mira izquierda

        // --- CONTROL DE ANIMACIONES ---
        if (movimiento.sqrMagnitude > 0.01f)
        {
            // El jugador se mueve → activar gato principal
            rigCat.SetActive(true);
            sleepSprite.SetActive(false);
            lickSprite.SetActive(false);

            anim.SetBool("isWalking", true);
            anim.SetBool("isSleeping", false);
            anim.SetBool("isLicking", false);

            idleTimer = 0f;
            yaTieneAnimacionPausa = false;
        }
        else
        {
            anim.SetBool("isWalking", false);

            // Contar tiempo quieto
            idleTimer += Time.deltaTime;

            if (idleTimer >= tiempoParaPausa && !yaTieneAnimacionPausa)
            {
                yaTieneAnimacionPausa = true;

                // Desactivar gato principal
                rigCat.SetActive(false);

                // Elegir aleatoriamente Sleep o Lick
                if (Random.value > 0.5f)
                {
                    sleepSprite.SetActive(true);
                    lickSprite.SetActive(false);
                    sleepAnim.Play("Sleep");
                }
                else
                {
                    lickSprite.SetActive(true);
                    sleepSprite.SetActive(false);
                    lickAnim.Play("Lick");
                }
            }
        }
    }
}
