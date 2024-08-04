using Godot;


namespace ProjectBriseis.addons.custom_gizmos;

[Tool]
public partial class CustomGizmoPlugin : EditorPlugin {
    
    readonly PlayerCustomGizmo _playerCustomGizmo = new PlayerCustomGizmo();
    
    public override void _EnterTree() {
        AddNode3DGizmoPlugin(_playerCustomGizmo);
    }

    public override void _ExitTree() {
        RemoveNode3DGizmoPlugin(_playerCustomGizmo);
    }
}