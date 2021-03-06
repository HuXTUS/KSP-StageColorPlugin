﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KSP.UI.Screens;
using System.Collections;

namespace HuXTUS
{

	[KSPAddon(KSPAddon.Startup.FlightAndEditor, false)]
	public class StageColorPlugin : MonoBehaviour
	{

		private float lastUpdate = 0.0f;
		private float updateInterval = 0.3f;
		
		ConfigUtils config;
		
		Dict dict;

		public Hashtable hashColors = new Hashtable();
		
		public PluginModes pluginMode = PluginModes.PART_COLORING;

		public StageColorPlugin()
		{
		}

		void Start()
		{
			if (!_isLoaded) {
				
				config = new ConfigUtils(this);				
				
				config.initStyles();			

				config.readConfig();
				
				dict = new Dict();
				
				_isLoaded = true;
			}			
		}
		
		void Update()
		{
			if ((Time.time - lastUpdate) > updateInterval) {
				lastUpdate = Time.time;
				
				if (HighLogic.LoadedSceneIsEditor) {
					updateInterval = 0.5f;
					
					if (pluginMode == PluginModes.SIMPLE)
						return;
					
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

						if (_guiExpanded) {
							icon.SetBackgroundColor(element.isHereBackground ? element.backgroundColor : ColorData.NONE_COLOR);
							icon.SetIconColor(element.isHereIcon ? element.iconColor : ColorData.NONE_COLOR);

							foreach (var gicon in icon.groupedIcons) {
								gicon.SetBackgroundColor(element.isHereBackground ? element.backgroundColor : ColorData.NONE_COLOR);
								gicon.SetIconColor(element.isHereIcon ? element.iconColor : ColorData.NONE_COLOR);
							}
						} else {

							if (element.isHereBackground)
								icon.SetBackgroundColor(element.backgroundColor);
							if (element.isHereIcon)
								icon.SetIconColor(element.iconColor);

							foreach (var gicon in icon.groupedIcons) {
								if (element.isHereBackground)
									gicon.SetBackgroundColor(element.backgroundColor);
								if (element.isHereIcon)
									gicon.SetIconColor(element.iconColor);
							}

						}
					} 
			}	
			
		}

		public Rect _windowsPosition = new Rect();
		bool _isLoaded = false;

		public GUIStyle _windowStyle, _labelStyle, _toggleStyle, _sliderStyle, _buttonApplyStyle, _buttonModeStyle, _buttonSimpleStyle, _buttonFunnyStyle, _buttonColorizingStyle, _buttonRainbowStyle, _buttonRandomStyle;
		public GUIStyle _sliderStyleThumbRed, _sliderStyleThumbGreen, _sliderStyleThumbBlue;

		bool _guiExpanded = false;
		
		
		ColorData _curentColorDataItem = null;
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

			
			if (_guiExpanded) {
				
				string caption;
				if (pluginMode == PluginModes.PART_COLORING)
					caption = selectedIcons[0].partType;
				else if (pluginMode == PluginModes.SIMPLE)
					caption = dict.WindowCaptionSingleMode;
				else
					caption = pluginMode.ToString();
				_windowsPosition = GUILayout.Window(10, _windowsPosition, OnWindowExpanded, caption, _windowStyle);
			} else {
				_windowsPosition = GUILayout.Window(10, _windowsPosition, OnWindowMinimized, "S C P", _windowStyle);	
			}
		}
		
		public void OnWindowExpanded(int windowId)
		{
			if (pluginMode == PluginModes.PART_COLORING) {
				drawInterfacePartColoring();
			} else if (pluginMode == PluginModes.SIMPLE) {
				drawInterfaceSimpleColoring();
			} 
			
		}
		
