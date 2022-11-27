using System;
using Resources.Scripts.ErrorHandlers;
using UnityEngine;
using Exception = Resources.Scripts.ErrorHandlers.Exception;

namespace Resources.Scripts.Resources
{
    [Serializable]
    public class Resource : IResource
    {
        public Transform _resource { get; private set; }
        public ResourceType _type { get; }
        public int _amount { get; private set; }
        
        public Resource(ResourceType type, int amount)
        {
            _type = type;
            _amount = amount;
        }
        
        public void SetTransform(Transform transform) => _resource = transform;
        public int ChangeAmount(int amount) => _amount += amount;

        public bool IsEmpty() => _amount <= 0;
        
        public InventoryResource Collect()
        {
            if (!IsEmpty()) return ResourcesManagers.Instance.GatherResource(this);
            
            Exception.Instance.DisplayError(ErrorType.ResourceNotAvailable);
            return null;
        }
    }
}