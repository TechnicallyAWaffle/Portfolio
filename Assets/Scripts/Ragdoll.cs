using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
public class Ragdoll : CharacterBase
{

    [SerializeField] private Sprite fullBody;
    [SerializeField] private Sprite noHeadBody;
    [SerializeField] private Vector2 headOffset = new Vector2(0, 0.3409999f);
    [SerializeField] private GameObject ragdollHead;
    private bool hasHead = true;

    private void DetachHead()
    {
        animator.SetBool("hasHead", false);
        ragdollHead.transform.position = (Vector2)(transform.position) + headOffset;
        ragdollHead.SetActive(true);
        hasHead = false;
    }

    private void ReAttachHead()
    {
        animator.SetBool("hasHead", true);
        ragdollHead.SetActive(false);
        hasHead = true;
    }

    protected override IEnumerator PerformUniqueAction()
    {
        if (hasHead)
            DetachHead();
        else
            ReAttachHead();
        yield return null;
    }

    protected override void FixedUpdate()
    {
        if (currentState == CharacterState.Idle)
        {
            if (hasHead)
                spriteRenderer.sprite = fullBody;
            else
                spriteRenderer.sprite = noHeadBody;
        }
        base.FixedUpdate();

    }

}
