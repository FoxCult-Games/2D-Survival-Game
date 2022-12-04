using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableManager : MonoBehaviour
{
    public static InteractableManager Instance;
    
    private List<Interactable> _interactablesInRange = new List<Interactable>();
    [SerializeField] private Interactable focusedInteractable;
    public Interactable FocusedInteractable => focusedInteractable;

    private void Awake()
    {
        Instance = this;
    }

    public void Refocus()
    {
        float closestDistance = Mathf.Infinity;
        Interactable closestInteractable = null;
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        if (!playerTransform || _interactablesInRange.Count == 0)
        {
            focusedInteractable = null;
            return;
        }

        if (playerTransform)
        {
            foreach (Interactable interactable in _interactablesInRange)
            {
                float distance = Vector3.Distance(interactable.transform.position, playerTransform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestInteractable = interactable;
                }
            }
        }

        if(closestInteractable && closestInteractable != focusedInteractable)
        {
            if(focusedInteractable) focusedInteractable.Unfocus();
            focusedInteractable = closestInteractable;
            focusedInteractable.Focus();
        }
    }

    public void AddInteractable(Interactable interactable)
    {
        _interactablesInRange.Add(interactable);
    }
    
    public void RemoveInteractable(Interactable interactable)
    {
        _interactablesInRange.Remove(interactable);
    }
}
