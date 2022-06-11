﻿//=============================================================================
// Adjust timings for notifications.
// Hide lower-right notification UI.
//=============================================================================

public class Notification : cmk.NMS.Script.ModClass
{
	protected override void Execute()
	{
		Try(() => GcBootLogoData());
		Try(() => GcPunctuationDelayTable());
		Try(() => GcUIGlobals());
	//	Try(() => GcNGuiLayerData());
		Try(() => GcNGuiLayerData_LowerNotify());
		Try(() => GcLeveledStatTable());
		Try(() => HudWeapons());
	}

	//...........................................................

	protected void GcBootLogoData()
	{
		var mbin = ExtractMbin<GcBootLogoData>(
			"METADATA/UI/BOOTLOGOPC.MBIN"
		);
		// disable boot logo's
		for( var i = 0; i < mbin.Textures.Length;    ++i ) mbin.Textures   [i].Value = "";
		for( var i = 0; i < mbin.DisplayTime.Length; ++i ) mbin.DisplayTime[i]       = 0.0f;
	}

	//...........................................................

	protected void GcPunctuationDelayTable()
	{
		var mbin = ExtractMbin<GcPunctuationDelayTable>(
			"METADATA/UI/SPECIALTEXTPUNCTUATIONDELAYDATA.MBIN"
		);
		// disable delay when displaying text e.g. npc dialogs
		foreach( var data in mbin.PunctuationDelays ) {
			data.DefaultDelay = 0;
			foreach( var punc in data.PunctuationList ) {
				punc.Delay = 0;
			}
		}
	}

	//...........................................................

	protected void GcUIGlobals()
	{
		var mbin = ExtractMbin<GcUIGlobals>(
			"GCUIGLOBALS.GLOBAL.MBIN"
		);

		var time = 0.5f;
		
		// always show distance to target, never time - broken ?
		mbin.HUDMarkerDistanceOrTimeDistance = 10000000;
		
		mbin.DiscoveryHelperTimings.DiscoverPlanetTotalTime   = time;  // 10
		mbin.DiscoveryHelperTimings.DiscoverPlanetMessageWait = time;  //  1
		mbin.DiscoveryHelperTimings.DiscoverPlanetMessageTime = time;  //  7

		mbin.MilestoneStingDisplayTime  = time;  // 6
		mbin.StageStingDisplayTime      = time;  // 6
		mbin.HUDDisplayTime             = time;  // 6
		mbin.StatsMessageDelayTime      = time;  // 12
		mbin.NotificationMinVisibleTime = time;  // 3
		mbin.MessageNotificationTime    = time;  // 12
		mbin.MessageTimeQuick           = time;  // 6
		
		mbin.IntroTiming.FadeInFlashTime = 0;  // 12
		
		mbin.DescriptionTextDelay = 0;  // 0.4
		
		mbin.ReportBaseFlashTime      = 0;  // 0.3
		mbin.ReportBaseFlashIntensity = 0;  // 1.9

		// don't seem to be used
		mbin.DistanceUnitM   =  "m";    // "u"
		mbin.DistanceUnitKM  = "km";    // "ks"
		mbin.DistanceUnitMpS =  "m/s";  // "u/s"
	}

	//...........................................................

