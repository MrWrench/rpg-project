using System;
using UnityEngine;

namespace Game.Input
{
    public class PlayerInput : CharacterInput
    {
        private void Update()
        {
            var x = UnityEngine.Input.GetAxis("Horizontal");
            var y = UnityEngine.Input.GetAxis("Vertical");
            MoveInput = new Vector2(x, y);
            Attack = UnityEngine.Input.GetButton("Fire1");
        }
    }
}
