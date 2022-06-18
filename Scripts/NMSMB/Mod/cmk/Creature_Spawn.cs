﻿//=============================================================================
// Adjust max creature size and spawn counts.
//=============================================================================

public class Creature_Spawn : cmk.NMS.Script.ModClass
{
	protected override void Execute()
	{
		Try(() => GcCreatureGlobals());
		Try(() => GcCreatureDataTable());
	}

	//...........................................................

	protected void GcCreatureGlobals()
	{
		var mbin = ExtractMbin<GcCreatureGlobals>(
			"GCCREATUREGLOBALS.MBIN"
		);
		mbin.MaxCreatureSize *= 2.0f;  // 8
	}

	//...........................................................

	protected void GcCreatureDataTable()
	{
		var mbin = ExtractMbin<GcCreatureDataTable>(
			"METADATA/SIMULATION/ECOSYSTEM/CREATUREDATATABLE.MBIN"
		);		
		mbin.Table.ForEach(CREATURE => {
			CREATURE.MaxScale *= 2.0f;
			foreach( var data in CREATURE.Data ) {
				if( data is GcCreatureSwarmData swarm ) {
					swarm.MaxCount = (int)System.Math.Round(swarm.MaxCount * 0.5) + 1;
				}
			}
		});
	}
}

//=============================================================================
