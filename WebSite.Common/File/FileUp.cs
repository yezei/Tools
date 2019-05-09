using System;
using System.IO;
using System.Web;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace WebSite.Common
{
    /// <summary>
    /// 文件上传类
    /// </summary>
    public class FileUp
    {
        #region 私有成员
        private bool _State = false;//返回上传状态
        private string _Message = "";//返回上传消息
        private int _MaxSize = 1024 * 1024;//最大单个上传文件 (默认)
        private string _FileType = "jpg;jpge;png;gif;doc;xls;txt;rar;zip;pdf";//所支持的上传类型用";"隔开 
        private string _SavePath = System.Web.HttpContext.Current.Server.MapPath(".") + "\\";//保存文件的实际路径 
        private HttpPostedFile _FormFile;//上传控件
        private string _InFileName = "";//生成文件名设置，为空代表自动生成文件名
        private string _OutFileName = "";//输出文件名
        private string _OutFileType = "";//输出文件名的后缀
        private int _FileSize = 0;//获取已经上传文件的大小
        private string _FileSizeConversion = "0 bytes";//获取已经上传文件的大小 已换算
        private int _MaxWidth = 5000;//图片最大宽度(像素)
        private int _MaxHeight = 5000;//图片最大高度(像素)

        private bool _IsWater = false;//是否打水印。
        private int _WaterStyle = 0;//设置加水印的方式0：文字水印模式，1：图片水印模式
        private int _WaterXY = 0;//图片水印位置 0=不使用 1=左上 2=中上 3=右上 4=左中 5=中中 6=右中 7=左下 8=中下 9=右下
        private string _WaterText = "三五互联";//设置水印文本内容
        private string _WaterFont = "宋体";//设置水印文本字体
        private int _WaterFontSize = 12;//设置水印字大小
        private string _WaterImage = System.Web.HttpContext.Current.Server.MapPath(".") + "\\Manage_SW\\style\\images\\Water.jpg";//图片水印地址
        private string _OutWaterFileName = "";//输出水印图文件名

        private bool _IsThumb = false;//是否生成缩略图
        private int _ThumbStyle = 0;//输出缩略图片样式，0：指定高宽裁减（不变形）， 1：指定高宽缩放（补白），2：指定宽，高按比例，3：指定高，宽按比例
        private int _ThumbWidth = 100;//输出缩略图宽度
        private int _ThumbHeight = 100;//输出缩略图高度
        private string _OutThumbFileName = "";//输出缩略图文件名

        #endregion

        #region 公有属性
        /// <summary>
        /// 返回上传状态
        /// </summary>
        public bool State
        {
            get { return _State; }
        }
        /// <summary>
        /// 返回上传消息
        /// </summary>
        public string Message
        {
            get { return _Message; }
        }
        /// <summary>
        /// 最大单个上传文件
        /// </summary>
        public int MaxSize
        {
            set { _MaxSize = value; }
        }
        /// <summary>
        /// 所支持的上传类型用";"隔开 
        /// </summary>
        public string FileType
        {
            set { _FileType = value; }
        }
        /// <summary>
        /// 保存文件的实际路径 
        /// </summary>
        public string SavePath
        {
            set { _SavePath = System.Web.HttpContext.Current.Server.MapPath(value); }
            get { return _SavePath; }
        }
        /// <summary>
        /// 上传控件
        /// </summary>
        public HttpPostedFile FormFile
        {
            set { _FormFile = value; }
        }
        /// <summary>
        /// 非自动生成文件名设置。
        /// </summary>
        public string InFileName
        {
            set { _InFileName = value; }
        }
        /// <summary>
        /// 输出文件名
        /// </summary>
        public string OutFileName
        {
            get { return _OutFileName; }
        }
        /// <summary>
        /// 输出文件的后缀
        /// </summary>
        public string OutFileType
        {
            get { return _OutFileType; }
        }
        /// <summary>
        /// 获取已经上传文件的大小
        /// </summary>
        public int FileSize
        {
            get { return _FileSize; }
        }
        /// <summary>
        /// 获取已经上传文件的大小 已换算
        /// </summary>
        public string FileSizeConversion
        {
            get { return _FileSizeConversion; }
        }
        /// <summary>
        /// 图片最大宽度(像素)
        /// </summary>
        public int MaxWidth
        {
            set { _MaxWidth = value; }
        }
        /// <summary>
        /// 图片最大高度(像素)
        /// </summary>
        public int MaxHeight
        {
            set { _MaxHeight = value; }
        }
        /// <summary>
        /// 是否打水印
        /// </summary>
        public bool IsWater
        {
            set { _IsWater = value; }
        }
        /// <summary>
        /// 设置加水印的方式0：文字水印模式，1：图片水印模式
        /// </summary>
        public int WaterStyle
        {
            set { _WaterStyle = value; }
        }
        /// <summary>
        /// //图片水印位置 0=不使用 1=左上 2=中上 3=右上 4=左中 5=中中 6=右中 7=左下 8=中下 9=右下
        /// </summary>
        public int WaterXY
        {
            set { _WaterXY = value; }
        }
        /// <summary>
        /// 设置水印文本内容
        /// </summary>
        public string WaterText
        {
            set { _WaterText = value; }
        }
        /// <summary>
        /// 设置水印文本字体
        /// </summary>
        public string WaterFont
        {
            set { _WaterFont = value; }
        }
        /// <summary>
        /// 设置水印字大小
        /// </summary>
        public int WaterFontSize
        {
            set { _WaterFontSize = value; }
        }
        /// <summary>
        /// 图片水印地址
        /// </summary>
        public string WaterImage
        {
            set { _WaterImage = value; }
        }
        /// <summary>
        /// 输出水印图文件名
        /// </summary>
        public string OutWaterFileName
        {
            get { return _OutWaterFileName; }
        }
        /// <summary>
        /// 是否生成缩略图
        /// </summary>
        public bool IsThumb
        {
            set { _IsThumb = value; }
        }
        /// <summary>
        /// 输出缩略图片样式，0：指定高宽裁减（不变形）， 1：指定高宽缩放（补白），2：指定宽，高按比例，3：指定高，宽按比例
        /// </summary>
        public int ThumbStyle
        {
            set { _ThumbStyle = value; }
        }
        /// <summary>
        /// 输出缩略图宽度
        /// </summary>
        public int ThumbWidth
        {
            set { _ThumbWidth = value; }
        }
        /// <summary>
        /// 输出缩略图高度
        /// </summary>
        public int ThumbHeight
        {
            set { _ThumbHeight = value; }
        }
        /// <summary>
        /// 输出缩略图文件名
        /// </summary>
        public string OutThumbFileName
        {
            get { return _OutThumbFileName; }
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 获取文件的后缀名 
        /// </summary>
        private string GetExt(string path)
        {
            return Path.GetExtension(path).Replace(".", "");
        }

        /// <summary>
        /// 获取输出文件的文件名
        /// </summary>
        private string GetFileName(string Ext)
        {
            if (_InFileName.Trim() == "")
                return DateTime.Now.ToString("yyyyMMddHHmmssfff") + "." + Ext;
            else
                return _InFileName + "." + Ext;
        }

        /// <summary>
        /// 是否需要打水印
        /// </summary>
        /// <param name="_fileExt">文件扩展名，不含“.”</param>
        private bool IsWaterMark(string Ext)
        {
            //判断是否开启水印
            if (_IsWater)
            {
                //判断是否可以打水印的图片类型
                ArrayList al = new ArrayList();
                al.Add("bmp");
                al.Add("jpeg");
                al.Add("jpg");
                al.Add("png");
                if (al.Contains(Ext.ToLower()))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 是否为图片文件
        /// </summary>
        /// <param name="_fileExt">文件扩展名，不含“.”</param>
        private bool IsImage(string Ext)
        {
            ArrayList al = new ArrayList();
            al.Add("bmp");
            al.Add("jpeg");
            al.Add("jpg");
            al.Add("gif");
            al.Add("png");
            if (al.Contains(Ext.ToLower()))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 检查上传的文件的类型，是否允许上传。
        /// </summary>
        private bool IsUpload(string Ext)
        {
            bool b = false;
            string[] arrFileType = _FileType.Split(';');
            foreach (string str in arrFileType)
            {
                if (str.ToLower() == Ext.ToLower())
                {
                    b = true;
                    break;
                }
            }
            return b;
        }

        /// <summary>
        /// 获取换算文件大小
        /// </summary>
        private string ConversionFileSize(int Size)
        {
            if (Size < 0)
            {
                throw new ArgumentOutOfRangeException("fileSize");
            }
            else if (Size >= 1024 * 1024 * 1024)
            {
                return string.Format("{0:########0.00} GB", ((Double)Size) / (1024 * 1024 * 1024));
            }
            else if (Size >= 1024 * 1024)
            {
                return string.Format("{0:####0.00} MB", ((Double)Size) / (1024 * 1024));
            }
            else if (Size >= 1024)
            {
                return string.Format("{0:####0} KB", ((Double)Size) / 1024);
            }
            else
            {
                return string.Format("{0} bytes", Size);
            }
        }

        /// <summary>
        /// 制作缩略图
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="newFileName">新图路径</param>
        /// <param name="maxWidth">最大宽度</param>
        /// <param name="maxHeight">最大高度</param>
        private static void MakeThumbnailImage(string fileName, string newFileName, int maxWidth, int maxHeight)
        {
            byte[] imageBytes = File.ReadAllBytes(fileName);
            System.Drawing.Image img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageBytes));
            MakeThumbnailImage(img, newFileName, maxWidth, maxHeight);
        }

        /// <summary>
        /// 制作缩略图
        /// </summary>
        /// <param name="original">图片对象</param>
        /// <param name="newFileName">新图路径</param>
        /// <param name="maxWidth">最大宽度</param>
        /// <param name="maxHeight">最大高度</param>
        private static void MakeThumbnailImage(System.Drawing.Image original, string newFileName, int maxWidth, int maxHeight)
        {
            Size _newSize = ResizeImage(original.Width, original.Height, maxWidth, maxHeight);

            using (System.Drawing.Image displayImage = new Bitmap(original, _newSize))
            {
                try
                {
                    displayImage.Save(newFileName, original.RawFormat);
                }
                finally
                {
                    original.Dispose();
                }
            }
        }
        /// <summary>
        /// 计算新尺寸
        /// </summary>
        /// <param name="width">原始宽度</param>
        /// <param name="height">原始高度</param>
        /// <param name="maxWidth">最大新宽度</param>
        /// <param name="maxHeight">最大新高度</param>
        /// <returns></returns>
        private static Size ResizeImage(int width, int height, int maxWidth, int maxHeight)
        {
            if (maxWidth <= 0)
                maxWidth = width;
            if (maxHeight <= 0)
                maxHeight = height;
            decimal MAX_WIDTH = (decimal)maxWidth;
            decimal MAX_HEIGHT = (decimal)maxHeight;
            decimal ASPECT_RATIO = MAX_WIDTH / MAX_HEIGHT;

            int newWidth, newHeight;
            decimal originalWidth = (decimal)width;
            decimal originalHeight = (decimal)height;

            if (originalWidth > MAX_WIDTH || originalHeight > MAX_HEIGHT)
            {
                decimal factor;
                if (originalWidth / originalHeight > ASPECT_RATIO)
                {
                    factor = originalWidth / MAX_WIDTH;
                    newWidth = Convert.ToInt32(originalWidth / factor);
                    newHeight = Convert.ToInt32(originalHeight / factor);
                }
                else
                {
                    factor = originalHeight / MAX_HEIGHT;
                    newWidth = Convert.ToInt32(originalWidth / factor);
                    newHeight = Convert.ToInt32(originalHeight / factor);
                }
            }
            else
            {
                newWidth = width;
                newHeight = height;
            }
            return new Size(newWidth, newHeight);
        }
        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="fileName">源图路径（绝对路径）</param>
        /// <param name="newFileName">缩略图路径（绝对路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式</param>    
        private static void MakeThumbnailImage(string fileName, string newFileName, int width, int height, int mode)
        {
            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(fileName);
            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (mode)
            {
                case 0://指定高宽裁减（不变形）                
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                case 1://指定高宽缩放（补白）
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    else
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    break;
                case 2://指定宽，高按比例                    
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case 3://指定高，宽按比例
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                default:
                    break;
            }

            //新建一个bmp图片
            Bitmap b = new Bitmap(towidth, toheight);
            try
            {
                //新建一个画板
                Graphics g = Graphics.FromImage(b);
                //设置高质量插值法
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                //设置高质量,低速度呈现平滑程度
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                //清空画布并以透明背景色填充
                g.Clear(Color.White);
                //g.Clear(Color.Transparent);
                //在指定位置并且按指定大小绘制原图片的指定部分
                g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight), new Rectangle(x, y, ow, oh), GraphicsUnit.Pixel);

                SaveImage(b, newFileName, GetCodecInfo("image/" + GetFormat(newFileName).ToString().ToLower()));
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                b.Dispose();
            }
        }
        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="image">Image 对象</param>
        /// <param name="savePath">保存路径</param>
        /// <param name="ici">指定格式的编解码参数</param>
        private static void SaveImage(System.Drawing.Image image, string savePath, ImageCodecInfo ici)
        {
            //设置 原图片 对象的 EncoderParameters 对象
            EncoderParameters parameters = new EncoderParameters(1);
            parameters.Param[0] = new EncoderParameter(Encoder.Quality, ((long)100));
            image.Save(savePath, ici, parameters);
            parameters.Dispose();
        }

        /// <summary>
        /// 获取图像编码解码器的所有相关信息
        /// </summary>
        /// <param name="mimeType">包含编码解码器的多用途网际邮件扩充协议 (MIME) 类型的字符串</param>
        /// <returns>返回图像编码解码器的所有相关信息</returns>
        private static ImageCodecInfo GetCodecInfo(string mimeType)
        {
            ImageCodecInfo[] CodecInfo = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo ici in CodecInfo)
            {
                if (ici.MimeType == mimeType)
                    return ici;
            }
            return null;
        }
        /// <summary>
        /// 得到图片格式
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <returns></returns>
        private static ImageFormat GetFormat(string name)
        {
            string ext = name.Substring(name.LastIndexOf(".") + 1);
            switch (ext.ToLower())
            {
                case "jpg":
                case "jpeg":
                    return ImageFormat.Jpeg;
                case "bmp":
                    return ImageFormat.Bmp;
                case "png":
                    return ImageFormat.Png;
                case "gif":
                    return ImageFormat.Gif;
                default:
                    return ImageFormat.Jpeg;
            }
        }
        /// <summary>
        /// 图片水印
        /// </summary>
        /// <param name="imgPath">服务器图片相对路径</param>
        /// <param name="filename">保存文件名</param>
        /// <param name="watermarkFilename">水印文件相对路径</param>
        /// <param name="watermarkStatus">图片水印位置 0=不使用 1=左上 2=中上 3=右上 4=左中  9=右下</param>
        /// <param name="quality">附加水印图片质量,0-100</param>
        /// <param name="watermarkTransparency">水印的透明度 1--10 10为不透明</param>
        private static void AddImageSignPic(string imgPath, string filename, string watermarkFilename, int watermarkStatus, int quality, int watermarkTransparency)
        {
            if (!File.Exists(HttpContext.Current.Server.MapPath(imgPath)))
                return;
            byte[] _ImageBytes = File.ReadAllBytes(HttpContext.Current.Server.MapPath(imgPath));
            Image img = Image.FromStream(new System.IO.MemoryStream(_ImageBytes));
            filename = HttpContext.Current.Server.MapPath(filename);

            if (watermarkFilename.StartsWith("/") == false)
                watermarkFilename = "/" + watermarkFilename;
            watermarkFilename = HttpContext.Current.Server.MapPath(watermarkFilename);
            if (!File.Exists(watermarkFilename))
                return;
            Graphics g = Graphics.FromImage(img);
            //设置高质量插值法
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            Image watermark = new Bitmap(watermarkFilename);

            if (watermark.Height >= img.Height || watermark.Width >= img.Width)
                return;

            ImageAttributes imageAttributes = new ImageAttributes();
            ColorMap colorMap = new ColorMap();

            colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
            colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
            ColorMap[] remapTable = { colorMap };

            imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

            float transparency = 0.5F;
            if (watermarkTransparency >= 1 && watermarkTransparency <= 10)
                transparency = (watermarkTransparency / 10.0F);


            float[][] colorMatrixElements = {
												new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
												new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
												new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
												new float[] {0.0f,  0.0f,  0.0f,  transparency, 0.0f},
												new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
											};

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            int xpos = 0;
            int ypos = 0;

            switch (watermarkStatus)
            {
                case 1:
                    xpos = (int)(img.Width * (float).01);
                    ypos = (int)(img.Height * (float).01);
                    break;
                case 2:
                    xpos = (int)((img.Width * (float).50) - (watermark.Width / 2));
                    ypos = (int)(img.Height * (float).01);
                    break;
                case 3:
                    xpos = (int)((img.Width * (float).99) - (watermark.Width));
                    ypos = (int)(img.Height * (float).01);
                    break;
                case 4:
                    xpos = (int)(img.Width * (float).01);
                    ypos = (int)((img.Height * (float).50) - (watermark.Height / 2));
                    break;
                case 5:
                    xpos = (int)((img.Width * (float).50) - (watermark.Width / 2));
                    ypos = (int)((img.Height * (float).50) - (watermark.Height / 2));
                    break;
                case 6:
                    xpos = (int)((img.Width * (float).99) - (watermark.Width));
                    ypos = (int)((img.Height * (float).50) - (watermark.Height / 2));
                    break;
                case 7:
                    xpos = (int)(img.Width * (float).01);
                    ypos = (int)((img.Height * (float).99) - watermark.Height);
                    break;
                case 8:
                    xpos = (int)((img.Width * (float).50) - (watermark.Width / 2));
                    ypos = (int)((img.Height * (float).99) - watermark.Height);
                    break;
                case 9:
                    xpos = (int)((img.Width * (float).99) - (watermark.Width));
                    ypos = (int)((img.Height * (float).99) - watermark.Height);
                    break;
            }

            g.DrawImage(watermark, new Rectangle(xpos, ypos, watermark.Width, watermark.Height), 0, 0, watermark.Width, watermark.Height, GraphicsUnit.Pixel, imageAttributes);

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo ici = null;
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.MimeType.IndexOf("jpeg") > -1)
                    ici = codec;
            }
            EncoderParameters encoderParams = new EncoderParameters();
            long[] qualityParam = new long[1];
            if (quality < 0 || quality > 100)
                quality = 80;

            qualityParam[0] = quality;

            EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qualityParam);
            encoderParams.Param[0] = encoderParam;

            if (ici != null)
                img.Save(filename, ici, encoderParams);
            else
                img.Save(filename);

            g.Dispose();
            img.Dispose();
            watermark.Dispose();
            imageAttributes.Dispose();
        }

        /// <summary>
        /// 文字水印
        /// </summary>
        /// <param name="imgPath">服务器图片相对路径</param>
        /// <param name="filename">保存文件名</param>
        /// <param name="watermarkText">水印文字</param>
        /// <param name="watermarkStatus">图片水印位置 0=不使用 1=左上 2=中上 3=右上 4=左中  9=右下</param>
        /// <param name="quality">附加水印图片质量,0-100</param>
        /// <param name="fontname">字体</param>
        /// <param name="fontsize">字体大小</param>
        private static void AddImageSignText(string imgPath, string filename, string watermarkText, int watermarkStatus, int quality, string fontname, int fontsize)
        {
            byte[] _ImageBytes = File.ReadAllBytes(HttpContext.Current.Server.MapPath(imgPath));
            Image img = Image.FromStream(new System.IO.MemoryStream(_ImageBytes));
            filename = HttpContext.Current.Server.MapPath(filename);

            Graphics g = Graphics.FromImage(img);
            //设置高质量插值法
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            Font drawFont = new Font(fontname, fontsize, FontStyle.Regular, GraphicsUnit.Pixel);
            SizeF crSize;
            crSize = g.MeasureString(watermarkText, drawFont);

            float xpos = 0;
            float ypos = 0;

            switch (watermarkStatus)
            {
                case 1:
                    xpos = (float)img.Width * (float).01;
                    ypos = (float)img.Height * (float).01;
                    break;
                case 2:
                    xpos = ((float)img.Width * (float).50) - (crSize.Width / 2);
                    ypos = (float)img.Height * (float).01;
                    break;
                case 3:
                    xpos = ((float)img.Width * (float).99) - crSize.Width;
                    ypos = (float)img.Height * (float).01;
                    break;
                case 4:
                    xpos = (float)img.Width * (float).01;
                    ypos = ((float)img.Height * (float).50) - (crSize.Height / 2);
                    break;
                case 5:
                    xpos = ((float)img.Width * (float).50) - (crSize.Width / 2);
                    ypos = ((float)img.Height * (float).50) - (crSize.Height / 2);
                    break;
                case 6:
                    xpos = ((float)img.Width * (float).99) - crSize.Width;
                    ypos = ((float)img.Height * (float).50) - (crSize.Height / 2);
                    break;
                case 7:
                    xpos = (float)img.Width * (float).01;
                    ypos = ((float)img.Height * (float).99) - crSize.Height;
                    break;
                case 8:
                    xpos = ((float)img.Width * (float).50) - (crSize.Width / 2);
                    ypos = ((float)img.Height * (float).99) - crSize.Height;
                    break;
                case 9:
                    xpos = ((float)img.Width * (float).99) - crSize.Width;
                    ypos = ((float)img.Height * (float).99) - crSize.Height;
                    break;
            }

            g.DrawString(watermarkText, drawFont, new SolidBrush(Color.White), xpos + 1, ypos + 1);
            g.DrawString(watermarkText, drawFont, new SolidBrush(Color.Black), xpos, ypos);

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo ici = null;
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.MimeType.IndexOf("jpeg") > -1)
                    ici = codec;
            }
            EncoderParameters encoderParams = new EncoderParameters();
            long[] qualityParam = new long[1];
            if (quality < 0 || quality > 100)
                quality = 80;

            qualityParam[0] = quality;

            EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qualityParam);
            encoderParams.Param[0] = encoderParam;

            if (ici != null)
                img.Save(filename, ici, encoderParams);
            else
                img.Save(filename);

            g.Dispose();
            img.Dispose();
        }
        #endregion

        #region 上传文件
        public void Upload()
        {
            string Ext = GetExt(_FormFile.FileName);//文件后缀
            string FileName = GetFileName(Ext);//文件名
            string ThumbFileName = GetFileName(Ext).Replace(".", "_t.");//缩略图文件名
            string WaterFileName = GetFileName(Ext).Replace(".", "_w.");//水印图文件名
            int FileLength = _FormFile.ContentLength;//获得文件大小，以字节为单位

            if (_FormFile == null || _FormFile.FileName.Trim() == "")
            {
                _Message = "请选择要上传文件！";
                return;
            }
            if (!IsUpload(Ext))
            {
                _Message = "不允许上传" + Ext + "类型的文件！";
                return;
            }
            if (FileLength > _MaxSize)
            {
                _Message = "文件超过限制的大小！";
                return;
            }
            try
            {
                //检查上传的物理路径是否存在，不存在则创建
                if (!Directory.Exists(_SavePath))
                {
                    Directory.CreateDirectory(_SavePath);
                }
                //保存文件
                _FormFile.SaveAs(_SavePath + FileName);

                if (IsImage(Ext))
                {
                    //获取图片的高度和宽度
                    byte[] imageBytes = File.ReadAllBytes(_SavePath + FileName);
                    System.Drawing.Image img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(imageBytes));
                    int _Width = img.Width;
                    int _Height = img.Height;
                    //如果是图片，检查图片是否超出最大尺寸，是则裁剪
                    if (IsImage(Ext) && (_Width > _MaxWidth || _Height > _MaxHeight))
                    {
                        MakeThumbnailImage(_SavePath + FileName, _SavePath + FileName, _MaxWidth, _MaxHeight);
                    }
                    //如果是图片，检查是否需要生成缩略图，是则生成
                    if (IsImage(Ext) && _IsThumb && _ThumbWidth > 0 && _ThumbHeight > 0)
                    {
                        MakeThumbnailImage(_SavePath + FileName, _SavePath + ThumbFileName, _ThumbWidth, _ThumbHeight, _ThumbStyle);
                    }
                    else
                    {
                        ThumbFileName = FileName; //不生成缩略图则返回原图
                    }
                    //如果是图片，检查是否需要打水印
                    if (IsWaterMark(Ext) && _IsWater)
                    {
                        switch (_WaterStyle)
                        {
                            case 0:
                                AddImageSignText(_SavePath, WaterFileName, _WaterText, _WaterXY, 100, _WaterFont, _WaterFontSize);
                                break;
                            case 1:
                                AddImageSignPic(_SavePath, WaterFileName, _WaterImage, _WaterXY, 100, 5);
                                break;
                        }
                    }
                }

                //返回
                _OutFileName = FileName;
                _OutFileType = Ext;
                _FileSize = FileLength;
                _FileSizeConversion = ConversionFileSize(_FileSize);

                _OutWaterFileName = WaterFileName;
                _OutThumbFileName = ThumbFileName;

                _State = true;
                _Message = "上传成功";
                return;
            }
            catch (Exception ex)
            {
                _Message = ex.ToString();
                return;
            }
        }
        #endregion
    }
}