extends Node2D




func _ready() -> void:
	#just need to call once
	MobileAds.initialize()
	MobileAds.AdMobSettings.config.banner.position
	MobileAds.AdMobSettings.config.banner.respect_safe_area
	MobileAds.config.banner.size = "ADAPTIVE"
	MobileAds.load_banner()
	MobileAds.load_interstitial()
	MobileAds.show_banner()
	
	
func _process(_delta):
	MobileAds.show_banner()


func _on_RestartButton_button_up():
	MobileAds.show_interstitial()
