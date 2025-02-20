using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour {

    string npcName;
    string[] sentences;
    int currentSentence;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) NextSentence();
    }

    public void GetSentences(string newNPCName, string[] newSentences)
    {
        npcName = newNPCName;
        sentences = newSentences;
        currentSentence = 0;
        WriteText();
    }

    public void NextSentence()
    {
        if (currentSentence < sentences.Length)
        {
            WriteText();
        }

        else GameManager.instance.ReturnUIManager().Pausa("Enter");
    }

    public void WriteText()
    {
        GameManager.instance.ReturnUIManager().WriteDialogue(npcName, sentences[currentSentence]);
        currentSentence++;
    }
}
