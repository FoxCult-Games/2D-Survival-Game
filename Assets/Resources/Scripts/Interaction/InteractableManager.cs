using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Resources.Scripts.Interaction
{
    public class InteractableManager : MonoBehaviour
    {
        public static InteractableManager Instance;

        private Transform _player;
        
        private List<Interactable> _interactablesInRange = new List<Interactable>();
        [SerializeField] private Interactable focusedInteractable;
        [SerializeField] private GameObject interactUI;
        public Interactable FocusedInteractable => focusedInteractable;

        private void Awake()
        {
            Instance = this;
            _player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        public void Refocus()
        {
            float closestDistance = Mathf.Infinity;
            Interactable closestInteractable = null;

            if (!_player || _interactablesInRange.Count == 0)
            {
                focusedInteractable = null;
                return;
            }

            if (_player)
            {
                foreach (Interactable interactable in _interactablesInRange)
                {
                    float distance = Vector3.Distance(interactable.transform.position, _player.position);

                    if (!(distance < closestDistance)) continue;
                    
                    closestDistance = distance;
                    closestInteractable = interactable;
                }
            }

            if (!closestInteractable || closestInteractable == focusedInteractable) return;
            
            if(focusedInteractable) focusedInteractable.Unfocus();
            focusedInteractable = closestInteractable;
            focusedInteractable.Focus();
        }

        public void AddInteractable(Interactable interactable)
        {
            _interactablesInRange.Add(interactable);
        }
        
        public void RemoveInteractable(Interactable interactable)
        {
            _interactablesInRange.Remove(interactable);
        }
        
        public GameObject GetInteractUI() => interactUI;
    }
}
