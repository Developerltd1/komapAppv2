using System;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace Utility
{
    public class ImageClass
    {
        public static Image GetImageFromBase64(string str)
        {
            Bitmap bm;
            using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(str)))
            {

                bm = new Bitmap(ms);
            }
            return bm;
        }
        public static string GetBase64StringFromImage(Image image)
        {
            string str;
            if (image != null)
            {
                using (Bitmap bm = new Bitmap(image))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        bm.Save(ms, ImageFormat.Png);
                        str = Convert.ToBase64String(ms.ToArray());
                    }
                }
                return str;
            }
            else
                return null;
        }
        public static Bitmap Resize(Bitmap imgPhoto, Size objSize, ImageFormat enuType)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;

            int destX = 0;
            int destY = 0;
            int destWidth = objSize.Width;
            int destHeight = objSize.Height;


            //Formual for aspected Ratio

            float AR = 0;
            AR = (float)sourceHeight / (float)sourceWidth;
            destHeight = int.Parse(Math.Round(decimal.Parse(((float)destWidth * AR).ToString()), 0).ToString());

            Bitmap bmPhoto;
            if (enuType == ImageFormat.Png)
                bmPhoto = new Bitmap(destWidth, destHeight, PixelFormat.Format32bppArgb);
            else if (enuType == ImageFormat.Gif)
                bmPhoto = new Bitmap(destWidth, destHeight); //PixelFormat.Format8bppIndexed should be the right value for a GIF, but will throw an error with some GIF images so it's not safe to specify.
            else
                bmPhoto = new Bitmap(destWidth, destHeight, PixelFormat.Format24bppRgb);

            //For some reason the resolution properties will be 96, even when the source image is different, so this matching does not appear to be reliable.
            //bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            //If you want to override the default 96dpi resolution do it here
            //bmPhoto.SetResolution(72, 72);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }

    }
}
