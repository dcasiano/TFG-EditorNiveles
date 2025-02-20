using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public Image[] HeartIcons;
    public GameObject fireballIcon, shieldIcon, lightningIcon, dialogueBox, debugMode;
    public GameObject[] FirstsBottons;
    public Slider bossSlider, fireballSlider, shieldSlider, lightningSlider, qSlider, wSlider, eSlider;
    float fireballSliderValue, shieldSliderValue, lightningSliderValue;
    GameObject player;
    public EventSystem eventSystem;
    void Start()
    {
        GameManager.instance.ThisUIManager(this);
        Invoke("UpdateLifeUI", 0.5f * Time.deltaTime);
        Invoke("EnableAbilityIcons", 2f * Time.deltaTime);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pausa("Escape");
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            ScenesMenu();
        }
    }

    //Cambia la escena
    public void ChangeScene(string Scene)
    {
        GameManager.instance.ChangeScene(Scene);
    }

    //Cierra el juego (informa por consola)
    public void QuitGame()
    {
        Debug.Log("Se ha cerrado el juego.");
        Application.Quit();
    }

    public GameObject GetActiveMenu()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            if (gameObject.transform.GetChild(i).gameObject.activeSelf == true && gameObject.transform.GetChild(i).tag == "Menu")
            {
                return gameObject.transform.GetChild(i).gameObject;
            }
        }

        return null;
    }

    public void Pausa(string cause)
    {
        eventSystem.SetSelectedGameObject(FirstsBottons[0]);
        if (cause == "Escape")
        {
            if (!GameManager.instance.IsOnDialogue())
            {
                if (GetActiveMenu() != null) GetActiveMenu().SetActive(false);
                else gameObject.transform.GetChild(0).gameObject.SetActive(true);
                GameManager.instance.Pause("Menu");
            }

            else if (GameManager.instance.IsOnDialogue())
            {
                DisableDialogueBox();
                GameManager.instance.Pause("Dialogue");
            }
        }

        else if (cause == "Enter")
        {
            if (!GameManager.instance.IsOnDialogue())
            {
                if (GetActiveMenu() != null) GetActiveMenu().SetActive(false);
                else gameObject.transform.GetChild(0).gameObject.SetActive(true);
                GameManager.instance.Pause("Menu");
            }

            else if (GameManager.instance.IsOnDialogue())
            {
                DisableDialogueBox();
                GameManager.instance.Pause("Dialogue");
            }
        }

        else if (cause == "NPC")
        {
            GameManager.instance.Pause("Dialogue");
        }
    }

    public void ChangeMenu(GameObject menu)
    {
        GetActiveMenu().SetActive(false);
        menu.SetActive(true);
    }

    public void UpdateLifeUI()
    {
        player = GameManager.instance.ReturnPlayer();
        if (player != null)
        {

            int playerActualLife = player.GetComponent<Life>().GetActualLife();

            for (int i = 0; i < HeartIcons.Length; i++)
            {
                if (i <= playerActualLife - 1) HeartIcons[i].enabled = true;

                else HeartIcons[i].enabled = false;
            }
        }

    }

    public void EnableAbilityIcons()
    {
        if (fireballIcon != null) fireballIcon.SetActive(GameManager.instance.ReturnAbilityValue("Fireball"));
        if (shieldIcon != null) shieldIcon.SetActive(GameManager.instance.ReturnAbilityValue("Shield"));
        if (lightningIcon != null) lightningIcon.SetActive(GameManager.instance.ReturnAbilityValue("Lightning"));
    }

    public void EnableDialogueBox(string npcName, string[] sentences)
    {
        if (dialogueBox != null)
        {
            dialogueBox.SetActive(true);
            dialogueBox.GetComponent<DialogueManager>().GetSentences(npcName, sentences);
            Pausa("NPC");
        }
    }

    public void DisableDialogueBox()
    {
        dialogueBox.SetActive(false);
    }

    public void WriteDialogue(string npcName, string sentence)
    {
        if (dialogueBox != null)
        {
            for (int i = 0; i < dialogueBox.transform.childCount; i++)
            {
                if (dialogueBox.transform.GetChild(i).name.Contains("TextCharacter")) dialogueBox.transform.GetChild(i).GetComponent<Text>().text = npcName;

                else if (dialogueBox.transform.GetChild(i).name.Contains("TextDialogue")) dialogueBox.transform.GetChild(i).GetComponent<Text>().text = sentence;
            }
        }
    }

    public void SetSliderValue(float value, string slider)
    {
        switch (slider)
        {
            case "Fireball":
                fireballSliderValue = value;
                fireballSlider.value = fireballSliderValue;
                //qSlider.value = fireballSliderValue;
                break;
            case "Shield":
                shieldSliderValue = value;
                shieldSlider.value = shieldSliderValue;
                //eSlider.value = shieldSliderValue;
                break;
            case "Lightning":
                lightningSliderValue = value;
                lightningSlider.value = lightningSliderValue;
                //wSlider.value = lightningSliderValue;
                break;
            case "Boss":
                bossSlider.value = value;
                break;
        }
    }

    public float ReturnSliderValue(string slider)
    {
        switch (slider)
        {
            case "Fireball":
                return fireballSliderValue;
            case "Shield":
                return shieldSliderValue;
            case "Lightning":
                return lightningSliderValue;
            default:
                return 0;
        }
    }
    public void EnableDebugMode()
    {
        debugMode.SetActive(true);
        GameManager.instance.ActivateAll();
    }
    //te lleva al canvas del menu
    public void ScenesMenu()
    {
        if (!GameManager.instance.IsOnDialogue() && !GameManager.instance.IsOnMenu())
        {
            eventSystem.SetSelectedGameObject(FirstsBottons[1]);
            if (GetActiveMenu() != null) GetActiveMenu().SetActive(false);
            else gameObject.transform.GetChild(transform.childCount - 1).gameObject.SetActive(true);
            GameManager.instance.Pause("Menu");
        }
    }
    //si eres chico
    public void Boy(string scene)
    {
        GameManager.instance.AreYouAGirl(false);
        GameManager.instance.ChangeScene(scene);
    }
    //si eres chica
    public void Girl(string scene)
    {
        GameManager.instance.AreYouAGirl(true);
        GameManager.instance.ChangeScene(scene);

    }
    //para el boton de trucos
    public void Cheat()
    {
        GameManager.instance.ActivateAll();
        GameManager.instance.ReturnUIManager().EnableAbilityIcons();
    }
    //Para desbloquear las puertas
    public void OpenDoors()
    {
        GameManager.instance.SetLevelManager().OpenDoors();
    }

    public void ChangeImage(GameObject image, Sprite newSprite)
    {
        image.GetComponent<Image>().sprite = newSprite;
    }

    public void ResetCheckPoint()
    {
        GameManager.instance.ChangeCheckpointPosition(new Vector2(-5.54f, -3.77f), new Vector2(0, 0));
    }
}