		void drawInterfacePartColoring()
		{
			GUILayout.BeginVertical();
			
			_curentColorDataItem.isHereBackground = GUILayout.Toggle(_curentColorDataItem.isHereBackground, _curentColorDataItem.isHereBackground ? dict.BackgroundColorON : dict.BackgroundColorOFF, _toggleStyle);
			if (_curentColorDataItem.isHereBackground) {
				_curentColorDataItem.backgroundColor.r = GUILayout.HorizontalSlider(_curentColorDataItem.backgroundColor.r, 0, 1, _sliderStyle, _sliderStyleThumbRed);
				_curentColorDataItem.backgroundColor.g = GUILayout.HorizontalSlider(_curentColorDataItem.backgroundColor.g, 0, 1, _sliderStyle, _sliderStyleThumbGreen);
				_curentColorDataItem.backgroundColor.b = GUILayout.HorizontalSlider(_curentColorDataItem.backgroundColor.b, 0, 1, _sliderStyle, _sliderStyleThumbBlue);
			}

			_curentColorDataItem.isHereIcon = GUILayout.Toggle(_curentColorDataItem.isHereIcon, _curentColorDataItem.isHereIcon ? dict.IconColorON : dict.IconColorOFF, _toggleStyle);
			if (_curentColorDataItem.isHereIcon) {
				_curentColorDataItem.iconColor.r = GUILayout.HorizontalSlider(_curentColorDataItem.iconColor.r, 0, 1, _sliderStyle, _sliderStyleThumbRed);
				_curentColorDataItem.iconColor.g = GUILayout.HorizontalSlider(_curentColorDataItem.iconColor.g, 0, 1, _sliderStyle, _sliderStyleThumbGreen);
				_curentColorDataItem.iconColor.b = GUILayout.HorizontalSlider(_curentColorDataItem.iconColor.b, 0, 1, _sliderStyle, _sliderStyleThumbBlue);			
			}

			drawFooter();
		
			GUILayout.EndVertical();			
			
			GUI.DragWindow();
			
		}
		
		float _randomTextColorHUE = 0;
		
		void drawInterfaceSimpleColoring()
		{
			GUILayout.BeginVertical();

			//Reset
			GUILayout.Label(dict.ResetStageIconsColors, _labelStyle);
			if (GUILayout.Button(dict.Reset1, _buttonSimpleStyle)) {
				clearStageColors();
			}
			
			GUILayout.BeginHorizontal();
			if (GUILayout.Button(dict.Toggle1, _buttonColorizingStyle)) {
				NiceColorGenerator.isRainbow = !NiceColorGenerator.isRainbow;
			}				
			string buttonColorizingModeCaption;
			if (NiceColorGenerator.isRainbow)
				buttonColorizingModeCaption = dict.ColorizingLikeRainbow;
			else
				buttonColorizingModeCaption = dict.ColorizingRandomColors;

			_randomTextColorHUE += 0.001f;
			if (_randomTextColorHUE > 1)
				_randomTextColorHUE -= 1;
			Color _randomTextColor = Color.HSVToRGB(_randomTextColorHUE, 1, 1);
			_buttonRandomStyle.normal.textColor = _randomTextColor;
			_buttonRandomStyle.hover.textColor = _randomTextColor;
			_buttonRandomStyle.active.textColor = _randomTextColor;			
			
			GUILayout.Button(buttonColorizingModeCaption, NiceColorGenerator.isRainbow ? _buttonRainbowStyle : _buttonRandomStyle);
			GUILayout.EndHorizontal();
			
			//All icons
			GUILayout.Label(dict.SetColorsToAllICONS, _labelStyle);
			
			GUILayout.BeginHorizontal();
			
			if (GUILayout.Button(dict.AllBackColors, _buttonSimpleStyle)) {
				randomizeAllStageIconsColors(true, false);
			}						
			
			if (GUILayout.Button(dict.AllIconColors, _buttonSimpleStyle)) {
				randomizeAllStageIconsColors(false, true);
			}
		
			GUILayout.EndHorizontal();


			//Stages
			GUILayout.Label(dict.ColorizeSTAGESSeparately, _labelStyle);
			
			GUILayout.BeginHorizontal();
			
			if (GUILayout.Button(dict.StagesBackColors, _buttonSimpleStyle)) {
				randomizeSeparatelyStageColors(true, false);
			}						
			
			if (GUILayout.Button(dict.StagesIconColors, _buttonSimpleStyle)) {
				randomizeSeparatelyStageColors(false, true);
			}
		
			GUILayout.EndHorizontal();

			
			drawFooter();
		
			GUILayout.EndVertical();			
			
			GUI.DragWindow();
		}
		
