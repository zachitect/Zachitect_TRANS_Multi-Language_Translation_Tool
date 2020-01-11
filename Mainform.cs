using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace Zachitect_TRANS
{
    public partial class Mainform : Form
    {
        string[] LanguageNames = new string[29]
        {
            "Auto-detect Language",
            "Arabic",
            "Bulgarian",
            "Cestina",
            "Chinese Simplified 简体中文",
            "Chinese Traditional 繁體中文",
            "Chinese Classical 文言文",
            "Chinese Cantonese 广东话",
            "Danish",
            "Dutch",
            "English",
            "Estonian",
            "Finnish",
            "French",
            "German",
            "Greek",
            "Hungarian",
            "Italian",
            "Japanese 日本語",
            "Korean 韓國語",
            "Polish",
            "Portuguese",
            "Romanian",
            "Russian",
            "Slovenian",
            "Spain",
            "Swedish",
            "Thai",  
            "Vietnamese"
        };
        string[] LanguageCodes = new string[29]
        {
            "auto",
            "ara",
            "bul",
            "cs",
            "zh",
            "cht",
            "wyw",
            "yue",
            "dan",
            "nl",
            "en",
            "est",
            "fin",
            "fra",
            "de",
            "el",
            "hu",
            "it",
            "jp",
            "kor",
            "pl",
            "pt",
            "rom",
            "ru",
            "slo",
            "spa",
            "swe",
            "th",
            "vie"
        };

        static String BDTranslate(string q, string from, string to) // 原文
        {
            // 源语言
            // 目标语言
            // 改成您的APP ID
            string appId = "20190405000284840";
            Random rd = new Random();
            string salt = rd.Next(100000).ToString();
            // 改成您的密钥
            string secretKey = "eYK0BQrpLj0tPtPppE32";
            string sign = EncryptString(appId + q + salt + secretKey);
            string url = "http://api.fanyi.baidu.com/api/trans/vip/translate?";
            url += "q=" + HttpUtility.UrlEncode(q);
            url += "&from=" + from;
            url += "&to=" + to;
            url += "&appid=" + appId;
            url += "&salt=" + salt;
            url += "&sign=" + sign;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            request.UserAgent = null;
            request.Timeout = 6000;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }
        // 计算MD5值
        public static string EncryptString(string str)
        {
            MD5 md5 = MD5.Create();
            // 将字符串转换成字节数组
            byte[] byteOld = Encoding.UTF8.GetBytes(str);
            // 调用加密方法
            byte[] byteNew = md5.ComputeHash(byteOld);
            // 将加密结果转换为字符串
            StringBuilder sb = new StringBuilder();
            foreach (byte b in byteNew)
            {
                // 将字节转换成16进制表示的字符串，
                sb.Append(b.ToString("x2"));
            }
            // 返回加密的字符串
            return sb.ToString();
        }
        public static string decode(string text)
        {
            byte[] mybyte = System.Convert.FromBase64String(text);
            string returntext = System.Text.Encoding.UTF8.GetString(mybyte);
            return returntext;
        }
        public Mainform()
        {
            InitializeComponent();
            this.TextInput.Clear();
            this.TextDisplay.Clear();

            this.TranslateFrom.Items.Clear();
            this.TranslateTo.Items.Clear();

            this.TranslateFrom.Items.AddRange(LanguageNames);
            this.TranslateTo.Items.AddRange(LanguageNames);

            this.TranslateFrom.SelectedIndex = 0;
            this.TranslateTo.SelectedIndex = 4;

            this.TextInput.Text = "Content to translate...";
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (TextInput.Text.Length > 0)
                {
                    string S = BDTranslate(this.TextInput.Text, LanguageCodes[this.TranslateFrom.SelectedIndex], LanguageCodes[this.TranslateTo.SelectedIndex]);
                    dynamic res = JsonConvert.DeserializeObject(S);
                    string[] displayresult = new string[res.trans_result.Count];
                    for (int i = 0; i < res.trans_result.Count; i++)
                    {
                        displayresult[i] = res.trans_result[i].dst;
                    }
                    this.TextDisplay.Text = string.Join("\n",displayresult);
                }
            }
            catch
            {

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            { 
                if(TextDisplay.Text.Length > 0)
                {
                    Clipboard.SetText(TextDisplay.Text);
                }
            }
            catch
            {

            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("https://www.zachitect.com/translate-tool");
            }
            catch
            {

            }
        }
    }
}
