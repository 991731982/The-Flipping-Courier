using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHits = 3; // 最大击中次数
    private int currentHits = 0; // 当前被击中的次数

    public void TakeDamage()
    {
        currentHits++;

        if (currentHits >= maxHits)
        {
            DestroyEnemy();
        }
    }

    private void DestroyEnemy()
    {
        // 销毁敌人
        Destroy(gameObject);
    }
}
