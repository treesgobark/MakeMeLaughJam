using ANLG.Utilities.FlatRedBall.Extensions;
using FlatRedBall.Audio;
using MakeMeLaughJam.Entities;
using Microsoft.Xna.Framework;

namespace MakeMeLaughJam.Screens
{
    public partial class GameScreen
    {
        private void ApplyKnockback(Player player, float multiplier = 1.0f)
        {
            player.PunchConsumed = true;
            AudioManager.Play(punchhit);
            
            if (player.DirectionFacing == HorizontalDirection.Left)
            {
                player.Velocity.X = player.PunchKnockbackSpeed * multiplier;
            }
            else
            {
                player.Velocity.X = -player.PunchKnockbackSpeed * multiplier;
            }
        }
        
        void OnPlayerVsConfirmLeverCollided (Entities.Player player, Entities.ConfirmLever confirmLever) 
        {
            if (player.IsPunchActive)
            {
                ApplyKnockback(player);
                OnConfirmationLeverPressed();
            }
        }
        
        void OnPlayerVsMusicBoxCollided (Entities.Player player, Entities.MusicBox musicBox) 
        {
            if (player.IsPunchActive)
            {
                ApplyKnockback(player);
                OnMusicBoxPressed();
            }
        }
        void OnPlayerVsActionSelectorCollided (Entities.Player player, Entities.ActionSelector actionSelector) 
        {
            if (player.IsPunchActive)
            {
                ApplyKnockback(player);
                OnActionSelectorPressed();
            }
        }
        void OnPlayerVsButtonCollided (Entities.Player player, Entities.Button wallButton) 
        {
            if (player.IsPunchActive)
            {
                if (wallButton.Name == "JohnButton")
                {
                    ApplyKnockback(player, 1.5f);
                    OnPuppetButtonPressed(Puppets.John);
                }
                
                if (wallButton.Name == "SmelvinButton")
                {
                    ApplyKnockback(player, 1.5f);
                    OnPuppetButtonPressed(Puppets.Smelvin);
                }
                
                if (wallButton.Name == "PeepoButton")
                {
                    player.PunchConsumed =  true;
                    player.Velocity      = -Vector3.UnitY * player.PunchKnockbackSpeed * 1.5f;
                    AudioManager.Play(punchhit);
                    OnPuppetButtonPressed(Puppets.Peepo);
                }
            }
        }
        
    }
}
