using Godot;
using System;

public partial class PingpongRotation : Node3D
{
	[Export] float amplitude = 0.06f;
	[Export] float frequency = 5f;
	float error = 0.1f;
    float actualAmplitude;
    float actualFrequency;


    public override void _Ready()
    {
        actualAmplitude = amplitude * (1f + (float)GD.RandRange(-error, error));
        actualFrequency = frequency* (1f + (float)GD.RandRange(-error, error));
    }

	public override void _Process(double delta)
	{
        Rotation = new Vector3(0, 0, (float)(Math.Sin(Time.GetTicksMsec() / 1000.0 * actualFrequency) * actualAmplitude));
    }
}
