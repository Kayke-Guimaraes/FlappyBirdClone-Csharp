extends Node2D

var GameM = preload("res://scenes/World.tscn")
var GameMain = GameM.instance()

func _ready():
# warning-ignore:return_value_discarded
	$World/Signals.connect("Restart", self, "RestartGame")
	
func _process(delta):
	if not $World/Signals.connect("Restart", self, "RestartGame"):
		$World/Signals.connect("Restart", self, "RestartGame")
	
func RestartGame():
	$".".add_child(GameMain)
	


