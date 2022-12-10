using System;
using UnityEngine;

namespace Resources.Scripts.Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager Instance { get; private set; }

        // private InventorySlot[] _slots;

        private void Awake()
        {
            Instance = this;
        }
    }
}