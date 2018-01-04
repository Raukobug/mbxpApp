using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Zen.Barcode;

namespace SitekickService
{
    public class Barcode
    {
        public string Code { get; set; }

        public Barcode(string base64)
        {
            Bytescout.BarCodeReader.Reader reader = new Bytescout.BarCodeReader.Reader();
            reader.BarcodeTypesToFind.Code128 = true;
            reader.ReadFromMemory(Convert.FromBase64String(base64));
            var foundCodes = reader.FoundBarcodes;
            if (foundCodes.Length > 0)
            {

                Code = foundCodes[0].ToString();
                Code = Code.Remove(0, 6);
                Code = new string(Code.TakeWhile(c => !Char.IsLetter(c)).ToArray());
                Code = Code.Remove(Code.Length - 1);
            }
        }

        public byte[] CreateBarcode()
        {
            if (!String.IsNullOrEmpty(Code))
            {
                MemoryStream ms = new MemoryStream();

                BarcodeDraw d = BarcodeDrawFactory.Code128WithChecksum;

                var metrics = d.GetDefaultMetrics(60);

                metrics.Scale = 2;

                var i = d.Draw(Code, metrics);

                i.Save(ms, ImageFormat.Jpeg);

                return ms.ToArray();
            }
            else
            {
                return new byte[0];
            }
        }
    }
}
