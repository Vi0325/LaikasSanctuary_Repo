using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Laika : MonoBehaviour, IInteractable
{
    [Header("Diálogo")]
    public LaikaDialogue dialogueData;
    public GameObject dialoguePanel;
    public TMP_Text dialogueText, nameText;
    public Image portraitImage;

    [Header("Interacción")]
    public GameObject iconoE; // <-- ARRASTRAR ICONO E
    public string escenaDestino = ""; // Si quieres cambiar de escena al terminar
    
    [Header("Configuración")]
    public bool cambiarEscenaAlTerminar = false;
    
    private int dialogueIndex;
    private bool isTyping, isDialogueActive;
    private bool jugadorCerca = false;
    
    void Start()
    {
        if (iconoE != null) iconoE.SetActive(false);
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
    }
    
    void Update()
    {
        // Detectar E si el jugador está cerca y NO hay diálogo activo
        if (jugadorCerca && Input.GetKeyDown(KeyCode.E) && !isDialogueActive)
        {
            Interact();
        }
    }
    
    public bool CanInteract()
    { 
        return !isDialogueActive;
    }

    public void Interact()
    {
        if (dialogueData == null)
            return;

        if (isDialogueActive)
        {
            NextLine();
        }
        else
        {
            StartDialogue();
        }
    }

    private void StartDialogue()
    {
        isDialogueActive = true;
        dialogueIndex = 0;

        if (iconoE != null) iconoE.SetActive(false);
        
        nameText.SetText(dialogueData.characterName);
        portraitImage.sprite = dialogueData.characterPortrait;
        dialoguePanel.SetActive(true);

        StartCoroutine(TypeLine());
    }

    void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.SetText(dialogueData.dialogueLines[dialogueIndex]);
            isTyping = false;
        }
        else if (++dialogueIndex < dialogueData.dialogueLines.Length)
        {
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
            
            // CAMBIAR DE ESCENA SI ESTÁ CONFIGURADO
            if (cambiarEscenaAlTerminar && !string.IsNullOrEmpty(escenaDestino))
            {
                if (GameManager.Instance != null)
                    GameManager.Instance.StartGame(escenaDestino);
                else
                    SceneManager.LoadScene(escenaDestino);
            }
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueText.SetText("");

        foreach (char letter in dialogueData.dialogueLines[dialogueIndex])
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(dialogueData.typingSpeed);
        }
        isTyping = false;
        
        if (dialogueData.autoProgressLines.Length > dialogueIndex && dialogueData.autoProgressLines[dialogueIndex])
        {
            yield return new WaitForSeconds(dialogueData.autoProgressDelays);
            NextLine();
        }
    }

    public void EndDialogue()
    {
        StopAllCoroutines();
        isDialogueActive = false;
        dialogueText.SetText("");
        dialoguePanel.SetActive(false);
        
        // Mostrar icono E otra vez si el jugador sigue cerca
        if (jugadorCerca && iconoE != null)
            iconoE.SetActive(true);
    }
    
    // 🟢 DETECCIÓN 2D - ¡ESTO FALTABA!
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
            if (iconoE != null && !isDialogueActive)
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
            if (isDialogueActive)
                EndDialogue();
        }
    }
}