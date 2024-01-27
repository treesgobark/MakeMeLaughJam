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
using FlatRedBall.Screens;
using MakeMeLaughJam.GumRuntimes;
using Microsoft.Xna.Framework;




namespace MakeMeLaughJam.Screens
{
    public partial class GameScreen
    {
        private float _currentAudienceHealth;

        public float CurrentAudienceHealth
        {
            get => _currentAudienceHealth;
            set
            {
                _currentAudienceHealth                      = value;
                var percent = 50 * _currentAudienceHealth / MaxAudienceHealth;
                GumScreen.AudienceBarInstance.BarPercentageLeft = percent;
                GumScreen.AudienceBarInstance.BarPercentageRight = percent;
                GumScreen.AudienceBarInstance.MasksInstance.CurrentMasksState = percent switch
                {
                    > 40 => MasksRuntime.Masks.Ecstatic,
                    > 30 => MasksRuntime.Masks.Satisfied,
                    > 20 => MasksRuntime.Masks.Indifferent,
                    > 10 => MasksRuntime.Masks.Upset,
                    _    => MasksRuntime.Masks.Livid,
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
            CurrentAudienceHealth -= 10 * TimeManager.SecondDifference;
        }

        private void HandleInput()
        {
            if (Player1.GameplayInputDevice.Pause.WasJustPressed && !ScreenManager.CurrentScreen.IsPaused)
            {
                ScreenManager.CurrentScreen.PauseThisScreen();
                GumScreen.CurrentPauseState = GameScreenGumRuntime.Pause.Main;
            }
        }

        void CustomDestroy()
        {


        }

        static void CustomLoadStaticContent(string contentManagerName)
        {


        }

    }
}
