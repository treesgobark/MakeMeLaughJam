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
using FlatRedBall.Screens;
using MakeMeLaughJam.Entities;
using MakeMeLaughJam.GumRuntimes;
using Microsoft.Xna.Framework;




namespace MakeMeLaughJam.Screens
{
    public partial class GameScreen
    {
        private float _currentAudienceHealth;
        
        public Puppets SelectedPuppet { get; set; }
        public Actions SelectedAction { get; set; }

        public List<AudienceGroup> AvailableAudience =>
            AudienceGroupList.Where(g =>
            {
                var found = g.SignSprite.CurrentChainName.Contains("Down");
                return found;
            }).ToList();

        public readonly string[] SignChains =
        [
            "SmelvinFire",
            "SmelvinPie",
            "SmelvinDance",
            "JohnFire",
            "JohnPie",
            "JohnDance",
            "PeepoFire",
            "PeepoPie",
            "PeepoDance"
        ];

        public readonly string[] SignDownChains = new[]
        {
            "SmelvinDown",
            "JohnDown",
            "PeepoDown",
        };

        public float CurrentAudienceHealth
        {
            get => _currentAudienceHealth;
            set
            {
                _currentAudienceHealth                      = value;
                if (_currentAudienceHealth > 100)
                {
                    _currentAudienceHealth = 100;
                }
                float percent = 100 * _currentAudienceHealth / MaxAudienceHealth;
                GumScreen.AudienceBarInstance.BarPercentageLeft = percent;
                GumScreen.AudienceBarInstance.BarPercentageRight = percent;
                GumScreen.FancyAudienceBarInstance.CurrentFilledState = percent switch
                {
                    > 38f * 100f / 38f => FancyAudienceBarRuntime.Filled._38,
                    > 37f * 100f / 38f => FancyAudienceBarRuntime.Filled._37,
                    > 36f * 100f / 38f => FancyAudienceBarRuntime.Filled._36,
                    > 35f * 100f / 38f => FancyAudienceBarRuntime.Filled._35,
                    > 34f * 100f / 38f => FancyAudienceBarRuntime.Filled._34,
                    > 33f * 100f / 38f => FancyAudienceBarRuntime.Filled._33,
                    > 32f * 100f / 38f => FancyAudienceBarRuntime.Filled._32,
                    > 31f * 100f / 38f => FancyAudienceBarRuntime.Filled._31,
                    > 30f * 100f / 38f => FancyAudienceBarRuntime.Filled._30,
                    > 29f * 100f / 38f => FancyAudienceBarRuntime.Filled._29,
                    > 28f * 100f / 38f => FancyAudienceBarRuntime.Filled._28,
                    > 27f * 100f / 38f => FancyAudienceBarRuntime.Filled._27,
                    > 26f * 100f / 38f => FancyAudienceBarRuntime.Filled._26,
                    > 25f * 100f / 38f => FancyAudienceBarRuntime.Filled._25,
                    > 24f * 100f / 38f => FancyAudienceBarRuntime.Filled._24,
                    > 23f * 100f / 38f => FancyAudienceBarRuntime.Filled._23,
                    > 22f * 100f / 38f => FancyAudienceBarRuntime.Filled._22,
                    > 21f * 100f / 38f => FancyAudienceBarRuntime.Filled._21,
                    > 20f * 100f / 38f => FancyAudienceBarRuntime.Filled._20,
                    > 19f * 100f / 38f => FancyAudienceBarRuntime.Filled._19,
                    > 18f * 100f / 38f => FancyAudienceBarRuntime.Filled._18,
                    > 17f * 100f / 38f => FancyAudienceBarRuntime.Filled._17,
                    > 16f * 100f / 38f => FancyAudienceBarRuntime.Filled._16,
                    > 15f * 100f / 38f => FancyAudienceBarRuntime.Filled._15,
                    > 14f * 100f / 38f => FancyAudienceBarRuntime.Filled._14,
                    > 13f * 100f / 38f => FancyAudienceBarRuntime.Filled._13,
                    > 12f * 100f / 38f => FancyAudienceBarRuntime.Filled._12,
                    > 11f * 100f / 38f => FancyAudienceBarRuntime.Filled._11,
                    > 10f * 100f / 38f => FancyAudienceBarRuntime.Filled._10,
                    > 9f  * 100f / 38f => FancyAudienceBarRuntime.Filled._9,
                    > 8f  * 100f / 38f => FancyAudienceBarRuntime.Filled._8,
                    > 7f  * 100f / 38f => FancyAudienceBarRuntime.Filled._7,
                    > 6f  * 100f / 38f => FancyAudienceBarRuntime.Filled._6,
                    > 5f  * 100f / 38f => FancyAudienceBarRuntime.Filled._5,
                    > 4f  * 100f / 38f => FancyAudienceBarRuntime.Filled._4,
                    > 3f  * 100f / 38f => FancyAudienceBarRuntime.Filled._3,
                    > 2f  * 100f / 38f => FancyAudienceBarRuntime.Filled._2,
                    > 1f  * 100f / 38f => FancyAudienceBarRuntime.Filled._1,
                    _                  => FancyAudienceBarRuntime.Filled._0,
                };

                if (percent <= 0)
                {
                    ScreenManager.MoveToScreen("GameOverScreen");
                }
            }
        }

