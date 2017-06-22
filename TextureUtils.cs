using System;
using System.Linq;
using UnityEngine;

namespace HuXTUS
{

	public static class Utils
	{
		
		public static Texture2D makeTexFromColor(int width, int height, Color col)
		{
			Color[] pix = new Color[width * height];
 
			for (int i = 0; i < pix.Length; i++)
				pix[i] = col;
 
			var result = new Texture2D(width, height);
			result.SetPixels(pix);
			result.Apply();
 
			return result;
		}

		public static Texture2D makeTexFunny(int width, int height, int mode)
		{
			float red = 0, blue = 0, green = 0;
			
			Color[] pix = new Color[width * height];

			for (int i = 0; i < pix.Length; i++) {
				red += 0.05f;
				blue += 0.05f;
				green += 0.05f;
				if (red > 1f)
					red = 0;
				if (blue > 1f)
					blue = 0;
				if (green > 1f)
					green = 0;
				pix[i] = new Color((mode == 1) ? 1 : red, (mode == 2) ? 1 : green, (mode == 3) ? 1 : blue, 0.5f);
			}
 
			var result = new Texture2D(width, height);
			result.SetPixels(pix);
			result.Apply();
 
			return result;
		}
		
		public static Texture2D makeTexRainbow(int width)
		{
			float hue = 0.0f;
			float step = 1f / (float)width;
			
			Color[] pix = new Color[width ];

			for (int i = 0; i < pix.Length; i++) {				
				pix[i] = Color.HSVToRGB(hue += step, 1, 1);
				if (hue > 1f)
					hue -= 1f;
			}
 
			var result = new Texture2D(width, 1);
			result.SetPixels(pix);
			result.Apply();
 
			return result;
		}
		

		
	}
}