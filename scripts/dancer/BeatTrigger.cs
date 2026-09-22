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
	float angle, previousAngle = 0;
	float rangeMin, rangeMax;
	AudioStreamPlayer3D audio;

	Sprite3D beatColor;
	GpuParticles3D particles;
	[Export] Texture2D texture;


    public override void _Ready()
    {
		parent = GetParent() as Node3D;
		beatColor = GetChild(1) as Sprite3D;
		particles = GetChild(2) as GpuParticles3D;

		feedbackStartSize = beatColor.PixelSize;

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

		if (Input.IsKeyPressed(Key.Ctrl))
		{
			if (
				(previousAngle < angle && CurrentAngle >= angle)
				||
				(angle < 0.1f && previousAngle > 5.03f && CurrentAngle < 1f)
			)
				audio.Play();
		} 

		previousAngle = CurrentAngle;
    }

	public async void Feedback()
	{
		particles.Restart();

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
		set
		{
			beatColor.Modulate = value;

			StandardMaterial3D material = new StandardMaterial3D();
			material.Transparency = BaseMaterial3D.TransparencyEnum.Alpha;
			material.ShadingMode = BaseMaterial3D.ShadingModeEnum.Unshaded;
			material.AlbedoColor = value;
			material.AlbedoTexture = texture;
			particles.MaterialOverride = material;
		}

    }

	public AudioStreamPlayer3D Audio
	{
		set => audio = value;
	}
}
