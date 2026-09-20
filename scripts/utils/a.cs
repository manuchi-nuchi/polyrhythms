using Godot;
using System;

public partial class a : Sprite3D
{
    Node3D other = null;
    [Export] float minSize = 0.01f;
    [Export] float maxSize = 0.05f;
    float startDistance;

    float distance, v;

    public override void _Process(double delta)
	{
        if (other == null)
            return;

		distance = GlobalPosition.DistanceTo(other.GlobalPosition);
        v = distance / startDistance;
        PixelSize = Mathf.Lerp(maxSize, minSize, v);
        GD.Print(startDistance + " : " + distance + " :: " +  v);
        GD.Print(GlobalPosition + " : " + other.GlobalPosition + "\n");
	}

    void OnAreaEntered(Node3D body)
    {
        other = body;
        startDistance = GlobalPosition.DistanceTo(body.GlobalPosition);
    }

    void OnAreaExited(Node3D body)
    {
        other = null;
        PixelSize = minSize;
    }
}
