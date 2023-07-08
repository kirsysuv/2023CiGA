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
    public Image FullIcon;
    public Image FullText;
    public Image low;
    public SpriteRenderer Touch;
    public bool canTouch;

    public float ColdDownTime = 2.5f;

    // Start is called before the first frame update
    void Start()
    {
        low.gameObject.SetActive(false);
        FullIcon.gameObject.SetActive(false);
        FullText.gameObject.SetActive(false);
        Energy = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.J))
        //{
        //    if (Energy < Max_Energy)
        //    {
        //        return;
        //    }
        //    if (GameObject.Find("Chara").GetComponent<PlyaerCtl>().unDamagable)
        //    {
        //        //无敌状态
        //        Debug.Log("无敌状态下攻击，在BattleUICtl处理");
        //        return;
        //    }
        //    OnTouchDown();
        //}
    }
    private void FixedUpdate()
    {
        float u = Max_Energy * Time.fixedDeltaTime / ColdDownTime;
        ColdDown(u);
    }

    public void ColdDown(float deltime)
    {
        //Debug.Log("获得能量:" + deltime);
        Energy = math.min(Energy + deltime, Max_Energy);
        UpdateView();
        if (Energy < Max_Energy)
        {
            canTouch = false;
        }
        else
        {
            canTouch = true;
        }
    }

    public void OnTouchDown()
    {
        Energy = 0;
        UpdateView();
        if (Energy < Max_Energy)
        {
            canTouch = false;
        }
        else
        {
            canTouch = true;
        }
    }

    private void UpdateView()
    {
        // update view
        if (Energy < Max_Energy)
        {
            low.gameObject.SetActive(true);
            FullText.gameObject.SetActive(false);
            FullIcon.gameObject.SetActive(false);

        }
        if (Energy == Max_Energy)
        {
            low.gameObject.SetActive(false);
            FullText.gameObject.SetActive(true);
            FullIcon.gameObject.SetActive(true);
        }
        float pct = Energy / Max_Energy;
        //Debug.Log(Energy + " " + pct);
        ProgressL.fillAmount = pct;
        ProgressR.fillAmount = pct;
        LineL.fillAmount = pct;
        LineR.fillAmount = pct;
    }
}
