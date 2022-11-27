using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Resources.Scripts.ErrorHandlers;
using UnityEngine;

namespace Resources.Scripts.SO
{
    [CreateAssetMenu(fileName = "Error Messages", menuName = "Error Handlers/Messages")]
    public class ErrorMessages : ScriptableObject
    {
        public ErrorMessage[] errorMessages;
        
        public string GetMessage(ErrorType type) => errorMessages.ToList().Find(err => err.Type == type).Message;
    }

    [Serializable]
    public struct ErrorMessage
    {
        [SerializeField] private ErrorType _type;
        [SerializeField] private string _message;
        
        public ErrorType Type => _type;
        public string Message => _message;
    }
}