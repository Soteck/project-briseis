using Godot;
using ProjectBriseis.Scripts.Manager;

namespace ProjectBriseis.Scripts.Player;

//This class represents a connected player to the server
public partial class Player : Node3D {

    public Team team { get; private set; } 
    
    public override void _EnterTree() {
        SetMultiplayerAuthority(Name.ToString().ToInt());
    }

}