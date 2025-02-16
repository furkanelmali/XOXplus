using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{ 
    [SerializeField] private GameObject[] menus;

    GameManager gameManager;
    GameObject currentMenu;

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
    }
}
