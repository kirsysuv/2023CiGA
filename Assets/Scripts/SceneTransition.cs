using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System;
using UnityEngine.UI;

public class SceneTransition : SingletonMonoBehaviour<SceneTransition>
{
    // Start is called before the first frame update
    public float fadeDuration = 2f;
    public string targetScene;
    AsyncOperation operation;
    private CanvasGroup canvasGroup;

    public GameObject noise;
    public GameObject fadeMask;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        // ��ȡ CanvasGroup ���
        canvasGroup = GetComponent<CanvasGroup>();
        //StartFadeIn(Const.BattleGuideScene);
        canvasGroup.alpha = 1;
    }

    private void Update()
    {
        if (operation != null)
        {
            if (operation.isDone)
            {
                //canvasGroup.DOFade(0, fadeDuration).OnComplete(() =>
                //{
                //    canvasGroup.alpha = 1;
                //});
                //fadeMask.SetActive(false);

                fadeMask.GetComponent<Image>().DOFade(0, fadeDuration);
                operation = null;
            }
        }
    }

    public void StartFadeIn(string name)
    {
        Debug.Log("��ʼFade");
        fadeMask.SetActive(true);
        //noise.SetActive(false);


        fadeMask.GetComponent<Image>().DOFade(1, fadeDuration).OnComplete(delegate { LoadTargetScene(name); });

        // ��ʼ��͸����Ϊ 0
        //  canvasGroup.alpha = 0;

        // ִ�н���Ч��
        //  canvasGroup.DOFade(1, fadeDuration).OnComplete(delegate { LoadTargetScene(name); });
    }

    private void LoadTargetScene()
    {
        SceneManager.LoadScene(name);
    }

    private void LoadTargetScene(string name)
    {
        Debug.Log("��ʼ����" + name);
        // ʹ�� SceneManager ����Ŀ�곡��
        operation = SceneManager.LoadSceneAsync(name);
    }
}
