using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thanks : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DOTween.Sequence().AppendInterval(2f).OnComplete(() =>
        {
            SceneTransition.Instance.StartFadeIn("InitScene");
        }).Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
