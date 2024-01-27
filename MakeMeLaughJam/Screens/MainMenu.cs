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
using Microsoft.Xna.Framework;

using MakeMeLaughJam.Entities;
using MakeMeLaughJam.GumRuntimes;


namespace MakeMeLaughJam.Screens
{
    public partial class MainMenu
    {

        void CustomInitialize()
        {
            GumScreen.CurrentMainMenuState = MainMenuGumRuntime.MainMenu.Main;
            
            Forms.MainMenuInstance.PlayButton.Click += (sender, args) => ScreenManager.MoveToScreen("Level1");
            Forms.MainMenuInstance.OptionsButton.Click += (sender, args) => GumScreen.CurrentMainMenuState = MainMenuGumRuntime.MainMenu.Options;
            Forms.MainMenuInstance.QuitButton.Click += (sender, args) => FlatRedBallServices.Game.Exit();
            
            Forms.OptionsInstance.BackButton.Click += (sender, args) => GumScreen.CurrentMainMenuState = MainMenuGumRuntime.MainMenu.Main;
        }

        void CustomActivity(bool firstTimeCalled)
        {
        }

        void CustomDestroy()
        {


        }

        static void CustomLoadStaticContent(string contentManagerName)
        {


        }

    }
}
