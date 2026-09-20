using Godot;
using System;

public partial class KeyMovement : Sprite3D
{
	[Export] float speed = 1;


	public override void _Process(double delta)
	{
		Vector3 movementInput = Vector3.Zero;
        if (Input.IsKeyPressed(Key.Up))
            movementInput.Y += 1;
        if (Input.IsKeyPressed(Key.Down))
            movementInput.Y -= 1;
        if (Input.IsKeyPressed(Key.Right))
            movementInput.X += 1;
        if (Input.IsKeyPressed(Key.Left))
            movementInput.X -= 1;

        GlobalPosition += movementInput * speed * (float)delta;
    }
}