        void CustomInitialize()
        {
            CurrentAudienceHealth = MaxAudienceHealth;
            
            GumScreen.CurrentPauseState = GameScreenGumRuntime.Pause.NotPaused;
            
            Forms.PauseInstance.OptionsButton.Click += (sender, args) =>
                GumScreen.CurrentPauseState = GameScreenGumRuntime.Pause.Options;
            
            Forms.PauseInstance.ResumeButton.Click += (sender, args) =>
            {
                ScreenManager.CurrentScreen.UnpauseThisScreen();
                GumScreen.CurrentPauseState = GameScreenGumRuntime.Pause.NotPaused;
            };
            
            Forms.PauseInstance.MainMenuButton.Click += (sender, args) =>
            {
                AudioManager.MasterSongVolume = MainMenu.SongVolume;
                ScreenManager.MoveToScreen("MainMenu");
            };
            Forms.PauseInstance.QuitButton.Click     += (sender, args) => FlatRedBallServices.Game.Exit();
            
            Forms.OptionsInstance.BackButton.Click += (sender, args) =>
                GumScreen.CurrentPauseState = GameScreenGumRuntime.Pause.Main;
            
            Forms.OptionsInstance.FullscreenCheckBox.IsChecked = CameraSetup.Data.IsFullScreen;
            Forms.OptionsInstance.FullscreenCheckBox.Checked += (sender, args) =>
            {
                CameraSetup.Data.IsFullScreen = true;
                if (CameraSetup.GraphicsDeviceManager != null)
                {
                    CameraSetup.ResetWindow();
                    CameraSetup.ResetCamera();
                }
            };
            
            Forms.OptionsInstance.FullscreenCheckBox.Unchecked += (sender, args) =>
            {
                CameraSetup.Data.IsFullScreen = false;
                if (CameraSetup.GraphicsDeviceManager != null)
                {
                    CameraSetup.ResetWindow();
                    CameraSetup.ResetCamera();
                }
            };

            Forms.OptionsInstance.MusicSlider.Minimum = 0;
            Forms.OptionsInstance.MusicSlider.Maximum = 100;
            Forms.OptionsInstance.MusicSlider.Value   = 50;
            Forms.OptionsInstance.MusicSlider.ValueChanged += (sender, args) =>
            {
                AudioManager.MasterSongVolume = (float)Forms.OptionsInstance.MusicSlider.Value / 100;
                MainMenu.SongVolume           = AudioManager.MasterSongVolume ?? 0.5f;
            };
            
            Forms.OptionsInstance.SoundEffectSlider.Minimum = 0;
            Forms.OptionsInstance.SoundEffectSlider.Maximum = 100;
            Forms.OptionsInstance.SoundEffectSlider.Value   = 50;
            Forms.OptionsInstance.SoundEffectSlider.ValueChanged += (sender, args) =>
                AudioManager.MasterSoundVolume = (float)Forms.OptionsInstance.SoundEffectSlider.Value / 100;
        }

