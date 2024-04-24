using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Enemy enemy;
    public Animator animator;
    public AudioSource playerass;
    public AudioClip punchClip;

    private float lastAttackTime;
    public float attackRate = 1f;  // Attacks per second

    void Start()
    {
        playerass = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (enemy != null && Vector3.Distance(transform.position, enemy.transform.position) <= 2f)
        {
            if (Input.GetMouseButtonDown(0) && Time.time > lastAttackTime + 1f / attackRate)
            {
                lastAttackTime = Time.time;
                if (!playerass.isPlaying)
                {
                    playerass.PlayOneShot(punchClip);
                }
                animator.SetTrigger("AttackEnemy");
                enemy.TakeDamage(7f);
            }
        }
    }
}
