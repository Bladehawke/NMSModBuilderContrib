﻿//=============================================================================
// Author: Jackty89
//=============================================================================

public class QuickSilverRewards : cmk.NMS.Script.ModClass
{
	public static int MBRewardChance 	= 15;
	public static int FleetRewardChance = 30;
	
	Tuple<string, int, int>[] MissionBoardRewards = new Tuple<string, int, int>[] {
		new ("R_MB_LOW",  	   100, 150),
		new ("R_MB_MED",  	   150, 200),
		new ("R_MB_HIGH", 	   200, 250),
		new ("R_MB_MEGA", 	   250, 300),
		new ("R_NEXUS_MED",    250, 300),
		new ("R_NEXUS_MED",    250, 300),
		new ("R_NEXUS_MED_C",  150, 300),
		new ("R_NEXUS_MEGA_C", 200, 450)
	};
		
	Tuple<string, int, int>[] FleetRewards = new Tuple<string, int, int>[] {
		new ("R_DIP_EASY_0",  100, 150),
		new ("R_DIP_EASY_1",  150, 200),
		new ("R_DIP_EASY_2",  200, 250),
		new ("R_DIP_EASY_3",  250, 300),
		
		new ("R_COM_EASY_0",  100, 150),
		new ("R_COM_EASY_1",  150, 200),
		new ("R_COM_EASY_2",  200, 250),
		
		new ("R_EXP_EASY_0",  100, 150),
		new ("R_EXP_EASY_1",  150, 200),
		new ("R_EXP_EASY_2",  200, 250),
		new ("R_EXP_EASY_3",  250, 300),
		
		new ("R_MINE_EASY_0", 100, 150),
		new ("R_MINE_EASY_1", 150, 200),
		new ("R_MINE_EASY_2", 200, 250),
		new ("R_MINE_EASY_3", 250, 300)
	};

	//...........................................................

	protected override void Execute()
	{
		AddQsRewards();
		AddQsFleetRewards();
	}


	//...........................................................
	protected void AddQsRewards()
	{
		var mbin = ExtractMbin<GcRewardTable>("METADATA/REALITY/TABLES/REWARDTABLE.MBIN");
			
		foreach( var rewardData in MissionBoardRewards ) 
		{
			string rewardID = rewardData.Item1;
			int minValue    = rewardData.Item2;			
			int maxValue    = rewardData.Item3;
			
			mbin.MissionBoardTable.Find(ENTRY => ENTRY.Id == rewardID).List.List.Add(
				RewardTableItem.Specials(minValue, maxValue, MBRewardChance, null)	
			);
				
		}
	}
	
	protected void AddQsFleetRewards()
	{
		var mbin = ExtractMbin<GcExpeditionRewardTable>("METADATA/REALITY/TABLES/EXPEDITIONREWARDTABLE.MBIN");
			
		foreach( var rewardData in FleetRewards ) 
		{
			string rewardID = rewardData.Item1;
			int minValue    = rewardData.Item2;			
			int maxValue    = rewardData.Item3;
			
			mbin.Table.Find(ENTRY => ENTRY.Id == rewardID).List.List.Add(
				RewardTableItem.Specials(minValue, maxValue, FleetRewardChance, null)	
			);
				
		}
	}
}

//=============================================================================
