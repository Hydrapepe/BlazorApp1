using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

namespace BlazorApp1.Pages.Windows
{
    public class Create_file
    {
        public static void Test(string memory)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            using (FileStream fs = File.Create("test.ps1"))
            {
                Encoding win1251 = Encoding.GetEncoding(1251);
                string info = UTF8ToWin1251(memory);
                using (var sr = new StreamWriter(fs, win1251))
                {
                    sr.Write(info);
                }
            }
        }
        public static string UTF8ToWin1251(string sourceStr)
        {
            Encoding utf8 = Encoding.UTF8;
            Encoding win1251 = Encoding.GetEncoding("Windows-1251");
            byte[] utf8Bytes = utf8.GetBytes(sourceStr);
            byte[] win1251Bytes = Encoding.Convert(utf8, win1251, utf8Bytes);
            return win1251.GetString(win1251Bytes);
        }
    }
}
