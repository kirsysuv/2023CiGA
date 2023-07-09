using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class PlyaerCtl : MonoBehaviour
{

    public float moveSpeed = 1f;
    public float tweenDuration = 0.3f;
    public GameObject Touch;
    public float Energy;
    public const float Max_Energy = 1000;

    public BloodUICtl blood;
    public bool unDamagable = false;

    private Rigidbody2D rb;
    private Collider2D touchCol;

    public GameObject slash;

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
        blood.BloodInit();

        // 设置起始透明度为 0
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);

        // 创建闪烁动画序列
        blinkSequence.Append(spriteRenderer.DOFade(1f, duration))
            .Append(spriteRenderer.DOFade(0f, duration))
            .SetLoops(-1).SetAutoKill(false); // 无限循环
    }


    /// <summary>
    /// TODO 根据碰撞体调用角色受伤代码
    /// </summary>
    public void Hurted()
    {
        if (unDamagable)
        {
            return;
        }

        AudioManager.PlayEffect(AudioManager.Effect_PlayerHited);

        blood.BloodHurted();
        unDamagable = true;
        blinkSequence.Play();
        DOTween.Sequence().AppendInterval(2f).OnComplete(endUndamagable).Play();

    }

    public void Dead()
    {

        Image failImg = GameObject.Find("BattleUI").GetComponent<BattleUICtl>().FailImg;

        failImg.DOFade(1, 1f).OnComplete(() => { GameObject.Find("BattleCtl").GetComponent<BattleCtl>().RestartScene(); });
        //DOTween.Sequence().Append(failImg.DOFade(1, 1f)).Append(failImg.DOFade(0, 1f)).OnComplete(() => { GameObject.Find("BattleCtl").GetComponent<BattleCtl>().NextScene(); }).Play();

    }

    private void endUndamagable()
    {

        if (Energy != Max_Energy)
        {
            unDamagable = false;
            blinkSequence.Pause();
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
        }
    }
    public void AttackComplete()
    {
        Energy = 0;
        if (Energy != Max_Energy)
        {
            unDamagable = false;
            blinkSequence.Pause();
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
        }
    }
    private bool canTouch()
    {
        return GameObject.Find("BattleUI").GetComponent<BattleUICtl>().canTouch;
    }

    public void Win()
    {
        unDamagable = true;
        GameObject.Find("BattleCtl").GetComponent<BattleCtl>().NextScene();
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.J) || (Input.GetKeyDown(KeyCode.Space)))
        {
            //Debug.Log("按下Touch" + GameObject.Find("BattleUI").GetComponent<BattleUICtl>().Energy);

            if (unDamagable)
            {
                //无敌状态下按下，可以进行特殊攻击
                Debug.Log("播放攻击Boss动画");

                Bounds bounds1 = touchCol.bounds;
                // 在碰撞体内创建一个盒状区域
                Collider2D[] colliders1 = Physics2D.OverlapBoxAll(bounds1.center, bounds1.size, 0f);

                // 遍历检测到的碰撞体
                foreach (Collider2D collider in colliders1)
                {
                    if (collider.gameObject.tag == "Boss")
                    {
                        collider.transform.parent.GetComponent<BOSSScript>().DecreasedBossHealth();
                        AttackComplete();
                        GameObject.Find("Energy").GetComponent<Image>().DOFade(0, 0.1f);
                        GameObject slash = Instantiate(this.slash);
                        slash.transform.parent = gameObject.transform;
                        slash.transform.position = collider.gameObject.transform.position;
                        slash.GetComponent<ParticleSystem>().Play();
                    }
                }

                // TODO teshu gongji



                return;
            }
            if (!canTouch())
            {
                Debug.Log("还用不了Touch");
                //还用不了
                return;
            }
            AudioManager.PlayEffect(AudioManager.Effect_TOUCH);
            GameObject.Find("BattleUI").GetComponent<BattleUICtl>().OnTouchDown();

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

                        AudioManager.PlayEffect(AudioManager.Effect_Change);

                        float currentPct = Energy / Max_Energy;
                        Energy = math.min(Energy + 1000, Max_Energy);
                        float newPct = Energy / Max_Energy;
                        GameObject.Find("Energy").GetComponent<Image>().DOFade(newPct, 0.25f);
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
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        // 计算移动方向和速度
        Vector2 movement = new Vector2(moveHorizontal, moveVertical).normalized * moveSpeed * Time.deltaTime;



        // 更新朝向
        //if (movement != Vector2.zero)
        //{
        //    transform.up = movement;
        //}

        // Tween 移动
        if (movement.magnitude > 0)
        {
            transform.position += new Vector3(movement.x, movement.y, 0);
        }


    }
}
