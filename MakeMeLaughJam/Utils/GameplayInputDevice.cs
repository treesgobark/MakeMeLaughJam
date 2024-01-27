using FlatRedBall.Input;
using MakeMeLaughJam.Entities;

namespace MakeMeLaughJam.Utils;

public class GameplayInputDevice
{
    public GameplayInputDevice(IInputDevice inputDevice, Player player)
    {
        switch (inputDevice)
        {
            case Xbox360GamePad gamePad:
                Movement = inputDevice.Default2DInput;
                Attack   = gamePad.GetButton(Xbox360GamePad.Button.RightShoulder);
                break;
            case Keyboard keyboard:
                Movement = keyboard.GetWasdInput();
                Attack   = InputManager.Mouse.GetButton(Mouse.MouseButtons.LeftButton);
                break;
        }
    }
    
    public I2DInput Movement { get; }
    public IPressableInput Attack { get; }
}