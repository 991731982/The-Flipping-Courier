using UnityEngine;
using System.Collections.Generic;

public class CameraObstacleHandler : MonoBehaviour
{
    public Transform player;              // ��Ҷ���
    public LayerMask obstacleLayer;       // �ϰ���Ĳ㼶���������߼��

    private Dictionary<GameObject, Material> originalMaterials = new Dictionary<GameObject, Material>();
    private List<GameObject> currentObstacles = new List<GameObject>();

    void Update()
    {
        // �����������ҷ�������
        Vector3 direction = player.position - transform.position;
        Ray ray = new Ray(transform.position, direction);
        RaycastHit[] hits = Physics.RaycastAll(ray, direction.magnitude, obstacleLayer);

        // �ָ�֮ǰ����Ϊ͸��������
        for (int i = currentObstacles.Count - 1; i >= 0; i--)
        {
            GameObject obstacle = currentObstacles[i];

            // ����Ƿ�Ϊ null������� null��ֱ�Ӵ��б����Ƴ�
            if (obstacle == null)
            {
                currentObstacles.RemoveAt(i);
                continue;
            }

            ResetObstacle(obstacle);
        }

        // ����ǰ��⵽���ϰ���
        foreach (RaycastHit hit in hits)
        {
            GameObject obstacle = hit.collider.gameObject;

            // �����ظ�����͸��
            if (!currentObstacles.Contains(obstacle))
            {
                SetObstacleTransparent(obstacle);
                currentObstacles.Add(obstacle);
            }
        }
    }

    // ��������Ϊ��͸��
    void SetObstacleTransparent(GameObject obstacle)
    {
        Renderer renderer = obstacle.GetComponent<Renderer>();
        if (renderer != null)
        {
            // ����ԭʼ���ʣ�������һ�Σ�
            if (!originalMaterials.ContainsKey(obstacle))
            {
                originalMaterials[obstacle] = renderer.material;
            }

            // �������ʸ�������͸��������
            Material transparentMaterial = new Material(renderer.material);
            transparentMaterial.SetFloat("_Mode", 2); // ����Ϊ͸��ģʽ
            transparentMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            transparentMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            transparentMaterial.SetInt("_ZWrite", 1); // �������д�룬����͸������Ӱ�����߼��
            transparentMaterial.DisableKeyword("_ALPHATEST_ON");
            transparentMaterial.EnableKeyword("_ALPHABLEND_ON");
            transparentMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            transparentMaterial.renderQueue = 3000;

            // ����͸����
            Color color = transparentMaterial.color;
            color.a = 0.3f; // ��͸��
            transparentMaterial.color = color;

            // Ӧ���޸ĺ��͸������
            renderer.material = transparentMaterial;
        }
    }

    // �ָ������ԭʼ����
    void ResetObstacle(GameObject obstacle)
    {
        if (obstacle == null) return; // ����Ƿ�Ϊ null

        Renderer renderer = obstacle.GetComponent<Renderer>();
        if (renderer != null && originalMaterials.ContainsKey(obstacle))
        {
            renderer.material = originalMaterials[obstacle]; // �ָ�ԭʼ����
            originalMaterials.Remove(obstacle); // ���ֵ����Ƴ�
        }
    }
}
