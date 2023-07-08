using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class BattleUICtl : MonoBehaviour
{
    //能量
    public float Energy = 0;
    public const float Max_Energy = 100;

    public Image ProgressL;
    public Image ProgressR;
    public Image LineL;
    public Image LineR;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GainEnergy(float energy)
    {
        Debug.Log("获得能量:" + energy);
        Energy = math.min(Energy + energy, Max_Energy);

        // update view
        float pct = Energy / Max_Energy;
        Debug.Log(Energy + " " + pct);
        ProgressL.fillAmount = pct;
        ProgressR.fillAmount = pct;
        LineL.fillAmount = pct;
        LineR.fillAmount = pct;

    }
}
