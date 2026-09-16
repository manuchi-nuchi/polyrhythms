using Godot;
using System;

public partial class ConstantIdentityRotation : Node3D
{
	public override void _Process(double delta)
	{
		GlobalRotation = Vector3.Zero;
	}
}
