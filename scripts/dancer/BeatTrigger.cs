using Godot;
using System;

public partial class BeatTrigger : Sprite3D
{
	[Export] float minSize = 0.003f;
	[Export] float maxSize = 0.006f;
	public Node3D other = null;

	Node3D parent;
	float angle;
	float rangeMin, rangeMax;


    public override void _Ready()
    {
		parent = GetParent() as Node3D;

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
}
