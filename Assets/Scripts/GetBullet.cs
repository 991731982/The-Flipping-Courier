using UnityEngine;

public class GetBullet: MonoBehaviour
{
    public int ammoToAdd = 10; // 每次进入检查点增加的弹药量

    private void OnTriggerEnter(Collider other)
    {
        // 检查是否是玩家进入检查点
        if (other.CompareTag("Player"))
        {
            // 获取玩家的 ShootingMechanic 脚本
            ShootingMechanic shootingMechanic = other.GetComponent<ShootingMechanic>();

            if (shootingMechanic != null)
            {
                // 增加玩家弹药
                shootingMechanic.AddAmmo(ammoToAdd);
                Debug.Log($"Player entered checkpoint. Ammo increased by {ammoToAdd}!");
            }
        }
    }
}
