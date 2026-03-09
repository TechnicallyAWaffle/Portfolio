using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterClickHandler : MonoBehaviour
{

    private static bool IsLeftClick() => Input.GetMouseButtonDown(0);
    private static bool IsRightClick() => Input.GetMouseButtonDown(1);

    // ── Drag state ────────────────────────────────────────────────────────────
    private bool isDragging = false;
    private Vector2 dragOffset;   // Offset from character pivot to click point

    private CharacterBase selectedCharacter;
    private Camera mainCam;

    // ── Lifecycle ─────────────────────────────────────────────────────────────
    private void Awake()
    {
        selectedCharacter = GetComponent<CharacterBase>();
        mainCam = Camera.main;
    }

    // ── Mouse events ──────────────────────────────────────────────────────────

    private void OnMouseDown()
    {
        Debug.Log("meow");
        Vector2 mouseWorldPoint = mainCam.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPoint, Vector2.zero);
        // Left click — inspect
        Debug.Log("Raycast Hit: " + hit);
        if (hit.collider != null && hit.collider.CompareTag("Character"))
        {
            selectedCharacter = hit.collider.GetComponent<CharacterBase>();
            HandleLeftClick();
            HandleRightClick();
        }
    }

    private void HandleRightClick()
    {
        if (IsRightClick())
        {
            // If the info panel is open, close it before dragging
            if (selectedCharacter.CurrentState == CharacterBase.CharacterState.Frozen)
                //CharacterInfoUI.Instance?.Hide();

            isDragging = true;
            dragOffset = (Vector2)transform.position - MouseWorldPos();
        }
    }

    private void HandleLeftClick()
    {
        if (IsLeftClick())
        {
            bool isAlreadyFrozen = selectedCharacter.CurrentState == CharacterBase.CharacterState.Frozen;
            if (isAlreadyFrozen)
            {
                // Second left click = dismiss
                selectedCharacter.Unfreeze();
            }
            else
            {
                selectedCharacter.Freeze();
            }
            return;
        }
    }

    private void OnMouseUp()
    {
        if (isDragging)
        {
            isDragging = false;

            selectedCharacter.Unfreeze();
        }
    }

    private void Update()
    {

        if (isDragging)
        {
            selectedCharacter.transform.position = MouseWorldPos() + dragOffset;

            // Safety: release if the right mouse button comes up between frames
            if (!Input.GetMouseButton(1))
            {
                isDragging = false;
                selectedCharacter.Unfreeze();
            }
        }
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private Vector2 MouseWorldPos()
    {
        return mainCam.ScreenToWorldPoint(Input.mousePosition);
    }

    
}