using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PolygonCollider2D))]
public class AutoColliderAnimator : MonoBehaviour
{
    private Animator animator;
    private PolygonCollider2D polygonCollider;
    private SpriteRenderer spriteRenderer;
    private string lastClipName;
    
    void Awake()
    {
        animator = GetComponent<Animator>();
        polygonCollider = GetComponent<PolygonCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        string clipName = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;

        if (clipName != lastClipName)
        {
            UpdateCollider();
            lastClipName = clipName;
        }
    }

    void UpdateCollider()
    {
        if (spriteRenderer.sprite != null)
        {
            int pathCount = spriteRenderer.sprite.GetPhysicsShapeCount();
            polygonCollider.pathCount = pathCount;
            for (int i = 0; i < pathCount; i++)
            {
                List<Vector2> shapeList = new List<Vector2>();
                spriteRenderer.sprite.GetPhysicsShape(i, shapeList);
                polygonCollider.SetPath(i, shapeList);
            }
        }
    }
}
