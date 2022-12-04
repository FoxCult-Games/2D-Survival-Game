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

        public void Collect()
        {
            resource.Collect();
        }
    }
}
