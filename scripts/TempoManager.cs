using Godot;
using System;

public partial class TempoManager : Node3D
{
	[Export] int bpm = 50;
    float t = 0;
    float maxT = 10000;
    public static float Angle = 0;
    public static float SecondsPerLoop;
    public static float Speed;
    public static int BPM;
    float offset;


    public override void _Ready()
	{
        ComputeSpeed();
	}


    void ComputeSpeed()
    {
        if (bpm == 0)
        {
            Speed = 0;
        }
        else
        {
            SecondsPerLoop = 60f / bpm;
            Speed = Mathf.Pi * 2 / SecondsPerLoop;
        }

        offset = Angle - GetCurrentAngle();

        BPM = bpm;

        GD.Print("new tempo: " + bpm);

        Actions.NewTempo?.Invoke();
    }


	public override void _Process(double delta)
	{
        t += (float)delta;

        // prevent sudden angle jumps
        // i *think* it happened by overflowing
        if (t > maxT)
        {
            t -= maxT;
            offset = Angle - GetCurrentAngle();
        }

        Angle = GetCurrentAngle() + offset;
    }

    float GetCurrentAngle()
    {
        return t * Speed;
    }


    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventKey keyEvent && keyEvent.Pressed && !keyEvent.Echo)
        {
            switch (keyEvent.Keycode)
            {
                case Key.Up:
                    bpm += 5;
                    ComputeSpeed();
                    break; 
                case Key.Down:
                    bpm -= 5;
                    if (bpm < 1)
                        bpm = 1;
                    ComputeSpeed();
                    break; 
                case Key.Right:
                    bpm++;
                    ComputeSpeed();
                    break;
                case Key.Left:
                    if (bpm > 0)
                    {
                        bpm--;
                        ComputeSpeed();
                    }
                    break;
            }
        }
    }
}