	protected void GcNGuiLayerData()
	{
		var paths = new [] {
			"UI/COMPONENTS/OPTIONS/OPTIONSMULTINUMBERBOX.MBIN",
			"UI/COMPONENTS/OPTIONS/OPTIONSNUMBERBUTTONS.MBIN",
			"UI/COMPONENTS/INTERACTWINDOW.MBIN",
			"UI/COMPONENTS/KEYBUTTONS.MBIN",
			"UI/COMPONENTS/KEYBUTTONSCOMPONENT.MBIN",
			"UI/COMPONENTS/LISTENTRY.MBIN",
			"UI/COMPONENTS/LISTPAGE.MBIN",
			"UI/COMPONENTS/OPTIONSAMOUNTBAR.MBIN",
			"UI/COMPONENTS/OPTIONSBAR.MBIN",
			"UI/COMPONENTS/OPTIONSBUTTON.MBIN",
			"UI/COMPONENTS/UIPANEL.MBIN",
			"UI/COMPONENTS/USERBAR.MBIN",
			"UI/INTERACTIONDIALOGPAGE.MBIN",
			"UI/INTERACTIONPAGE.MBIN"
		};
		foreach( var path in paths ) {
			var mbin = ExtractMbin<GcNGuiLayerData>(path);
			GcNGuiLayerData(mbin);
		}
	}
	
	protected void GcNGuiLayerData( GcNGuiLayerData DATA )  // recursive
	{
		TkNGuiGraphicStyle(DATA.Style);
		foreach( var child in DATA.Children ) {
			     if( child is GcNGuiLayerData   layer   ) GcNGuiLayerData(layer);  // recurse
			else if( child is GcNGuiTextData    text    ) TkNGuiGraphicStyle(text.GraphicStyle);
			else if( child is GcNGuiGraphicData graphic ) TkNGuiGraphicStyle(graphic.Style);
		}
	}

	protected void TkNGuiGraphicStyle( TkNGuiGraphicStyle STYLE )
	{
		STYLE.AnimTime  = 0;  // usually ~0.5
		STYLE.AnimSplit = 0;  // usually ~0.4
	}
	
	//...........................................................

	// hide all lower-right notification ui elements
	protected void GcNGuiLayerData_LowerNotify()
	{
		var mbin = ExtractMbin<GcNGuiLayerData>(
			"UI/HUD/HUDNOTIFICATIONPANEL.MBIN"
		);		
		var notify = mbin.Children.FindFirst<GcNGuiLayerData>(CHILD => 			
			CHILD.ElementData.ID == "LOWER_NOTIFY"
		);
		foreach( var child in notify.Children ) {
			     if( child is GcNGuiLayerData   layer   ) layer  .ElementData.IsHidden = true;
			else if( child is GcNGuiTextData    text    ) text   .ElementData.IsHidden = true;
			else if( child is GcNGuiGraphicData graphic ) graphic.ElementData.IsHidden = true;
		}
	}

	//...........................................................

	protected void GcLeveledStatTable()
	{
		var paths = new [] {
			"METADATA/GAMESTATE/STATS/LEVELEDSTATSTABLE.MBIN",
			"METADATA/GAMESTATE/STATS/LEVELEDSTATSTABLESURVIVALMODE.MBIN",
		};		
		foreach( var path in paths ) {
			var mbin = ExtractMbin<GcLeveledStatTable>(path);
			mbin.Table.ForEach(ITEM => {
           		ITEM.StatMessageType = StatMessageTypeEnum.Silent;  // no more "Met 10 Aliens" popups
           		if( ITEM.StatId == "PIRATES_KILLED" ) {
           			ITEM.ShowInTerminal = false;
           			ITEM.ShowStatLevel  = false;
           		}
           	});
		}
	}
	
	//...........................................................

	// poc: change background for weapon heat bar display so can better see % heated
	protected void HudWeapons()
	{
		var mbin = ExtractMbin<GcNGuiLayerData>(
			"UI/HUD/HUDWEAPONS.MBIN"
		);
		// blah, no ID's for most parent branches.
		// proper way would be to do recursive search through all child branches.
		var root     = mbin.Children[0] as GcNGuiLayerData;
		var bar      = root.Children[1] as GcNGuiLayerData;
		var overheat = bar.Children.FindFirst<GcNGuiLayerData>(CHILD => CHILD.ElementData.ID == "OVERHEAT");
		var style    = bar.Style.Default;
		style.Colour = new(0, 0, 0.5f, 0.5f);
	}
}

//=============================================================================
