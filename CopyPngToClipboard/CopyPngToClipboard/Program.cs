using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace CopyPngToClipboard
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            string filePath = "c:\\temp\\simple.png";

            BitmapSource image = new BitmapImage(new Uri(filePath));

            MemoryStream pngMemStream = new MemoryStream();
            using (FileStream fileStream = File.OpenRead(filePath))
            {
                pngMemStream.SetLength(fileStream.Length);
                fileStream.Read(pngMemStream.GetBuffer(), 0, (int)fileStream.Length);
            }

            string htmlFragment = ClipboardHelper.GetHtmlDataString(String.Format("<img src='data:image/png;base64,{0}' />", Convert.ToBase64String(pngMemStream.ToArray())));

            Clipboard.Clear();
            
            IDataObject clipboardDataObject = new DataObject();
            
            clipboardDataObject.SetData(DataFormats.Text, filePath);
            clipboardDataObject.SetData(DataFormats.Bitmap, image);
            clipboardDataObject.SetData("PNG", pngMemStream);
            clipboardDataObject.SetData(DataFormats.Html, htmlFragment);

            Clipboard.SetDataObject(clipboardDataObject, true); 
        }
    }
}
