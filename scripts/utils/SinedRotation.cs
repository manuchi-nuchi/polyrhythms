using Godot;
using System;

public partial class SinedRotation : Node3D
{
	[Export] int bpm = 60;
	float secondsPerLoop, speed, angle;
	[Export] float offset = 10;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}


    Vector3 newPosition;
	public override void _Process(double delta)
	{
        if (bpm == 0)
        {
            speed = 0;
        }
        else
        {
            secondsPerLoop = 60f / bpm;
            speed = Mathf.Pi * 2 / secondsPerLoop;
        }

        angle = Time.GetTicksMsec() / 1000f * -speed;
        newPosition.X = Mathf.Sin(angle);
        newPosition.Y = Mathf.Cos(angle);
        newPosition *= offset;

        Position = newPosition;
    }
}
