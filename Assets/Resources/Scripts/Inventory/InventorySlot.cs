using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Resources.Scripts.Inventory
{
    public class InventorySlot<T> : MonoBehaviour
    {
        private bool _isActive = false;
        private T _itemInSlot;

        public T ItemInSlot() => _itemInSlot;
        public void SetItem(T item) => _itemInSlot = item;
        public void RemoveItem() => _itemInSlot = default(T);
        
        public bool IsActive() => _isActive;
        public void SetActive() => _isActive = true;
        public void SetInactive() => _isActive = false;
    }
}
