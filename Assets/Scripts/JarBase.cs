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
    private Vector3 initialSize;
    private JarManager jarManager;
    [SerializeField] protected GameObject characterCanvas;
    protected Animator characterCanvasAnimator;

    //Tuning Vars
    private Vector2 initialPosition;
    [SerializeField] protected float sizeChangeOnHover;
    [SerializeField] protected float lerpDuration;
    [SerializeField] protected Vector3 openJarPosition;
    [SerializeField] protected Vector2 jarHoverScaleChange;
    [SerializeField] protected CharacterBase[] characters;
    [SerializeField] protected float jarSelectLockoutTimer = 1.5f;

    //Runtime Vars
    [Header("Runtime, do not edit")]
    [SerializeField] protected bool isOpen = false;
    [SerializeField] protected bool canSelect = true;

    public void Lockout() => StartCoroutine(JarSelectLockoutTimer());

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialSize = transform.localScale;
        initialPosition = transform.position;
        animator = GetComponent<Animator>();
        jarManager = JarManager.Instance;
        characterCanvasAnimator = characterCanvas.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        
    }

    public void SelectJar()
    {
        if (!canSelect)
            return;

        jarManager.SwitchOpenJar(this);

        animator.SetBool("isHovering", false);
        if (isOpen)
        {
            characterCanvasAnimator.SetTrigger("PopDown");
            ReturnJarElements();
        }
        else
        {
            animator.SetTrigger("OpenJar");
            characterCanvasAnimator.SetTrigger("PopUp");
        }
        isOpen = !isOpen;
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

    private IEnumerator JarSelectLockoutTimer()
    {
        canSelect = false;
        yield return new WaitForSeconds(jarSelectLockoutTimer);
        canSelect = true;
    }


    public void EmptyJarElements()
    {
        foreach (JarElement element in JarElements)
        {
            if(element)
                element.Toss();
        }
    }

    public void ReturnJarElements()
    {
        foreach (JarElement element in JarElements)
        {
            if (element)
                element.Return();
        }
    }


    public void OnMouseEnter()
    {
        transform.localScale = initialSize += new Vector3(sizeChangeOnHover, sizeChangeOnHover, 0);
        animator.SetBool("isHovering", true);
    }

    public void OnMouseExit()
    {
        transform.localScale = initialSize;
        animator.SetBool("isHovering", false);
    }

}
