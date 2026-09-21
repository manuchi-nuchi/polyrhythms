using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;

public partial class Dancer : Node3D
{
	[Export] Array<Joint> joints;
    [Export] float maxBodyOffset = 1;
    [Export] Node3D body;
    Curve ease = new Curve();
    float duration = 0.2f;

    [Export] PackedScene beatScene;
    [Export] PackedScene beatFeedbackScene;
    [Export] Node3D tempoVisuals;
    int beats = 2;
    Array<BeatTrigger> beatTriggers;
    [Export] float feedbackDuration = 1;

    [Export] int dancingDirection = 1;
    [Export] Array<Sprite3D> colored;
    [Export] float hueOffset = 0.2f;


    public override void _Ready()
    {
        ease.AddPoint(Vector2.Zero);
        ease.AddPoint(Vector2.One);
    }

    float midHue;
    Color oppositeColor;
    void Paint()
    {
        // chest and head are at 0 and 1
        // rest, which gets interpolated, is placed from 2

        float hueA = GD.Randf();
        float huheB = hueA < 0.8f ? hueA + hueOffset : hueA - hueOffset;

        float step = 1f / (colored.Count - 3);
        for (int i = 0; i < colored.Count - 2; i++)
        {
            colored[i + 2].Modulate = Color.FromHsv(
                Mathf.Lerp(hueA, huheB, step * i),
                1,
                1
            );
        }

        midHue = Mathf.Lerp(hueA, huheB, .5f);
        colored[0].Modulate = Color.FromHsv(midHue, .9f, .9f);
        colored[1].Modulate = Color.FromHsv(midHue, 1, 1);


        oppositeColor = Color.FromHsv((midHue + .5f) % 1f, 1, 1);
        foreach (BeatTrigger beat in beatTriggers)
            beat.Color = oppositeColor;
    }

    public void Init(int beats)
    {
        Node3D instantiated;
        float angle = 2f * Mathf.Pi / beats;
        beatTriggers = new Array<BeatTrigger>();

        for (int i = 0; i < beats; i++)
        {
            instantiated = beatScene.Instantiate() as Node3D;
            instantiated.Rotation = new Vector3(0, 0, angle * i);
            AddChild(instantiated);
            beatTriggers.Add(instantiated.GetChild(0) as BeatTrigger);
        }

        foreach (Joint joint in joints)
            joint.Init(dancingDirection);

        Paint();
    }


    public void NewPose()
    {

        bool inTime = false;
        foreach(BeatTrigger beat in beatTriggers)
        {
            if (beat.Inside)
            {
                inTime = true;
                beat.Feedback();
                ////
                break;
            }
        }

        if (!inTime)
        {
            BadBeatFeedback();

            ////
        }

        foreach (Joint joint in joints)
            joint.NewPose(!inTime);
        MoveBody();
    }

    async void MoveBody()
    {
        Vector3 from = body.Position;
        Vector3 to = new Vector3(
            (float)GD.RandRange(-maxBodyOffset, maxBodyOffset),
            (float)GD.RandRange(-maxBodyOffset, maxBodyOffset),
            0
        );
        float t = 0, v, startTime = Time.GetTicksMsec(), now;

        while (t < duration)
        {
            now = Time.GetTicksMsec();
            t = (now - startTime) / 1000;
            v = ease.Sample(t);

            body.Position = from.Lerp(to, v);

            await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        }
    }

    void BadBeatFeedback()
    {
        Sprite3D feedback = beatFeedbackScene.Instantiate() as Sprite3D;
        AddChild(feedback);
        feedback.GlobalPosition = tempoVisuals.GlobalPosition;
        Fade(feedback);
    }

    async void Fade(Sprite3D target)
    {
        float t = 0, v;
        float startTime = Time.GetTicksMsec(), now;
        Color color = new Color(1, 1, 1, 1);

        while (t < feedbackDuration)
        {
            now = Time.GetTicksMsec();
            t = (now - startTime) / 1000f;
            v = t / feedbackDuration;

            color.A = 1f - v;
            target.Modulate = color;

            await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        }

        target.QueueFree();
    }
}
