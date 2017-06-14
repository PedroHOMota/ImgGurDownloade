using System;
using System.Net;
using System.Xml;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.IO;

namespace consoleWebCrawler
{
    class Program
    {
        static void Main(string[] args)
        {

            WebClient client = new WebClient();
            HtmlDocument html = new HtmlDocument();
            Console.WriteLine("enter the imgur url: ");
            string url = Console.ReadLine();

            Console.WriteLine("Download starting");
            string source = client.DownloadString(url);

            html.LoadHtml(source);
            //Get all divs that contains the link to the images
            HtmlNodeCollection nodes = html.DocumentNode.SelectNodes("//div[@id='inside']/div[2]/div[2]/div[2]/div");
            string folder = url.Substring(url.LastIndexOf("/") + 1);
            try
            {
                Directory.CreateDirectory(folder);
            }
            catch (Exception)
            {
                Console.WriteLine("Folder already exists, do you wish to remove it to continue with the download?\n1-yes 2-no");
                int op = int.Parse(Console.ReadLine());
                switch (op)
                {
                    case 1: //Delete current folder
                    {
                            Directory.Delete(folder);
                            Directory.CreateDirectory(folder);
                            break;
                    }
                    default: //If the user choose anything other than 1 close the application
                        {
                            Console.WriteLine("Closing application");
                            System.Threading.Thread.Sleep(5000);
                            System.Environment.Exit(0);
                            break;
                        }
                }

            }
            for (int i = 0; i < nodes.Count-1; i++)
            {
                Console.WriteLine("Downloading {0} of {1}",i+1,nodes.Count);
                url = "http://i.imgur.com/" + nodes[i].Id + ".jpg"; //Get the unique img id and insert on the pre-formatted url to download the img
                client.DownloadFile(url, folder + "\\" + i + ".jpg");
           }
            Console.WriteLine("Finished");
            Console.ReadKey();


        }
    }
}
