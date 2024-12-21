using Godot; 
using System;

public class Player : KinematicBody2D
{
	// Vetor de velocidade do personagem
	private Vector2 Velocity = Vector2.Zero;

	// Constantes para gravidade e altura do pulo
	private const float GRAVITY = 800f;
	private const float JUMP_HEIGHT = -370f;

	// Variáveis para controle do jogo e estados do personagem
	public bool Start = false; // Indica se o jogo já começou
	public bool Death = false; // Indica se o personagem está "morto"
	public bool Jumper; // Indica se o jogador pulou
	public bool Click; // Indica se o jogador clicou
	public bool Pause; // Indica se o jogo está pausado
	public float CountPause; // Tempo de cooldown para inputs após despausar
	public bool ForPause; // Flag que ativa o cooldown para pause

	// Método chamado quando o objeto é renderizado na tela
	public override void _Ready()
	{
		// Inicia a animação inicial do personagem
		GetNode<AnimationPlayer>("AnimationPlayer").Play("FlyStart");
		ForPause = false; // Inicializa o controle de pausa como falso
		Pause = false; // Inicializa a pausa como falsa
		CountPause = 0f; // Reseta o contador de cooldown
	}

	// Método chamado a cada frame, responsável pela lógica principal
	public override void _PhysicsProcess(float delta)
	{
		/* Resolve o bug de pulo involuntário ao despausar o jogo.
		Bloqueia os inputs por alguns milissegundos após o despausar. */
		if (ForPause == true && CountPause < 0.2)
		{
			Pause = true; // Ativa a pausa temporária
			CountPause += delta; // Incrementa o tempo do cooldown

			// Finaliza o cooldown após 0.2 segundos
			if (CountPause >= 0.2)
			{
				Pause = false; // Desativa a pausa
				CountPause = 0f; // Reseta o contador
				ForPause = false; // Reseta a flag de pausa
			}
		}

		// Captura os comandos de entrada do jogador (teclado ou mouse)
		Jumper = Input.IsActionJustPressed("ui_accept");
		Click = Input.IsActionJustPressed("ui_click");

		// Realiza o pulo se as condições permitirem
		if ((Jumper || Click) && Start && !Death && Pause == false)
		{
			Jump();
		}

		// Aplica gravidade e rotaciona o personagem se o jogo começou
		if (Start == true)
		{
			GetNode<AnimationPlayer>("AnimationPlayer").Stop(); // Para a animação inicial
			Velocity.y += GRAVITY * delta; // Aplica gravidade ao personagem
			RotateBird(); // Rotaciona o personagem
		}

		// Limita a velocidade de voo e queda para evitar exageros
		Velocity.y = Mathf.Clamp(Velocity.y, -1000, 1000);

		// Move o personagem com base na física e na velocidade
		var collision = MoveAndSlide(Velocity);
	}

	/* Função responsável por executar a lógica de pulo,
	incluindo o movimento, a animação e o som */
	private void Jump()
	{
		Velocity.y = JUMP_HEIGHT; // Define a velocidade do pulo
		GetNode<AnimatedSprite>("Anims").Play("fly"); // Inicia a animação de voo
		GetNode<AnimatedSprite>("Anims").Frame = 0; // Reseta o quadro da animação
		GetNode<AudioStreamPlayer>("Sounds/wing").Play(); // Toca o som de pulo
	}

	// Função que ajusta a rotação do personagem durante o voo ou queda
	private void RotateBird()
	{
		// Rotaciona para baixo se estiver caindo e o ângulo for menor que 90 graus
		if (!IsOnWall() && Velocity.y > 0 && Mathf.Rad2Deg(Rotation) < 90)
		{
			Rotation += 2 * Mathf.Deg2Rad(1);
		}
		// Rotaciona para cima se estiver subindo e o ângulo for maior que -30 graus
		else if (!IsOnWall() && Velocity.y < 0 && Mathf.Rad2Deg(Rotation) > -30)
		{
			Rotation -= 2 * Mathf.Deg2Rad(7);
		}
	}

	// Método chamado quando o sinal de pausa é ativado
	private void _on_Signals_SignalPause()
	{
		ForPause = true; // Ativa a flag de cooldown para pausa
	}
}
