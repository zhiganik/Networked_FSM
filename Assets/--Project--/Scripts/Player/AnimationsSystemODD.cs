using System;
using Fusion;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace __Project__.Scripts.Player
{
    [RequireComponent(typeof(Animator))]
    public class AnimationsSystemODD : NetworkBehaviour
    {
        [SerializeField] private AnimationClip idleCLip;
        [SerializeField] private AnimationClip moveCLip;
        [SerializeField] private AnimationClip runCLip;
        
        private Animator _animator;
        private PlayableGraph _playableGraph;
        private AnimationPlayableOutput _playableOutput;
        
        private AnimationMixerPlayable _topLevelMixer;
        private AnimationMixerPlayable _locomotionMixer;

        private AnimationClipPlayable _idle;
        private AnimationClipPlayable _walkForward;
        private AnimationClipPlayable _run;
        
        private void OnValidate()
        {
            _animator ??= GetComponent<Animator>();
        }

        private void Awake()
        {
            _animator ??= GetComponent<Animator>();
        }

        public override void Spawned()
        {
            if(!HasStateAuthority) return;
            
            _playableGraph = PlayableGraph.Create("Animation System");
            _playableOutput = AnimationPlayableOutput.Create(_playableGraph, "Animation", _animator);

            _topLevelMixer = AnimationMixerPlayable.Create(_playableGraph, 2);
            _playableOutput.SetSourcePlayable(_topLevelMixer);

            _locomotionMixer = AnimationMixerPlayable.Create(_playableGraph, 2);
            _topLevelMixer.ConnectInput(0, _locomotionMixer, 0);
            
            _playableGraph.GetRootPlayable(0)
                .SetInputWeight(0, 1f);

            _idle = AnimationClipPlayable.Create(_playableGraph, idleCLip);
            _walkForward = AnimationClipPlayable.Create(_playableGraph, moveCLip);
            _run = AnimationClipPlayable.Create(_playableGraph, runCLip);

            _idle.GetAnimationClip().wrapMode = WrapMode.Loop;
            _walkForward.GetAnimationClip().wrapMode = WrapMode.Loop;
            _run.GetAnimationClip().wrapMode = WrapMode.Loop;
            
            _locomotionMixer.ConnectInput(0, _idle, 0);
            _locomotionMixer.ConnectInput(1, _walkForward, 0);
            //_locomotionMixer.ConnectInput(2, _run, 0);
            _playableGraph.Play();
        }

        public void UpdateLocomotion(Vector3 velocity, float maxSpeed)
        {
            float weight = Mathf.InverseLerp(0f, maxSpeed, velocity.magnitude);
            _locomotionMixer.SetInputWeight(0, 1f - weight);
            _locomotionMixer.SetInputWeight(1, weight);
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            if(!HasStateAuthority) return;
            
            if (_playableGraph.IsValid())
            {
                _playableGraph.Destroy();
            }
        }
    }
}