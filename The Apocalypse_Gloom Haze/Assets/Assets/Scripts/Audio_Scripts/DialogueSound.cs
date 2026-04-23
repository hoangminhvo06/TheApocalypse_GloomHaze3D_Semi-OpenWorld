using UnityEngine;

public class DialogueSound : MonoBehaviour
{
   public AudioSource dialogueSource;
   public AudioClip dialogueClip;

   public void PlayDialogue()
    {
        dialogueSource.clip = dialogueClip;
        dialogueSource.Play(); // Phát đoạn hội thoại/ Play the dialogue
    }
}
