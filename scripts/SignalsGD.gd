extends Node2D

#onready var MessageLabel = $Label
#onready var NotificationTimer = $Timer


func _ready():
	pass
#	NotificationTimer.start()
#
#
#func _on_NotificationTimer_timeout():
#	MessageLabel.text = "Notification: This is a scheduled message!"
#	schedule_notification("This is a scheduled notification!", 5.0)
#
#func schedule_notification(message, delay):
#	OS.android_schedule_notification(message, delay)
#
#func android_schedule_notification(message, delay):
#	if Engine.has_feature("android_notifications"):
#		var id = 1
#		var title = "Notification"
#		var channel_id = "default"
#		var channel_name = "Default"
#		var channel_description = "This is the default notification channel"
#		var importance = 3
#		var sound = ""
#		var vibrate = [0, 250, 250, 250]
#		var lights = [0x00, 0xFF, 0xFF, 0x00, 0xFF, 0x00, 0x00]
#		var led_on_ms = 500
#		var led_off_ms = 500
#		var colorized = false
#		var show_when = false
#		var ticker = message
#		var small_icon = "path/to/small/icon"
#		var large_icon = "path/to/large/icon"
#		var group = ""
#		var group_summary = ""
#		var persistent = false
#		var auto_cancel = true
#		var auto_cancel_time = 0
#		var local_only = false
#		var allow_while_idle = true
#		var extras = {}
#		var notification = {"id": id, "title": title, "channel_id": channel_id, "channel_name": channel_name, "channel_description": channel_description, "importance": importance, "sound": sound, "vibrate": vibrate, "lights": lights, "led_on_ms": led_on_ms, "led_off_ms": led_off_ms, "colorized": colorized, "show_when": show_when, "ticker": ticker, "small_icon": small_icon, "large_icon": large_icon, "group": group, "group_summary": group_summary, "persistent": persistent, "auto_cancel": auto_cancel, "auto_cancel_time": auto_cancel_time, "local_only": local_only, "allow_while_idle": allow_while_idle, "extras": extras}
		
