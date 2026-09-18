using Godot;
using System;

public partial class MainManager : Node3D
{
	[Export] Dancer lDancer;
	[Export] Dancer rDancer;


    public override void _Ready()
    {
		int beats;

		beats = Mathf.Abs((int)GD.Randi()) % 4 + 2;
        lDancer.Init(beats);

		beats = Mathf.Abs((int)GD.Randi()) % 4 + 2;
        rDancer.Init(beats);
    }


	public override void _Process(double delta)
	{
	}


    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsPressed() && !@event.IsEcho())
		{
			if (@event.IsActionPressed("l"))
				lDancer.NewPose();
			if (@event.IsActionPressed("r"))
				rDancer.NewPose();
		}
    }
}
