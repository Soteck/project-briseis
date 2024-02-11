using Godot;
using ProjectBriseis.Scripts.AutoLoad;
using MultiplayerAutoLoad = ProjectBriseis.Scripts.AutoLoad.Multiplayer.MultiplayerAutoLoad;

namespace ProjectBriseis.Scripts.Player {
    public partial class Player : CharacterBody3D {

        [Export]
        private Node _onlinePlayerController;
        
        [Export]
        private Node _offlinePlayerController;
        
        public override void _Ready() {
            ClientStatusChanged(MultiplayerAutoLoad.instance.clientConnectionStatus);
            MultiplayerAutoLoad.instance.OnClientStatusChange += ClientStatusChanged;
        }

        private void ClientStatusChanged(bool status) {
            if (status) {
                //connected
                _offlinePlayerController.ProcessMode = ProcessModeEnum.Disabled;
                _onlinePlayerController.ProcessMode = ProcessModeEnum.Inherit;
            } else {
                //disconnected
                _onlinePlayerController.ProcessMode = ProcessModeEnum.Disabled;
                _offlinePlayerController.ProcessMode = ProcessModeEnum.Inherit;
            }
        }
    }
}