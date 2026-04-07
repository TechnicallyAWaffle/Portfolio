using NUnit.Framework;
using System.Collections;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class JarBase : MonoBehaviour
{
    //Refs
    private Animator animator;
    [SerializeField] private List<JarElement> JarElements = new();

    //Tuning Vars
    private Vector2 initialPosition;
    [SerializeField] private float lerpDuration;
    [SerializeField] private Vector3 openJarPosition;
    [SerializeField] private Vector2 jarHoverScaleChange;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialPosition = transform.position;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        
    }

    public void SelectJar()
    {
        animator.SetBool("isHovering", false);
        animator.SetTrigger("OpenJar");
    }

    public void ReturnToShelf()
    {
        animator.SetTrigger("ReturnToShelf");
    }

    /*
    private IEnumerator Lerp(Vector3 targetPosition)
    {
        while (true)
        {
            float timeElapsed = 0;
            Vector3 startPosition = transform.localPosition;
            Vector3 localTargetPosition = transform.InverseTransformPoint(targetPosition);

            while (timeElapsed < lerpDuration)
            {
                float t = timeElapsed / lerpDuration;
                transform.localPosition = Vector3.Lerp(startPosition, localTargetPosition, t);

                timeElapsed += Time.deltaTime;
                yield return null; 
            }
            transform.localPosition = localTargetPosition;
        }
    }
    */

    public void EmptyJarElements()
    {
        foreach (JarElement element in JarElements)
        {
            if(element)
                element.Toss();
        }
    }

    public void OnMouseEnter()
    {
        animator.SetBool("isHovering", true);
    }

    public void OnMouseExit()
    {
        animator.SetBool("isHovering", false);
    }

}
