using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void pauseGameFromMenu()
    {
        GameObject pauseMenu = this.gameObject.transform.GetChild(0).gameObject;
        Debug.Log(pauseMenu);
        //pauseMenu.ac

        if (pauseMenu.activeInHierarchy == false)
        {
            pauseMenu.SetActive(true);

            Time.timeScale = 0;
        }
        else
        {
            pauseMenu.SetActive(false);

            Time.timeScale = 1;
            //Cursor.visible = false;
        }
        Debug.Log("ran pause");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
