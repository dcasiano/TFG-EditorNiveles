using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalDialogueManager : MonoBehaviour
{
    public UIManager uIManager;
    public GameObject characterIcon;
    public string nextScene;

    [System.Serializable]
    public struct Name
    {
        public string npcName;
        public int numberOfSentences;
        public Sprite sprite;
    }

    [SerializeField]
    public Name[] names;
    

    [TextArea(3, 10)]
    public string[] sentences;
    

    int currentSentence, currentName, currentNameRepetition;


    private void Start()
    {
        NextSentence();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) NextSentence();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.instance.SetAbilityTrue("Lightning");
            LoadNextScene();
        }

    }

    void NextSentence()
    {
        if (currentSentence < sentences.Length)
        {
            WriteText();

            if (currentNameRepetition >= names[currentName].numberOfSentences)
            {
                currentName++;
                currentNameRepetition = 0;
            }
        }

        else
        {
            GameManager.instance.SetAbilityTrue("Lightning");
            LoadNextScene();
        }
    }

    void WriteText()
    {
        uIManager.WriteDialogue(names[currentName].npcName, sentences[currentSentence]);
        uIManager.ChangeImage(characterIcon, names[currentName].sprite);
        currentSentence++;
        currentNameRepetition++;
    }

    void LoadNextScene()
    {
        GameManager.instance.ChangeScene(nextScene);
    }
}
