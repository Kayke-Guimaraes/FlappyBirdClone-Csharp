using Godot;
using System;

public class Signals : Node2D
{
	// Declaração de várias variáveis que representam diferentes nós da cena
	public KinematicBody2D Player;
	public Node2D Obstacules;
	public Control ControlView;
	public Label Score, BestScore;
	public Button StartButton, RestartButton, OptionsButton, CreditButton, ExitButton;
	public TextureRect TextureGameOver;
	public ColorRect WhiteColorRect;
	public Panel PanelRect;
	public Directory Dir;
	public Tween TweenFinalScore, TweenFinalBestScore;
	public Label FinalScore, FinalBestScore, LabelNewScore;
	public Node2D Background;
	public Node2D Ground, SpriteBack1, SpriteBack2;
	public Sprite SpriteGround1, SpriteGround2;
	public TouchScreenButton Touch;
	
	// Sinais que são emitidos em eventos específicos
	[Signal]
	public delegate void Restart();
	
	[Signal]
	public delegate void SignalPause();
	
	// Variáveis para controlar o estado do jogo e pontuação
	public double GetFinalBestScore;
	public bool NewScore;
	
	
	String SaveBestScore = "user://saves/savebst.cfg"; // Caminho para salvar a melhor pontuação
	ConfigFile Config = new ConfigFile(); // Configuração para salvar o score
	
	public int Point;
	public bool AnimationExecuted;
	public bool endBack_Ground;
	
	// Função chamada quando a cena é carregada
	public override void _Ready()
	{
		// Inicializa os nós da cena e os componentes necessários
		Player = GetNode<KinematicBody2D>("../Player");
		Obstacules = GetNode<Node2D>("../Obstacules");
		ControlView = GetNode<Control>("../CanvasLayer/Control");
		Background = GetNode<Node2D>("../Background");
		Ground = GetNode<Node2D>("../Ground");
		SpriteBack1 = Background.GetNode<Node2D>("../Background/SpriteBack1Node");
		SpriteBack2 = Background.GetNode<Node2D>("../Background/SpriteBack2Node");
		SpriteGround1 = Ground.GetNode<Sprite>("../Ground/SpriteGround1");
		SpriteGround2 = Ground.GetNode<Sprite>("../Ground/SpriteGround3");
		
		// Inicializa os rótulos de pontuação
		Score = GetNode<Label>("../CanvasLayer/Control/Score");
		BestScore = GetNode<Label>("../CanvasLayer/Control/BestScore");
		
		// Inicializa os botões de controle
		StartButton = GetNode<Button>("../CanvasLayer/Control/StartButton");
		RestartButton = GetNode<Button>("../CanvasLayer/Control/RestartButton");
		CreditButton = GetNode<Button>("../CanvasLayer/Control/CreditButton");
		OptionsButton = GetNode<Button>("../CanvasLayer/Control/OptionsButton");
		ExitButton = GetNode<Button>("../CanvasLayer/Control/ExitButton");
		
		// Inicializa outros elementos de UI e animações
		TextureGameOver = GetNode<TextureRect>("../CanvasLayer/Control/GameOverTexture");
		WhiteColorRect = GetNode<ColorRect>("../CanvasLayer/Control/WhiteCrash");
		PanelRect = GetNode<Panel>("../CanvasLayer/Control/Panel");
		
		// Inicializa tweens para animações
		TweenFinalScore = GetNode<Tween>("../CanvasLayer/Control/TweenFinalScore");
		TweenFinalBestScore = GetNode<Tween>("../CanvasLayer/Control/TweenFinalBestScore");
		
		FinalScore = GetNode<Label>("../CanvasLayer/Control/Panel/LabelScore/FinalScore");
		FinalBestScore = GetNode<Label>("../CanvasLayer/Control/Panel/LabelBestScore/FinalBestScore");
		
		LabelNewScore = GetNode<Label>("../CanvasLayer/Control/Panel/LabelNew");
		Touch = GetNode<TouchScreenButton>("../CanvasLayer/Touch");
		GetNode<Control>("../CanvasLayer/ControlOption").Visible = false;
		
		
		Dir = new Directory();
		
		// Ajusta o tamanho da área de controle para o tamanho da janela
		ControlView.RectSize = GetViewportRect().Size;
		
		// Conecta sinais de eventos (exemplo: game over, score)
		Obstacules.Connect("GameOver", this, "OnGameOver");
		Obstacules.Connect("Scored", this, "ScoreForLabel");
		
		// Esconde alguns elementos da UI na inicialização
		Score.Visible = false;
		BestScore.Visible = true;
		TextureGameOver.Visible = false;
		WhiteColorRect.Visible = false;
		PanelRect.Visible = false;
		RestartButton.Visible = false;
		LabelNewScore.Visible = false;
		
		AnimationExecuted = false;
		NewScore = false;
		endBack_Ground = false;
		//SystemArc.Visible = true;
		
		// Ajuste da posição do botão Restart
		var PositionRestBtn = RestartButton.RectPosition;
		PositionRestBtn.y = 677;
		RestartButton.RectPosition = PositionRestBtn;
		
		
		Point = 0; // Inicializa a pontuação
		LoadScore(); // Carrega a melhor pontuação salva
	}
	
