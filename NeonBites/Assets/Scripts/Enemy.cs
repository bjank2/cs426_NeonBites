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

    public float attackDistance = 5.0f;

    public float speed = 10.0f;
    public Transform target;
    public float rotationSpeed = 10.0f;
    private bool canMove = false;

    public UnityEngine.UI.Slider healthBar;
    public UnityEngine.UI.Image healthBarFill;
    void Start()
    {
        animator = GetComponent<Animator>();
        playerHealth = GameObject.FindObjectOfType<PlayerHealth>();
        healthBar.value = 100;
        healthBarFill.color = Color.green;
        canMove = true;
        canAttack = true;
    }

    // Update is called once per frame

    void Update()
    {

        float distanceToTarget = Vector3.Distance(transform.position, playerHealth.transform.position);

        if (distanceToTarget <= 25.0f)
        {
         Debug.Log("Enemy is close to player"); 
        
            if (distanceToTarget > 3.0f)
            {
                if (canMove)
                {
                    animator.SetBool("Move", true);

                    Vector3 targetDirection = target.position - transform.position;
                    targetDirection.y = 0;
                    Quaternion rotation = Quaternion.LookRotation(targetDirection, Vector3.up);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
                    transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                }

            }
            else
            {
                animator.SetBool("Move", false);
                if (canAttack && Time.time >= nextAttackTime)
                {
                    float attackDecision = Random.value;
                    float healthFactor = EnemyHealth / 100;
                    float attackThreshold = Mathf.Lerp(0.8f, 0.3f, 1 - healthFactor);
                    if (attackDecision > attackThreshold)
                    {
                        AttackPlayer();
                        animator.SetTrigger("Attack");
                        nextAttackTime = Time.time + 3f / attackRate;

                    }
                }
            }
        }
    }
    public void Wave()
    {
        animator.SetTrigger("Wave");
    }

    public void AttackPlayer()
    {
        Debug.Log("Attacking player");
        if (playerHealth != null)
        {
            animator.SetBool("Move", false);
            Debug.Log("Player Health is not null");
            playerHealth.TakeDamage(7f);
        }
    }

    public void TakeDamage(float amount)
    {
        float attackDistanceToPlayer = Vector3.Distance(transform.position, playerHealth.transform.position);
        if (attackDistanceToPlayer <= attackDistance)
        {
            EnemyHealth -= amount;
            TMP_EnemyHealth.text = "Enemy Health: " + EnemyHealth.ToString();
            healthBarFill.color = Color.Lerp(Color.red, Color.green, EnemyHealth / 100);
            healthBar.value = EnemyHealth;

            if (EnemyHealth <= 0f)
            {
                TMP_EnemyHealth.text = "Enemy Health: 0";
                Die();
            }
        }
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
    canMove = true;

}



}
