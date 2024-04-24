using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Enemy : MonoBehaviour
{
    public float EnemyHealth = 100f;
    public Animator animator;
    private PlayerHealth playerHealth;
    private PlayerAttack playerAttack;
    public Transform target;
    public float speed = 10.0f;
    public float rotationSpeed = 10.0f;
    private Vector3 initialPosition;

    public UnityEngine.UI.Slider healthBar;
    public UnityEngine.UI.Image healthBarFill;
    [SerializeField] private TMPro.TextMeshProUGUI TMP_EnemyHealth;

    private float nextAttackTime = 0f;
    public float attackRate = 2f;
    private bool canMove = true;
    private float distanceToPlayer;
    public AudioSource audiosrc;
    public AudioClip hurtSound;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerHealth = GameObject.FindObjectOfType<PlayerHealth>();
        playerAttack = GameObject.FindObjectOfType<PlayerAttack>();
        healthBar.value = 100;
        healthBarFill.color = Color.green;
        initialPosition = transform.position;

        audiosrc = GetComponent<AudioSource>();
    }

    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, playerHealth.transform.position);

        if (distanceToPlayer < 5f)
        {
            target.gameObject.GetComponent<PlayerAttack>().enemy = this;
            FollowPlayer();
            if (distanceToPlayer < 2f && Time.time >= nextAttackTime)
            {
                AttackPlayer();
            }
        }
        else
        {
            target.gameObject.GetComponent<PlayerAttack>().enemy = null;
            ReturnToInitialPosition();
        }
    }

    void FollowPlayer()
    {
        if (canMove && distanceToPlayer >= 2f)
        {
            animator.SetBool("Move", true);
            Vector3 targetDirection = target.position - transform.position;
            targetDirection.y = 0;
            Quaternion rotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("Move", false);
        }
    }

    void ReturnToInitialPosition()
    {
        if (Vector3.Distance(transform.position, initialPosition) > 0.1f)
        {
            animator.SetBool("Move", true);
            Quaternion rotation = Quaternion.LookRotation(initialPosition - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, speed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("Move", false);
        }
    }

    public void AttackPlayer()
    {
        canMove = false;
        animator.SetTrigger("Attack");
        nextAttackTime = Time.time + 1f / attackRate;
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(7f);
        }
        canMove = true;
    }

    public void TakeDamage(float amount)
    {
        EnemyHealth -= amount;
        //TMP_EnemyHealth.text = "Enemy Health: " + EnemyHealth.ToString();
        healthBarFill.color = Color.Lerp(Color.red, Color.green, EnemyHealth / 100);
        healthBar.value = EnemyHealth;

        audiosrc.PlayOneShot(hurtSound);

        if (EnemyHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    public void Wave()
    {
        if (animator != null)
        {
            animator.SetTrigger("Wave");
        }
    }

}
