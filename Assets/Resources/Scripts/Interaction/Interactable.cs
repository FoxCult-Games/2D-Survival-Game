using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Events;

namespace Resources.Scripts.Interaction
{
    [RequireComponent(typeof(Collider2D))]
    public class Interactable : MonoBehaviour
    {
        private Collider2D _collider;
        private InteractableManager _interactableManager;
        private SpriteRenderer _spriteRenderer;
        
        [SerializeField] private float interactionTime = 2f;
        private bool _isEnabled = true;
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
            _interactableManager = InteractableManager.Instance;
            _spriteRenderer = GetComponent<SpriteRenderer>();
            onInteract.AddListener(StartTimer);
        }

        public void Focus()
        {
            if (isInteracting) return;
            
            SpawnInteractionUI();
            isFocused = true;
        }

        public void Unfocus()
        {
            RemoveInteractionUI();
            isFocused = false;
        }

        public void Interact()
        {
            if (!_isEnabled || !isInRange || !isFocused || isInteracting) return;
            
            isInteracting = true;
            RemoveInteractionUI();
            onInteract.Invoke();
        }

        private void SpawnInteractionUI()
        {
            if (!_isEnabled) return;
            
            Vector2 position = transform.position;
            GameObject interactUI = Instantiate(
                _interactableManager.GetInteractUI(), 
                new Vector2(position.x, position.y + _spriteRenderer.size.y), 
                Quaternion.identity,
                transform
            );
            
            PlayInteractAnimation(interactUI);
        }

        private void RemoveInteractionUI()
        {
            GameObject interactionUI = transform.GetComponentInChildren<InteractionBox>()?.gameObject;
            if(interactionUI) Destroy(interactionUI);
        }

        private void PlayInteractAnimation(GameObject interactUI)
        {
            if (!_isEnabled) return;
            
            Animator animator = interactUI.GetComponent<Animator>();
            animator.Play("Interaction Box Opening");
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

        public bool IsEnabled() => _isEnabled;
        public void Enable() => _isEnabled = true;
        public void Disable() => _isEnabled = false;
    }
}
