using Godot;
using System;
using static Godot.GD;

public class Obstacules : Node2D
{
	// Sinais que serão emitidos durante o jogo
	[Signal]
	public delegate void GameOver();  // Emitido quando o jogador colide com um obstáculo
	[Signal]
	public delegate void Scored();   // Emitido quando o jogador passa entre os obstáculos e faz pontos
	
	float TimerSpawn = 0f;  // Timer para controlar o tempo entre o spawn dos obstáculos
	PackedScene Obstacule;  // Variável para carregar a cena do obstáculo
	Node2D SpawnObstacule;  // Variável para armazenar a instância do obstáculo que será gerado
	
	// Função chamada quando o objeto é renderizado na tela (inicialização)
	public override void _Ready()
	{
		// Carrega a cena que contém o obstáculo a partir de um arquivo de recurso
		Obstacule = (PackedScene)ResourceLoader.Load("res://scenes/Obstacule.tscn");
		SetProcess(false);  // Desativa o processamento enquanto o jogo não estiver rodando
	}
	
	// Função chamada a cada frame, onde a lógica de atualização do jogo ocorre
	public override void _Process(float delta)
	{
		TimerSpawn = TimerSpawn + delta;  // Incrementa o timer com o delta (tempo entre frames)
		
		// Quando o timer ultrapassar o valor definido, cria um novo obstáculo
		if(TimerSpawn > 1.5) // 1.3 ficaria mais dinâmico; antigo era 1.7
		{
			TimerSpawn = 0f;  // Reseta o timer
			Spawn();  // Chama a função para spawnar o obstáculo
		}
	}
	
	// Função para gerar obstáculos na tela
	private void Spawn()
	{
		// Cria uma instância do obstáculo a partir da cena carregada
		SpawnObstacule = (Node2D)Obstacule.Instance();
		
		// Conecta os sinais de colisão e pontuação aos métodos correspondentes
		SpawnObstacule.Connect("BodyCrash", this, "OnBodyCrash");
		SpawnObstacule.Connect("AreaScore", this, "OnAreaScore");
		
		// Define a posição do obstáculo aleatoriamente
		var Pos = SpawnObstacule.Position;
		
		Randomize();  // Randomiza o gerador de números aleatórios
		Pos.x = GetViewportRect().Size.x + 40;  // Define a posição inicial fora da tela, à direita
		Pos.y = (float)RandRange(68, 448);  // Define uma posição aleatória para a coordenada Y entre 68 e 448
		
		SpawnObstacule.Position = Pos;  // Atribui a posição ao obstáculo
		AddChild(SpawnObstacule);  // Adiciona o obstáculo à cena
		SpawnObstacule.AddToGroup("Obstacules");  // Adiciona o obstáculo a um grupo para facilitar o controle
	}
	
	// Função chamada quando o jogador colide com o obstáculo
	private void OnBodyCrash()
	{
		// Emite o sinal de GameOver para notificar outros sistemas
		EmitSignal("GameOver");
		
		// Chama todos os obstáculos para finalizar
		GetTree().CallGroup("Obstacules", "end");
		
		// Desativa o processamento dos obstáculos
		SetProcess(false);
	}
	
	// Função chamada quando o jogador passa entre os obstáculos e marca pontos
	private void OnAreaScore()
	{
		// Emite o sinal de Scored para notificar que o jogador fez pontos
		EmitSignal("Scored");
	}
}
