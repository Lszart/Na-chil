using Godot;
using System;

public class HammerSwitch : Node2D
{
    private AnimationPlayer player;
    public override void _Ready()
    {
        player = GetNode<AnimationPlayer>("AnimationPlayer");
    }
    public void OnSwitchBodyEntered(Area area)
    {
        player.Play("Move");
    }
    public void OnSwitchBodyExited(Area area)
    {
        player.Stop(false);
    }
}
