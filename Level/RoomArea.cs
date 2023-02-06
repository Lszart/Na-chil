using Godot;
using System;

public class RoomArea : Area2D
{
    [Export]
    private Vector2 CameraPositon = Vector2.Zero;
    [Export]
    private Vector2 CameraZoom = Vector2.One;
    [Export]
    private float RopeLength = 100;
    public void OnRoomAreaBodyEntered(Node body){
        Camera2D camera= GetTree().Root.GetNode<Camera2D>("Node2D/Camera2D");
        camera.Position = CameraPositon;
        camera.Zoom = CameraZoom;
        Player p = body.GetNode<Player>(".");
        p.SetAnchor(GetNode<Node2D>("Anchor").GlobalPosition,RopeLength);
    }

}
