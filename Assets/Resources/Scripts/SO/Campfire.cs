using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MinMaxCurve = UnityEngine.ParticleSystem.MinMaxCurve;

namespace Resources.Scripts.SO
{
    [CreateAssetMenu(menuName = "Data Objects/Campfire", fileName = "Campfire")]
    public class Campfire : ScriptableObject
    {
        [SerializeField] private int stagesAmount;
        [SerializeField] private Color[] colors;
        [SerializeField] private AnimationClip unlitAnimation;
        [SerializeField] private AnimationClip[] animations;
        [SerializeField] private CampfireParticles[] particles;
        [SerializeField] private float lifeTime;
        [SerializeField] private float heatRadius;
        
        public int StagesAmount => stagesAmount;
        public float LifeTime => lifeTime;
        public float HeatRadius => heatRadius;
        public AnimationClip UnlitAnimation => unlitAnimation;
        public AnimationClip[] Animations => animations;
        public CampfireParticles[] Particles => particles;
        
        private void OnValidate()
        {
            if (colors.Length == stagesAmount) return;
            
            Color[] newColors = new Color[stagesAmount];
            AnimationClip[] newAnimations = new AnimationClip[stagesAmount];
            CampfireParticles[] newParticles = new CampfireParticles[stagesAmount];

            for (int i = 0; i < newColors.Length; i++)
            {
                newColors[i] = i < colors.Length ? colors[i] : Color.white;
                newAnimations[i] = i < animations.Length ? animations[i] : new AnimationClip();
                newParticles[i] = i < particles.Length ? particles[i] : new CampfireParticles();
            }
            
            colors = newColors;
            animations = newAnimations;
            particles = newParticles;
        }
    }

    public enum CampfireState
    {
        NOT_LIT,
        BURNING,
        BURNT,
    }

    [Serializable]
    public struct CampfireParticles
    {
        [SerializeField] private int stage;
        [SerializeField] private float particlesAmount;
        [SerializeField] private MinMaxCurve particlesSize;
        [SerializeField] private float particlesSpeed;
        
        public int Stage => stage;
        public float ParticlesAmount => particlesAmount;
        public MinMaxCurve ParticlesSize => particlesSize;
        public float ParticlesSpeed => particlesSpeed;
    }
}

