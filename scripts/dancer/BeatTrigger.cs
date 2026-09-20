using Godot;
using System;

public partial class BeatTrigger : Area3D
{
	[Export] Sprite3D visuals;
	[Export] float minSize = 0.003f;
	[Export] float maxSize = 0.006f;
	public Node3D other = null;

	[Export] Node3D root;
	float angle;


	public bool inside = false;


    public override void _Ready()
    {
		angle = root.Rotation.Z;
    }

    public override void _Process(double delta)
    {
		if (angle < TempoManager.Angle % Mathf.Tau)
			; ////
    }


	void OnAreaEntered(Node3D body)
	{
		inside = true;
		visuals.PixelSize = maxSize;
    }

	void OnAreaExited(Node3D body)
	{
		inside = false;
		visuals.PixelSize = minSize;
    }
}
