using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;

/// <summary>
///DrawingImg 的摘要说明
/// </summary>
public class DrawingImg
{

    /// <summary>
    /// 图片重绘
    /// Xu,Tao
    /// </summary>
    /// <Author>Xu,Tao</Author>
    /// <Date>2011-1-02-10</Date>
    public DrawingImg()
    {
        //1
        //TODO: 在此处添加构造函数逻辑
        //
    }

    const int width = 1024, height = 768;
    public void Building(HttpPostedFile file, string strPath)
    {
        if (file != null && file.ContentLength > 0)
        {
            Bitmap map = null;
            System.Drawing.Image originalImage = null;
            Graphics g = null;
            try
            {

                originalImage = System.Drawing.Image.FromStream(file.InputStream);



                if (originalImage.Width > originalImage.Height)
                {
                    if (originalImage.Width > 1024)
                    {
                        float fWidth = originalImage.Width / 1024;

                        map = new Bitmap((int)(originalImage.Width / fWidth), (int)(originalImage.Height / fWidth));
                    }
                    else
                    {

                        map = new Bitmap(originalImage.Width, originalImage.Height);
                    }
                }
                else
                {
                    if (originalImage.Height > 768)
                    {
                        float fHeight = originalImage.Height / 768;
                        map = new Bitmap((int)(originalImage.Width / fHeight), (int)(originalImage.Height / fHeight));
                    }
                    else
                    {
                        map = new Bitmap(originalImage.Width, originalImage.Height);
                    }
                }

                //map = new Bitmap(1024 , 768);

                g = Graphics.FromImage(map);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.Clear(Color.Transparent);

                if (originalImage.Width > originalImage.Height)
                {
                    if (originalImage.Width > 1024)
                    {
                        float fWidth = originalImage.Width / 1024;
                        g.DrawImage(originalImage, new RectangleF(0, 0, originalImage.Width, originalImage.Height),
                            new RectangleF(0, 0, originalImage.Width * fWidth, originalImage.Height * fWidth), GraphicsUnit.Pixel);
                    }
                    else
                    {
                        g.DrawImage(originalImage, new Rectangle(0, 0, originalImage.Width, originalImage.Height), new Rectangle(0, 0, originalImage.Width, originalImage.Height), GraphicsUnit.Pixel);
                    }
                }
                else
                {
                    if (originalImage.Height > 768)
                    {
                        float fHeight = originalImage.Height / 768;
                        g.DrawImage(originalImage, new RectangleF(0, 0, originalImage.Width, originalImage.Height), new RectangleF(0, 0, originalImage.Width * fHeight, originalImage.Height * fHeight), GraphicsUnit.Pixel);
                    }
                    else
                    {
                        g.DrawImage(originalImage, new Rectangle(0, 0, originalImage.Width, originalImage.Height), new Rectangle(0, 0, originalImage.Width, originalImage.Height), GraphicsUnit.Pixel);
                    }
                }

                //if (originalImage.Height > originalImage.Width)
                //{
                //    double dHeight = 768 / double.Parse(originalImage.Height.ToString());



                //    g.DrawImage(originalImage, new Rectangle(0, 0, 768, 1024), new Rectangle(0, 0, Convert.ToInt32 (originalImage.Width * dHeight), Convert.ToInt32(originalImage.Height * dHeight)), GraphicsUnit.Pixel);
                //}
                //else
                //{
                //    g.DrawImage(originalImage, new Rectangle(0, 0, 1024, 768), new Rectangle(0, 0, originalImage.Width, originalImage.Height), GraphicsUnit.Pixel);
                //}

                map.Save(strPath, ImageFormat.Jpeg);
            }
            catch (Exception ex)
            {
                //Response.Write(string.Concat(ex.Message, ex.Source, ex.Data));
            }
            finally
            {
                g.Dispose();
                map.Dispose();
                originalImage.Dispose();
            }

        }
        //{

        //    Bitmap map = new Bitmap(width, height);
        //    Image originalImage = null;
        //    Graphics g = null;
        //    try
        //    {
        //        originalImage = Image.FromStream(file.InputStream);
        //        g = Graphics.FromImage(map);
        //        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
        //        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
        //        g.Clear(Color.Transparent);
        //        g.DrawImage(originalImage, new Rectangle(0, 0, width, height), new Rectangle(0, 0, originalImage.Width, originalImage.Height), GraphicsUnit.Pixel);
        //        map.Save(strPath, ImageFormat.Jpeg);
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    finally
        //    {
        //        g.Dispose();
        //        map.Dispose();
        //        originalImage.Dispose();
        //    }
        //}
    }
}
