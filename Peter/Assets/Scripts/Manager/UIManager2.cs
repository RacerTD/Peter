using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager2 : ManagerModule<UIManager2>
{
    public GameObject PauseMenu;

    public void OpenPauseMenu()
    {
        PauseMenu.SetActive(true);
    }

    public void ClosePauseMenu()
    {
        PauseMenu.SetActive(false);
    }
}
