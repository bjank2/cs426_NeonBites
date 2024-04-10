using UnityEngine;

public class CollideToFight : MonoBehaviour
{
    public Animator fighter1Animator;
    public Animator fighter2Animator;

    public RuntimeAnimatorController punchingAnimatorController; // Reference to the punching animator controller
    public RuntimeAnimatorController dodgingAnimatorController; // Reference to the dodging animator controller

    void OnCollisionEnter(Collision collision)
    {
        // Check if collision involves Fighter2
        if (collision.gameObject.CompareTag("Fighter2"))
        {
            //fighters collide
            Debug.Log("Fighters collide");
            
            // Switch Fighter1's animator controller to punching
            fighter1Animator.runtimeAnimatorController = punchingAnimatorController;

            // Switch Fighter2's animator controller to dodging
            fighter2Animator.runtimeAnimatorController = dodgingAnimatorController;
        }
    }
}
