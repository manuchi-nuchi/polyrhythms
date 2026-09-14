using Godot;
using System;

public partial class ConstantRotation : Node3D
{
	[Export] float secondsPerLoop = 1f;
	float speed;


	public override void _Ready()
	{
		speed = Mathf.Pi * 2 / secondsPerLoop;
	}


	Vector3 newRotation = Vector3.Zero;
	public override void _Process(double delta)
	{
		newRotation.Z = Time.GetTicksMsec() / 1000f * speed;
		if (newRotation.Z > 360)
			newRotation.Z -= 360;
		Rotation = newRotation;
	}
}
