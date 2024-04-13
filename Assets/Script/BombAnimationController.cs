using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAnimationController : MonoBehaviour
{
    private Animator animator;
    private CircleCollider2D circleCollider;
    private Destroy destroyScript;

    void Start()
    {
        animator = GetComponent<Animator>();
        circleCollider = GetComponent<CircleCollider2D>();
        destroyScript = GetComponent<Destroy>();
        StartCoroutine(CheckAnimationState());
    }

    IEnumerator CheckAnimationState()
    {
        yield return new WaitForSeconds(1f);
        circleCollider.radius = 1f;
        while (true)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                destroyScript.Destroying();
                yield break;
            }
            yield return null;
        }
    }
}
