using Discord.Webhook;
using System;
using System.IO;
using DotNetEnv;

namespace csthrowlogger
{
    class Webhook
    {
        static string token;
        static string webhookID;

        public static void ChangeDir()
        {
            Environment.CurrentDirectory = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\"));
            try { Env.Load(); }
            catch (Exception)
            {
                Console.WriteLine("Cannot find the .env file :(! Press any key to exit");
                Console.ReadKey();
                Environment.Exit(0);
            }
            token = Environment.GetEnvironmentVariable("WEBHOOK_ID");
            webhookID = Environment.GetEnvironmentVariable("WEBHOOK_TOKEN");
        }

        public static void SendMessage(string message)
        {
            try
            {
                DiscordWebhookClient discord = new DiscordWebhookClient(Convert.ToUInt64(token), webhookID);
                discord.SendMessageAsync(message);
            }   
            catch (Exception)
            {
                Console.WriteLine("OwO Something went wrong sending the information to discord! Check to see if your webhook credentials are correct! Press any key to exit");
                Console.ReadKey();
                Environment.Exit(0);
            }
        }
    }
}