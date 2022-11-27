using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Resources.Scripts.SO
{
    [CreateAssetMenu(menuName = "Data Objects/Campfire", fileName = "Campfire")]
    public class Campfire : ScriptableObject
    {
        [SerializeField] private int stagesAmount;
        [SerializeField] private Color[] colors;
        [SerializeField] private AnimationClip unlitAnimation;
        [SerializeField] private AnimationClip[] animations;
        [SerializeField] private float lifeTime;
        [SerializeField] private float heatRadius;
        [SerializeField] private CampfireState state;
        
        [SerializeField] private bool hasBeenLit;
        
        public int StagesAmount => stagesAmount;
        public float LifeTime => lifeTime;
        public float HeatRadius => heatRadius;
        public CampfireState State => state;
        
        public bool HasBeenLit => hasBeenLit;

        public void Init()
        {
            hasBeenLit = false;
            state = CampfireState.NOT_LIT;
        }
        
        public void SetState(CampfireState newState)
        {
            state = newState;
            if(newState == CampfireState.BURNING) hasBeenLit = true;
        }

        public AnimationClip SetVisuals(int stage, bool useColor = false)
        {
            return stage == 0 ? unlitAnimation : animations[stagesAmount - stage];
        }
        
        private void OnValidate()
        {
            if (colors.Length == stagesAmount) return;
            
            Color[] newColors = new Color[stagesAmount];
            AnimationClip[] newAnimations = new AnimationClip[stagesAmount];

            for (int i = 0; i < newColors.Length; i++)
            {
                newColors[i] = i < colors.Length ? colors[i] : Color.white;
                newAnimations[i] = i < animations.Length ? animations[i] : null;
            }
            
            colors = newColors;
            animations = newAnimations;
        }
    }

    public enum CampfireState
    {
        NOT_LIT,
        BURNING,
        BURNT,
    }
}

