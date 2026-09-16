using Godot;
using System;

public partial class ConstantRotation : Node3D
{
	[Export] int bpm = 60;
	float secondsPerLoop;
	float speed;


	public override void _Ready()
	{
	}


	Vector3 newRotation = Vector3.Zero;
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

		newRotation.Z = Time.GetTicksMsec() / 1000f * speed;
		if (newRotation.Z > 360)
			newRotation.Z -= 360;
		Rotation = newRotation;
	}
}
