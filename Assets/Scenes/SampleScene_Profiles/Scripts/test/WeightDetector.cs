using UnityEngine;
using System.Collections; // 确保包含 IEnumerator 所需的命名空间

public class WeightDetector : MonoBehaviour
{
    public Transform cube; // 需要移動的方塊
    public float moveAmount = 1f; // 上升或下降的量
    public float moveSpeed = 0.5f; // 移動速度

    private void OnTriggerEnter(Collider other)
    {
        Bullet bullet = other.GetComponent<Bullet>();
        if (bullet != null)
        {
            if (bullet.bulletType == Bullet.BulletType.Heavy)
            {
                StartCoroutine(MoveCube(Vector3.down));
            }
            else if (bullet.bulletType == Bullet.BulletType.Light)
            {
                StartCoroutine(MoveCube(Vector3.up));
            }
        }
    }

    private IEnumerator MoveCube(Vector3 direction)
    {
        if (cube != null)
        {
            Vector3 targetPosition = cube.position + direction * moveAmount;
            while (Vector3.Distance(cube.position, targetPosition) > 0.01f)
            {
                cube.position = Vector3.Lerp(cube.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }
            cube.position = targetPosition;
        }
    }
}