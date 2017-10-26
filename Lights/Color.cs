﻿namespace Lights
{
    public class Color
    {
        public int Red { get; set; }
        
        public int Green { get; set; }
        
        public int Blue { get; set; }

        public Color(int red, int green, int blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }

        public bool IsRed()
        {
            return this.Red == 255 && this.Green == 0 && this.Blue == 0;
        }

        public static Color NewRed()
        {
            return new Color(255, 0, 0);
        }

        public static Color NewBlue()
        {
            return new Color(0, 0, 255);
        }

        public static Color NewGreen()
        {
            return new Color(0, 255, 0);
        }

        public static Color NewWhite()
        {
            return new Color(255, 255, 255);
        }
        
        public static Color NewBlack()
        {
            return new Color(0, 0, 0);
        }

        public override bool Equals(object obj)
        {
            if (obj as Color == null)
            {
                return false;
            }

            var objColor = (Color) obj;

            return this.Red == objColor.Red && this.Blue == objColor.Blue && this.Green == objColor.Green;
        }
        
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Red;
                hashCode = (hashCode * 397) ^ Green;
                hashCode = (hashCode * 397) ^ Blue;
                return hashCode;
            }
        }
    }
}