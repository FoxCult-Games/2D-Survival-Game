using UnityEngine;

namespace Resources.Scripts.Resources
{
    public class InventoryResource : IResource
    {
        public ResourceType _type { get; }
        public int _amount { get; private set; }
        
        public InventoryResource(ResourceType type, int amount)
        {
            _type = type;
            _amount = amount;
        }

        public int ChangeAmount(int amount)
        {
            if (_amount + amount < 0) _amount = 0;
            return _amount += amount;
        }

        public bool IsEmpty() => _amount <= 0;
    }
}