        void CustomActivity(bool firstTimeCalled)
        {
            HandleInput();
        }

        private void HandleInput()
        {
            if (Player1.GameplayInputDevice.Pause.WasJustPressed)
            {
                if (!ScreenManager.CurrentScreen.IsPaused)
                {
                    ScreenManager.CurrentScreen.PauseThisScreen();
                    GumScreen.CurrentPauseState = GameScreenGumRuntime.Pause.Main;
                }
                else
                {
                    ScreenManager.CurrentScreen.UnpauseThisScreen();
                    GumScreen.CurrentPauseState = GameScreenGumRuntime.Pause.NotPaused;
                }
            }
        }

        void CustomDestroy()
        {


        }

        static void CustomLoadStaticContent(string contentManagerName)
        {


        }

        public void AudienceStartMoving()
        {
            foreach (var audienceGroup in AudienceGroupList)
            {
                audienceGroup.AudienceSprite.CurrentChainName = "Shaking";
                audienceGroup.AudienceSprite.AnimationSpeed = FlatRedBallServices.Random.Between(0.5f, 1.5f);
            }
            foreach (var audienceGroup in AudienceThatLooksCoolList)
            {
                audienceGroup.AudienceSprite.CurrentChainName = "Shaking";
                audienceGroup.AudienceSprite.AnimationSpeed   = FlatRedBallServices.Random.Between(0.5f, 1.5f);
            }
            AudienceStopMovingIn(AudienceMovingDuration);
        }

        public async Task AudienceStopMovingIn(double delay)
        {
            await TimeManager.DelaySeconds(delay);
            AudienceStopMoving();
        }

        public void AudienceStopMoving()
        {
            foreach (var audienceGroup in AudienceGroupList)
            {
                audienceGroup.AudienceSprite.CurrentChainName = "Idle";
            }
            foreach (var audienceGroup in AudienceThatLooksCoolList)
            {
                audienceGroup.AudienceSprite.CurrentChainName = "Idle";
            }
        }

        public void RandomAudienceRandomRequest()
        {
            var audience = AvailableAudience;
            if (audience.Count == 0)
            {
                return;
            }
            var group    = FlatRedBallServices.Random.In(AvailableAudience);
            group.SignSprite.CurrentChainName = FlatRedBallServices.Random.In(SignChains);
        }

        public void TryFulfillAudienceRequests(string chainName)
        {
            bool wasSomethingFulfilled = false;
            foreach (var group in AudienceGroupList)
            {
                if (group.SignSprite.CurrentChainName != chainName)
                {
                    continue;
                }

                wasSomethingFulfilled = true;
                AudienceRequestFulfilled(group);
            }

            if (wasSomethingFulfilled)
            {
                AudienceStartMoving();
                CurrentAudienceHealth += AudienceHealthPercentGainOnSuccess;
                AudioManager.Play(CrowdLaughter);
            }
        }

        public void AudienceRequestFulfilled(AudienceGroup group)
        {
            group.SignSprite.CurrentChainName = FlatRedBallServices.Random.In(SignDownChains);
        }

        public bool IsAudienceAvailable(AudienceGroup group)
        {
            return !group.SignSprite.CurrentChainName.Contains("Down");
        }

        protected abstract void OnPuppetButtonPressed(Puppets puppet);
        protected abstract void OnConfirmationLeverPressed();
        protected abstract void OnMusicBoxPressed();
        protected abstract void OnActionSelectorPressed();
    }
}
