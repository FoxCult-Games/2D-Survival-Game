using System.Collections;
using System.Collections.Generic;
using Resources.Scripts.SO;
using UnityEngine;

namespace Resources.Scripts.ErrorHandlers
{
    public class Exception : MonoBehaviour
    {
        [SerializeField] private ErrorMessages errorMessages;
        
        public static Exception Instance { get; private set; }
        
        private void Awake()
        {
            Instance = this;
        }
        
        public void DisplayError(ErrorType type)
        {
            Debug.Log("Error: " + type + ",\n" + errorMessages.GetMessage(type));
        }
    }
}
