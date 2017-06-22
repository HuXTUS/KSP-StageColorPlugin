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

		static System.Random rnd = new System.Random();

		static Color last = new Color();
		
		public static bool isRainbow = true;
		
		static float step = 0.2f;
		static float hueValue = 0.0f;
		
		public static void reset(int count)
		{
			step = (count > 0) ? 1.0f / (float)count : 0.2f;
			hueValue += step;
		}
				
		public static Color next()
		{
			
			if (isRainbow) {
				hueValue += step;
				if (hueValue > 1)
					hueValue -= 1.0f;
				return Color.HSVToRGB(hueValue, 1, 1);
			} else {

				Color c;

				do {
					c = Color.HSVToRGB(rndFloat(), 1, 1);
				} while (getColorsDistance(c, last) < 0.15d);
			
				last = c;			
			
				return c;
			}
		}

		static double getColorsDistance(Color c1, Color c2)
		{

			float h0, s0, v0;
			float h1, s1, v1;

			Color.RGBToHSV(c1, out h0, out s0, out v0);
			Color.RGBToHSV(c2, out h1, out s1, out v1);

			double distance = Math.Abs(h1 - h0);

			return distance;
		}
		
		static float rndFloat()
		{
			return (float)rnd.NextDouble();
		}
 
	}

}
