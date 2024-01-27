using FlatRedBall.Input;
using MakeMeLaughJam.Entities;
using Microsoft.Xna.Framework.Input;
using Keyboard = FlatRedBall.Input.Keyboard;
using Mouse = FlatRedBall.Input.Mouse;

namespace MakeMeLaughJam.Utils;

public class GameplayInputDevice
{
    public GameplayInputDevice(IInputDevice inputDevice, Player player)
    {
        Movement = inputDevice.Default2DInput;
        Pause    = inputDevice.DefaultPauseInput;
        
        switch (inputDevice)
        {
            case Xbox360GamePad gamePad:
                Attack   = gamePad.GetButton(Xbox360GamePad.Button.X);
                break;
            case Keyboard keyboard:
                Attack   = InputManager.Mouse.GetButton(Mouse.MouseButtons.LeftButton);
                break;
        }
    }
    
    public I2DInput Movement { get; }
    public IPressableInput Attack { get; }
    public IPressableInput Pause { get; }
}