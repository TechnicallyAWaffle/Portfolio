using System.Collections;
using UnityEngine;

/// <summary>
/// Attach to each object that gets tossed out of the jar.
/// Call Toss() from TossManager to launch it toward its target.
/// </summary>
public class JarElement : MonoBehaviour
{
    [SerializeField] private Vector2 targetPosition;
    [SerializeField] private float arcHeight = 1.5f;
    [SerializeField] private float duration = 0.6f;

    public void Toss() => StartCoroutine(FlyToTarget());

    private IEnumerator FlyToTarget()
    {
        Vector2 start = transform.position;
        Vector2 end = targetPosition;
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
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Vector2 start = transform.position;
        Vector2 control = Vector2.Lerp(start, targetPosition, 0.5f) + Vector2.up * arcHeight;

        Gizmos.color = Color.yellow;
        Vector2 prev = start;
        for (int i = 1; i <= 20; i++)
        {
            float t = i / 20f, u = 1f - t;
            Vector2 point = (u * u * start) + (2f * u * t * control) + (t * t * targetPosition);
            Gizmos.DrawLine(prev, point);
            prev = point;
        }

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(targetPosition, 0.1f);
    }
#endif
}