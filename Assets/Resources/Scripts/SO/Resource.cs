using System.Collections;
using System.Collections.Generic;
using Resources.Scripts.Resources;
using UnityEngine;

namespace Resources.Scripts.SO
{
    [CreateAssetMenu(fileName = "New Resource", menuName = "Inventory/Resource")]
    public class Resource : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private ResourceType _type;
        [SerializeField] private Sprite _icon;
        [SerializeField] private GameObject _prefab;
        
        public string Name => _name;
        public ResourceType Type => _type;
        public Sprite Icon => _icon;
        public GameObject Prefab => _prefab;
    }
}
