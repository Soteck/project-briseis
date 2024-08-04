#if TOOLS
using Godot;

namespace ProjectBriseis.addons.custom_gizmos;

public partial class PlayerCustomGizmo : EditorNode3DGizmoPlugin {
    private float _size = 0.25f;

    public PlayerCustomGizmo() {
        CreateMaterial("main", Colors.Red);
        CreateHandleMaterial("handles");
    }

    public override string _GetGizmoName() {
        return "Player GIZMO";
    }

    public override bool _HasGizmo(Node3D forNode3D) {
        Variant script = forNode3D.GetScript();
        if (script.Obj != null) {
            return (script.Obj as CSharpScript)?.ResourcePath == "res://Scripts/Player/Player.cs";
        }

        return base._HasGizmo(forNode3D);
    }

    public override void _Redraw(EditorNode3DGizmo gizmo) {
        Vector3[] lines = new Vector3[2];
        lines[0] = new Vector3(0, -_size, 0);
        lines[1] = new Vector3(0, _size, 0);

        Vector3[] lines2 = new Vector3[2];
        lines2[0] = new Vector3(_size, 0, 0);
        lines2[1] = new Vector3(-_size, 0, 0);

        Vector3[] lines3 = new Vector3[2];
        lines3[0] = new Vector3(0, 0, _size);
        lines3[1] = new Vector3(0, 0, -_size);

        gizmo.AddLines(lines, GetMaterial("main", gizmo), false);
        gizmo.AddLines(lines2, GetMaterial("main", gizmo), false);
        gizmo.AddLines(lines3, GetMaterial("main", gizmo), false);
    }
}
#endif