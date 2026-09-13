using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;

public partial class Dancer : Node3D
{
	[Export] Array<Joint> joints;
    [Export] float maxBodyOffset = 1;
    [Export] Node3D body;
    Curve ease = new Curve();
    float duration = 0.2f;


    public override void _Ready()
    {
        ease.AddPoint(Vector2.Zero);
        ease.AddPoint(Vector2.One);
    }


    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventKey eventKey)
        {
            if (eventKey.Pressed && eventKey.Keycode == Key.N && !@event.IsEcho())
            {
                NewPose();
            }
        }
    }

    void NewPose()
    {
        foreach (Joint joint in joints)
            joint.NewPose();

        MoveBody();
    }

    async void MoveBody()
    {
        Vector3 from = body.Position;
        Vector3 to = new Vector3(
            (float)GD.RandRange(-maxBodyOffset, maxBodyOffset),
            (float)GD.RandRange(-maxBodyOffset, maxBodyOffset),
            0
        );
        float t = 0, v, startTime = Time.GetTicksMsec(), now;

        while (t < duration)
        {
            now = Time.GetTicksMsec();
            t = (now - startTime) / 1000;
            v = ease.Sample(t);

            body.Position = from.Lerp(to, v);

            await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        }
    }
}
