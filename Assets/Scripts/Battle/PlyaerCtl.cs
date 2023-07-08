using DG.Tweening;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlyaerCtl : MonoBehaviour
{

    public float moveSpeed = 1f;
    public float tweenDuration = 0.3f;
    public GameObject Touch;
    public float Energy;
    public const float Max_Energy = 1000;

    public bool unDamagable = false;

    private Rigidbody2D rb;
    private Collider2D touchCol;

    //无敌闪烁
    private SpriteRenderer spriteRenderer;
    private DG.Tweening.Sequence blinkSequence;
    // 闪烁间隔
    public float duration = 0.5f;


    private void Awake()
    {
        DOTween.Init();
        DOTween.defaultAutoPlay = AutoPlay.AutoPlayTweeners;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        blinkSequence = DOTween.Sequence();
        Energy = 0;

        // 设置起始透明度为 0
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);

        // 创建闪烁动画序列
        blinkSequence.Append(spriteRenderer.DOFade(1f, duration))
            .Append(spriteRenderer.DOFade(0f, duration))
            .SetLoops(-1).SetAutoKill(false); // 无限循环
    }

    private void DisableTouch()
    {

    }

    private bool canTouch()
    {
        return GameObject.Find("BattleUI").GetComponent<BattleUICtl>().canTouch;
    }

    private void Update()
    {


        // Touch
        if (Input.GetKeyDown(KeyCode.J) || (Input.GetKeyDown(KeyCode.Space)))
        {
            //Debug.Log("按下Touch" + GameObject.Find("BattleUI").GetComponent<BattleUICtl>().Energy);

            if (unDamagable)
            {
                //无敌状态下按下
                Debug.Log("播放攻击Boss动画");

                GameObject.Find("BattleCtl").GetComponent<BattleCtl>().RestartScene();
                return;
            }
            if (!canTouch())
            {
                Debug.Log("还用不了Touch");
                //还用不了
                return;
            }
            Debug.Log("冷却结束");
            //View
            Touch.SetActive(true);
            Color color = Touch.GetComponent<SpriteRenderer>().color;
            color.a = 1f;
            Touch.GetComponent<SpriteRenderer>().color = color;
            Touch.GetComponent<SpriteRenderer>()
                .DOFade(0F, 0.5F)
                .OnComplete(() => { Touch.SetActive(false); });
            touchCol = GameObject.Find("Touch").GetComponent<Collider2D>();
            Bounds bounds = touchCol.bounds;
            // 在碰撞体内创建一个盒状区域
            Collider2D[] colliders = Physics2D.OverlapBoxAll(bounds.center, bounds.size, 0f);

            // 遍历检测到的碰撞体
            foreach (Collider2D collider in colliders)
            {
                if (collider.gameObject.tag == "Danmu")
                {
                    Debug.Log(collider.gameObject.name);

                    Danmu danmu = collider.gameObject.GetComponent<Danmu>();
                    if (danmu != null && danmu.gainEnergy)
                    {
                        // 1000能量 = 全不透明
                        Debug.Log("吸取弹幕获取能量， 200一个." + Energy);
                        float currentPct = Energy / Max_Energy;
                        Energy = math.min(Energy + 200, Max_Energy);
                        float newPct = Energy / Max_Energy;
                        GameObject.Find("Energy").GetComponent<Image>().DOFade(newPct, 0.5f);
                        if (Energy == Max_Energy)
                        {
                            Debug.Log("进入无敌状态");
                            //进入无敌状态
                            unDamagable = true;
                            blinkSequence.Play();
                        }
                    }

                    Destroy(collider.gameObject);

                }
            }
        }

        // 获取输入
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // 计算移动方向和速度
        Vector2 movement = new Vector2(moveHorizontal, moveVertical).normalized * moveSpeed;

        // 应用移动
        rb.velocity = movement;

        // 更新朝向
        //if (movement != Vector2.zero)
        //{
        //    transform.up = movement;
        //}

        // Tween 移动
        if (movement.magnitude > 0)
        {
            transform.DOMove((Vector2)transform.position + movement, tweenDuration);
        }


    }
}
