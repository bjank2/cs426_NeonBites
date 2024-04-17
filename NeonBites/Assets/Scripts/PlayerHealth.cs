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

    public UnityEngine.UI.Slider healthBar;
    public UnityEngine.UI.Image healthBarFill;
    [SerializeField] private TMPro.TextMeshProUGUI TMP_PlayerHealth;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.value = 100;
        healthBarFill.color = Color.green;

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
        TMP_PlayerHealth.text = "Health: " + currentHealth;
        healthBar.value = currentHealth;
        healthBarFill.color = Color.Lerp(Color.red, Color.green, currentHealth / maxHealth);
        if (currentHealth <= 0f)
        {
            Die();
            TMP_PlayerHealth.text = "Health: 0";
        }
        // StartCoroutine(PlayDamage(1.5f));
        // gameObject.GetComponent<PlayerInput>().enabled = true;

    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Repair") {
            currentHealth = maxHealth;
        }
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
