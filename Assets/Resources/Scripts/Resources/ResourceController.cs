using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Resources.Scripts.Resources
{
    public class ResourceController : MonoBehaviour
    {
        [SerializeField] private int _amount;
        [SerializeField] private ResourceType _resourceType;
        
        [SerializeField] private Resource resource;

        private void Awake()
        {
            resource = new Resource(_resourceType, _amount);
            resource.SetTransform(transform);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
                ResourcesManagers.Instance.AddResourceInRange(resource);
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
                ResourcesManagers.Instance.RemoveResourceInRange(resource);
        }
    }
}
