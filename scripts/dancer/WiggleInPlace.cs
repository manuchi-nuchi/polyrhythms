using Godot;
using System;

public partial class WiggleInPlace : Node3D
{
    [Export] float xFrequency = 0;
    [Export] float yFrequency = 0;
    [Export] float zFrequency = 0;
    [Export] float xAmplitude = 0;

    [Export] float yAmplitude = 0;
    [Export] float zAmplitude = 0;


    Vector3 newPosition = Vector3.Zero;
	public override void _Process(double delta)
    {
        newPosition.X = Mathf.Sin(Time.GetTicksMsec() * xFrequency) * xAmplitude;
        newPosition.Y = Mathf.Sin(Time.GetTicksMsec() * yFrequency) * yAmplitude;
        newPosition.Z = Mathf.Sin(Time.GetTicksMsec() * zFrequency) * zAmplitude;

        Position = newPosition;
	}
}
