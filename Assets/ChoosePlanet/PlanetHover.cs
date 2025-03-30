using UnityEngine;
using UnityEngine.UI;

public class PlanetHover : MonoBehaviour
{
    public float scaleMultiplier = 1.2f;
    public float rotationSpeed = 30f;
    public string planetDescription;
    public GameObject uiPanel; // 要顯示的 UI Panel (含文字)
    public Text uiText;        // 顯示介紹文字的 UI Text

    private Vector3 originalScale;
    private bool isHovered = false;

    void Start()
    {
        originalScale = transform.localScale;
        if (uiPanel != null)
            uiPanel.SetActive(false);
    }

    void Update()
    {
        if (isHovered)
        {
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }
    }

    void OnMouseEnter()
    {
        isHovered = true;
        transform.localScale = originalScale * scaleMultiplier;

        if (uiPanel != null)
        {
            uiPanel.SetActive(true);
            uiText.text = planetDescription;
        }
    }

    void OnMouseExit()
    {
        isHovered = false;
        transform.localScale = originalScale;

        if (uiPanel != null)
            uiPanel.SetActive(false);
    }
}
