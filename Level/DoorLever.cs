using Godot;
using System;

public class DoorLever : StaticBody2D
{
    private AnimationPlayer player;
    public override void _Ready()
    {
        player = GetNode<AnimationPlayer>("AnimationPlayer");
    }
    public void OnLeverBodyEntered(Node body){
        player.Play("DoorOpen", player.CurrentAnimationPosition);
    }
    public void OnLeverBodyExited(Node body){
        player.Play("DoorClose", player.CurrentAnimationPosition);
    }
}