	// Função chamada a cada frame
	public override void _Process(float delta)
	{
		// Lógica de movimento de fundo e chão (movimento infinito)
		if(endBack_Ground == false)
		{
			var SpriteBack1_Pos = SpriteBack1.Position;
			var SpriteBack2_Pos = SpriteBack2.Position;
			var SpriteGround1_Pos = SpriteGround1.Position;
			var SpriteGround2_Pos = SpriteGround2.Position;
			
			// Desloca o fundo e o chão
			SpriteBack1_Pos.x -= 0.7f;
			SpriteBack2_Pos.x -= 0.7f;
			SpriteGround1_Pos.x -= 2.7f;
			SpriteGround2_Pos.x -= 2.7f;
			
			// Reposiciona o fundo e o chão quando saem da tela (efeito de looping)
			if(SpriteBack1_Pos.x < -603)
			{
				SpriteBack1_Pos.x = 995;
			}
			if(SpriteBack2_Pos.x < -603)
			{
				SpriteBack2_Pos.x = 995;
			}
			
			if(SpriteGround1_Pos.x < -602)
			{
				SpriteGround1_Pos.x = 996;
			}
			if(SpriteGround2_Pos.x < -602)
			{
				SpriteGround2_Pos.x = 996;
			}
			
			// Aplica as novas posições
			SpriteBack1.Position = SpriteBack1_Pos;
			SpriteBack2.Position = SpriteBack2_Pos;
			SpriteGround1.Position = SpriteGround1_Pos;
			SpriteGround2.Position = SpriteGround2_Pos;
		}
		
		//Botão invísivel por enquanto
		if(GetNode<WindowDialog>("../CanvasLayer/ControlOption/WindowOption").GetCloseButton().IsPressed())
		{
			GetNode<Control>
			("../CanvasLayer/ControlOption").Visible = false;
			
			GetNode<WindowDialog>
			("../CanvasLayer/ControlOption/WindowOption").Hide();
			
			GetNode<WorldEnvironment>("../WorldEnvironment")
			.Environment.DofBlurNearAmount = (float)0;
			//OptionsButton.Visible = true;
			CreditButton.Visible = true;
			ExitButton.Visible = true;
		}
		
		// Verifica se o botão de fechar a janela de créditos foi pressionado
		if(GetNode<WindowDialog>("../CanvasLayer/ControlOption/WindowCredit").GetCloseButton().IsPressed())
		{
			// Oculta os créditos e retorna ao estado inicial
			GetNode<Control>
			("../CanvasLayer/ControlOption").Visible = false;
			
			GetNode<WindowDialog>
			("../CanvasLayer/ControlOption/WindowCredit").Hide();
			
			GetNode<WorldEnvironment>("../WorldEnvironment")
			.Environment.DofBlurNearAmount = (float)0;
			StartButton.Visible = true;
			//OptionsButton.Visible = true;
			CreditButton.Visible = true;
			ExitButton.Visible = true;
		}
	}
	
	// Função chamada quando o jogo acaba (recebe um sinal)
	private void OnGameOver()
	{
		ForGameOver();
	}
	
	// Função para finalizar o jogo quando o player colide com o chão (recebe um sinal)
	private void _on_Ground_Area_body_entered(object body)
	{
		ForGameOver();
	}
	
	// Função que realiza ações de "game over"
	private void ForGameOver()
	{
		// Desativa o processamento dos obstáculos e o personagem morre
		Obstacules.SetProcess(false);
		GetTree().CallGroup("Obstacules", "End");
		Player.Set("Death", true);
		//Player.Set("Velocity.y", 0);
		
		if(Score.Visible == true){
			// Toca som de colisão quando o jogador morre
			GetNode<AudioStreamPlayer>("smack").Play();
		}
		Score.Visible = false;
		endBack_Ground = true;
		GetNode<TextureButton>("../CanvasLayer/Control/Pause").Visible = false;
		
		// Executa a animação do game over
		switch (AnimationExecuted)
		{
			case false:
				WhiteColorRect.Visible = true;
				WhiteColorRect.GetNode<AnimationPlayer>("Animation").Play("whitecrash");
				AnimationExecuted = true;
				GetNode<Timer>("../CanvasLayer/Control/TimerGameOver").Start();
				break;
			
			default:
				
				break;
		}
	}
	
	// Função que inicia o jogo quando o botão "Start" é pressionado
	private void _on_StartButton_button_up()
	{
		Obstacules.SetProcess(true);
		Player.Set("Start", true);
		Player.Call("Jump");
		StartButton.Visible = false;
		OptionsButton.Visible = false;
		CreditButton.Visible = false;
		ExitButton.Visible = false;
		Score.Visible = true;
		BestScore.Visible = false;
		GetNode<TextureButton>("../CanvasLayer/Control/Pause").Visible = true;
	}
	
