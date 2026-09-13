using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;

public partial class Dancer : Node3D
{
	[Export] Array<Joint> joints;


    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventKey eventKey)
        {
            if (eventKey.Pressed && eventKey.Keycode == Key.N)
            {
                foreach (Joint joint in joints)
                    joint.NewPose(true);
            }
        }
    }
}
