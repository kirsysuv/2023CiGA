using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitSceneUICtl : MonoBehaviour
{
    public Button start;
    public Button end;

    // Start is called before the first frame update
    void Start()
    {
        start.onClick.AddListener(StartGame);
        end.onClick.AddListener(EndGame);
    }

    private void EndGame()
    {
        Application.Quit();
    }

    private void StartGame()
    {
        SceneTransition.Instance.StartFadeIn(Const.BattleGuideScene);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
