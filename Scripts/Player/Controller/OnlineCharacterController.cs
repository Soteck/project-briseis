using Godot;

namespace ProjectBriseis.Scripts.Player.Controller;

public partial class OnlineCharacterController : OfflinePlayerController {
    public override void _EnterTree() {
        SetMultiplayerAuthority(int.Parse(Name));
    }
}
