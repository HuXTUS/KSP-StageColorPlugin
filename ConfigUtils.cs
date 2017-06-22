using System;
using System.Linq;
using UnityEngine;
using KSP.IO;
using System.Collections;

namespace HuXTUS
{
	public class ConfigUtils
	{
		readonly StageColorPlugin main;
		PluginConfiguration cfg;
		
		public System.Random rnd = new System.Random();
		
		public ConfigUtils(StageColorPlugin p)
		{
			this.main = p;
		}

		public	void readConfig()
		{
			cfg = PluginConfiguration.CreateForType<StageColorPlugin>();
			cfg.load();

			readGUIConfig();

			string strColors = cfg.GetValue<string>("colors");
			if (strColors != null) {
				var splittedColors = strColors.Split(new string[] { ",", ";" }, StringSplitOptions.None);
				for (int i = 0; i < splittedColors.Count() - 2; i += 3) {
					var colorData = new ColorData(XKCDColors.ColorTranslator.FromHtml(splittedColors[i + 1]), XKCDColors.ColorTranslator.FromHtml(splittedColors[i + 2]));
					main.hashColors[splittedColors[i]] = colorData;
				}			
			}
		}
		
		public void saveConfig()
		{
			saveGUIConfig();
			saveColorsToCfg();
			
			cfg.save();
		}
		
		void readGUIConfig()
		{
			main._windowsPosition = cfg.GetValue<Rect>("gui_position");

			string guiMode = cfg.GetValue<string>("gui_mode", PluginModes.PART_COLORING.ToString());
			main.pluginMode = (PluginModes)Enum.Parse(typeof(PluginModes), guiMode);
			
			NiceColorGenerator.isRainbow = cfg.GetValue<bool>("IsRainbow");

		}

		void saveGUIConfig()
		{
			cfg["gui_position"] = new Rect(main._windowsPosition);
			
			cfg["gui_mode"] = main.pluginMode.ToString();
			
			cfg["IsRainbow"] = NiceColorGenerator.isRainbow;
			
			  
		}
		
