using Godot;
using System;

public partial class Joint : Node3D
{
	[Export] double min;
	[Export] double max;


	Vector3 newRotation = Vector3.Zero;
	public void NewPose(bool insta = false)
	{
		newRotation.Z = (float)GD.RandRange(min, max);
		Rotation = newRotation;
	}
}
