using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Resources.Scripts.SO
{
    [CreateAssetMenu(menuName = "Data Objects/Player", fileName = "Player")]
    public class Player : ScriptableObject
    {
        [Header("Player movement")]
        [SerializeField] private float moveSpeed;
        
        [SerializeField] private float dashSpeed;
        [SerializeField] private float dashCooldown;
        
        [Header("Player stats")]
        [SerializeField] private float maxHealth;
        
        public float MoveSpeed => moveSpeed;
        public float DashSpeed => dashSpeed;
        public float DashCooldown => dashCooldown;
        public float MaxHealth => maxHealth;
    }
}
