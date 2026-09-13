using Godot;
using System;

public partial class PingpongRotation : Node3D
{
	[Export] float amplitude = 1;
	[Export] float frequency = 1;


	public override void _Process(double delta)
	{
        Rotation = new Vector3(0, 0, (float)(Math.Sin(Time.GetTicksMsec() / 1000.0 * frequency) * amplitude));
    }
}
