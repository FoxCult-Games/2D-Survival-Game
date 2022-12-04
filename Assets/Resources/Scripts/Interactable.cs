using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class Interactable : MonoBehaviour
{
    private Collider2D _collider;
    
    [SerializeField] private float interactionTime = 2f;
    [SerializeField] private bool isInteracting = false;
    [SerializeField] private bool isInRange = false;
    [SerializeField] private bool isFocused = false;
    
    [Header("")]
    [SerializeField] private UnityEvent onInteract;

    [SerializeField] private UnityEvent onInteractEnd;

    [SerializeField] private UnityEvent onIntersectionEnter;
    [SerializeField] private UnityEvent onIntersectionExit;
    
    [Header("Visuals")]
    [SerializeField] private bool drawGizmos = true;
    [SerializeField] private Color defaultColor = Color.yellow;
    [SerializeField] private Color intersectionColor = Color.red;
    [SerializeField] private Color focusColor = Color.green;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        onInteract.AddListener(StartTimer);
    }

    public bool Focus() => isFocused = true;
    public bool Unfocus() => isFocused = false;
    
    public void Interact()
    {
        if (isInRange && isFocused && !isInteracting)
        {
            isInteracting = true;
            onInteract.Invoke();
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        if (!drawGizmos) return;
        
        Gizmos.color = isFocused 
            ? focusColor : isInRange
                ? intersectionColor : defaultColor;
        Gizmos.DrawWireSphere(transform.position, _collider ? _collider.bounds.extents.x : 3f);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        InteractableManager.Instance.AddInteractable(this);
        isInRange = true;
        onIntersectionEnter.Invoke();
    }
    
    private void OnTriggerExit2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        InteractableManager.Instance.RemoveInteractable(this);
        isInRange = false;
        onIntersectionExit.Invoke();
    }
    
    private void StartTimer() => StartCoroutine(InteractionTimer());
    
    private IEnumerator InteractionTimer()
    {
        yield return new WaitForSeconds(interactionTime);
        onInteractEnd.Invoke();
        isInteracting = false;
    }
}
