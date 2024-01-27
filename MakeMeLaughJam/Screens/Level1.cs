using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using FlatRedBall;
using FlatRedBall.Input;
using FlatRedBall.Instructions;
using FlatRedBall.AI.Pathfinding;
using FlatRedBall.Graphics.Animation;
using FlatRedBall.Gui;
using FlatRedBall.Math;
using FlatRedBall.Math.Geometry;
using FlatRedBall.Localization;
using Microsoft.Xna.Framework;

using MakeMeLaughJam.Entities;


namespace MakeMeLaughJam.Screens
{
    public partial class Level1
    {
        void CustomInitialize()
        {


        }

        void CustomActivity(bool firstTimeCalled)
        {
            MusicBoxPercentage -= MusicBoxPercentageLossPerSecond * TimeManager.SecondDifference;
        }

        void CustomDestroy()
        {


        }

        static void CustomLoadStaticContent(string contentManagerName)
        {


        }

        protected override void OnPuppetButtonPressed(Puppets puppet)
        {
            SelectedPuppet = puppet;
        }

        protected override void OnConfirmationLeverPressed()
        {
            switch (SelectedPuppet)
            {
                case Puppets.Peepo:
                    PeepoPuppet.InterpolateToState(Puppet.Deployment.Up, PuppetDeployTime);
                    break;
                case Puppets.Smelvin:
                    SmelvinPuppet.InterpolateToState(Puppet.Deployment.Up, PuppetDeployTime);
                    break;
                case Puppets.John:
                    JohnPuppet.InterpolateToState(Puppet.Deployment.Up, PuppetDeployTime);
                    break;
            }
        }

        protected override void OnMusicBoxPressed()
        {
            MusicBoxPercentage += MusicBoxPercentageGainOnHit;
        }

        protected override void OnActionSelectorPressed()
        {
            switch (SelectedAction)
            {
                case Actions.Pie:
                    SelectedAction = Actions.Dance;
                    break;
                case Actions.Dance:
                    SelectedAction = Actions.Fire;
                    break;
                case Actions.Fire:
                    SelectedAction = Actions.Pie;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(SelectedAction));
            }
        }
    }

    public enum Puppets
    {
        Peepo,
        Smelvin,
        John,
    }

    public enum Actions
    {
        Pie,
        Fire,
        Dance,
    }
}
