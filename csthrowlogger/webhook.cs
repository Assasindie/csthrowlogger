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
                DiscordWebhookClient discord = new DiscordWebhookClient(544127788039536674, "xQYThRwfC6DoOEnRKSNoF5yrppKQWlrgfB11MF28negvok-uFS2SSfl6lHl4jJucOYSH");
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