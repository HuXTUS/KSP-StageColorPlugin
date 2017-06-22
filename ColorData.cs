using System;
using System.Linq;
using UnityEngine;

namespace HuXTUS
{

	public class ColorData
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
}
