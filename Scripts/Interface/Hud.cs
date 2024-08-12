using System.Globalization;
using Godot;
using ProjectBriseis.Scripts.Manager;

namespace ProjectBriseis.Scripts.Interface;

public partial class Hud : Control {

    
	[ExportGroup("External dependencies")]
	[Export]
	public MatchManager MatchManager;

    
	[ExportGroup("Internal dependencies")]
	[Export]
	public Label MapTimeLabel;
	
	
	public override void _Process(double delta) {
		MapTimeLabel.Text = MatchManager?.MapTime.ToString(CultureInfo.CurrentCulture);
	}
}