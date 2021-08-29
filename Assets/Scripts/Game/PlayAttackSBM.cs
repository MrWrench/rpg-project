using System;
using Game;
using UniRx;
using UnityEngine;

namespace Game
{
    public class PlayAttackSBM : StateMachineBehaviour
    {
        public int AttackIndex = 0;
        public float Delay;
        private readonly int _animIDAttackIndex = Animator.StringToHash("AttackIndex");

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetInteger(_animIDAttackIndex, AttackIndex);
            if (Delay > 0)
                Observable.Timer(TimeSpan.FromSeconds(Delay))
                    .Subscribe(_ => PlayVFX(animator));
            else
                PlayVFX(animator);
        }

        private void PlayVFX(Animator animator)
        {
            var player = animator.GetComponent<PlayerCharacter>();
            player.PlayAttackTrail();
        }
    }
}
