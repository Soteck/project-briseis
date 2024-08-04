using Godot;
using MultiplayerAutoLoad = ProjectBriseis.Scripts.AutoLoad.Multiplayer.MultiplayerAutoLoad;

namespace ProjectBriseis.Scripts.Player {
    public partial class Character : CharacterBody3D {
        [Export]
        private Node _onlinePlayerController;

        [Export]
        private Node _offlinePlayerController;

        //
        // private Guid _characterGuid;
        //
        // private Guid _playerOwnerGuid;

        public override void _Ready() {
            ClientStatusChanged(MultiplayerAutoLoad.instance.clientConnectionStatus);
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