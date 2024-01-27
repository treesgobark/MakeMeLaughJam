using ANLG.Utilities.FlatRedBall.Extensions;
using MakeMeLaughJam.Entities;
using Microsoft.Xna.Framework;

namespace MakeMeLaughJam.Screens
{
    public partial class GameScreen
    {
        void OnButtonVsPlayerCollided (Button button, Player player) 
        {
            if (player.IsPunchActive)
            {
                player.Velocity += button.Position.GetVectorTo(player.Position).NormalizedOrZero() * player.PunchKnockbackSpeed;
            }
        }
        
    }
}
