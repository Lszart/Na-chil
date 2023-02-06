using Godot;
using System;

public class BoxSpawnSwitch : Area2D
{
    [Export]
    private Vector2 BoxRelativePosition = Vector2.Right;
    private PackedScene Box = (PackedScene)ResourceLoader.Load("res://Level/RigidBodyBox.tscn");
    private Node2D MyBox = null;
    public override void _Ready()
    {
        
    }
    public void OnBoxSwitchBodyEntered(Node body){
        if(MyBox == null){
            MyBox = Box.Instance() as Node2D;            
            GetParent().AddChild(MyBox);
        }        
        MyBox.GetNode<RigidBodyBox>(".").move((BoxRelativePosition*20) + GlobalPosition);
        
    }
}