		void drawFooter()
		{
			GUILayout.BeginHorizontal();
				
			if (GUILayout.Button(dict.Mode, _buttonModeStyle)) {
				changeMode();
			}
				
			if (GUILayout.Button(dict.ApplyAndHide, _buttonApplyStyle)) {
				_windowsPosition.height = 10;

				config.saveConfig();

				_guiExpanded = false;
				_windowStyle.fixedWidth = 80;
			}
			GUILayout.EndHorizontal();
			
		}
		 
		public void OnWindowMinimized(int windowId)
		{

			GUILayout.BeginVertical();
			
			if (GUILayout.Button(dict.Show, _buttonFunnyStyle)) {
				var selectedIcons = StageManager.Selection;			
				if ((selectedIcons == null) || (selectedIcons.Count == 0)) {
					ScreenMessages.PostScreenMessage("<color=orange>" + "Select stage icon first" + "</color>", 3f, ScreenMessageStyle.UPPER_CENTER);
					return;
				} else {
					_guiExpanded = true;
					_windowStyle.fixedWidth = 250;
				}
			}        
			
			GUILayout.EndVertical();			
			
			GUI.DragWindow();
			
			
		}
		
		void changeMode()
		{
			
			_windowsPosition.height = 10;
			
			if (pluginMode == PluginModes.PART_COLORING)
				pluginMode = PluginModes.SIMPLE;
			else if (pluginMode == PluginModes.SIMPLE)
				pluginMode = PluginModes.PART_COLORING;
			
			if (pluginMode == PluginModes.SIMPLE)
				clearStageColors();
			
		}
		
		void clearStageColors()
		{
			foreach (var stages in StageManager.Instance.Stages)
				foreach (var icon in stages.Icons) {

					icon.SetBackgroundColor(ColorData.NONE_COLOR);
					icon.SetIconColor(ColorData.NONE_COLOR);

					foreach (var gicon in icon.groupedIcons) {
						gicon.SetBackgroundColor(ColorData.NONE_COLOR);
						gicon.SetIconColor(ColorData.NONE_COLOR);
					}
				} 
		}
		

		int getStageIconsCount()
		{
			int count = 0;

			foreach (var stages in StageManager.Instance.Stages)
				count += stages.Icons.Count();

			return count;
		}

		void randomizeAllStageIconsColors(bool isBack, bool isIcon)
		{
			
			NiceColorGenerator.reset(getStageIconsCount());

			foreach (var stages in StageManager.Instance.Stages)
				foreach (var icon in stages.Icons) {
				
					Color color = NiceColorGenerator.next();

					if (isBack)
						icon.SetBackgroundColor(color);
					if (isIcon)
						icon.SetIconColor(color);

					foreach (var gicon in icon.groupedIcons) {
						if (isBack)
							gicon.SetBackgroundColor(color);
						if (isIcon)
							gicon.SetIconColor(color);
					}
				}  
		}
		
		void randomizeSeparatelyStageColors(bool isBack, bool isIcon)
		{
			
			NiceColorGenerator.reset(StageManager.Instance.Stages.Count());

			foreach (var stages in StageManager.Instance.Stages) {
				Color rndColor = NiceColorGenerator.next();
				foreach (var icon in stages.Icons) {

					if (isBack)
						icon.SetBackgroundColor(rndColor);
					if (isIcon)
						icon.SetIconColor(rndColor);

					foreach (var gicon in icon.groupedIcons) {
						if (isBack)
							gicon.SetBackgroundColor(rndColor);
						if (isIcon)
							gicon.SetIconColor(rndColor);
					}
				}
			}
		}

 
		void OnDestroy()
		{
			config.saveConfig();
		}
	}
}