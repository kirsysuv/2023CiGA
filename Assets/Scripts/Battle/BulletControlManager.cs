using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControlManager : MonoBehaviour
{

    public List<GameObject> PhaseBullet;

    private void Awake()
    {
        PhaseBullet = new List<GameObject>();
        foreach (Transform item in GetComponentsInChildren<Transform>())
        {
            if (item.gameObject != this.gameObject)
            {
                PhaseBullet.Add(item.gameObject);
            }

        }
    }
    private void OnEnable()
    {
        Initial();
    }
    public void Initial()
    {
        CloseAllPhase();
    }
    public void ChangeBulletPhase(int phase = 1)
    {
        for (int i = 0; i < PhaseBullet.Count; i++)
        {
            if (i == phase - 1)
            {
                PhaseBullet[i].SetActive(true);
            }
            else
            {
                PhaseBullet[i].SetActive(false);
            }
        }
    }
    public void CloseAllPhase()
    {
        for (int i = 0; i < PhaseBullet.Count; i++)
        {
            PhaseBullet[i].SetActive(false);
        }
    }


}
