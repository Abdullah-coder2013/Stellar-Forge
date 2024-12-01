using UnityEngine;

public class Lock : MonoBehaviour
{
    [SerializeField] private Animator animator;
    
    public void Clicked() {
        animator.SetBool("IsClicked", true);

        if (animator.GetBool("IsClicked")) {
            animator.SetBool("IsClicked", false);
        }

    }
}
