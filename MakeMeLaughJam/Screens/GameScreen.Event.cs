using ANLG.Utilities.FlatRedBall.Extensions;
using MakeMeLaughJam.Entities;
using Microsoft.Xna.Framework;

namespace MakeMeLaughJam.Screens
{
    public partial class GameScreen
    {
        void OnPlayerVsConfirmLeverCollided (Entities.Player player, Entities.ConfirmLever confirmLever) 
        {
            if (player.IsPunchActive)
            {
                player.PunchConsumed = true;
                
                var toPlayer = confirmLever.Position.GetVectorTo(player.Position);
                if (toPlayer.X >= 0)
                {
                    toPlayer = Vector3.UnitX;
                }
                else
                {
                    toPlayer = -Vector3.UnitX;
                }
                player.Velocity += toPlayer * player.PunchKnockbackSpeed;
                
                OnConfirmationLeverPressed();
            }
        }
        void OnPlayerVsMusicBoxCollided (Entities.Player player, Entities.MusicBox musicBox) 
        {
            if (player.IsPunchActive)
            {
                player.PunchConsumed = true;
                
                player.Velocity += Vector3.UnitX * player.PunchKnockbackSpeed;
                
                OnMusicBoxPressed();
            }
        }
        void OnPlayerVsActionSelectorCollided (Entities.Player player, Entities.ActionSelector actionSelector) 
        {
            if (player.IsPunchActive)
            {
                player.PunchConsumed = true;
                
                player.Velocity += -Vector3.UnitX * player.PunchKnockbackSpeed;
                
                OnActionSelectorPressed();
            }
        }
        void OnPlayerVsButtonCollided (Entities.Player player, Entities.Button wallButton) 
        {
            if (player.IsPunchActive)
            {
                player.PunchConsumed = true;
                
                var vector = wallButton.Position.GetVectorTo(player.Position);
                if (wallButton.CurrentFacingState == Button.Facing.Right)
                {
                    vector = Vector3.UnitX;
                }
                else if (wallButton.CurrentFacingState == Button.Facing.Left)
                {
                    vector = -Vector3.UnitX;
                }
                else if (wallButton.CurrentFacingState == Button.Facing.Down)
                {
                    vector = -Vector3.UnitY;
                }

                player.Velocity += vector * player.PunchKnockbackSpeed;

                if (wallButton.Name == "JohnButton")
                {
                    OnPuppetButtonPressed(Puppets.John);
                }
                
                if (wallButton.Name == "SmelvinButton")
                {
                    OnPuppetButtonPressed(Puppets.Smelvin);
                }
                
                if (wallButton.Name == "PeepoButton")
                {
                    OnPuppetButtonPressed(Puppets.Peepo);
                }
            }
        }
        
    }
}
