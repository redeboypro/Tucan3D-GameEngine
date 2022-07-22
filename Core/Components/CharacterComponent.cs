using System;
using Tucan3D_GameEngine.GameComponents;

namespace Tucan3D_GameEngine.Core.Components
{
    public class CharacterComponent : IComponent
    {
        private float radius = 0.5f;
        private bool isGrounded = false;
        private Action landingEvent;

        public float Radius
        {
            get => radius;
            set => radius = value;
        }

        public bool IsGrounded => isGrounded;

        public void AssignLandingEvent()
        {
            
        }
    }
}