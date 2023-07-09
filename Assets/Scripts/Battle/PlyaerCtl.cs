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

    //�޵���˸
    private SpriteRenderer spriteRenderer;
    private DG.Tweening.Sequence blinkSequence;
    // ��˸���
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

        // ������ʼ͸����Ϊ 0
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);

        // ������˸��������
        blinkSequence.Append(spriteRenderer.DOFade(1f, duration))
            .Append(spriteRenderer.DOFade(0f, duration))
            .SetLoops(-1).SetAutoKill(false); // ����ѭ��
    }


    /// <summary>
    /// TODO ������ײ����ý�ɫ���˴���
    /// </summary>
    public void Hurted()
    {
        blood.BloodHurted();
        unDamagable = true;
        blinkSequence.Play();
        DOTween.Sequence().AppendInterval(2f).OnComplete(endUndamagable).Play();

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

    private bool canTouch()
    {
        return GameObject.Find("BattleUI").GetComponent<BattleUICtl>().canTouch;
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.J) || (Input.GetKeyDown(KeyCode.Space)))
        {
            //Debug.Log("����Touch" + GameObject.Find("BattleUI").GetComponent<BattleUICtl>().Energy);

            if (unDamagable)
            {
                //�޵�״̬�°���
                Debug.Log("���Ź���Boss����");

                //TODO ���⹥��
                GameObject.Find("BattleCtl").GetComponent<BattleCtl>().NextScene();
                return;
            }
            if (!canTouch())
            {
                Debug.Log("���ò���Touch");
                //���ò���
                return;
            }
            AudioManager.PlayEffect(AudioManager.Effect_TOUCH);
            GameObject.Find("BattleUI").GetComponent<BattleUICtl>().OnTouchDown();

            Debug.Log("��ȴ����");
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
            // ����ײ���ڴ���һ����״����
            Collider2D[] colliders = Physics2D.OverlapBoxAll(bounds.center, bounds.size, 0f);

            // ������⵽����ײ��
            foreach (Collider2D collider in colliders)
            {
                if (collider.gameObject.tag == "Danmu")
                {
                    Debug.Log(collider.gameObject.name);

                    Danmu danmu = collider.gameObject.GetComponent<Danmu>();
                    if (danmu != null && danmu.gainEnergy)
                    {
                        // 1000���� = ȫ��͸��
                        Debug.Log("��ȡ��Ļ��ȡ������ 200һ��." + Energy);

                        AudioManager.PlayEffect(AudioManager.Effect_Change);

                        float currentPct = Energy / Max_Energy;
                        Energy = math.min(Energy + 200, Max_Energy);
                        float newPct = Energy / Max_Energy;
                        GameObject.Find("Energy").GetComponent<Image>().DOFade(newPct, 0.5f);
                        if (Energy == Max_Energy)
                        {
                            Debug.Log("�����޵�״̬");
                            //�����޵�״̬
                            unDamagable = true;
                            blinkSequence.Play();
                        }
                    }

                    Destroy(collider.gameObject);

                }
            }
        }

        // ��ȡ����
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        // �����ƶ�������ٶ�
        Vector2 movement = new Vector2(moveHorizontal, moveVertical).normalized * moveSpeed * Time.deltaTime;



        // ���³���
        //if (movement != Vector2.zero)
        //{
        //    transform.up = movement;
        //}

        // Tween �ƶ�
        if (movement.magnitude > 0)
        {
            transform.position += new Vector3(movement.x, movement.y, 0);
        }


    }
}
