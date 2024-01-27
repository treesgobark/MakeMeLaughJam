using System;
using System.Collections.Generic;
using System.Text;
using FlatRedBall;
using FlatRedBall.Input;
using FlatRedBall.Instructions;
using FlatRedBall.AI.Pathfinding;
using FlatRedBall.Graphics.Animation;
using FlatRedBall.Graphics.Particle;
using FlatRedBall.Math.Geometry;
using MakeMeLaughJam.Utils;
using Microsoft.Xna.Framework;

namespace MakeMeLaughJam.Entities
{
    public partial class Player
    {
        public bool IsPunching { get; set; }

        private double _lastPunchTime { get; set; } = -69_420;
        private double TotalPunchTime => PunchDuration + PunchRecovery;
        public bool CanPunch => TimeManager.CurrentScreenSecondsSince(_lastPunchTime)      > TotalPunchTime;
        public bool IsPunchActive => TimeManager.CurrentScreenSecondsSince(_lastPunchTime) < PunchDuration;
        private GameplayInputDevice GameplayInputDevice { get; set; }
        
        /// <summary>
        /// Initialization logic which is executed only one time for this Entity (unless the Entity is pooled).
        /// This method is called when the Entity is added to managers. Entities which are instantiated but not
        /// added to managers will not have this method called.
        /// </summary>
        private void CustomInitialize()
        {

        }

        partial void CustomInitializePlatformerInput()
        {
            GameplayInputDevice = new GameplayInputDevice(InputDevice, this);
        }

        private void CustomActivity()
        {
            HandleInput();
            if (CanPunch)
            {
                IsPunching = false;
                AxisAlignedRectangleInstance.Color = Color.White;
            }
        }

        private void HandleInput()
        {
            if (GameplayInputDevice.Attack.WasJustPressed)
            {
                if (CanPunch)
                {
                    _lastPunchTime                     = TimeManager.CurrentScreenTime;
                    IsPunching                         = true;
                    AxisAlignedRectangleInstance.Color = Color.Red;
                }
            }
        }

        private void CustomDestroy()
        {


        }

        private static void CustomLoadStaticContent(string contentManagerName)
        {


        }
    }
}
