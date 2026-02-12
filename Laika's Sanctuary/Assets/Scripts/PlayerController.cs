using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator anim;
    private float idleTimer = 0f;
    private bool yaTieneAnimacionPausa = false;

    [Header("Tiempo para activar pausa")]
    public float tiempoParaPausa = 5f;

    [Header("Referencias a GameObjects")]
    public GameObject rigCat;      // GameObject con rig y Animator
    public GameObject sleepSprite; // GameObject con sprite durmiendo
    public GameObject lickSprite;  // GameObject con sprite lamiendo

    void Start()
    {
        anim = rigCat.GetComponent<Animator>();
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        Vector2 movimiento = new Vector2(moveX, moveY).normalized;

        // Movimiento
        transform.Translate(movimiento * 3f * Time.deltaTime);

        // Voltear gato según dirección
        if (movimiento.x > 0.01f)
            rigCat.transform.localScale = new Vector3(1, 1, 1);
        else if (movimiento.x < -0.01f)
            rigCat.transform.localScale = new Vector3(-1, 1, 1);

        // Control de animaciones y estados
        if (movimiento.sqrMagnitude > 0.01f)
        {
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

            idleTimer += Time.deltaTime;

            if (idleTimer >= tiempoParaPausa && !yaTieneAnimacionPausa)
            {
                yaTieneAnimacionPausa = true;

                rigCat.SetActive(false);

                if (Random.value > 0.5f)
                {
                    sleepSprite.SetActive(true);
                    lickSprite.SetActive(false);
                }
                else
                {
                    lickSprite.SetActive(true);
                    sleepSprite.SetActive(false);
                }
            }
        }
    }
}
