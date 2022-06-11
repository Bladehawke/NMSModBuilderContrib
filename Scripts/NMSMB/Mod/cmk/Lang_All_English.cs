﻿//=============================================================================
// Copy the english text to all other language mbins.
// You want to make sure tyhis script runs as one of the last ones, in-case
// prior scripts add new language prefixes and mbins.
//=============================================================================

[NMSScriptDisabled]  // intended as example for lang manip
public class Lang_All_English : cmk.NMS.Script.ModClass
{
	protected override void Execute()
	{
		// Simple();
		Mbin();
	}	

	//...........................................................
	
	// 2 min 17 sec execute
	protected void Simple()
	{
		var english = Game.Languages.Get(LangId.English);
		foreach( var id in LangId.List ) {
			if( id == LangId.English ) continue;
			foreach( var key in english.List ) {
				SetLanguageText(id, key.Id, key.Text);
			}
		}
	}
	
	//...........................................................
	
	// 1 min 45 sec execute - not indexed
	//       30 sec execute -     indexed
	protected void Mbin()
	{
		var prefixes = GetLanguageGamePrefixes();
		
		foreach( var prefix in prefixes ) {
			// since we aren't changing english we can get a non-cached version
			// of the mbin directly from the PCBANKS collection.
			var english = Game.PCBANKS.ExtractMbin<TkLocalisationTable>(
				$"LANGUAGE/{prefix}_{LangId.English.Name}.MBIN"
			);
			
			foreach( var id in LangId.List ) {
				if( id == LangId.English ) continue;
				
				// get the same mbin for the other languages.
				// since we want to mod these versions we use the script
				// ExtractMbin so they get cached.
				var path = $"LANGUAGE/{prefix}_{id.Name}.MBIN";
				var lang = ExtractMbin<TkLocalisationTable>(path);
				
				// to make as quick as possible we'll index the lang table
				// entries so we can bsearch lookups.
				// we could just sort lang.Table but then diffs wouldn't align.
				var index = new List<TkLocalisationEntry>(lang.Table.Count);
				for( var i = 0;  i < lang.Table.Count;  ++i ) {
					index.Add(lang.Table[i]);
				}
				index.Sort((LHS, RHS) => string.Compare(LHS.Id, RHS.Id));
				
				foreach( var english_entry in english.Table ) {
					var lang_entry = index.Bsearch(english_entry.Id,
						(ENTRY, KEY) => string.Compare(ENTRY.Id, KEY)
					);
					if( lang_entry == null ) {
						// Value is wrapped string, click on Id and look below
						// the intellisense will show the actual type - NMSString0x20A.
						Log.AddWarning($"{path} missing {english_entry.Id.Value}");
						continue;
					}
					// since each language mbin has fields for each language,
					// but only populates the field for the given mbin language,
					// we need a way to get that specific field.
					// the Ref extension method does that, for a given entry
					// it returns a ref to the field for the specified language.
					lang_entry.Ref(id) = english_entry.Ref(LangId.English);
				}
			}
		}		
	}	
}

//=============================================================================
