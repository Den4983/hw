using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _20210216_http
{
    #region MyRegion
    //class Program
    //{
    //    static void Main(string[] args)
    //    {
    //        HttpWebRequest req = HttpWebRequest.Create("https://itstep.zp.ua/ru") as HttpWebRequest;
    //        req.Method = "POST";

    //        req.Headers.Add(HttpRequestHeader.AcceptLanguage, "ru-ru");
    //        req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/88.0.4324.150 Safari/537.36";
    //        req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";

    //        foreach(var h in req.Headers)
    //            Console.WriteLine($"{h}: {req.Headers[h.ToString()]}");


    //        HttpWebResponse res = req.GetResponse() as HttpWebResponse;

    //        using (var fstream = File.Open("index.html", FileMode.OpenOrCreate))
    //        {
    //            res.GetResponseStream().CopyTo(fstream);
    //        }

    //        //using (StreamReader reader = new StreamReader(res.GetResponseStream(), Encoding.UTF8))
    //        //{
    //        //    Console.WriteLine(reader.ReadToEnd());
    //        //}

    //    }
    //} 
    #endregion


    class Program
    {
        static void Main()
        {
            HttpWebRequest request = HttpWebRequest.Create("https://google.com") as HttpWebRequest;
            request.Method = "GET";

            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "ru-ru");
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/88.0.4324.150 Safari/537.36";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

            using (var fstream = File.Open("index.html", FileMode.OpenOrCreate))
            {
                response.GetResponseStream().CopyTo(fstream);
            }

            HttpListener server = new HttpListener();
            server.Prefixes.Add("http://localhost:8080/");
            server.Start();

            Console.WriteLine("Wait for request...");
            var ctx = server.GetContext(); // blocking

            var res = ctx.Response;

            using (var fstream = File.Open("index.html", FileMode.Open))
            {
                Stream stream = res.OutputStream;

                fstream.CopyTo(stream);

                stream.Close();
            }

            Thread.Sleep(1000);//а без этого не работает...

            server.Stop();
            Console.WriteLine("Server stoped...");
        }
    }
}
