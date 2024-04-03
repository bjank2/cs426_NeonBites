using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public bool isDead = false;
    public Animator animator;

     [SerializeField] private TMPro.TextMeshProUGUI TMP_PlayerHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    public void TakeDamage(float amount)
    {
        if (isDead) return;
        currentHealth -= amount;
        TriggerDamageAnimation();
        Debug.Log("Damage taken. Player health: " + currentHealth);
        if (currentHealth <= 0f)
        {
            Die();
        }
       TMP_PlayerHealth.text = "Health: " + currentHealth.ToString();

    }

    void Die() {
        isDead = true;
        Debug.Log("Player died");
        Destroy(gameObject);
    }

    void TriggerDamageAnimation() {
        animator.SetTrigger("TakeDamage");
    }
}
