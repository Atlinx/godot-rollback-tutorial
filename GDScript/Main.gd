extends Node2D

onready var connection_panel = $CanvasLayer/ConnectionPanel
onready var host_field = $CanvasLayer/ConnectionPanel/GridContainer/HostField
onready var port_field = $CanvasLayer/ConnectionPanel/GridContainer/PortField
onready var message_label = $CanvasLayer/MessageLabel
onready var sync_lost_label = $CanvasLayer/SyncLostLabel


func _ready() -> void:
	get_tree().connect("network_peer_connected", self, "_on_network_peer_connected")
	get_tree().connect("network_peer_disconnected", self, "_on_network_peer_disconnected")
	get_tree().connect("server_disconnected", self, "_on_server_disconnected")
	SyncManager.connect("sync_started", self, "_on_SyncManager_sync_started")
	SyncManager.connect("sync_stopped", self, "_on_SyncManager_sync_stopped")
	SyncManager.connect("sync_lost", self, "_on_SyncManager_sync_lost")
	SyncManager.connect("sync_regained", self, "_on_SyncManager_sync_regained")
	SyncManager.connect("sync_error", self, "_on_SyncManager_sync_error")
	sync_lost_label.visible = false


func _on_ServerButton_pressed():
	var peer = NetworkedMultiplayerENet.new()
	peer.create_server(int(port_field.text), 1)
	get_tree().network_peer = peer
	message_label.text = "Listening..."
	connection_panel.visible = false


func _on_ClientButton_pressed():
	var peer = NetworkedMultiplayerENet.new()
	peer.create_client(host_field.text, int(port_field.text))
	get_tree().network_peer = peer
	message_label.text = "Connecting..."
	connection_panel.visible = false


func _on_network_peer_connected(peer_id: int):
	message_label.text = "Connected with id: %s" % [peer_id]
	SyncManager.add_peer(peer_id)
	
	$ServerPlayer.set_network_master(1)
	if get_tree().is_network_server():
		$ClientPlayer.set_network_master(peer_id)
	else:
		$ClientPlayer.set_network_master(get_tree().get_network_unique_id())
	
	if get_tree().is_network_server():
		message_label.text = "Starting..."
		yield(get_tree().create_timer(2.0), "timeout")
		SyncManager.start()


func _on_network_peer_disconnected(peer_id: int):
	message_label.text = "Disconnected with id: %s" % [peer_id]
	SyncManager.remove_peer(peer_id)


func _on_server_disconnected():
	_on_network_peer_disconnected(1)


func _on_ResetButton_pressed():
	SyncManager.stop()
	SyncManager.clear_peers()
	var peer = get_tree().network_peer
	if peer:
		peer.close_connection()
	get_tree().reload_current_scene()


func _on_SyncManager_sync_started():
	message_label.text = "Started!"
	
	
func _on_SyncManager_sync_stopped():
	pass


func _on_SyncManager_sync_lost():
	sync_lost_label.visible = true


func _on_SyncManager_sync_regained():
	sync_lost_label.visible = false


func _on_SyncManager_sync_error(msg: String):
	message_label.text = "Fatal sync error: " + msg
	sync_lost_label.visible = false
	
	var peer = get_tree().network_peer
	if peer:
		peer.close_connection()
	SyncManager.clear_peers()
