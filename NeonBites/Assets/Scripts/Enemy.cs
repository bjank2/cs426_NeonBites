using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float EnemyHealth = 100f;
    private float attackProbability = 0.5f;
    public float attackRate = 2f;
    private float nextAttackTime = 0f;
    [SerializeField] private TMPro.TextMeshProUGUI TMP_EnemyHealth;

    public Animator animator;
    private PlayerHealth playerHealth;
    private bool canAttack = false;
    void Start()
    {
        animator = GetComponent<Animator>();
        playerHealth = GameObject.FindObjectOfType<PlayerHealth>();
    }

    // Update is called once per frame

    void Update()
    {
        if (canAttack && Time.time >= nextAttackTime)
        {
            float attackDecision = Random.value;
            float healthFactor = EnemyHealth / 100;
            float attackThreshold = Mathf.Lerp(0.8f, 0.3f, 1 - healthFactor);
            if (attackDecision > attackThreshold)
            {
                AttackPlayer();
                nextAttackTime = Time.time + 13f / attackRate;

            }
        }
    }

        public void AttackPlayer()
        {
            Debug.Log("Attacking player");
            if (playerHealth != null)
            {  
                Debug.Log("Player Health is not null");
                animator.SetTrigger("isAttacking");
                playerHealth.TakeDamage(7f);
            }
        }

        public void TakeDamage(float amount)
        {
            EnemyHealth -= amount;
            if (EnemyHealth <= 0f)
            {
                Die();
            }
            TMP_EnemyHealth.text = "Enemy Health: " + EnemyHealth.ToString();
        }

        void Die()
        {
            Debug.Log("Enemy died");
            Destroy(gameObject);
        }


        public void EnableAttacking()
        {
            Debug.Log("Enemy is attacking");
            canAttack = true;
            animator.SetTrigger("Angry");
        }
    }
