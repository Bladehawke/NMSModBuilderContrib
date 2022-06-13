﻿//=============================================================================

//=============================================================================

public class bShips48 : cmk.NMS.Script.ModClass
{
  protected override void Execute()
  {
    GcInventoryTable();
  }

  //...........................................................

  protected void GcInventoryTable()
  {
    var mbin = ExtractMbin<GcInventoryTable>(
      "METADATA/REALITY/TABLES/INVENTORYTABLE.MBIN"
    );
    var data = mbin.GenerationData.GenerationDataPerSizeType;
    for (var i = SizeTypeEnum.SciSmall; i <= SizeTypeEnum.SailLarge; i++)
    {
      data[(int)i].MinSlots = 48;
      data[(int)i].MaxSlots = 48;
      data[(int)i].MinTechSlots = 48;
      data[(int)i].MaxTechSlots = 48;
      data[(int)i].MinCargoSlots = 48;
      data[(int)i].MaxCargoSlots = 48;
    }
  }
}


//=============================================================================
