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

    CheckInternetConnection checkInternet;

    private static readonly Color ActiveColor = new Color(0f, 188f / 255f, 1f);
    
    void Start()
    {
        checkInternet = FindObjectOfType<CheckInternetConnection>();
        checkForInternetConnection();
        RefreshGameModeUI();
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
        RefreshGameModeUI();
    }

    public void ChooseStarter(int mode)
    {
        GameManager.Instance.stateChooser = mode;
    }
    
    private void RefreshGameModeUI()
    {
        if (!gameMode1 || !gameMode2 || !GameManager.Instance) return;

        if (GameManager.Instance.gameMode == 0)
        {
            gameMode1.color = ActiveColor;
            gameMode2.color = Color.white;
        }
        else
        {
            gameMode1.color = Color.white;
            gameMode2.color = ActiveColor;
        }
    }


    void checkForInternetConnection()
    {
        if (!checkInternet.IsInternetAvailable())
        {
            
            Debug.Log("İnternet bağlantısı yok! Oyundan çıkılıyor...");
               foreach(GameObject i in menus)
            {
                if(i.activeSelf && i != menus[5])
                {
                    currentMenu = i;
                    i.SetActive(false);
                }
                if(i==menus[5])
                {
                    i.SetActive(true);
                }
            }
                
        }
        else
        {
        
            Debug.Log("İnternet bağlantısı var! Oyun devam ediyor...");
            foreach(GameObject i in menus)
            {
                if(i == menus[5])
                {
                    i.SetActive(false);
                }
                else if(i == currentMenu && i != menus[5])
                {
                    i.SetActive(true);
                }

            }
            
        }
    }

   
}
