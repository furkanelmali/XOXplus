using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{ 
    [SerializeField] private GameObject[] menus;

    GameObject currentMenu;

    [SerializeField] private TextMeshProUGUI gameMode1, gameMode2;

    void Update()
    {
        if(GameManager.Instance.gameMode == 0)
        {
            gameMode1.color = new Color(0, 188, 255);
            gameMode2.color = Color.white;
        }
        else
        {
            gameMode1.color = Color.white;
            gameMode2.color = new Color(0, 188, 255);
        }
        
    }

     public void CloseBtn()
    {
        currentMenu.SetActive(true);
    }
   

    public void TakeCurrentMenu()
    {
        foreach(GameObject i in menus)
        {
            if(i.activeSelf)
            {
                currentMenu = i;
            }
        }
    }

    public void HomeBtn()
    {
        SceneManager.LoadScene(0);
    }

    public void GameModeBtn(int gameMode)
    {
        GameManager.Instance.gameMode = gameMode;
        PlayerPrefs.SetInt("gameMode", gameMode);
    }

    public void ChooseStarter(int mode)
    {
     GameManager.Instance.stateChooser = mode;
    }
    

   
}
