﻿//=============================================================================
// The offical line is you're unearthing ancient knowledge,
// the reality is you just like playing in the dirt.
//=============================================================================

public class Archaeologist : cmk.NMS.Script.ModClass
{
	protected override void Execute()
	{
		// Starting ship:
		// - dropship, x2 side boxes
		
		// Starting Multitool
		// - vanilla (random)
		
		// Inventory
		// - vanilla
		
		// - auto-mark buried items and ruins
		var Scan_Auto = Script<Scan_Auto>();
		Scan_Auto.EnableBones        = true;
		Scan_Auto.EnableGrave        = true;
		Scan_Auto.EnableTreasureRuin = true;
	}
}

//=============================================================================
