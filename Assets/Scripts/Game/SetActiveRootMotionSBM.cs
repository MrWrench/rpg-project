using UnityEngine;

namespace Game
{
    public class SetActiveRootMotionSBM : StateMachineBehaviour
    {
        [SerializeField] private bool _value = true;
        private readonly int _parameterHash = Animator.StringToHash("UseRootMotion");

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool(_parameterHash, _value);
        }
    }
}
