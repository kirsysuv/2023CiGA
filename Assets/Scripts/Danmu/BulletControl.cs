using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
    // Start is called before the first frame update

    public int Count = 1; //一次生成的子弹的数量
    public float LifeTime = 4f; //子弹生命周期
    public float CdTime = 0.1f; //子弹间隔时间 =0时只触发一次
    public float Speed = 10; //子弹移动速度
    public float Angle = 0; //旋转角度
    public float Distance = 0; // 子弹间的间隔
    public float SelfRotation = 0; // 每帧自转角度
    public float AddRotation = 0; // 每帧自转角度增量
    public float CenterDis = 0; // 与发射点的距离

    public Color BulletColor = Color.white; //子弹的颜色
    public Vector3 R_Offset = Vector3.zero; //初始旋转的偏移量
    public Vector3 P_Offset = Vector3.zero;  //位置的偏移量

    public Vector3 RChange
    {
        get
        {
            return R_Offset;
        }
        set
        {
            R_Offset = value;
            rotation = Quaternion.Euler(R_Offset.x, R_Offset.y, R_Offset.z);
            selfRotation = transform.rotation * rotation;
        }
    }

    public float DelayTime = 0;
    Quaternion selfRotation;
    int LimitI = 0;

    //自机狙
    [Header("自机狙的话请勾选")]
    public bool isZiJiJu;
    //自机狙角度额外偏转
    public float ziJiJuZOffset;

    //间隔弹
    [Header("间隔弹的话请勾选")]
    public bool isJianGeDan;
    //间隔多久射一波
    public float jianGeRestTime;
    //一波射多久
    public float jianGeContinueTime;
    //间隔Timer
    public float jianGeTimer = 0f;
    //间隔时间中
    public bool isBanning = false;


    //平行弹
    [Header("平行弹的话请勾选")]
    public bool isParallel;
    //子弹平行间隔
    public float parallelDistance = 0f;

    bool AutoShoot = false;

    //玩家位置，在开始的时候寻找
    public Transform playerPos;
    //当前初始旋转
    public Quaternion rotation = new Quaternion();

    BaseGameObjectPool m_bullet1_pool;
    GameObject m_BulletPrefab;
    private void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        m_BulletPrefab = (GameObject)Resources.Load("Prefabs/Danmu");
        m_bullet1_pool = GameObjectPoolManager.Instance.CreatGameObjectPool<BaseGameObjectPool>("Bullet1_Pool");
        m_bullet1_pool.prefab = m_BulletPrefab;

        rotation = Quaternion.Euler(R_Offset.x, R_Offset.y, R_Offset.z);
        selfRotation = transform.rotation * rotation;

        if (CdTime != 0)
        {
            AutoShoot = true;
        }

    }
    private void FixedUpdate()
    {
        LimitI++;
        if (isJianGeDan)
        {
            jianGeTimer += Time.fixedDeltaTime;
            if (isBanning == false)
            {
                if (jianGeTimer > jianGeContinueTime)
                {
                    isBanning = true;
                    jianGeTimer = 0;
                }
            }
            else
            {
                if (jianGeTimer > jianGeRestTime)
                {
                    isBanning = false;
                    jianGeTimer = 0;
                }
            }
        }
        if (AutoShoot && !isBanning)
        {
            Shoot();
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            RChange = R_Offset + new Vector3(0, 0, 90);
        }
    }
    public void Shoot()
    {
        //非自机狙子弹  差异就是角度
        if (!isZiJiJu)
        {
            SelfRotation += AddRotation;
            SelfRotation = SelfRotation >= 360 ? SelfRotation - 360 : SelfRotation;

            var q = Quaternion.Euler(0, 0, SelfRotation);
            selfRotation *= q;
            BulletData bulletData = new BulletData();

            bulletData.SetValue(transform.position + P_Offset, selfRotation, Count, LifeTime, Speed, Angle, Distance, BulletColor, DelayTime, CenterDis, isParallel, parallelDistance);
            if (LimitI > CdTime * 50 || CdTime == 0)
            {
                BulletManager.Instance.ShootConfig(bulletData, m_bullet1_pool);
                LimitI = 0;
            }
        }
        //自机狙子弹
        else
        {
            Vector3 tempV3 = (playerPos.position - (transform.position + P_Offset)).normalized;
            float angle = Vector2.SignedAngle(Vector2.right, tempV3);
            Quaternion tempQuaternion = Quaternion.Euler(0, 0, angle - 90 + ziJiJuZOffset);

            BulletData bulletData = new BulletData();

            bulletData.SetValue(transform.position + P_Offset, tempQuaternion, Count, LifeTime, Speed, Angle, Distance, BulletColor, DelayTime, CenterDis, isParallel, parallelDistance);
            if (LimitI > CdTime * 50 || CdTime == 0)
            {
                BulletManager.Instance.ShootConfig(bulletData, m_bullet1_pool);
                LimitI = 0;
            }
        }


    }

}
