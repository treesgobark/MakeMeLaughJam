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
using Microsoft.Xna.Framework.Input;


namespace MakeMeLaughJam.Screens
{
    public partial class MainMenu
    {

        void CustomInitialize()
        {
            GumScreen.CurrentMainMenuState = MainMenuGumRuntime.MainMenu.Main;
            
            Forms.MainMenuInstance.ButtonStartInstance.Click += (sender, args) => ScreenManager.MoveToScreen("Level1");
            Forms.MainMenuInstance.ButtonOptionsInstance.Click += (sender, args) => GumScreen.CurrentMainMenuState = MainMenuGumRuntime.MainMenu.Options;
            Forms.MainMenuInstance.ButtonCreditsInstance.Click += (sender, args) => GumScreen.CurrentMainMenuState = MainMenuGumRuntime.MainMenu.Credits    ;
            Forms.MainMenuInstance.ButtonQuitInstance.Click += (sender, args) => FlatRedBallServices.Game.Exit();
            
            Forms.OptionsInstance.BackButton.Click += (sender, args) => GumScreen.CurrentMainMenuState = MainMenuGumRuntime.MainMenu.Main;
            
            Forms.CreditsInstance.BackButton.Click += (sender, args) => GumScreen.CurrentMainMenuState = MainMenuGumRuntime.MainMenu.Main;
        }

        void CustomActivity(bool firstTimeCalled)
        {
            if (InputManager.Xbox360GamePads[0].GetButton(Xbox360GamePad.Button.Start).WasJustPressedOrRepeated
                || InputManager.Keyboard.GetKey(Keys.Escape).WasJustPressed)
            {
                if (GumScreen.CurrentMainMenuState is MainMenuGumRuntime.MainMenu.Credits or MainMenuGumRuntime.MainMenu.Options)
                {
                    GumScreen.CurrentMainMenuState = MainMenuGumRuntime.MainMenu.Main;
                }
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
