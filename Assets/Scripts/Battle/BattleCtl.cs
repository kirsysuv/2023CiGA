using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCtl : MonoBehaviour
{

    public string currentScene;
    public string nextScene;
    // Start is called before the first frame update
    
    public void RestartScene()
    {
        SceneTransition.Instance.StartFadeIn(currentScene);
    }

    public void NextScene()
    {
        SceneTransition.Instance.StartFadeIn(nextScene);
    }
}
