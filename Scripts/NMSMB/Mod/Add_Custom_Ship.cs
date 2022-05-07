﻿//=============================================================================

public class Add_Custom_Ship : cmk.NMS.Script.ModClass
{
	protected override void Execute()
	{
		GcRewardTable();
	}

	//...........................................................

	protected void GcRewardTable()
	{
		var mbin = ExtractMbin<GcRewardTable>(
			"METADATA/REALITY/TABLES/REWARDTABLE.MBIN"
		);
		var ship = RewardSpecificShip.Create(
			"R_CMK_SHIP_001", "My Custom Ship",
			ShipClassEnum.Alien, InventoryClassEnum.S,
			"MODELS/COMMON/SPACECRAFT/S-CLASS/BIOPARTS/BIOSHIP_PROC.SCENE.MBIN",
			unchecked((long)0xf437579a7a76d819), 24,
			new() {
				Inventory.Technology("SHIPJUMP_ALIEN", 200, 200, 0),
				Inventory.Technology("SHIPGUN_ALIEN",  100, 100, 0),
				Inventory.Technology("SHIELD_ALIEN",   200, 200, 0),
				Inventory.Technology("SHIPLAS_ALIEN",  100, 100, 0),
				Inventory.Technology("LAUNCHER_ALIEN", 200, 200, 0),
				Inventory.Technology("WARP_ALIEN",     120, 120, 0)
			},
			new() {
				new(){ BaseStatID = "SHIP_DAMAGE",     Value = 1 },
				new(){ BaseStatID = "SHIP_SHIELD",     Value = 1 },
				new(){ BaseStatID = "SHIP_HYPERDRIVE", Value = 1 },
				new(){ BaseStatID = "ALIEN_SHIP",      Value = 1 }
			}
		);
		mbin.GenericTable.Add(ship);		
	}
}

//=============================================================================
