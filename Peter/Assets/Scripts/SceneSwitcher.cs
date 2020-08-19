using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSwitcher : MonoBehaviour
{
    [SerializeField] private string sceneToLoad = "Insert Scene Name here";
    private bool switched = false;

    /// <summary>
    /// Switches to the given scene
    /// </summary>
    public void SwitchScene()
    {
        if (!switched)
        {
            switched = true;
            GameManager.Instance.SwitchToOtherScene(sceneToLoad);
        }
    }
}
