using System.Collections;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterClickHandler : MonoBehaviour
{

    // ── Drag state ────────────────────────────────────────────────────────────
    [SerializeField] private float leftClickHoldTime;
    [SerializeField] private bool isDragging = false;
    private Vector2 dragOffset;   // Offset from character pivot to click point

    private CharacterBase selectedCharacter;
    private Camera mainCam;

    // ── Lifecycle ─────────────────────────────────────────────────────────────
    private void Awake()
    {
        mainCam = Camera.main;
    }

    private void OnMouseUp()
    {
        if (isDragging)
        {
            isDragging = false;

            selectedCharacter.Unfreeze();
        }
    }

    private IEnumerator MouseHoldTimer()
    {
        yield return new WaitForSeconds(leftClickHoldTime);
        isDragging = true;
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CheckInitialClick();
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (isDragging)
            {
                ReleaseCharacter();
            }
            else
            {
                SelectCharacter();
            }
        }   

        if (isDragging)
        {
            selectedCharacter.transform.position = MouseWorldPos() + dragOffset;
   
        }
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private void ReleaseCharacter()
    {
        isDragging = false;
        selectedCharacter.Unfreeze();
    }

    private void SelectCharacter()
    {
        StopAllCoroutines();
        bool isAlreadyFrozen = selectedCharacter.CurrentState == CharacterBase.CharacterState.Frozen;
        if (isAlreadyFrozen)
        {
            // Second left click = dismiss
            selectedCharacter.Unfreeze();
        }
        else
        {
            Debug.Log("Selected " + selectedCharacter.gameObject.name);
            selectedCharacter.Freeze();
        }
        return;
    }

    private void CheckInitialClick()
    {
        Vector2 mouseWorldPoint = mainCam.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPoint, Vector2.zero);
        if (hit.collider != null && hit.collider.CompareTag("Character"))
        {
            StartCoroutine(MouseHoldTimer());
            selectedCharacter = hit.collider.GetComponent<CharacterBase>();
        }
    }



    private Vector2 MouseWorldPos()
    {
        return mainCam.ScreenToWorldPoint(Input.mousePosition);
    }

    
}