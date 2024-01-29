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
using FlatRedBall.Glue.StateInterpolation;
using FlatRedBall.Graphics.Animation;
using FlatRedBall.Gui;
using FlatRedBall.Math;
using FlatRedBall.Math.Geometry;
using FlatRedBall.Localization;
using FlatRedBall.Screens;
using Microsoft.Xna.Framework;

using MakeMeLaughJam.Entities;
using MakeMeLaughJam.GumRuntimes;
using StateInterpolationPlugin;


namespace MakeMeLaughJam.Screens
{
    public partial class GameOverScreen
    {
        public bool EndedEarly = false;
        void CustomInitialize()
        {
            Camera.Main.X = 0;
            Camera.Main.Y = 0;
            GumScreen.CurrentScoreState                  =  GameOverScreenGumRuntime.Score.Hide;
            Forms.ScoreDisplayInstance.MenuButton.Click  += (sender, args) => ScreenManager.MoveToScreen("MainMenu");
            Forms.ScoreDisplayInstance.StartButton.Click += (sender, args) => ScreenManager.MoveToScreen("Level1");
            Forms.ScoreDisplayInstance.ButtonQuitInstance.Click += (sender, args) => FlatRedBallServices.Game.Exit();
            FadeIn();
        }

        void CustomActivity(bool firstTimeCalled)
        {
            if (!EndedEarly && (InputManager.Mouse.ButtonPushed(Mouse.MouseButtons.LeftButton)
                                || InputManager.Xbox360GamePads[0].ButtonPushed(Xbox360GamePad.Button.A)))
            {
                EndedEarly                                                      = true;
                EndScreenAnimationThingInstance.SpriteInstance.CurrentChainName = "Slash";
            }
            
            if (EndScreenAnimationThingInstance.SpriteInstance.CurrentChainName == "Slash" && EndScreenAnimationThingInstance.SpriteInstance.JustCycled)
            {
                EndScreenAnimationThingInstance.SpriteInstance.CurrentChainName                  = "Final";
                GumScreen.ScoreDisplayInstance.TextInstance.Text = $"You performed for {Level1.TimeElapsed:0.00} seconds";
                GumScreen.CurrentScoreState                      = GameOverScreenGumRuntime.Score.Show;
                AudioManager.Play(slash);
            }
        }

        void CustomDestroy()
        {


        }

        static void CustomLoadStaticContent(string contentManagerName)
        {


        }

        public async Task FadeIn()
        {
            await EndScreenAnimationThingInstance.FadeCover.TweenAsync(x => EndScreenAnimationThingInstance.FadeCover.Alpha = x, 1, 0, FadeInTime, InterpolationType.Quadratic, Easing.InOut);
            await TimeManager.DelaySeconds(TimeBeforeAnimation);
            if (EndedEarly)
            {
                return;
            }
            EndScreenAnimationThingInstance.SpriteInstance.CurrentChainName = "Slash";
        }

    }
}
