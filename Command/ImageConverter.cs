using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DoroonNet.Command
{
    class ImageConverter
    {

        //static int dropped = 0;
        //static int FpsCount = 0;
        //static int doubsle = 0;
        ////static string img_dir = Func_exe_path();
        //static string fileName2 = null;

        //static List<long> imgTimeAvgli = new List<long>();
        //static StringBuilder Hextxt = new StringBuilder();

        ////public static string ImgPath = img_dir + @"ImageCache\";

        //public static List<string> imgTxtDate = new List<string>();
        //public static List<string> imgJpgDate = new List<string>();
        

        //public static Task HexTXTbuffer(string HexTxt)
        //{
        //    ConsolePrint ConSoPt = new ConsolePrint();

        //    Hextxt.Append(HexTxt);
        //    if (Hextxt.ToString().Contains("537A547A4E"))
        //    {
        //        FpsCount += 1;
        //        dropped += 1;                
        //        UDPServer.tims.Stop();
        //        HexWriterTxtAsync(Hextxt.ToString());
        //        Hextxt.Clear();
        //        imgTimeAvgli.Add(UDPServer.tims.ElapsedMilliseconds);
        //        UDPServer.tims.Restart();
        //        Hextxt.Clear();
        //        //ConSoPt.ConsoPrint(dropped.ToString());
        //        //FPStest();
        //    }

        //    if (dropped > 99)
        //    {
        //        var avgResutl = imgTimeAvgli.Average();
        //        ConSoPt.ConsoPrint("IMG AVG " + avgResutl + "ms");
        //        //MessageShow = ("IMG AVG " + avgResutl + "ms");
        //        dropped = 0;
        //        imgTimeAvgli.Clear();
        //    }

        //    return Task.CompletedTask;
        //}

        //public static Task HexWriterTxtAsync(string hexString)
        //{
        //    FileStream fs;
        //    StreamWriter sw;
        //    ConsolePrint ConSoPt = new ConsolePrint();
        //    string hexfiletext = null; 

        //    try
        //    {
        //        string filePath = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss-fffff") + ".txt";//DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss-fffff") + ".txt"
        //        //ConSoPt.ConsoPrint("Img_Recv");

        //        if (fileName2 != null)
        //        {
        //            //Console.WriteLine("fileName2 != null");
        //            using (StreamReader sr = new StreamReader(ImgDir + @"\ImageCache\" + fileName2))
        //            {
        //                hexfiletext = sr.ReadToEnd();
        //            }

        //            if (hexfiletext.Contains("537A547A4E"))
        //            {
        //                //Console.WriteLine("組合 " + fileName2);
        //                filePath = fileName2;
        //                File.WriteAllTextAsync(ImgDir + @"\ImageCache\" + fileName2, hexfiletext.Replace("537A547A4E", string.Empty).Replace("\r\n", string.Empty));
        //                hexfiletext = string.Empty;
        //            }
        //            else
        //            {
        //                //Console.WriteLine("OK " + fileName2);
        //                hexfiletext = string.Empty;
        //                //fileName2 = null;
        //            }
        //            //fileName2 = null;
        //        }


        //        if (hexString.Contains("537A547A4E0153544801"))
        //        {
        //            doubsle += 1;
        //            Console.WriteLine("In 切 " + doubsle);
        //            //LogSave(recvlog, logsp, img_dir, filePath);

        //            if (hexString.Contains("537A547A4E537A547A4E"))
        //            {
        //                Console.ForegroundColor = ConsoleColor.Green;
        //                Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!");
        //                Console.ResetColor();
        //            }
        //            else
        //            {
        //                //Console.WriteLine(hexString);
        //                string[] tArray = hexString.Split(new string[] { "537A547A4E0153544801" }, StringSplitOptions.RemoveEmptyEntries);
        //                fs = new FileStream(ImgDir + @"\ImageCache\" + filePath, FileMode.Append);
        //                sw = new StreamWriter(fs);
        //                sw.WriteLine(tArray[0]);
        //                sw.Close();
        //                fs.Close();
        //                Console.ForegroundColor = ConsoleColor.Yellow;
        //                //Console.WriteLine(filePath + " 切 " + doubsle);
        //                Console.ResetColor();
        //                ConvertHexToByteArray(filePath);
        //                imgTxtDate.Add(filePath);

        //                //Console.WriteLine(filePath + " 新 " + fileName2 + " Array " + tArray.Length);

        //                if (tArray.Length == 2)
        //                {
        //                    fileName2 = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss-fffff") + ".txt";
        //                    fs = new FileStream(ImgDir + @"\ImageCache\" + fileName2, FileMode.Append);
        //                    sw = new StreamWriter(fs);
        //                    sw.WriteLine(tArray[1].Replace("\r\n", string.Empty) + "537A547A4E");
        //                    sw.Close();
        //                    fs.Close();
        //                }


        //                hexString = String.Empty;
        //                doubsle = 0;
        //            }
                    



        //        }
        //        else
        //        {
        //            //ConSoPt.ConsoPrint("Data is ok " + filePath + "  " + fileName2);//沒有連包

        //            fs = new FileStream(ImgDir + @"\ImageCache\" + filePath, FileMode.Append);//fileName2
        //            sw = new StreamWriter(fs);

        //            if (hexString.Contains("537A547A4E"))//結尾
        //            {                        
        //                //ConSoPt.ConsoPrint("Final");
        //                //LogSave(recvlog, logsp, s_dir, filePath + "3");
        //                sw.WriteLine(hexString.Replace("537A547A4E", string.Empty));
        //                sw.Close();
        //                fs.Close();
        //                ConvertHexToByteArray(filePath);
        //                imgTxtDate.Add(filePath);
        //            }
        //            else
        //            {
        //                ConSoPt.ConsoPrint("Not Final");
        //                //sw.WriteLine(hexString);
        //                sw.Close();
        //                fs.Close();
        //                //sw.WriteLine(hexString); //sw.Close(); //fs.Close();
        //            }

        //        }

        //        hexString = string.Empty;
        //    }
        //    catch (Exception e)
        //    {
        //        doubsle = 0;
        //        Console.ForegroundColor = ConsoleColor.Red;
        //        Console.WriteLine(e.Message + "!!");
        //        Console.ResetColor();
        //    }

        //    return Task.CompletedTask;
        //}

        //public static Task ConvertHexToByteArray(string ImghexString)
        //{
        //    ConsolePrint ConSoPt = new ConsolePrint();

        //    string ImgFile;
        //    string byteToBase64String;
        //    string imgTxt_dir = Func_exe_path() + "\\";
        //    string hexfiletext = File.ReadAllText(imgTxt_dir + @"\ImageCache\" + ImghexString).Replace("0153544801", string.Empty).Replace("\r\n", string.Empty);

        //    byte[] byteArray = new byte[hexfiletext.Length / 2];          

        //    for (int index = 0; index < byteArray.Length; index++)
        //    {
        //        string byteValue = hexfiletext.Substring(index * 2, 2);
        //        //Console.WriteLine(byteValue);
        //        if (byteValue == "\r\n")
        //        {
        //            Console.WriteLine("error " + ImghexString);
        //        }
        //        else
        //        {
        //            byteArray[index] = byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
        //        }
        //    }

        //    ImgFile = ImghexString.Replace(".txt", ".jpg");
            
        //    byteToBase64String = Convert.ToBase64String(byteArray, 0, byteArray.Length, Base64FormattingOptions.None);
        //    File.WriteAllBytesAsync(imgTxt_dir + @"\ImageCache\" + ImgFile, Convert.FromBase64String(byteToBase64String));

        //    hexfiletext = string.Empty;
        //    Array.Clear(byteArray, 0, byteArray.Length);

        //    imgJpgDate.Add(ImgFile);
        //    //ConSoPt.ConsoPrint(imgJpgDate[imgJpgDate.Count-1]);
        //    File.Delete(imgTxt_dir + @"\ImageCache\" + ImghexString);
        //    return Task.CompletedTask;
        //}


        //public static Task FPStest()
        //{
        //    ConsolePrint ConSoPt = new ConsolePrint();
        //    while (true)
        //    {
        //        //fpsCount = FpsCount;
        //        ConSoPt.ConsoPrint("FPS:" + FpsCount.ToString());
        //        //Thread.Sleep(1000);
        //        //ConSoPt.ConsoPrint("FPS:" + FpsCount.ToString());
        //        FpsCount = 0;
        //        Thread.Sleep(1000);
        //    }
        //    //return Task.CompletedTask;

        //}


    }

    class ImageConverterV2
    {
        static VariableRes varRes = new VariableRes();
        //static int FpsCount = 0;
        static StringBuilder Hextxt = new StringBuilder();
        static string ImgDir = MainWindow.Func_exe_path();
        static string filePath = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss-fffff") + ".txt";

        public static string ImgPath = ImgDir + @"ImageCache\";
        public static List<string> imgTxtDate = new List<string>();
        public static List<string> imgJpgDate = new List<string>();
        static int fps = 0;

        public static Task HexTXTbufferV2(string HexTxt)
        {
            ConsolePrint ConSoPt = new ConsolePrint();
            Hextxt.Append(HexTxt);
            try
            {
                if (Hextxt.ToString().Contains("537A547A4E"))
                {
                    fps += 1;
                    using (FileStream fs = new FileStream(ImgPath + filePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))//, FileAccess.Write, FileShare.ReadWrite
                    {
                        using (StreamWriter StreamWriter = new StreamWriter(fs))
                        {
                            StreamWriter.Write(Hextxt.ToString().Replace("0153544801", string.Empty).Replace("537A547A4E", string.Empty));
                            StreamWriter.Close();
                            StreamWriter.Dispose();
                            imgTxtDate.Add(filePath);

                        }
                        fs.Close();
                        fs.Dispose();
                    }
                    ConvertHexToByteArray(filePath);//ConvertHexToByteArray(filePath);
                    filePath = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss-fffff") + ".txt";
                }
                else
                {
                    using (FileStream fs = new FileStream(ImgPath + filePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                    {
                        using (StreamWriter StreamWriter = new StreamWriter(fs))
                        {
                            StreamWriter.Write(Hextxt.ToString().Replace("0153544801", string.Empty));
                            StreamWriter.Close();
                            StreamWriter.Dispose();
                        }
                        fs.Close();
                        fs.Dispose();
                    }
                }

                if (imgTxtDate.Count > 5)
                {
                    //ConSoPt.ConsoPrint(imgTxtDate.Count.ToString()+" "+ (imgTxtDate.Count - 10));
                    //ConvertHexToByteArray(imgTxtDate[imgTxtDate.Count-5]);
                }

            }
            catch (Exception E)
            {
                ConSoPt.ConsoPrint(E.ToString());
            }


            Hextxt.Clear();

            return Task.CompletedTask;

        }

        public static Task ConvertHexToByteArray(string ImghexString)
        {
            ConsolePrint ConSoPt = new ConsolePrint();

            string ImgFile;
            string hexfiletextv;
            string byteToBase64String;
            string imgTxt_dir = ImgDir + "\\";
            //var hexfiletextv = File.ReadAllText(@"E:\Program\DoroonNet\bin\Debug\net5.0-windows\ImageCache\" + ImghexString).Replace("\r\n", string.Empty);
            //using var fs = new FileStream(@"E:\Program\DoroonNet\bin\Debug\net5.0-windows\ImageCache\" + ImghexString, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            //using var sr = new StreamReader(fs);
            try
            {
                using (FileStream fs = new FileStream(ImgPath + ImghexString, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    using (StreamReader StreamReader = new StreamReader(fs))
                    {
                        hexfiletextv = StreamReader.ReadToEnd();
                        //Console.WriteLine(hexfiletextv);
                        StreamReader.Close();
                        StreamReader.Dispose();
                    }
                    fs.Close();
                    fs.Dispose();
                }
                //string content = await reader.ReadToEndAsync();
                //Thread.Sleep(100);

                byte[] byteArray = new byte[hexfiletextv.Length / 2];

                for (int index = 0; index < byteArray.Length; index++)
                {
                    string byteValue = hexfiletextv.Substring(index * 2, 2);
                    //Console.WriteLine(byteValue);
                    if (byteValue == "\r\n")
                    {
                        Console.WriteLine("error " + ImghexString);
                    }
                    else
                    {
                        byteArray[index] = byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
                    }
                }

                ImgFile = ImghexString.Replace(".txt", ".jpg");
                byteToBase64String = Convert.ToBase64String(byteArray, 0, byteArray.Length, Base64FormattingOptions.None);
                File.WriteAllBytesAsync(ImgPath + ImgFile, Convert.FromBase64String(byteToBase64String));
                hexfiletextv = string.Empty;
                imgJpgDate.Add(ImgFile);
                Array.Clear(byteArray, 0, byteArray.Length);
                
            }
            catch
            {
                //ConSoPt.ConsoPrint(E.ToString());
            }

            return Task.CompletedTask;
        }

        public static void FPS_Func(object sender, EventArgs e)
        {
            //while (true)
            //{

            //}
            VariableRes.fpsCount = fps;
            //ConSoPt.ConsoPrint("FPS:" + VariableRes.fpsCount.ToString());
            fps = 0;
        }
        

    }
}
