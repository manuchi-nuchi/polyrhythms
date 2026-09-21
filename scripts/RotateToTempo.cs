using Godot;
using System;

public partial class RotateToTempo : Node3D
{
    Vector3 newRotation = Vector3.Zero;
    public override void _Process(double delta)
    {
        newRotation.Z = TempoManager.Angle;
        Rotation = newRotation;
    }
}
