using Godot;
using System;

public class DoubleDoor : StaticBody2D
{
    private AnimationPlayer player;
    [Export]
    bool s1 = false,s2 = false;
    public override void _Ready()
    {
        player = GetNode<AnimationPlayer>("AnimationPlayer");
    }
    public void OnSwitch1BodyEntered(Area area)
    {
        s1 = true;
        if(s2==true){
            check();
        }        
    }
    public void OnSwitch2BodyEntered(Area area)
    {
        s2 = true;
        if(s1==true){
            check();
        }  
    }
    public void OnSwitch1BodyExited(Area area)
    {
        s1 = false;
        check();
    }
    public void OnSwitch2BodyExited(Area area)
    {
        s2 = false;
        check();
    }
    void check(){
        if(s1 && s2){
            player.Play("DoorOpen", player.CurrentAnimationPosition);
        }else{
            player.PlayBackwards("DoorOpen",player.CurrentAnimationPosition);
        }
    }
}
