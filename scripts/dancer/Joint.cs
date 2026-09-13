using Godot;
using System;

public partial class Joint : Node3D
{
	[Export] double minAngle;
	[Export] double maxAngle;
	[Export] float duration = 0.2f;
	[Export] Curve ease;


	public override void _Ready()
	{
		NewPose(true);
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
		float from = Rotation.Z;
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
