using Godot;
using System;
using System.Collections.Generic;

public class Rope : Node2D
{
    [Export]
    private float SegmentLenght = 4;
    [Export]
    private float RopeCloseTolerance = 4;
    private Godot.Collections.Array<Node2D> ropeSegments = new Godot.Collections.Array<Node2D>();
    private PackedScene ropeSegment = (PackedScene)ResourceLoader.Load("res://Level/RopeSegment.tscn");
    private Node2D RopeStart;
    private Node2D RopeEnd;
    private PinJoint2D RopeStartJoint;
    private PinJoint2D RopeEndJoint;
    private Vector2[] RopePoints;


    public override void _Ready()
    {
        RopeStart = GetNode<Node2D>("RopeStart");
        RopeEnd = GetNode<Node2D>("RopeEnd");
        RopeStartJoint = RopeStart.GetNode<PinJoint2D>("CollisionShape2D/PinJoint2D");
        RopeEndJoint = RopeEnd.GetNode<PinJoint2D>("CollisionShape2D/PinJoint2D");
    }

    public void PlayerRope(Node2D player)
    {
        RopeEnd = player;
        RopeEndJoint = player.GetNode<PinJoint2D>("CollisionShape2D/PinJoint2D");
        GetNode<Node>("RopeEnd").QueueFree();
    }

    public override void _Process(float delta)
    {
        RopePoints = (Vector2[])CreatePoints().ToArray();
        if (RopePoints.Length != 0)
        {
            Update();
        }
    }
    public void SpawnRope(Vector2 startPos, Vector2 endPos)
    {
        RopeStart.GlobalPosition = startPos;
        RopeEnd.GlobalPosition = endPos;
        startPos = RopeStartJoint.GlobalPosition;
        endPos = RopeEndJoint.GlobalPosition;
        float distance = startPos.DistanceTo(endPos);
        int SegmentAmount = Mathf.RoundToInt(distance / SegmentLenght);
        var angle = (endPos - startPos).Angle();
        CreateRope((int)SegmentAmount, RopeStart, endPos, (float)angle);
    }
    public void IncreaseRope()
    {
        Node2D parent = ropeSegments[ropeSegments.Count - 1];
        Vector2 DistanceToEnd = RopeEndJoint.GlobalPosition - parent.GlobalPosition;
        parent.LookAt(RopeEndJoint.GlobalPosition);
        var angle = DistanceToEnd.Angle();

        if (DistanceToEnd.Length() < SegmentLenght)
        {
            parent.GlobalPosition -= DistanceToEnd;
        }
        parent = AddSegment(parent, ropeSegments.Count, (float)angle);

        parent.Name = "RopeSegment" + ropeSegments.Count;
        ropeSegments.Add(parent);
        RopeEndJoint.NodeB = parent.GetPath();
    }
    public void DecreaseRope()
    {

    }
    public float RopeLength(){
        return SegmentLenght * ropeSegments.Count;
    }
    private void CreateRope(int amount, Node2D parent, Vector2 endPos, float angle)
    {
        for (int i = 0; i < amount; i++)
        {
            parent = AddSegment(parent, i, angle);
            parent.Name = "RopeSegment" + i;
            ropeSegments.Add(parent);
            var JointPos = parent.GetNode<PinJoint2D>("CollisionShape2D/PinJoint2D").GlobalPosition;
            if (JointPos.DistanceTo(endPos) < RopeCloseTolerance)
            {
                break;
            }
        }
        RopeEndJoint.NodeA = RopeEnd.GetPath();
        RopeEndJoint.NodeB = ropeSegments[ropeSegments.Count - 1].GetPath();
    }

    private RopeSegment AddSegment(Node parent, int id, float angle)
    {
        PinJoint2D joint = parent.GetNode<PinJoint2D>("CollisionShape2D/PinJoint2D");
        RopeSegment Segment = ropeSegment.Instance() as RopeSegment;
        Segment.GlobalPosition = joint.GlobalPosition;
        Segment.GlobalRotation = angle;
        Segment.parent = this;
        Segment.id = id;
        AddChild(Segment);
        joint.NodeA = parent.GetPath();
        joint.NodeB = Segment.GetPath();
        return Segment;
    }

    private List<Vector2> CreatePoints()
    {
        List<Vector2> RopePoints = new List<Vector2>();

        RopePoints.Add(RopeStartJoint.GlobalPosition);
        foreach (var p in ropeSegments)
        {
            RopePoints.Add(p.GlobalPosition);
        }
        RopePoints.Add(RopeEndJoint.GlobalPosition);
        return RopePoints;
    }


    public override void _Draw()
    {
        DrawPolyline(RopePoints, Colors.Black);
    }
}
