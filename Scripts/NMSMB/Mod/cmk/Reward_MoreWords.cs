﻿//=============================================================================
// Learn more words when interacting with NPC's, Knowledge stones, ...
//=============================================================================

public class Reward_MoreWords : cmk.NMS.Script.ModClass
{
	public static uint  BonusWords      = 3;
	public static uint  BonusAtlasWords = 6;
	
	public static float BonusWordChance      = 100.0f;
	public static float BonusAtlasWordChance =  50.0f;

	//...........................................................
	
	protected override void Execute()
	{
		var mbin = ExtractMbin<GcRewardTable>(
			"METADATA/REALITY/TABLES/REWARDTABLE.MBIN"
		);
		
		var GcRewardTable_t             = typeof(GcRewardTable);
		var GcGenericRewardTableEntry_t = typeof(List<GcGenericRewardTableEntry>);
		
		// GcRewardTable has a bunch of List<GcGenericRewardTableEntry> fields, instead of a list of them.
		// Use reflection to iterate through all the List<GcGenericRewardTableEntry> fields.
		foreach( var field in GcRewardTable_t.GetFields() ) {
			if( field.FieldType == GcGenericRewardTableEntry_t ) {
				GcGenericRewardTableEntryList(field.GetValue(mbin) as List<GcGenericRewardTableEntry>);
			}
		}
	}

	//...........................................................
	
	// entry.Id:
	// WORD                   - Stone
	// (race)_WORD            - Reward for helping NPC, single interaction
	// TEACHWORD_(race)       - Monolith | Plaque | Ruins
	// (race)_WORD_(category) - NPC, pick a category, repeat interaction 
	protected void GcGenericRewardTableEntryList( List<GcGenericRewardTableEntry> LIST )
	{
		foreach( var entry in LIST ) {
			var source_race     = RaceEnum.Diplomats;  // WORD uses None, none use Diplomats
			var source_category = WordEnum.MISC;
			
			foreach( var reward in entry.List.List ) {
				if( reward.Reward is GcRewardTeachWord word ) {
					entry.List.RewardChoice = RewardChoiceEnum.GiveAll;
					source_race     = word.Race.AlienRace;
					source_category = word.Category.gcwordcategorytableEnum;
					break;
				}
			}
			if( source_race == RaceEnum.Diplomats ) continue;  // no learn word reward found
			
			// add BonusWords more words, same race & category, 100% chance
			for( var i = 0; i < BonusWords; ++i ) {
				var word = RewardTableItem.TeachWord(source_race, source_category, BonusWordChance);
				entry.List.List.Add(word);
			}
			
			// add BonusAtlasWords more words, Atlas, misc category, BonusAtlasWordChance% chance
			for( var i = 0; i < BonusAtlasWords; ++i ) {
				var word = RewardTableItem.TeachWord(RaceEnum.Atlas, WordEnum.MISC, BonusAtlasWordChance);			
				entry.List.List.Add(word);
			}
		}
	}
}

//=============================================================================
