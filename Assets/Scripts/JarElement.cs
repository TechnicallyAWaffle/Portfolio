using System.Collections;
using UnityEngine;

/// <summary>
/// Attach to each object that gets tossed out of the jar.
/// Call Toss() from TossManager to launch it toward its target.
/// </summary>
public class JarElement : MonoBehaviour
{
    //Tuning Vars
    [SerializeField] private Vector3 outOfJarPosition;
    [SerializeField] private float arcHeight = 1.5f;
    [SerializeField] public float duration = 1.25f;
    private CharacterBase characterBaseScript;

    //Refs
    protected Vector3 initialPosition;


    public void Toss() => StartCoroutine(FlyToTarget(outOfJarPosition));
    public void Return() => StartCoroutine(FlyToTarget(initialPosition));

    private void Start()
    {
        initialPosition = transform.localPosition;
        Debug.Log("Transform intial position: " + initialPosition);
        if (TryGetComponent<CharacterBase>(out CharacterBase characterBase))
        {
            characterBase.Freeze();
            characterBaseScript = characterBase;
        }

    }

    private IEnumerator FlyToTarget(Vector3 targetPosition)
    {
        if (characterBaseScript)
            characterBaseScript.Freeze();

        Vector2 start = transform.position;
        Vector2 end = transform.parent != null
    ? (Vector2)transform.parent.TransformPoint(targetPosition)
    : targetPosition;
        Vector2 control = Vector2.Lerp(start, end, 0.5f) + Vector2.up * arcHeight;

        

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float u = 1f - t;

            transform.position = (u * u * start) + (2f * u * t * control) + (t * t * end);
            yield return null;
        }

        transform.position = end;

        if (characterBaseScript)
            characterBaseScript.Unfreeze();

    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Vector2 start = transform.position;
        Vector2 worldTarget = transform.parent != null
            ? (Vector2)transform.parent.TransformPoint(outOfJarPosition)
            : outOfJarPosition;
        Vector2 control = Vector2.Lerp(start, worldTarget, 0.5f) + Vector2.up * arcHeight;

        Gizmos.color = Color.yellow;
        Vector2 prev = start;
        for (int i = 1; i <= 20; i++)
        {
            float t = i / 20f, u = 1f - t;
            Vector2 point = (u * u * start) + (2f * u * t * control) + (t * t * worldTarget);
            Gizmos.DrawLine(prev, point);
            prev = point;
        }

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(worldTarget, 0.1f);
    }
#endif
}