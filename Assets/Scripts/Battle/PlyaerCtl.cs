using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlyaerCtl : MonoBehaviour
{

    public GameObject Blood;

    
    // Start is called before the first frame update
    void Start()
    {
        Blood = GameObject.Find("Blood");
        Blood.SendMessage("BloodInit");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("space");
            Blood.SendMessage("BloodHurted");
        }
    }
}
