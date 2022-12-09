using Resources.Scripts.Interaction;
using UnityEngine;
using UnityEngine.Events;

namespace Resources.Scripts.Player
{
    public class CharacterController2D : MonoBehaviour
    {
        [SerializeField] private SO.Player playerData;
        private PlayerInputs _playerInputs;
        
        private Vector2 _velocity;
        private float _lastDash = -100f;

        [SerializeField] private UnityEvent onPlayerMove;

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
            onPlayerMove.Invoke();
        }
        
        private void Dash()
        {
            if(_lastDash + playerData.DashCooldown > Time.time) return;
            
            transform.Translate(_velocity * (playerData.DashSpeed * Time.fixedDeltaTime));
            _lastDash = Time.time;
        }

        private void Interact()
        {
            if(InteractableManager.Instance.FocusedInteractable) 
                InteractableManager.Instance.FocusedInteractable.Interact();
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
