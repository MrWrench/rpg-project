using UnityEngine;

namespace Game.Input
{
    public abstract class CharacterInput : MonoBehaviour
    {
        public Vector2 MoveInput { get; protected set; }
        public bool Attack { get; protected set; }
    }
}
