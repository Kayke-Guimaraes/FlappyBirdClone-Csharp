using Godot;
using System;

//Não está sendo usado por enquanto
public class ControlOption : Control
{
	[Signal]
	public delegate void SignalPermission();
	public FileDialog Explorer;
	
	public override void _Ready()
	{
		Explorer = GetNode<FileDialog>("WindowOption/FileDialog");
		
	}
	private void _on_ChangeMusicButton_button_up()
	{
		EmitSignal("SignalPermission");
		//OS.RequestPermissions();
		if(OS.RequestPermissions() == true)
		{
			Explorer.Hide();
			Explorer.Show();
		}
		else if(OS.RequestPermissions() == false)
		{
			GetNode<AcceptDialog>("WindowOption/AcceptDialog").Show();
		}
		
	}
}

