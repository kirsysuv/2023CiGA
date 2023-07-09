using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletData
{
    public Vector3 Position = Vector3.zero;//子弹生成的位置
    public Quaternion direction = Quaternion.identity;//子弹的方向
    public int Count = 1; //一次生成的子弹的数量
    public float LifeTime = 4f; //子弹生命周期
    public float Speed = 10; //子弹移动速度
    public float Angle = 0; //旋转角度
    public float DelayTime = 0;
    public float CenterDis = 0; // 与发射点的距离
    public float SpecialChance = 1f;
    //子弹样式
    public BulletType bType;
    //是否是平行弹
    public bool isParallel;
    //平行距离
    public float parallelDistance;

    public void SetValue(Vector3 Position, Quaternion direction, int Count, float LifeTime, float Speed, float Angle,   float InvokeTime, float CenterDis, bool isParallel, float parallelDistance,float SpecialChance,BulletType bulletType)
    {
        this.Position = Position;
        this.direction = direction;
        this.Count = Count;
        this.LifeTime = LifeTime;
        this.Speed = Speed;
        this.Angle = Angle;
        this.DelayTime = InvokeTime;
        this.CenterDis = CenterDis;
        this.isParallel = isParallel;
        this.parallelDistance = parallelDistance;
        this.SpecialChance = SpecialChance;
        this.bType= bulletType;

    }
}
