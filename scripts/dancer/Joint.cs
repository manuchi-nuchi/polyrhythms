using Godot;
using System;

public partial class Joint : Node3D
{
	[Export] double minAngle;
	[Export] double maxAngle;
	float duration = 0.2f;
	float maxDelay = 0.2f;
	Curve ease = new Curve();
	int unnecessaryTurns = 1;


	public override void _Ready()
	{
        ease.AddPoint(Vector2.Zero);
        ease.AddPoint(Vector2.One);

		NewPose(true);
	}

	public void Init(int sign)
	{
		unnecessaryTurns *= sign;
	}


	Vector3 newRotation = Vector3.Zero;
	public void NewPose(bool insta = false)
	{
		newRotation.Z = (float)GD.RandRange(minAngle, maxAngle);

		if (insta)
		{
			Rotation = newRotation;
		}
		else
		{
			MoveTo(newRotation.Z);
		}
	}

	async void MoveTo(float to)
	{
		//await ToSignal(GetTree().CreateTimer(GD.RandRange(0f, maxDelay)), "timeout");

		float from = Rotation.Z - Mathf.Pi * 2f * unnecessaryTurns;
		float t = 0, v;
		float startTime = Time.GetTicksMsec(), now;

		while (t < duration)
		{
			now = Time.GetTicksMsec();
			t = (now - startTime) / 1000f;
			v = t / duration;
			newRotation.Z = Mathf.Lerp(from, to, ease.Sample(v));
			
			Rotation = newRotation;

			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
	}
}
