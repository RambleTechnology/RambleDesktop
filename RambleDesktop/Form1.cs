using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using Aspose.Words;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Resources;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RambleDesktop
{
    public partial class Form1 : Form
    {


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            setDesktop();
        }

        /// <summary>
        /// 设置桌面壁纸
        /// </summary>
        private void setDesktop()
        {
            Bitmap img = getImgAsync();
            string baseDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
            string today = DateTime.Now.ToString("d");
            string path = baseDirectory + "img" + "\\" + today;
            if (!Directory.Exists(path))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(path);
                directoryInfo.Create();
            }
            string imgPath = path + "/AAA.bmp";
            img.Save(imgPath, System.Drawing.Imaging.ImageFormat.Bmp);
            SystemParametersInfo(20, 0, imgPath, 0x2);
        }

        /// <summary>
        /// 获取图片
        /// </summary>
        /// <returns></returns>
        private Bitmap getImgAsync()
        {
            //Bitmap img = RambleDesktop.Properties.Resources._1_png;
            //return img;
            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create("https://cn.bing.com/");
            HttpWebResponse response = (HttpWebResponse)myReq.GetResponse();
            Stream receiveStream = response.GetResponseStream();
            StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
            string str = readStream.ReadToEnd();

            var parser = new HtmlParser();
            AngleSharp.Html.Dom.IHtmlDocument htmlDocument = parser.ParseDocument(str);
            IElement element = htmlDocument.GetElementById("preloadBg");
            string href = element.GetAttribute("href");
            Bitmap wallpaper = getFormUrl(href);
            return wallpaper;
        }

        public Image GetPictureImage(string Url)
        {
            WebRequest webreq = WebRequest.Create(Url);
            WebResponse webres = webreq.GetResponse();
            using (Stream stream = webres.GetResponseStream())
            {
                return System.Drawing.Image.FromStream(stream);
            }
        }


        public Bitmap getFormUrl(string url)
        {
            var webC = new System.Net.WebClient();
            var bmp = new Bitmap(webC.OpenRead(url));
            return bmp;
        }




        /// <summary>
        /// 设置 windows 桌面壁纸
        /// </summary>
        /// <param name="uAction"></param>
        /// <param name="uParam"></param>
        /// <param name="lpvParam"></param>
        /// <param name="fuWinIni"></param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
        public static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);


    }
}
