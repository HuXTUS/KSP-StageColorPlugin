
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KSP.IO;
using KSP.UI.Screens;
using System.Collections;

namespace HuXTUS
{

	[KSPAddon(KSPAddon.Startup.FlightAndEditor, false)]
	public class StageColorPlugin : MonoBehaviour
	{
		
		private float lastUpdate = 0.0f;
		private float updateInterval = 0.3f;
		
		PluginConfiguration cfg;
		
		class ColorData
		{
			
			public static Color NONE_COLOR = XKCDColors.White;

			public Color backgroundColor, iconColor;
			public bool isHereBackground, isHereIcon;
			
			public ColorData(Color background, Color icon)
			{
				this.backgroundColor = background;
				this.iconColor = icon;				
				
				this.isHereBackground = !background.Equals(NONE_COLOR);
				this.isHereIcon = !icon.Equals(NONE_COLOR);
			}
			
			public ColorData()
			{
				this.backgroundColor = NONE_COLOR;
				this.iconColor = NONE_COLOR;
				
				this.isHereBackground = false;
				this.isHereIcon = false;
			}
		}
		
		Hashtable hashColors = new Hashtable();

		public StageColorPlugin()
		{
		}
		
		void Start()
		{
			if (!_isLoaded) {
				initStyles();
				readConfig();
				
				_isLoaded = true;
			}			
		}
		
		void readConfig()
		{
			cfg = PluginConfiguration.CreateForType<StageColorPlugin>();
			cfg.load();

			_windowsPosition = cfg.GetValue<Rect>("gui_position");

			string strColors = cfg.GetValue<string>("colors");
			if (strColors != null) {
				var splittedColors = strColors.Split(new string[] { ",", ";" }, StringSplitOptions.None);
				for (int i = 0; i < splittedColors.Count() - 2; i += 3) {
					var colorData = new ColorData(XKCDColors.ColorTranslator.FromHtml(splittedColors[i + 1]), XKCDColors.ColorTranslator.FromHtml(splittedColors[i + 2]));
					hashColors[splittedColors[i]] = colorData;
				}			
			}
		}
		
		void saveConfig()
		{
			saveGUIPosition();
			saveColorsToCfg();
			
			cfg.save();
		}
		
		void saveGUIPosition()
		{
			cfg["gui_position"] = new Rect(_windowsPosition);
		}
		
		void saveColorsToCfg()
		{
			string s = "";
			
			foreach (DictionaryEntry element in hashColors) {
				s += element.Key + ",";
				
				var it = (ColorData)element.Value;
				
				s += (it.isHereBackground) ? XKCDColors.ColorTranslator.ToHex(it.backgroundColor) : XKCDColors.ColorTranslator.ToHex(ColorData.NONE_COLOR);
				s += ",";
				s += (it.isHereIcon) ? XKCDColors.ColorTranslator.ToHex(it.iconColor) : XKCDColors.ColorTranslator.ToHex(ColorData.NONE_COLOR);
				s += ";";
			}
			
			cfg["colors"] = s;
		}

		void Update()
		{
			if ((Time.time - lastUpdate) > updateInterval) {
				lastUpdate = Time.time;
				
				if (HighLogic.LoadedSceneIsEditor) {
					updateInterval = 0.5f;
				} else {
					if (updateInterval < 5.0f)
						updateInterval += 0.5f;
				}

				if (StageManager.Selection.Count > 0) {
					
					string partType = StageManager.Selection[0].partType;
					
					var element = (ColorData)hashColors[partType];
					
					if (element == null) {
						element = new ColorData();
						hashColors[partType] = element;
					}
					
				}

				foreach (var stages in StageManager.Instance.Stages)
					foreach (var icon in stages.Icons) {

						string partType = icon.partType;					
						var element = (ColorData)hashColors[partType];

						if (element == null) {
							element = new ColorData();
							hashColors[partType] = element;
						}

						icon.SetBackgroundColor(element.isHereBackground ? element.backgroundColor : ColorData.NONE_COLOR);
						icon.SetIconColor(element.isHereIcon ? element.iconColor : ColorData.NONE_COLOR);

						foreach (var gicon in icon.groupedIcons) {
							gicon.SetBackgroundColor(element.isHereBackground ? element.backgroundColor : ColorData.NONE_COLOR);
							gicon.SetIconColor(element.isHereIcon ? element.iconColor : ColorData.NONE_COLOR);
						}
					} 
			}	
			
		}

		private Rect _windowsPosition = new Rect();
		private bool _isLoaded = false;
		
		private GUIStyle _windowStyle, _labelStyle, _toggleStyle, _sliderStyle, _buttonStyle, _buttonFunnyStyle;
		private GUIStyle _sliderStyleThumbRed, _sliderStyleThumbGreen, _sliderStyleThumbBlue;