		void saveColorsToCfg()
		{
			string s = "";
			
			foreach (DictionaryEntry element in main.hashColors) {
				s += element.Key + ",";
				
				var it = (ColorData)element.Value;
				
				s += (it.isHereBackground) ? XKCDColors.ColorTranslator.ToHex(it.backgroundColor) : XKCDColors.ColorTranslator.ToHex(ColorData.NONE_COLOR);
				s += ",";
				s += (it.isHereIcon) ? XKCDColors.ColorTranslator.ToHex(it.iconColor) : XKCDColors.ColorTranslator.ToHex(ColorData.NONE_COLOR);
				s += ";";
			}
			
			cfg["colors"] = s;
		}
		
		
		public void initStyles()
		{
			main._windowStyle = new GUIStyle(HighLogic.Skin.window);
			main._windowStyle.fixedWidth = 250;

			main._labelStyle = HighLogic.Skin.label;
			
			main._toggleStyle = HighLogic.Skin.toggle;
			
			main._sliderStyle = HighLogic.Skin.horizontalSlider;
			
			
			main._buttonApplyStyle = new GUIStyle(HighLogic.Skin.button);
			main._buttonApplyStyle.normal.background = Utils.makeTexFromColor(1, 1, XKCDColors.Green);
			main._buttonApplyStyle.hover.background = Utils.makeTexFromColor(1, 1, XKCDColors.GreenApple);
			main._buttonApplyStyle.active.background = Utils.makeTexFromColor(1, 1, XKCDColors.GreenYellow);
			
			main._buttonModeStyle = new GUIStyle(HighLogic.Skin.button);
			main._buttonModeStyle.normal.background = Utils.makeTexFromColor(1, 1, XKCDColors.Blue);
			main._buttonModeStyle.hover.background = Utils.makeTexFromColor(1, 1, XKCDColors.LightBlue);
			main._buttonModeStyle.active.background = Utils.makeTexFromColor(1, 1, XKCDColors.Blue_Purple);
			
			main._buttonSimpleStyle = new GUIStyle(HighLogic.Skin.button);
			main._buttonSimpleStyle.normal.background = Utils.makeTexFromColor(1, 1, XKCDColors.DarkIndigo);
			main._buttonSimpleStyle.hover.background = Utils.makeTexFromColor(1, 1, XKCDColors.Indigo);
			main._buttonSimpleStyle.active.background = Utils.makeTexFromColor(1, 1, XKCDColors.LightIndigo);
			
			main._buttonFunnyStyle = new GUIStyle(HighLogic.Skin.button);
			main._buttonFunnyStyle.normal.background = Utils.makeTexFunny(250, 250, 2);
			main._buttonFunnyStyle.hover.background = Utils.makeTexFunny(250, 250, 3);
			main._buttonFunnyStyle.active.background = Utils.makeTexFunny(250, 250, 1);	

			main._buttonColorizingStyle = new GUIStyle(HighLogic.Skin.button);
			main._buttonColorizingStyle.normal.background = Utils.makeTexFromColor(1, 1, XKCDColors.Gold);
			main._buttonColorizingStyle.hover.background = Utils.makeTexFromColor(1, 1, XKCDColors.LightGold);
			main._buttonColorizingStyle.active.background = Utils.makeTexFromColor(1, 1, XKCDColors.PaleGold);
			
			main._buttonRandomStyle = new GUIStyle(HighLogic.Skin.button);
			main._buttonRandomStyle.normal.background = Utils.makeTexFromColor(1, 1, XKCDColors.AlmostBlack);
			main._buttonRandomStyle.hover.background = Utils.makeTexFromColor(1, 1, XKCDColors.AlmostBlack);
			main._buttonRandomStyle.active.background = Utils.makeTexFromColor(1, 1, XKCDColors.AlmostBlack);
			
			main._buttonRainbowStyle = new GUIStyle(HighLogic.Skin.button);
			Texture2D texRainbow = Utils.makeTexRainbow(250);
			main._buttonRainbowStyle.normal.background = texRainbow;
			main._buttonRainbowStyle.hover.background = texRainbow;
			main._buttonRainbowStyle.active.background = texRainbow;	
			main._buttonRainbowStyle.normal.textColor = XKCDColors.AlmostBlack;
			main._buttonRainbowStyle.hover.textColor = XKCDColors.AlmostBlack;
			main._buttonRainbowStyle.active.textColor = XKCDColors.AlmostBlack;
			
			main._buttonColorizingStyle = new GUIStyle(HighLogic.Skin.button);
			main._buttonColorizingStyle.normal.background = Utils.makeTexFromColor(1, 1, XKCDColors.Gold);
			main._buttonColorizingStyle.hover.background = Utils.makeTexFromColor(1, 1, XKCDColors.LightGold);
			main._buttonColorizingStyle.active.background = Utils.makeTexFromColor(1, 1, XKCDColors.PaleGold);

			var redThumbTex = Utils.makeTexFromColor(1, 1, Color.red);
			main._sliderStyleThumbRed = new GUIStyle(HighLogic.Skin.horizontalSliderThumb);
			main._sliderStyleThumbRed.normal.background = redThumbTex;
			main._sliderStyleThumbRed.hover.background = redThumbTex;
			main._sliderStyleThumbRed.focused.background = redThumbTex;
			main._sliderStyleThumbRed.active.background = redThumbTex;
			
			var greenThumbTex = Utils.makeTexFromColor(1, 1, Color.green);
			main._sliderStyleThumbGreen = new GUIStyle(HighLogic.Skin.horizontalSliderThumb);
			main._sliderStyleThumbGreen.normal.background = greenThumbTex;
			main._sliderStyleThumbGreen.hover.background = greenThumbTex;
			main._sliderStyleThumbGreen.focused.background = greenThumbTex;
			main._sliderStyleThumbGreen.active.background = greenThumbTex;
			
			var blueThumbTex = Utils.makeTexFromColor(1, 1, Color.blue);
			main._sliderStyleThumbBlue = new GUIStyle(HighLogic.Skin.horizontalSliderThumb);
			main._sliderStyleThumbBlue.normal.background = blueThumbTex;
			main._sliderStyleThumbBlue.hover.background = blueThumbTex;
			main._sliderStyleThumbBlue.focused.background = blueThumbTex;
			main._sliderStyleThumbBlue.active.background = blueThumbTex;
		
		}
	}
}