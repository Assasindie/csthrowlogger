using Discord.Webhook;
using System;
using System.IO;

namespace csthrowlogger
{
    class webhook
    {

        public static void SendMessage(string message)
        {
            try
            {
                DiscordWebhookClient discord = new DiscordWebhookClient(544040123864711181, "25UL8swXTHQuZOLlP3lXlx9NIWqkwVU_QM9EDqo8EvgJUkm0DrvJd4SGbXApygiE3kMo");
                discord.SendMessageAsync(message);
            }
            catch(Exception)
            {
                Console.WriteLine("OwO Something went wrong sending the information to discord! Check to see if your webhook credentials are correct! Press any key to exit");
                Console.ReadKey();
                Environment.Exit(0);
            }

        }
    }
}