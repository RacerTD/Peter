using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public void Quit()
    {
        GameManager.Instance.CurrentGameState = GameState.Quit;
    }
}
