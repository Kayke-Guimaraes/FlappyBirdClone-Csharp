using Godot;
using System;

public class Obstacule : Node2D
{
	[Signal]
	public delegate void BodyCrash();
	[Signal]
	public delegate void AreaScore();
	
	public Vector2 Velocity;
	public float Speed = 225f;
	
	public override void _Ready()
	{
		
	}
	
	public override void _Process(float delta)
	{
		Velocity = Position;
		Velocity.x -= Speed * delta;
		Position = Velocity;
	}
	
	
	private void _on_Up_Obst_body_entered(object body)
	{
		EmitSignal("BodyCrash");
	}
	
	
	private void _on_Down_Obst_body_entered(object body)
	{
		EmitSignal("BodyCrash");
	}
	
	
	private void _on_Points_Area_body_entered(object body)
	{
		EmitSignal("AreaScore");
	}
	
	
	private void _on_Visibility_Notifier2D_screen_exited()
	{
		QueueFree();
	}
	
	private void End()
	{
		SetProcess(false);
	}
}
