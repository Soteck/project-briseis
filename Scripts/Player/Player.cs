using Godot;

namespace ProjectBriseis.Scripts.Player;

//This class represents a connected player to the server
public partial class Player : Node3D {
    public override void _EnterTree() {
        SetMultiplayerAuthority(Name.ToString().ToInt());
    }

}