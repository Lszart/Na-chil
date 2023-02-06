using Godot;
using System;

public class RigidBodyBox : RigidBody2D
{
    private bool Move = false;
    Vector2 MovePos = Vector2.Zero;
    public void move(Vector2 pos)
    {
        Move = true;
        MovePos = pos;
    }

    public override void _IntegrateForces(Physics2DDirectBodyState state)
    {
        if (Move)
        {
            LinearVelocity = Vector2.Zero;
            state.Transform = new Transform2D(0,MovePos);
            Move = false;
        }
    }
}
