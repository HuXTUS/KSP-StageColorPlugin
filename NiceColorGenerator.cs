using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KSP.UI.Screens;
using System.Collections;

namespace HuXTUS
{
	public static class NiceColorGenerator
	{
		
	 
		static string[] xHexColors = {
			"#f6008a",
			"#e7b300",
			"#ffe938",
			"#cafc10",
			"#417e2d",
			"#00ce38",
			"#c6cc6f",
			"#ff8160",
			"#fcb172",
			"#c02cb3",
			"#9d8bff",
			"#b75046",
			"#71b400",
			"#018c1c",
			"#fe8a00"
		};
		
		static NiceColorGenerator()
		{
			foreach (var element in xHexColors) {
				niceColors.Add(XKCDColors.ColorTranslator.FromHtml(element));
			}
			
		}

		
		static List<Color> niceColors = new List<Color>();
		
		static int index = 0;
		static System.Random rnd = new System.Random();

		public static void reset()
		{
			index = 0;			
		}
		
		
		static Color getNiceRandomColor()
		{
			if (index < niceColors.Count())
				return niceColors[index++];
			
			return new Color((float)rnd.NextDouble(), (float)rnd.NextDouble(), (float)rnd.NextDouble(), 1.0f);
		}
				
		public static Color next()
		{
			return getNiceRandomColor();
		}
	}
}
