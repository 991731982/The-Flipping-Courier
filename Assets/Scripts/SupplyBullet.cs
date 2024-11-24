using UnityEngine;

public class SupplyBullet : MonoBehaviour
{
    public int ammoToAdd = 5; // 击中敌人时增加的弹药量
    public string enemyTag = "Enemy"; // 敌人的Tag
    public string playerTag = "Player"; // 玩家的Tag

    private void OnCollisionEnter(Collision collision) // 3D碰撞检测
    {
        if (collision.gameObject.CompareTag(enemyTag))
        {
            // 获取玩家的射击脚本
            ShootingMechanic shootingMechanic = FindObjectOfType<ShootingMechanic>();

            if (shootingMechanic != null)
            {
                // 增加玩家弹药量
                shootingMechanic.AddAmmo(ammoToAdd);
            }

            // 销毁子弹
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) // 触发器版本（可选）
    {
        if (other.CompareTag(enemyTag))
        {
            ShootingMechanic shootingMechanic = FindObjectOfType<ShootingMechanic>();

            if (shootingMechanic != null)
            {
                shootingMechanic.AddAmmo(ammoToAdd);
            }

            Destroy(gameObject);
        }
    }
}
