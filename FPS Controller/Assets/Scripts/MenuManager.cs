using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    void Start()
    {
        pausePanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(!pausePanel.activeInHierarchy)
            {
                Cursor.visible = true;
                PauseGame();
            }
            else if(pausePanel.activeInHierarchy)
            {
                Cursor.visible = false;
                UnpauseGame();
            }
        }
               
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
        Debug.Log("Game Paused");
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        Debug.Log("Game Unpased");
    }

    public void QuitGame()
    {
        //NO MAIN MENU YET
        Debug.Log("Game Quit, Main Menu Loaded");
        SceneManager.LoadScene("MainMenu");
    }

}
