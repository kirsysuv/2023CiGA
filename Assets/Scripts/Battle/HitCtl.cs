using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCtl : MonoBehaviour
{

    public PlyaerCtl player;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Danmu")
        {
            Debug.Log("±»»÷ÖÐ" + collision.gameObject);
            player.Hurted();
        }
    }
}
