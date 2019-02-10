using Discord.Webhook;
using System;
using System.IO;

namespace csthrowlogger
{
    class Webhook
    {

        public static void SendMessage(string message)
        {
            try
            {
                DiscordWebhookClient discord = new DiscordWebhookClient(544084693658894337, "JgcxpJtbO7PbhgDhSv0C86QaXKw_Z5UCgiFC6OmXaVOETCZ54HFjYzsdx9DTASSAqEHa");
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