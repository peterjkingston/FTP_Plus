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

            Console.Write("Username-> ");
            string username = Console.ReadLine();

            Console.Write("Password-> ");
            SecureString password = GetPassword();

            Console.WriteLine("\nFetching peterjkingston.com\n");

            FTPConnection connection = new FTPConnection("peterjkingston.com", username, password);
            connection.FailedLogon += DisplayMessage;
            FolderNode mainNodeFTP = new FolderNode(connection);

            string localDirectory = @"C:\users\peter\Desktop\httpdocs";
            FolderNode mainNodeLocal = new FolderNode(localDirectory);
            FileSystemWatcher watcher = new FileSystemWatcher(localDirectory);
            
            Console.ReadKey(true);
        }

        static SecureString GetPassword()
        {
            SecureString pass = new SecureString();
            int keyPosition = 0;

            ConsoleKey escapeKey = ConsoleKey.Enter;
            ConsoleKey deleteKey = ConsoleKey.Delete | ConsoleKey.Backspace;

            bool escape = false;
            
            do
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if(key.Key != escapeKey)
                {
                    if(key.Key != deleteKey)
                    {
                        Console.Write("*");
                        pass.InsertAt(keyPosition++, key.KeyChar);
                    }
                    else
                    {
                        //Suppose to backspace... doesn't.
                        Console.Write("\b");
                        pass.InsertAt(keyPosition--, '\b');
                    }
                }

                escape = key.Key == escapeKey ? true : false;
            } while (!escape);

            return pass;
        }

        static void DisplayMessage(object sender, string message)
        {
            Console.WriteLine(message);
        }
    }
}
