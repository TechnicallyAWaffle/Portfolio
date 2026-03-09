using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class CharacterBase : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] protected float moveSpeed = 2f;
    [SerializeField] protected float waypointReachDistance = 0.1f;

    [Header("Wandering")]
    [SerializeField] protected float wanderRadius = 4f;
    [SerializeField] protected float minIdleTime = 1.5f;
    [SerializeField] protected float maxIdleTime = 4f;

    [Header("Unique Action")]
    [SerializeField] protected float minUniqueActionInterval = 5f;
    [SerializeField] protected float maxUniqueActionInterval = 12f;

    [Header("Sprite")]
    [SerializeField] protected bool flipSpriteOnMove = true;


    public enum CharacterState { Idle, Moving, Frozen, UniqueAction }
    protected CharacterState currentState = CharacterState.Idle;

    protected Rigidbody2D rb;
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;

    protected Vector2 originPosition;
    protected Vector2 targetPosition;

    private Coroutine behaviorCoroutine;
    private Coroutine uniqueActionTimerCoroutine;

    protected static readonly int AnimIsMoving = Animator.StringToHash("isMoving");
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        rb.freezeRotation = true;

        originPosition = transform.position;
    }

    protected virtual void Start()
    {
        behaviorCoroutine = StartCoroutine(BehaviorLoop());
        uniqueActionTimerCoroutine = StartCoroutine(UniqueActionTimer());
    }

    protected virtual void OnDisable()
    {
        if (behaviorCoroutine != null) StopCoroutine(behaviorCoroutine);
        if (uniqueActionTimerCoroutine != null) StopCoroutine(uniqueActionTimerCoroutine);
    }

    // ── Core behavior loop ────────────────────────────────────────────────────

    public void Freeze()
    {
        SetState(CharacterState.Frozen);
        animator.SetBool(AnimIsMoving, false);
    }

    public void Unfreeze()
    {
        SetState(CharacterState.Idle);
    }

    /// <summary>Main loop: idle → pick target → walk → repeat.</summary>
    private IEnumerator BehaviorLoop()
    {
        while (true)
        {
            // ── IDLE ──────────────────────────────────────────────────────────
            SetState(CharacterState.Idle);
            float idleTime = Random.Range(minIdleTime, maxIdleTime);
            yield return new WaitForSeconds(idleTime);

            if (currentState == CharacterState.Frozen)
            {
                yield return new WaitUntil(() => currentState != CharacterState.Frozen);
                continue;
            }

            targetPosition = PickWanderTarget();
            if (currentState == CharacterState.Idle)
                SetState(CharacterState.Moving);
            else
                continue;

                while (Vector2.Distance(rb.position, targetPosition) > waypointReachDistance)
                {
                    // Yield each frame while moving (physics done in FixedUpdate)
                    yield return null;

                    // Abort movement if a unique action was triggered
                    if (currentState == CharacterState.Frozen)
                        yield break; // BehaviorLoop will restart after action ends
                }

            rb.linearVelocity = Vector2.zero;
        }
    }

    protected virtual void FixedUpdate()
    {
        if (currentState == CharacterState.Moving)
        {
            Vector2 dir = (targetPosition - rb.position).normalized;
            rb.linearVelocity = dir * moveSpeed;

            if (flipSpriteOnMove && spriteRenderer != null)
                spriteRenderer.flipX = dir.x < 0f;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    // ── Unique action timer ───────────────────────────────────────────────────

    /// <summary>Fires a unique action at a random interval, independent of the move loop.</summary>
    private IEnumerator UniqueActionTimer()
    {
        while (true)
        {
            float wait = Random.Range(minUniqueActionInterval, maxUniqueActionInterval);
            yield return new WaitForSeconds(wait);

            // Don't interrupt an already-running unique action
            if (currentState == CharacterState.UniqueAction) continue;

            if (currentState == CharacterState.Frozen)
            {
                yield return new WaitUntil(() => currentState != CharacterState.Frozen);
                continue;
            }

            yield return StartCoroutine(RunUniqueAction());

            if (behaviorCoroutine != null) StopCoroutine(behaviorCoroutine);
            behaviorCoroutine = StartCoroutine(BehaviorLoop());
        }
    }

    private IEnumerator RunUniqueAction()
    {
        SetState(CharacterState.UniqueAction);
        yield return StartCoroutine(PerformUniqueAction());
        // State is reset inside PerformUniqueAction or here as fallback
        if (currentState == CharacterState.UniqueAction)
            SetState(CharacterState.Idle);
    }

    // ── Overrideable unique action ────────────────────────────────────────────

    /// <summary>
    /// Override this in each character subclass to define their special behaviour.
    /// Use "yield return" to wait for animations, movements, etc.
    /// Call SetState(CharacterState.Idle) when done (or let the base handle it).
    /// </summary>
    protected virtual IEnumerator PerformUniqueAction()
    {
        yield return null;
    }

    // ── State management ──────────────────────────────────────────────────────

    public void SetState(CharacterState newState)
    {
        currentState = newState;
        UpdateAnimator();
        OnStateChanged(newState);
        Debug.Log(gameObject.name + "state changed to " + newState);
    }

    /// <summary>Called whenever the state changes. Override for extra logic.</summary>
    protected virtual void OnStateChanged(CharacterState newState) { }

    private void UpdateAnimator()
    {
        if (animator == null) return;
        animator.SetBool(AnimIsMoving, currentState == CharacterState.Moving);
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    /// <summary>Returns a random point within wanderRadius of the spawn position.</summary>
    protected virtual Vector2 PickWanderTarget()
    {
        return originPosition + new Vector2 (Random.Range(0, wanderRadius), 0);
    }

    /// <summary>Walk to a specific world-space position (for use inside unique actions).</summary>
    protected IEnumerator WalkTo(Vector2 destination, float speedOverride = -1f)
    {
        float speed = speedOverride > 0f ? speedOverride : moveSpeed;
        SetState(CharacterState.Moving);

        while (Vector2.Distance(rb.position, destination) > waypointReachDistance)
        {
            Vector2 dir = (destination - rb.position).normalized;
            rb.linearVelocity = dir * speed;

            if (flipSpriteOnMove && spriteRenderer != null)
                spriteRenderer.flipX = dir.x < 0f;

            yield return new WaitForFixedUpdate();
        }

        rb.linearVelocity = Vector2.zero;
    }

    /// <summary>Make the character face a direction (useful before playing a reaction).</summary>
    protected void FaceDirection(float xDir)
    {
        if (spriteRenderer != null && flipSpriteOnMove)
            spriteRenderer.flipX = xDir < 0f;
    }

    public CharacterState CurrentState => currentState;
}