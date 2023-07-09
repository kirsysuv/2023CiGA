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
        AudioManager.PlayBgm(AudioManager.BGM_MainManu);
    }

    private void EndGame()
    {
        AudioManager.PlayEffect(AudioManager.Effect_UIClick);

        Application.Quit();
    }

    private void StartGame()
    {
        AudioManager.PlayEffect(AudioManager.Effect_UIClick);

        SceneTransition.Instance.StartFadeIn("1");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
