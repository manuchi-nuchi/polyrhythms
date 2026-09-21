using Godot;
using System;

public partial class BeatTrigger : Sprite3D
{
	[Export] float minSize = 0.003f;
	[Export] float maxSize = 0.006f;

	float feedbackStartSize;
	[Export] float feedbackTargetSize = .01f;
	[Export] float feedbackDuration = .5f;

	Node3D parent;
	float angle;
	float rangeMin, rangeMax;

	Sprite3D beatColor;


    public override void _Ready()
    {
		parent = GetParent() as Node3D;
		beatColor = GetChild(1) as Sprite3D;

		angle = parent.Rotation.Z;

		if (angle > 0)
		{
			rangeMin = angle - Globals.ErrorMargin;
			rangeMax = angle + Globals.ErrorMargin;
		}
		else
		{
			rangeMin = Mathf.Tau - Globals.ErrorMargin;
			rangeMax = Globals.ErrorMargin;
		}
    }

    public override void _Process(double delta)
    {
		PixelSize = Inside ? maxSize : minSize;
    }

	public async void Feedback()
	{
		float t = 0, v;
		float startTime = Time.GetTicksMsec(), now;

		while (t < feedbackDuration)
		{
			now = Time.GetTicksMsec();
			t = (now - startTime) / 1000f;
			v = t / feedbackDuration;
			beatColor.PixelSize = Mathf.Lerp(feedbackTargetSize, feedbackStartSize, v);

			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}

		beatColor.PixelSize = feedbackStartSize;
	}


	public bool Inside
	{
		get => angle > 0 ?
            CurrentAngle > rangeMin && CurrentAngle < rangeMax :
            CurrentAngle > rangeMin || CurrentAngle < rangeMax;
    }

	float CurrentAngle
	{
		get => TempoManager.Angle % Mathf.Tau;
    }

	public Color Color
	{
		set => beatColor.Modulate = value;
	}
}
