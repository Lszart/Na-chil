using Godot;
using System;

public class DoorSwitch : StaticBody2D
{
    private AnimationPlayer player;
    [Export]
    private String MyBox = "Box";
    public override void _Ready()
    {
        player = GetNode<AnimationPlayer>("AnimationPlayer");
    }
    public void OnSwitchBodyEntered(Area area)
    {
        if (area.Name.Equals(MyBox))
        {
            player.Play("DoorOpen", player.CurrentAnimationPosition);
        }
    }
    public void OnSwitchBodyExited(Area area)
    {
        if (area.Name.Equals(MyBox))
        {
            player.Play("DoorClose", player.CurrentAnimationPosition);
        }
    }
}
