using UnityEngine;

[CreateAssetMenu (fileName = "NewLaikaDialogue", menuName = "Laika Dialogue")]
public class LaikaDialogue : ScriptableObject
{
    public string characterName;
    public Sprite characterPortrait;
    public string[] dialogueLines;
    public float typingSpeed = 0.05f;
    public AudioClip voiceSound;
    public float voicePitch = 1f;
    public bool[] autoProgressLines;
    public float autoProgressDelays = 1.5f;
}
