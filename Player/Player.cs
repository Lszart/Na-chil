using Godot;
using System;

public class Player : RigidBody2D
{
    [Export]
    private float Strength = 200;
    [Export]
    private float SprintBoost = 200;
    [Export]
    private float BoostPenalty = -100;
    [Export]
    private float MaxSprintGauge = 5;
    private float SprintGauge;
    private float boost = 0;
    private Vector2 ApplliedForces = Vector2.Zero;
    [Export]
    public Vector2 AnchorPoint = Vector2.Zero;
    private PackedScene Rope = (PackedScene)ResourceLoader.Load("res://Level/Rope.tscn");
    private Rope r;
    private AnimationNodeStateMachinePlayback stateMachine;
    private AnimationTree animTree;
    [Export]
    private float RopeLength = 50;
    private Timer RopeTimer;
    private Timer SprintCooldown;
    private bool IsSprinting = false;
    private Sprite CharacterSprite;
    private ProgressBar RopeGauge;
    public override void _Ready()
    {
        animTree = GetNode<AnimationTree>("AnimationTree");
        animTree.Active = true;
        stateMachine = (AnimationNodeStateMachinePlayback)animTree.Get("parameters/playback");
        RopeTimer = GetNode<Timer>("RopeTimer");
        SprintCooldown = GetNode<Timer>("SprintCooldown");
        SprintGauge = MaxSprintGauge;
        CharacterSprite = GetNode<Sprite>("Sprite");
        RopeGauge = GetNode<ProgressBar>("../Camera2D/UI/ProgressBar");
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (LinearVelocity.Length() > 1)
        {
            stateMachine.Travel("Walk");
            animTree.Set("parameters/Walk/TimeScale/scale", Mathf.Min(LinearVelocity.Length() / 100, 1));
            if (LinearVelocity.x < 0)
            {
                CharacterSprite.Scale = new Vector2(-1, 1);
            }
            else
            {
                CharacterSprite.Scale = new Vector2(1, 1);
            }
        }
        else
        {
            stateMachine.Travel("IDLE");
        }
        if(r != null)
        {
            RopeGauge.MaxValue = RopeLength;
            RopeGauge.Value = r.RopeLength();
        }
    }
    private Vector2 ReadInput(float delta)
    {
        Vector2 input = Vector2.Zero;
        if (Input.IsActionPressed("up"))
        {
            input.y = -1;
        }
        if (Input.IsActionPressed("down"))
        {
            input.y = 1;
        }
        if (Input.IsActionPressed("left"))
        {
            input.x = -1;
        }
        if (Input.IsActionPressed("right"))
        {
            input.x = 1;
        }
        if (Input.IsActionJustPressed("action"))
        {
            SetAnchor(AnchorPoint, RopeLength);
        }
        if (Input.IsActionPressed("more") && RopeTimer.IsStopped() && r != null)
        {
            if (r.RopeLength() < RopeLength)
            {
                r.IncreaseRope();
                RopeTimer.Start();
            }
        }
        if (Input.IsActionPressed("sprint"))
        {
            if (SprintCooldown.IsStopped())
            {
                boost = SprintBoost;
                SprintGauge -= delta;
                if (SprintGauge <= 0)
                {
                    boost = BoostPenalty;
                    SprintCooldown.Start();
                }
            }
            else if (SprintGauge <= MaxSprintGauge)
            {
                SprintGauge += delta;
            }
            else
            {
                SprintGauge = MaxSprintGauge;
            }
        }
        else if (SprintCooldown.IsStopped())
        {
            boost = 0;
            if (SprintGauge <= MaxSprintGauge)
            {
                SprintGauge += delta;
            }
            else
            {
                SprintGauge = MaxSprintGauge;
            }
        }
        return input;
    }
    void AttachRope(Vector2 Anchor)
    {
        var rope = Rope.Instance();
        GetParent().AddChild(rope);
        r = rope.GetNode<Rope>(".");
        r.PlayerRope(this);
        r.SpawnRope(Anchor, Position);
    }
    public void SetAnchor(Vector2 anchor, float Length)
    {
        if (r != null)
        {
            r.QueueFree();
        }
        AnchorPoint = anchor;
        AttachRope(anchor);
        RopeLength = Length;
    }
    void ForcePerFrame(Vector2 Force)
    {
        ApplliedForces += Force;
        AddCentralForce(Force);
    }
    public override void _PhysicsProcess(float delta)
    {
        ForcePerFrame(-ApplliedForces);
        Vector2 Direction = ReadInput(delta).Normalized();
        ForcePerFrame(Direction * (Strength + boost));
        //if(LinearVelocity.Length() == 0){
        //    ForcePerFrame(-Weight*MStatic*LinearVelocity.Normalized());
        //}else{
        //    ForcePerFrame(-Weight*MMoving*LinearVelocity.Normalized());
        //}
    }
}
