using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using FlatRedBall;
using FlatRedBall.Input;
using FlatRedBall.Instructions;
using FlatRedBall.AI.Pathfinding;
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

        public float CurrentAudienceHealth
        {
            get => _currentAudienceHealth;
            set
            {
                _currentAudienceHealth                      = value;
                var percent = 100 * _currentAudienceHealth / MaxAudienceHealth;
                GumScreen.AudienceBarInstance.BarPercentageLeft = percent;
                GumScreen.AudienceBarInstance.BarPercentageRight = percent;
                // GumScreen.AudienceBarInstance.MasksInstance.CurrentMasksState = percent switch
                // {
                //     > 40 => MasksRuntime.Masks.Ecstatic,
                //     > 30 => MasksRuntime.Masks.Satisfied,
                //     > 20 => MasksRuntime.Masks.Indifferent,
                //     > 10 => MasksRuntime.Masks.Upset,
                //     _    => MasksRuntime.Masks.Livid,
                // };
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
            
            Forms.PauseInstance.MainMenuButton.Click += (sender, args) => ScreenManager.MoveToScreen("MainMenu");
            Forms.PauseInstance.QuitButton.Click += (sender, args) => FlatRedBallServices.Game.Exit();
            
            Forms.OptionsInstance.BackButton.Click += (sender, args) =>
                GumScreen.CurrentPauseState = GameScreenGumRuntime.Pause.Main;
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

            if (Player1.GameplayInputDevice.Attack.WasJustPressed)
            {
                AudienceStartMoving();
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
            }
            // foreach (var audienceGroup in audiencelis)
            // {
            //     audienceGroup.AudienceSprite.CurrentChainName = "Shaking";
            // }
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
            // foreach (var audienceGroup in audiencelis)
            // {
            //     audienceGroup.AudienceSprite.CurrentChainName = "Shaking";
            // }
        }

        public void RandomAudienceRandomRequest()
        {
            var group = FlatRedBallServices.Random.In(AvailableAudience);
            group.SignSprite.CurrentChainName = FlatRedBallServices.Random.In(SignChains);
        }

        public List<AudienceGroup> AvailableAudience =>
            AudienceGroupList.Where(g =>
            {
                var found = g.SignSprite.CurrentChainName.Contains("Down");
                return found;
            }).ToList();

        public void TryFulfillAudienceRequests(string chainName)
        {
            foreach (var group in AudienceGroupList)
            {
                if (group.SignSprite.CurrentChainName != chainName)
                {
                    continue;
                }

                AudienceRequestFulfilled(group);
            }
        }

        public void AudienceRequestFulfilled(AudienceGroup group)
        {
            CurrentAudienceHealth += AudienceHealthPercentGainOnSuccess;
            group.SignSprite.CurrentChainName = FlatRedBallServices.Random.In(SignDownChains);
        }

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
