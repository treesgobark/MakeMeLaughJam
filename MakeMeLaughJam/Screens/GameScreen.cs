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

        void CustomInitialize()
        {
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
