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
        private float _musicBoxPercentage = 100;

        public float MusicBoxPercentage
        {
            get => _musicBoxPercentage;
            set
            {
                _musicBoxPercentage                       = value;
                if (_musicBoxPercentage < 0)
                {
                    _musicBoxPercentage = 0;
                }
                MusicBox1.SpriteInstance.CurrentChainName = _musicBoxPercentage switch
                {
                    >80 => "Green",
                    >60 => "GreenYellow",
                    >40 => "Yellow",
                    >20 => "Orange",
                    _ => "Red",
                };
            }
        }

        private Puppet SelectedPuppetEntity => SelectedPuppet switch
        {
            Puppets.Smelvin => SmelvinPuppet,
            Puppets.John    => JohnPuppet,
            Puppets.Peepo   => PeepoPuppet,
            _ => null,
        };
        
        void CustomInitialize()
        {
        }

        void CustomActivity(bool firstTimeCalled)
        {
            if (!IsPaused)
            {
                MusicBoxPercentage    -= MusicBoxPercentageLossPerSecond * TimeManager.SecondDifference;
                CurrentAudienceHealth -= AudiencePercentLossPerSecond    * TimeManager.SecondDifference;
            }
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

            switch (puppet)
            {
                case Puppets.None: 
                    SmelvinButton.CurrentIsPressedState = Button.IsPressed.UnpressedRed;
                    JohnButton.CurrentIsPressedState    = Button.IsPressed.UnpressedYellow;
                    PeepoButton.CurrentIsPressedState   = Button.IsPressed.UnpressedBlue;
                    break;
                case Puppets.Smelvin: 
                    SmelvinButton.CurrentIsPressedState = Button.IsPressed.PressedRed;
                    JohnButton.CurrentIsPressedState    = Button.IsPressed.UnpressedYellow;
                    PeepoButton.CurrentIsPressedState   = Button.IsPressed.UnpressedBlue;
                    break;
                case Puppets.John: 
                    SmelvinButton.CurrentIsPressedState = Button.IsPressed.UnpressedRed;
                    JohnButton.CurrentIsPressedState    = Button.IsPressed.PressedYellow;
                    PeepoButton.CurrentIsPressedState   = Button.IsPressed.UnpressedBlue;
                    break;
                case Puppets.Peepo: 
                    SmelvinButton.CurrentIsPressedState = Button.IsPressed.UnpressedRed;
                    JohnButton.CurrentIsPressedState    = Button.IsPressed.UnpressedYellow;
                    PeepoButton.CurrentIsPressedState   = Button.IsPressed.PressedBlue;
                    break;
            }
        }

        protected override void OnConfirmationLeverPressed()
        {
            ConfirmLever1.CrankLever();

            if (SelectedPuppetEntity is null)
            {
                return;
            }
            
            SelectedPuppetEntity.InterpolateToState(Puppet.Deployment.Up, PuppetDeployTime);

            SelectedPuppetEntity.CurrentPuppetNameState = (SelectedPuppet, SelectedAction) switch
            {
                (Puppets.Smelvin, Actions.Fire)  => Puppet.PuppetName.SmelvinFire,
                (Puppets.Smelvin, Actions.Pie)   => Puppet.PuppetName.SmelvinPie,
                (Puppets.Smelvin, Actions.Dance) => Puppet.PuppetName.SmelvinDancing,
                (Puppets.John, Actions.Fire)     => Puppet.PuppetName.JohnFire,
                (Puppets.John, Actions.Pie)      => Puppet.PuppetName.JohnPie,
                (Puppets.John, Actions.Dance)    => Puppet.PuppetName.JohnDancing,
                (Puppets.Peepo, Actions.Fire)    => Puppet.PuppetName.PeepoFire,
                (Puppets.Peepo, Actions.Pie)     => Puppet.PuppetName.PeepoPie,
                (Puppets.Peepo, Actions.Dance)   => Puppet.PuppetName.PeepoDancing,
            };
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
                    SelectedAction                                  = Actions.Dance;
                    ActionSelector1.SpriteInstance.CurrentChainName = "Dance";
                    break;
                case Actions.Dance:
                    SelectedAction                                  = Actions.Fire;
                    ActionSelector1.SpriteInstance.CurrentChainName = "Fire";
                    break;
                case Actions.Fire:
                    SelectedAction                                  = Actions.Pie;
                    ActionSelector1.SpriteInstance.CurrentChainName = "Pie";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(SelectedAction));
            }
        }
    }

    public enum Puppets
    {
        None,
        Smelvin,
        John,
        Peepo,
    }

    public enum Actions
    {
        Fire,
        Pie,
        Dance,
    }
}
