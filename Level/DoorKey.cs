using Godot;
using System;

public class DoorKey : StaticBody2D
{
    private AnimationPlayer player;
    private Node key;
    public override void _Ready()
    {
        player = GetNode<AnimationPlayer>("AnimationPlayer");
        key = GetNode<Node>("../Key");
    }
    public void OnKeyBodyEntered(Node body){
        GD.Print(body.Name);
        if(body.Name.Equals("Player")){
            key.QueueFree();
            player.Play("DoorOpen");
        }
    }
}
