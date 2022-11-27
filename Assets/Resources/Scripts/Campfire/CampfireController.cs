using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Resources.Scripts.SO;
using Unity.VisualScripting;
using UnityEngine.Rendering.Universal;

namespace Resources.Scripts.Campfire
{
    public class CampfireController : MonoBehaviour
    {
        [SerializeField] private SO.Campfire campfireData;
        private GameObject _campfireGameObject;

        private Light2D _campfireLight;
        private Animator _campfireAnimator;
        private float _lightIntensity;
        private float _lightRange;
        
        public event EventHandler<EventArgs> OnCampfireLit; 
        public event EventHandler<EventArgs> OnCampfireExtinguished;
        public event EventHandler<EventArgs> OnCampfireBurnedOut; 
        public event EventHandler<EventArgs> OnCampfireReplenished;
        
        private Coroutine _burningCoroutine;

        private int _currentStage;

        [HideInInspector] public bool playerInRange = false;

        public static CampfireController Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            
            _campfireGameObject = GameObject.FindGameObjectWithTag("Campfire");
            
            _campfireLight = _campfireGameObject.GetComponent<Light2D>();
            _campfireAnimator = _campfireGameObject.GetComponent<Animator>();
            _lightIntensity = _campfireLight.intensity;
            _lightRange = _campfireLight.pointLightOuterRadius;
            
            OnCampfireExtinguished += ChangeState;
            OnCampfireReplenished += ChangeState;
            OnCampfireReplenished += ResetBurning;
            
            OnCampfireLit += StartBurning;
            OnCampfireLit += ChangeState;
            OnCampfireBurnedOut += StopBurning;
        }

        private void Start()
        {
            campfireData.Init();
            _currentStage = 0;
            ChangeState(this, EventArgs.Empty);
        }

        private void ChangeVisuals(AnimationClip newVisual)
        {
            _campfireAnimator.Play(newVisual.name);
                
            _campfireLight.intensity = _lightIntensity * ((float)_currentStage / campfireData.StagesAmount);
            _campfireLight.pointLightOuterRadius = _lightRange * ((float)_currentStage / campfireData.StagesAmount);
        }
        
        private void ChangeState(object sender, EventArgs e)
        {
            AnimationClip newVisual = campfireData.SetVisuals(_currentStage);
            ChangeVisuals(newVisual);
        }
        
        private void StartBurning(object sender, EventArgs e)
        {
            _burningCoroutine = StartCoroutine(Burning());
        }
        
        private void StopBurning(object sender, EventArgs e)
        {
            StopCoroutine(_burningCoroutine);
        }
        
        private void StopBurning()
        {
            StopCoroutine(_burningCoroutine);
        }

        public bool Replenish()
        {
            if (_currentStage >= campfireData.StagesAmount) return false;

            if (_currentStage <= 0 && !campfireData.HasBeenLit) return Lit();

            _currentStage++;
            OnCampfireReplenished?.Invoke(this, EventArgs.Empty);
            
            return true;
        }

        private bool Lit()
        {
            _currentStage = campfireData.StagesAmount;
            campfireData.SetState(CampfireState.BURNING);
            OnCampfireLit?.Invoke(this, EventArgs.Empty);

            return true;
        }
        
        private void ResetBurning(object o, EventArgs e)
        {
            StopBurning();
            _burningCoroutine = StartCoroutine(Burning());
        }
        
        private IEnumerator Burning()
        {
            while (true)
            {
                yield return new WaitForSeconds(campfireData.LifeTime);
                _currentStage--;
                if (_currentStage == 0) OnCampfireBurnedOut?.Invoke(this, EventArgs.Empty);
                else OnCampfireExtinguished?.Invoke(this, EventArgs.Empty);
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            
            playerInRange = true;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            
            playerInRange = false;
        }
    }
}
