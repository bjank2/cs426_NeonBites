using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
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


    private IEnumerator PlayDamage(float duration)
    {
        // Wait for the specified duration
        yield return new WaitForSeconds(duration);
    }
    public void TakeDamage(float amount)
    {
        if (isDead) return;
        currentHealth -= amount;
        TriggerDamageAnimation();
        Debug.Log("Damage taken. Player health: " + currentHealth);
        TMP_PlayerHealth.text = "Health: " + currentHealth;

        if (currentHealth <= 0f)
        {
            Die();
            TMP_PlayerHealth.text = "Health: 0";
        }
        // StartCoroutine(PlayDamage(1.5f));
        // gameObject.GetComponent<PlayerInput>().enabled = true;

    }

    void Die()
    {
        isDead = true;
        Debug.Log("Player died");
        Destroy(gameObject);
    }

    void TriggerDamageAnimation()
    {
        animator.SetTrigger("TakeDamage");
        // gameObject.GetComponent<PlayerInput>().enabled = false;

    }
}
