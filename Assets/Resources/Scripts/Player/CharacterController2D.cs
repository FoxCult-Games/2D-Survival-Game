using System;
using System.Collections;
using System.Collections.Generic;
using Resources.Scripts;
using Resources.Scripts.Campfire;
using Resources.Scripts.Resources;
using UnityEditor;
using UnityEngine;

namespace Resources.Scripts.Player
{
    public class CharacterController2D : MonoBehaviour
    {
        [SerializeField] private SO.Player playerData;
        private PlayerInputs _playerInputs;
        
        private Vector2 _velocity;
        private float _lastDash = -100f;

        private void Awake()
        {
            if (!playerData) return;
            
            _playerInputs = new PlayerInputs();
            _playerInputs.Gameplay.Move.performed += ctx => _velocity = ctx.ReadValue<Vector2>();
            _playerInputs.Gameplay.Move.canceled += ctx => _velocity = Vector2.zero;
            
            _playerInputs.Gameplay.Dash.performed += ctx => Dash();

            _playerInputs.Gameplay.Interaction.performed += ctx => Interact();
        }

        private void FixedUpdate()
        {
            if(_velocity != Vector2.zero) Move(_velocity);
        }

        private void Move(Vector2 direction)
        {
            transform.Translate(direction * (playerData.MoveSpeed * Time.fixedDeltaTime));
        }
        
        private void Dash()
        {
            if(_lastDash + playerData.DashCooldown > Time.time) return;
            
            transform.Translate(_velocity * (playerData.DashSpeed * Time.fixedDeltaTime));
            _lastDash = Time.time;
        }

        private void Interact()
        {
            if (CampfireController.Instance != null && CampfireController.Instance.playerInRange)
            {
                CampfireController.Instance.Replenish();
            } 
            else if (ResourcesManagers.Instance.ClosestResourceInRange(transform.position) != null)
            {
                ResourcesManagers.Instance.ClosestResourceInRange(transform.position)?.Collect();
            }
        }

        private void OnEnable()
        {
            _playerInputs.Enable();
        }

        private void OnDisable()
        {
            _playerInputs.Disable();
        }
    }
}