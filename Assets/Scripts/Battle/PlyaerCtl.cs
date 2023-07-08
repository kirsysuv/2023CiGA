using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class PlyaerCtl : MonoBehaviour
{

    public float moveSpeed = 5f;
    public float tweenDuration = 0.3f;
    public bool a;

    private Rigidbody2D rb;
    private Collider2D touchCol;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        touchCol = GameObject.Find("Touch").GetComponent<Collider2D>();

    }

    private void Update()
    {

        // Touch
        if (Input.GetKeyDown(KeyCode.J) || (Input.GetKeyDown(KeyCode.Space)))
        {
            Debug.Log("按下Touch");
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
                        Debug.Log("获取能量");
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
        if (movement != Vector2.zero)
        {
            transform.up = movement;
        }

        // Tween 移动
        if (movement.magnitude > 0)
        {
            transform.DOMove((Vector2)transform.position + movement, tweenDuration);
        }


    }
}
