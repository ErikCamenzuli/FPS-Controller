using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    void Start()
    {
        pausePanel.SetActive(false);
        if (pausePanel != null)
            return;

    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //opening pause panel
            if(!pausePanel.activeInHierarchy)
            {
                Cursor.visible = true;
                PauseGame();
            }
            //closing pause panel
            else if(pausePanel.activeInHierarchy)
            {
                Cursor.visible = false;
                UnpauseGame();
            }
        }

        if (pausePanel != null)
            return;

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
        Cursor.visible = false;
        Debug.Log("Game Unpased");
    }

    public void SelectLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

}
