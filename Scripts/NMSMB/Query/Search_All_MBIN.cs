﻿//=============================================================================
// Iterate through all mbin's in all game pak files.
// Create an ebin text string for each.
// Search the ebin text for a specific string.
// Log all mbin's with the matching string.
//=============================================================================

public class Search_All_MBIN: cmk.NMS.Script.QueryClass
{
	protected override void Execute()
	{
		Log.AddInformation($"Iterating ...");

		// ~13  sec to just extract and loop through all 70,422 (3.90) mbin's.
		// ~3.5 min to get ebin for each mbin and search ebin for a string.
		var count = 0;
		Game.PCBANKS.ForEachMbin(( MBIN, LOG, CANCEL ) => {
			Interlocked.Increment(ref count);
			var ebin   = MBIN.RawEBIN();
			var index  = ebin.IndexOf("MESSAGEMODULE"); //, StringComparison.OrdinalIgnoreCase);
			if( index >= 0 ) LOG.AddInformation($"{MBIN.Path.Full}[{ebin.Length}] @ {index}.");
		}, Log, Cancel);

		Log.AddSuccess($"Finished: examined {count} mbin's");
	}
}

//=============================================================================
