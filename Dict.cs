using System;
using KSP.Localization;

namespace HuXTUS
{
 
	public class Dict
	{
		public string Show,WindowCaptionSingleMode, BackgroundColorON, BackgroundColorOFF, IconColorON, IconColorOFF, 
			ResetStageIconsColors, Reset1, Toggle1, ColorizingLikeRainbow, ColorizingRandomColors, 
			SetColorsToAllICONS, AllBackColors, AllIconColors, ColorizeSTAGESSeparately,
			StagesBackColors, StagesIconColors, Mode, ApplyAndHide;
		
		public Dict()
		{
			this.WindowCaptionSingleMode = Localizer.GetStringByTag("#StageColorPlugin_WindowCaptionSingleMode");
			this.BackgroundColorON = Localizer.GetStringByTag("#StageColorPlugin_BackgroundColorON");
			this.BackgroundColorOFF = Localizer.GetStringByTag("#StageColorPlugin_BackgroundColorOFF");
			this.IconColorON = Localizer.GetStringByTag("#StageColorPlugin_IconColorON");
			this.IconColorOFF = Localizer.GetStringByTag("#StageColorPlugin_IconColorOFF");
			this.ResetStageIconsColors = Localizer.GetStringByTag("#StageColorPlugin_ResetStageIconsColors");
			this.Reset1 = Localizer.GetStringByTag("#StageColorPlugin_Reset1");
			this.Toggle1 = Localizer.GetStringByTag("#StageColorPlugin_Toggle1");
			this.ColorizingLikeRainbow = Localizer.GetStringByTag("#StageColorPlugin_ColorizingLikeRainbow");
			this.ColorizingRandomColors = Localizer.GetStringByTag("#StageColorPlugin_ColorizingRandomColors");
			this.SetColorsToAllICONS = Localizer.GetStringByTag("#StageColorPlugin_SetColorsToAllICONS");
			this.AllBackColors = Localizer.GetStringByTag("#StageColorPlugin_AllBackColors");
			this.AllIconColors = Localizer.GetStringByTag("#StageColorPlugin_AllIconColors");
			this.ColorizeSTAGESSeparately = Localizer.GetStringByTag("#StageColorPlugin_ColorizeSTAGESSeparately");
			this.StagesBackColors = Localizer.GetStringByTag("#StageColorPlugin_StagesBackColors");
			this.StagesIconColors = Localizer.GetStringByTag("#StageColorPlugin_StagesIconColors");
			this.Mode = Localizer.GetStringByTag("#StageColorPlugin_Mode");
			this.ApplyAndHide = Localizer.GetStringByTag("#StageColorPlugin_ApplyAndHide");
			this.Show = Localizer.GetStringByTag("#StageColorPlugin_Show");
			
			
			
		}
	}
}