	//Botão invisivel por enquanto
	private void _on_OptionsButton_button_up()
	{
		GetNode<Control>("../CanvasLayer/ControlOption").Visible = true;
		
		GetNode<WindowDialog>
		("../CanvasLayer/ControlOption/WindowOption").Show();
		
		GetNode<WorldEnvironment>("../WorldEnvironment")
		.Environment.DofBlurNearAmount = (float)0.15;
		
		OptionsButton.Visible = false;
		CreditButton.Visible = false;
		ExitButton.Visible = false;
		
	}
	
	// Função que mostra os creditos
	private void _on_CreditButton_button_up()
	{
		GetNode<Control>("../CanvasLayer/ControlOption").Visible = true;
		
		GetNode<WindowDialog>
		("../CanvasLayer/ControlOption/WindowCredit").Show();
		
		GetNode<WorldEnvironment>("../WorldEnvironment")
		.Environment.DofBlurNearAmount = (float)0.15;
		
		StartButton.Visible = false;
		OptionsButton.Visible = false;
		CreditButton.Visible = false;
		ExitButton.Visible = false;
	}
	
	// Função para fechar o jogo quando o botão 'Exit' é pressionado
	private void _on_Exit_button_up()
	{
		GetTree().Quit();
	}
	
	// Função para reiniciar o jogo caso o botao de reset seja apertado
	private void _on_RestartButton_button_up()
	{
		GetTree().ReloadCurrentScene();
		//EmitSignal("Restart");
		//GetNode<Node2D>("..").QueueFree();
	}
	
	private void ScoreForLabel()
	{
		Point += 1;
		Score.Text = Convert.ToString(Point);
		GetNode<AudioStreamPlayer>("point").Play();
	}
	
	// Função para salvar a melhor pontuação caso seja maior que a anterior
	private void SaveScore()
	{
		if(!Dir.DirExists("user://saves/"))
		{
			Dir.MakeDirRecursive("user://saves/");
		}
		
		Config.Load(SaveBestScore);
		if(Convert.ToInt32(Config.GetValue("Save Best Score", "BestScores", 0)) < Point)
		{
			Config.SetValue("Save Best Score", "BestScores", Point);
			Config.Save(SaveBestScore);
			NewScore = true;
		}
		
	}
	
	// Função para carregar a melhor pontuação
	private void LoadScore()
	{
		Config.Load(SaveBestScore);
		
		BestScore.Text = 
		$"Best Score: {Convert.ToString(Config.GetValue("Save Best Score", "BestScores", 0))}";
		GetFinalBestScore = Convert.ToDouble(Config.GetValue("Save Best Score", "BestScores", 0));
	}
	
	/* Invocado quando o timer de 1 segundo termina (timer que é invocado 
	quando o player morre)*/
	private void _on_TimerGameOver_timeout()
	{
		SaveScore();
		LoadScore();
		TextureGameOver.Visible = true;
		WhiteColorRect.Visible = false;
		TextureGameOver.GetNode<AnimationPlayer>("AnimGameOver")
		.Play("jumpgameover");
		GetNode<AudioStreamPlayer>("swooshing").Play();
	}
	
	// Invocado quando a animação do jump do gameover termina
	private void _on_AnimGameOver_animation_finished(String anim_name)
	{
		PanelRect.Visible = true;
		PanelRect.GetNode<AnimationPlayer>("AnimPanel").Play("panelAnim");
	}
	
	// Invocado quando a animação do painel termina
	private void _on_AnimPanel_animation_finished(String anim_name)
	{
		RestartButton.GetNode<AnimationPlayer>("AnimRestartButton")
		.Play("animRestartButton");
		RestartButton.Visible = true;
		
		TweenFinalScore.InterpolateMethod(this, "CountForFinalScore", 0, Point, 1, 
		Tween.TransitionType.Linear, Tween.EaseType.InOut);
		
		TweenFinalBestScore.InterpolateMethod(this, "CountForFinalBestScore", 0, 
		GetFinalBestScore, 1, Tween.TransitionType.Linear, Tween.EaseType.InOut);
		
		TweenFinalScore.Start();
		TweenFinalBestScore.Start();
		
		if(NewScore == true)
		{
			LabelNewScore.Visible = true;
			LabelNewScore.GetNode<AnimationPlayer>("AnimNewScore").Play("AnimNew");
		}
	}
	
	private void CountForFinalScore(int GiveFinalScore)
	{
		FinalScore.Text = GiveFinalScore.ToString();
	}
	
	private void CountForFinalBestScore(int GiveFinalBestScore)
	{
		FinalBestScore.Text = GiveFinalBestScore.ToString();
	}
	
	private void _on_Pause_toggled(bool button_pressed)
	{
		if (button_pressed)
		{
			// Pausa o jogo
			GetTree().Paused = true;
			EmitSignal("SignalPause");

		}
		else
		{
			// Despausa o jogo
			GetTree().Paused = false;
			EmitSignal("SignalPause");
		}
	}
}

