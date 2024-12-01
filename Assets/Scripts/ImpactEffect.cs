using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactEffect : MonoBehaviour
{
    [SerializeField] private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
        {
            Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
        }
    }

    
}
