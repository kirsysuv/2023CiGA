using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodUICtl : MonoBehaviour
{

    public GameObject blood1;
    public GameObject blood2;
    public GameObject blood3;
    public PlyaerCtl plyaer;
    public int lifeCount;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 开始战斗，重新开始战斗时调用
    /// </summary>
    public void BloodInit()
    {
        lifeCount = 3;
        UpdateState();
    }
    public void BloodHurted()
    {
        Debug.Log("Hurted");
        if (lifeCount > 0)
        {
            lifeCount--;
            UpdateState();
        }

    }

    private void UpdateState()
    {
        switch (lifeCount)
        {
            case 0:
                blood1.SetActive(false);
                blood2.SetActive(false);
                blood3.SetActive(false);
                plyaer.Dead();
                break;
            case 1:
                blood1.SetActive(true);
                blood2.SetActive(false);
                blood3.SetActive(false);
                break;
            case 2:
                blood1.SetActive(true);
                blood2.SetActive(true);
                blood3.SetActive(false);
                break;
            case 3:
                blood1.SetActive(true);
                blood2.SetActive(true);
                blood3.SetActive(true);
                break;
        }
    }

}
