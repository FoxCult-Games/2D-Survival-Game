using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using Resources.Scripts.SO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Resources.Scripts.Resources
{
    public class ResourcesPanel : MonoBehaviour
    {
        [SerializeField] private SO.Resource[] resources;
    
        [SerializeField] private GameObject resourcePrefab;
        [SerializeField] private Transform resourceParent;

        private void Awake()
        {
            ResourcesManagers resourceManager = ResourcesManagers.Instance;
            
            if(!resourceManager) return;
            
            foreach (SO.Resource resource in resources)
            {
                GameObject resourcePanel = Instantiate(resourcePrefab, resourceParent);
                resourcePanel.GetComponentInChildren<Image>().sprite = resource.Icon;
                resourcePanel.GetComponentInChildren<TextMeshProUGUI>().text = resourceManager.GetResourceAmount(resource.Type).ToString();

                ResourcePanelItem resourcePanelItem = resourcePanel.AddComponent<ResourcePanelItem>();
                resourcePanelItem.ResourceType = resource.Type;
            }
            
            resourceManager.OnResourceAmountChanged += UpdateResourceAmount;
        }
        
        private void UpdateResourceAmount(object sender, ResourceEventArgs e)
        {
            List<ResourcePanelItem> resourcePanels = resourceParent.GetComponentsInChildren<ResourcePanelItem>().ToList();
            resourcePanels
                .Find(panel => panel.ResourceType == e._type)
                .GetComponentInChildren<TextMeshProUGUI>().text = e._amount.ToString();
        }
    }
}
