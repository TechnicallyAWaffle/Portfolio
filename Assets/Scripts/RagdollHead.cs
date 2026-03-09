using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RagdollHead : CharacterBase
{

    [SerializeField] float rotationSpeed;
    [SerializeField] float popForce;

    private void OnEnable()
    {
        rb.AddForce(new Vector2(0, popForce), ForceMode2D.Impulse);
    }

    protected override void FixedUpdate()
    {
        if (currentState == CharacterState.Moving)
        {
            rb.AddForce(Vector2.Normalize((Vector2)(transform.position) - targetPosition) * moveSpeed, ForceMode2D.Force);
            rb.MoveRotation(rb.rotation + rotationSpeed * Time.fixedDeltaTime);
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
    

 

}
