using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindFan : MonoBehaviour
{
    public float windForce = 100f;  // �L������
    public Vector3 boxSize = new Vector3(5f, 3f, 3f); // �L��Ӱ푹��� (�L, ��, ��)
    public float massThreshold = 60f; // Ӱ��|�����ֵ
    private GameObject windEffect;

    void Start()
    {
        // ����һ����ҕ�����L�����@ʾ
        windEffect = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Destroy(windEffect.GetComponent<Collider>()); // �h����ײ�w
        windEffect.transform.SetParent(transform);
        windEffect.transform.localScale = boxSize;

        // �O��͸�����|
        MeshRenderer renderer = windEffect.GetComponent<MeshRenderer>();
        Material transparentMaterial = new Material(Shader.Find("Standard"));
        transparentMaterial.color = new Color(0f, 0.5f, 1f, 0.3f); // ��͸���ɫ
        transparentMaterial.SetFloat("_Mode", 3); // �O�Þ�͸��ģʽ
        transparentMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        transparentMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        transparentMaterial.SetInt("_ZWrite", 0);
        transparentMaterial.DisableKeyword("_ALPHATEST_ON");
        transparentMaterial.EnableKeyword("_ALPHABLEND_ON");
        transparentMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        transparentMaterial.renderQueue = 3000;

        renderer.material = transparentMaterial;
    }


    void FixedUpdate()
    {
        Vector3 windDirection = transform.right;
        Vector3 windOrigin = transform.position + windDirection * (boxSize.x / 2);
        Collider[] colliders = Physics.OverlapBox(windOrigin, boxSize / 2, transform.rotation);

        foreach (Collider col in colliders)
        {
            Rigidbody rb = col.GetComponent<Rigidbody>();
            if (rb != null && rb.mass < massThreshold)
            {
                rb.AddForce(windDirection * windForce, ForceMode.Acceleration);
            }
        }

        // �����L�Ŀ�ҕ���^��λ��
        windEffect.transform.position = windOrigin;
        windEffect.transform.rotation = transform.rotation;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f, 0.5f, 1f, 0.3f);
        Gizmos.matrix = Matrix4x4.TRS(transform.position + transform.right * (boxSize.x / 2), transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, boxSize);
    }
}
