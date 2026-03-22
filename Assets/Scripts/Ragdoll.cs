using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
public class Ragdoll : CharacterBase
{

    [SerializeField] private Sprite fullBody;
    [SerializeField] private Sprite noHeadBody;
    [SerializeField] private Vector2 headOffset = new(0, 0.3409999f);
    [SerializeField] private GameObject ragdollHead;
    private bool hasHead = true;

    private static readonly int ragdollWalk = Animator.StringToHash("Ragdoll Walk");
    private static readonly int ragdollWalkNoHead = Animator.StringToHash("Ragdoll Walk No Head");
    private static readonly int ragdollIdle = Animator.StringToHash("RagdollHasHeadIdle");
    private static readonly int ragdollIdleNoHead = Animator.StringToHash("RagdollNoHeadIdle");

    protected override void OnStateChanged(CharacterState newState)
    {
        if (newState == CharacterState.Moving)
        {
            animator.Play(hasHead ? ragdollWalk : ragdollWalkNoHead, 0);
        }
        else if(newState == CharacterState.Idle)
        {
            animator.Play(hasHead ? ragdollIdle : ragdollIdleNoHead, 0);
        }
    }

    private void DetachHead()
    {
        hasHead = false;
        OnStateChanged(currentState);
        ragdollHead.transform.position = (Vector2)(transform.position) + headOffset;
        ragdollHead.SetActive(true);
    }

    private void ReAttachHead()
    {
        hasHead = true;
        OnStateChanged(currentState);
        ragdollHead.SetActive(false);
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
        base.FixedUpdate();

    }

}
