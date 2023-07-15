using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonEvents : MonoBehaviour
{

    public List<Button> buttons;

    int cursor = 3;
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            cursor += 1;
            if (cursor >= 3)
            {
                cursor = 0;
            }

            if (cursor != 3) buttons[cursor].Select();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            cursor -= 1;
            if (cursor <= -1)
            {
                cursor = 2;
            }

            if (cursor != 3) buttons[cursor].Select();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            switch (cursor)
            {
                case 0:
                    Enter1Stage();
                    break;
                case 1:
                    Enter2Stage();
                    break;
                case 2:
                    Quit();
                    break;
                default:
                    break;
            }
        }
        
    }


    public void Enter1Stage()
    {
        SceneManager.LoadScene("Stage01");
    }

    public void Enter2Stage()
    {
        SceneManager.LoadScene("Stage02");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
