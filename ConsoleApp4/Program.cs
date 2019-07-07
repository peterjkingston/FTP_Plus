using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security;
using FTP_Plus;

namespace ConsoleApp4
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to FTP Plus!");

            Console.WriteLine("Username->");
            string username = Console.ReadLine();

            Console.WriteLine("Passowrd->");
            SecureString password = GetPassword();

            Console.WriteLine("\nFetching peterjkingston.com\n");

            FTPConnection connection = new FTPConnection("peterjkingston.com", username, password);
            FolderNode mainNode = new FolderNode(connection);
            
            Console.ReadKey(true);
        }

        static SecureString GetPassword()
        {
            SecureString pass = new SecureString();
            int keyPosition = 0;

            ConsoleKey escapeKey = ConsoleKey.Enter;
            bool escape = false;
            
            do
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if(key.Key != escapeKey)
                {
                    Console.Write("*");
                    pass.InsertAt (keyPosition++, key.KeyChar);
                }

                escape = key.Key == escapeKey ? true : false;
            } while (!escape);

            return pass;
        }
    }
}
