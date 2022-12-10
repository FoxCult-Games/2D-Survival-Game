using UnityEngine;

namespace Resources.Scripts.SO
{
    public class InventorySlot<T> : ScriptableObject
    {
        private T _item;
        private Sprite _itemSprite;

        public T Item() => _item;
        public Sprite ItemSprite() => _itemSprite;
    }
}