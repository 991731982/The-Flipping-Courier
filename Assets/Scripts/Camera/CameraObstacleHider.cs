using UnityEngine;
using System.Collections.Generic;

public class CameraObstacleHandler : MonoBehaviour
{
    public Transform player;              // 玩家对象
    public LayerMask obstacleLayer;       // 障碍物的层级，用于射线检测

    private Dictionary<GameObject, Material> originalMaterials = new Dictionary<GameObject, Material>();
    private List<GameObject> currentObstacles = new List<GameObject>();

    void Update()
    {
        // 从摄像机到玩家发射射线
        Vector3 direction = player.position - transform.position;
        Ray ray = new Ray(transform.position, direction);
        RaycastHit[] hits = Physics.RaycastAll(ray, direction.magnitude, obstacleLayer);

        // 恢复之前设置为透明的物体
        for (int i = currentObstacles.Count - 1; i >= 0; i--)
        {
            GameObject obstacle = currentObstacles[i];

            // 检查是否为 null，如果是 null，直接从列表中移除
            if (obstacle == null)
            {
                currentObstacles.RemoveAt(i);
                continue;
            }

            ResetObstacle(obstacle);
        }

        // 处理当前检测到的障碍物
        foreach (RaycastHit hit in hits)
        {
            GameObject obstacle = hit.collider.gameObject;

            // 避免重复设置透明
            if (!currentObstacles.Contains(obstacle))
            {
                SetObstacleTransparent(obstacle);
                currentObstacles.Add(obstacle);
            }
        }
    }

    // 设置物体为半透明
    void SetObstacleTransparent(GameObject obstacle)
    {
        Renderer renderer = obstacle.GetComponent<Renderer>();
        if (renderer != null)
        {
            // 保存原始材质（仅保存一次）
            if (!originalMaterials.ContainsKey(obstacle))
            {
                originalMaterials[obstacle] = renderer.material;
            }

            // 创建材质副本用于透明化处理
            Material transparentMaterial = new Material(renderer.material);
            transparentMaterial.SetFloat("_Mode", 2); // 设置为透明模式
            transparentMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            transparentMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            transparentMaterial.SetInt("_ZWrite", 1); // 启用深度写入，避免透明物体影响射线检测
            transparentMaterial.DisableKeyword("_ALPHATEST_ON");
            transparentMaterial.EnableKeyword("_ALPHABLEND_ON");
            transparentMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            transparentMaterial.renderQueue = 3000;

            // 设置透明度
            Color color = transparentMaterial.color;
            color.a = 0.3f; // 半透明
            transparentMaterial.color = color;

            // 应用修改后的透明材质
            renderer.material = transparentMaterial;
        }
    }

    // 恢复物体的原始材质
    void ResetObstacle(GameObject obstacle)
    {
        if (obstacle == null) return; // 检查是否为 null

        Renderer renderer = obstacle.GetComponent<Renderer>();
        if (renderer != null && originalMaterials.ContainsKey(obstacle))
        {
            renderer.material = originalMaterials[obstacle]; // 恢复原始材质
            originalMaterials.Remove(obstacle); // 从字典中移除
        }
    }
}
