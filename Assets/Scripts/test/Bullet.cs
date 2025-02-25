using UnityEngine;

public class Bullet : MonoBehaviour
{
    public enum BulletType { Heavy, Light } // 定义子弹类型
    public BulletType bulletType; // 这个子弹的类型

    public float lifeTime = 3f; // 子弹存活时间

    void Start()
    {
        Destroy(gameObject, lifeTime); // 3秒后销毁子弹
    }
}
