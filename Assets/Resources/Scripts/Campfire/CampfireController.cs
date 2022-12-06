using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
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
        private ParticleSystem _campfireParticles;
        private float _lightIntensity;
        private float _lightRange;
        private bool hasBeenLit = false;
        private CampfireState state;
        
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
            _campfireParticles = _campfireGameObject.GetComponentInChildren<ParticleSystem>();
            
            _campfireLight = _campfireGameObject.GetComponent<Light2D>();
            _campfireAnimator = _campfireGameObject.GetComponent<Animator>();
            _lightIntensity = _campfireLight.intensity;
            _lightRange = _campfireLight.pointLightOuterRadius;
            
            OnCampfireExtinguished += ChangeState;
            OnCampfireReplenished += ChangeState;
            OnCampfireReplenished += ResetBurning;
            
            OnCampfireLit += StartBurning;
            OnCampfireLit += ChangeState;
            OnCampfireLit += EnableParticles;
            OnCampfireBurnedOut += StopBurning;
            OnCampfireBurnedOut += DisableParticles;
        }

        private void Start()
        {
            Init();
            _currentStage = 0;
            ChangeState(this, EventArgs.Empty);
        }

        private void ChangeVisuals(AnimationClip newVisual)
        {
            _campfireAnimator.Play(newVisual.name);
                
            _campfireLight.intensity = _lightIntensity * ((float)_currentStage / campfireData.StagesAmount);
            _campfireLight.pointLightOuterRadius = _lightRange * ((float)_currentStage / campfireData.StagesAmount);

            ParticleSystem.MainModule campfireParticlesMain = _campfireParticles.main;
            ParticleSystem.EmissionModule campfireParticlesEmission = _campfireParticles.emission;
            CampfireParticles campfireParticles = campfireData.Particles.ToList().FirstOrDefault(stages => stages.Stage == _currentStage);
            
            campfireParticlesMain.startSize = campfireParticles.ParticlesSize;
            campfireParticlesMain.startSpeed = campfireParticles.ParticlesSpeed;
            campfireParticlesEmission.rateOverTime = campfireParticles.ParticlesAmount;
        }
        
        private void ChangeState(object sender, EventArgs e)
        {
            AnimationClip newVisual = SetVisuals(_currentStage);
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

        public void Replenish()
        {
            if (_currentStage >= campfireData.StagesAmount) return;

            if (_currentStage <= 0 && !hasBeenLit)
            {
                Lit();
                return;
            }

            _currentStage++;
            OnCampfireReplenished?.Invoke(this, EventArgs.Empty);
        }

        private void Lit()
        {
            _currentStage = campfireData.StagesAmount;
            state = CampfireState.BURNING;
            OnCampfireLit?.Invoke(this, EventArgs.Empty);
        }

        private void EnableParticles(object o, EventArgs e)
        {
            _campfireParticles.Play();
        }
        
        private void DisableParticles(object o, EventArgs e)
        {
            _campfireParticles.Stop();
        }
        
        private void ResetBurning(object o, EventArgs e)
        {
            StopBurning();
            _burningCoroutine = StartCoroutine(Burning());
        }

        public void Init()
        {
            hasBeenLit = false;
            state = CampfireState.NOT_LIT;
        }

        public AnimationClip SetVisuals(int stage, bool useColor = false)
        {
            return stage == 0 ? campfireData.UnlitAnimation : campfireData.Animations[campfireData.StagesAmount - stage];
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
