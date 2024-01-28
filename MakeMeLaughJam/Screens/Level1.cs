using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using FlatRedBall;
using FlatRedBall.Input;
using FlatRedBall.Instructions;
using FlatRedBall.AI.Pathfinding;
using FlatRedBall.Audio;
using FlatRedBall.Graphics.Animation;
using FlatRedBall.Gui;
using FlatRedBall.Math;
using FlatRedBall.Math.Geometry;
using FlatRedBall.Localization;
using Microsoft.Xna.Framework;

using MakeMeLaughJam.Entities;
using Microsoft.Xna.Framework.Media;


namespace MakeMeLaughJam.Screens
{
    public partial class Level1
    {
        private float _musicBoxPercentage = 100;

        public double LastRequestTime { get; set; } = -69420;
        public static double TimeElapsed { get; set; }
        public bool ShouldRequest => TimeManager.CurrentScreenSecondsSince(LastRequestTime) > TimeBetweenRequests;
        public double CurrentTimeBetweenRequests => TimeBetweenRequests;

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
            TimeElapsed = 0;
            MediaPlayer.Play(PuppetStruggleWarpEnd);
            MediaPlayer.IsRepeating = true;
        }

        void CustomActivity(bool firstTimeCalled)
        {
            if (!IsPaused)
            {
                MusicBoxPercentage            -= MusicBoxPercentageLossPerSecond * TimeManager.SecondDifference;
                AudioManager.MasterSongVolume =  MathHelper.Lerp(MinVolume * MainMenu.SongVolume, MainMenu.SongVolume, MusicBoxPercentage / 100);
                CurrentAudienceHealth         -= AudiencePercentLossPerSecond * TimeManager.SecondDifference;
                TimeElapsed                   += TimeManager.SecondDifference;
            }

            if (ShouldRequest)
            {
                RandomAudienceRandomRequest();
                LastRequestTime = TimeManager.CurrentScreenTime;
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
            MovePuppetDown(SelectedPuppetEntity, PuppetDeployedDuration);

            switch (SelectedAction)
            {
                case Actions.Fire: AudioManager.Play(fire);
                    break;
                case Actions.Pie: AudioManager.Play(piesplat);
                    break;
                case Actions.Dance: AudioManager.Play(littledance);
                    break;
            }

            switch (SelectedPuppet, SelectedAction)
            {
                case (Puppets.Smelvin, Actions.Fire):
                    SelectedPuppetEntity.CurrentPuppetNameState = Puppet.PuppetName.SmelvinFire;
                    TryFulfillAudienceRequests("SmelvinFire");
                    break;
                case (Puppets.Smelvin, Actions.Pie):
                    SelectedPuppetEntity.CurrentPuppetNameState = Puppet.PuppetName.SmelvinPie;
                    TryFulfillAudienceRequests("SmelvinPie");
                    break;
                case (Puppets.Smelvin, Actions.Dance):
                    SelectedPuppetEntity.CurrentPuppetNameState = Puppet.PuppetName.SmelvinDancing;
                    TryFulfillAudienceRequests("SmelvinDance");
                    break;
                case (Puppets.John, Actions.Fire):
                    SelectedPuppetEntity.CurrentPuppetNameState = Puppet.PuppetName.JohnFire;
                    TryFulfillAudienceRequests("JohnFire");
                    break;
                case (Puppets.John, Actions.Pie):
                    SelectedPuppetEntity.CurrentPuppetNameState = Puppet.PuppetName.JohnPie;
                    TryFulfillAudienceRequests("JohnPie");
                    break;
                case (Puppets.John, Actions.Dance):
                    SelectedPuppetEntity.CurrentPuppetNameState = Puppet.PuppetName.JohnDancing;
                    TryFulfillAudienceRequests("JohnDance");
                    break;
                case (Puppets.Peepo, Actions.Fire):
                    SelectedPuppetEntity.CurrentPuppetNameState = Puppet.PuppetName.PeepoFire;
                    TryFulfillAudienceRequests("PeepoFire");
                    break;
                case (Puppets.Peepo, Actions.Pie):
                    SelectedPuppetEntity.CurrentPuppetNameState = Puppet.PuppetName.PeepoPie;
                    TryFulfillAudienceRequests("PeepoPie");
                    break;
                case (Puppets.Peepo, Actions.Dance):
                    SelectedPuppetEntity.CurrentPuppetNameState = Puppet.PuppetName.PeepoDancing;
                    TryFulfillAudienceRequests("PeepoDance");
                    break;
                default:
                    throw new ArgumentOutOfRangeException("(SelectedPuppet, SelectedAction)");
            }

            SmelvinButton.CurrentIsPressedState = Button.IsPressed.UnpressedRed;
            JohnButton.CurrentIsPressedState    = Button.IsPressed.UnpressedYellow;
            PeepoButton.CurrentIsPressedState   = Button.IsPressed.UnpressedBlue;
            SelectedPuppet                      = Puppets.None;
        }

        protected async Task MovePuppetDown(Puppet puppet, double afterSeconds)
        {
            await TimeManager.DelaySeconds(afterSeconds);
            puppet.InterpolateToState(Puppet.Deployment.Down, PuppetDeployTime);
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
