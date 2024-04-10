using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyAnim : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float fadeOutTime = .5f;
    public TMPro.TextMeshProUGUI textMesh;
    private float alpha = 1;
    void Awake()
    {
        textMesh = GetComponent<TMPro.TextMeshProUGUI>();
    }

    public void Initialize(int changeAmount) {
        textMesh.text = (changeAmount >= 0 ? "+" : "") + changeAmount.ToString();
        alpha = 1;
    }

    void Update() {
        transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);
        alpha -= Time.deltaTime / fadeOutTime;
        textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, alpha);
        if (alpha <= 0) {
            Destroy(gameObject);
        }
    }
}