		private void initStyles()
		{
			_windowStyle = new GUIStyle(HighLogic.Skin.window);
			_windowStyle.fixedWidth = 250;

			_labelStyle = HighLogic.Skin.label;
			
			_toggleStyle = HighLogic.Skin.toggle;
			
			_sliderStyle = HighLogic.Skin.horizontalSlider;
			
			_buttonStyle = HighLogic.Skin.button;
			
			_buttonFunnyStyle = new GUIStyle(HighLogic.Skin.button);
			_buttonFunnyStyle.normal.background = Utils.makeTexFunny(250, 250, 2);
			_buttonFunnyStyle.hover.background = Utils.makeTexFunny(250, 250, 3);
			_buttonFunnyStyle.active.background = Utils.makeTexFunny(250, 250, 1);			

			var redThumbTex = Utils.makeTexFromColor(1, 1, Color.red);
			_sliderStyleThumbRed = new GUIStyle(HighLogic.Skin.horizontalSliderThumb);
			_sliderStyleThumbRed.normal.background = redThumbTex;
			_sliderStyleThumbRed.hover.background = redThumbTex;
			_sliderStyleThumbRed.focused.background = redThumbTex;
			_sliderStyleThumbRed.active.background = redThumbTex;
			
			var greenThumbTex = Utils.makeTexFromColor(1, 1, Color.green);
			_sliderStyleThumbGreen = new GUIStyle(HighLogic.Skin.horizontalSliderThumb);
			_sliderStyleThumbGreen.normal.background = greenThumbTex;
			_sliderStyleThumbGreen.hover.background = greenThumbTex;
			_sliderStyleThumbGreen.focused.background = greenThumbTex;
			_sliderStyleThumbGreen.active.background = greenThumbTex;
			
			var blueThumbTex = Utils.makeTexFromColor(1, 1, Color.blue);
			_sliderStyleThumbBlue = new GUIStyle(HighLogic.Skin.horizontalSliderThumb);
			_sliderStyleThumbBlue.normal.background = blueThumbTex;
			_sliderStyleThumbBlue.hover.background = blueThumbTex;
			_sliderStyleThumbBlue.focused.background = blueThumbTex;
			_sliderStyleThumbBlue.active.background = blueThumbTex;
		
		}

		private bool _guiExpanded = false;
		
		private ColorData _curentColorDataItem = null;
		protected void OnGUI()
		{
			
			if (!HighLogic.LoadedSceneIsEditor)
				return;
			
			var selectedIcons = StageManager.Selection;
			if ((selectedIcons == null) || (selectedIcons.Count == 0))
				return;
			
			_curentColorDataItem = (ColorData)hashColors[selectedIcons[0].partType];
			
			if ((_windowsPosition.xMin <= 1) && (_windowsPosition.yMin <= 1)) {	
				_windowsPosition.xMin = 500;
				_windowsPosition.yMin = 500;
			}

			_windowsPosition.height = 10;
			if (_guiExpanded) {
				_windowStyle.fixedWidth = 250;
				_windowsPosition = GUILayout.Window(10, _windowsPosition, OnWindowExpanded, selectedIcons[0].partType, _windowStyle);
			} else {
				_windowStyle.fixedWidth = 80;
				_windowsPosition = GUILayout.Window(10, _windowsPosition, OnWindowMinimized, "S C P", _windowStyle);	
			}
		}
		
		public void OnWindowExpanded(int windowId)
		{
			GUILayout.BeginVertical();
			
			_curentColorDataItem.isHereBackground = GUILayout.Toggle(_curentColorDataItem.isHereBackground, _curentColorDataItem.isHereBackground ? "Background color ON" : "Background color OFF", _toggleStyle);
			if (_curentColorDataItem.isHereBackground) {
				_curentColorDataItem.backgroundColor.r = GUILayout.HorizontalSlider(_curentColorDataItem.backgroundColor.r, 0, 1, _sliderStyle, _sliderStyleThumbRed);
				_curentColorDataItem.backgroundColor.g = GUILayout.HorizontalSlider(_curentColorDataItem.backgroundColor.g, 0, 1, _sliderStyle, _sliderStyleThumbGreen);
				_curentColorDataItem.backgroundColor.b = GUILayout.HorizontalSlider(_curentColorDataItem.backgroundColor.b, 0, 1, _sliderStyle, _sliderStyleThumbBlue);
			}

			_curentColorDataItem.isHereIcon = GUILayout.Toggle(_curentColorDataItem.isHereIcon, _curentColorDataItem.isHereIcon ? "Icon color ON" : "Icon color OFF", _toggleStyle);
			if (_curentColorDataItem.isHereIcon) {
				_curentColorDataItem.iconColor.r = GUILayout.HorizontalSlider(_curentColorDataItem.iconColor.r, 0, 1, _sliderStyle, _sliderStyleThumbRed);
				_curentColorDataItem.iconColor.g = GUILayout.HorizontalSlider(_curentColorDataItem.iconColor.g, 0, 1, _sliderStyle, _sliderStyleThumbGreen);
				_curentColorDataItem.iconColor.b = GUILayout.HorizontalSlider(_curentColorDataItem.iconColor.b, 0, 1, _sliderStyle, _sliderStyleThumbBlue);			
			}
			
			if (GUILayout.Button("Apply and hide", _buttonStyle)) {

				saveConfig();
				
				_guiExpanded = false;
			}        

			GUILayout.EndVertical();			
			
			GUI.DragWindow();
		}

		public void OnWindowMinimized(int windowId)
		{
			GUILayout.BeginVertical();

			if (GUILayout.Button("Show", _buttonFunnyStyle)) {
				var selectedIcons = StageManager.Selection;			
				if ((selectedIcons == null) || (selectedIcons.Count == 0)) {
					ScreenMessages.PostScreenMessage("<color=orange>" + "Select stage icon first" + "</color>", 3f, ScreenMessageStyle.UPPER_CENTER);
					return;
				} else {
					_guiExpanded = true;
				}
			}        
			
			GUILayout.EndVertical();			
			
			GUI.DragWindow();
		}

 
		void OnDestroy()
		{
			saveConfig();
		}
	}
}
