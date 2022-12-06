using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Resources.Scripts.ErrorHandlers;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.Serialization;
using Exception = Resources.Scripts.ErrorHandlers.Exception;

namespace Resources.Scripts.Resources
{
    public class ResourcesManagers : MonoBehaviour
    {
        private ResourcesPanel _resourcesPanel;
        
        private List<Resource> _resources = new List<Resource>();
        
        [SerializeField] private List<InventoryResource> _inventoryResources = new List<InventoryResource>();

        private List<Resource> _resourcesInRange = new List<Resource>();
        
        public event EventHandler<Resource> OnResourceAdded;
        public event EventHandler<Resource> OnResourceRemoved;

        public event EventHandler<Resource> OnResourceGathered; 
        
        public event EventHandler<ResourceEventArgs> OnResourceAmountChanged; 

        public static ResourcesManagers Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            LoadResources();
            
            _resourcesPanel = FindObjectOfType<ResourcesPanel>();
        }

        public void AddResourceInRange(Resource resource)
        {
            _resourcesInRange.Add(resource);
            OnResourceAdded?.Invoke(this, resource);
        }
        
        public void RemoveResourceInRange(Resource resource)
        {
            _resourcesInRange.Remove(resource);
            OnResourceRemoved?.Invoke(this, resource);
        }
        
        public Resource? ClosestResourceInRange(Vector3 position)
        {
            if(_resourcesInRange.Count == 0) return null;
            
            Resource closestResource = default!;
            float closestDistance = Mathf.Infinity;
            
            foreach (Resource resource in _resourcesInRange)
            {
                float distance = Vector3.Distance(position, resource._resource.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestResource = resource;
                }
            }
            
            return closestResource;
        }
        
        public InventoryResource GatherResource(Resource resource)
        {
            OnResourceGathered?.Invoke(this, resource);
            
            int? newAmount = _inventoryResources.FirstOrDefault(r => r._type == resource._type)?.ChangeAmount(1);
            
            OnResourceAmountChanged?.Invoke(this, new ResourceEventArgs(resource._type, newAmount ?? 0));
            
            resource.ChangeAmount(-1);
            
            return _inventoryResources.Find(r => r._type == resource._type);
        }

        private void LoadResources()
        {
            string[] resourcesTypes = System.Enum.GetNames (typeof(ResourceType));

            foreach (string resourceType in resourcesTypes)
            {
                _inventoryResources.Add(new InventoryResource((ResourceType)System.Enum.Parse(typeof(ResourceType), resourceType), 0));
            }
        }
        
        public int GetResourceAmount(ResourceType type) => _inventoryResources.ToList().Find(r => r._type == type)._amount;
        
        public bool HasResource(ResourceType type) => _inventoryResources.ToList().Find(r => r._type == type)._amount > 0;
        
        public bool EnoughResources(ResourceType type, int amount) => _inventoryResources.ToList().Find(r => r._type == type)._amount >= amount;
    }

    public class ResourceEventArgs
    {
        public ResourceType _type { get; private set; }
        public int _amount { get; private set; }

        public ResourceEventArgs(ResourceType type, int amount)
        {
            _type = type;
            _amount = amount;
        }
    }

}