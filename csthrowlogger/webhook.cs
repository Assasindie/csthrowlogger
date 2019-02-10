using Discord.Webhook;
using System.IO;

namespace csthrowlogger
{
    class webhook
    {
        static DiscordWebhookClient discord = new DiscordWebhookClient(544040123864711181, "25UL8swXTHQuZOLlP3lXlx9NIWqkwVU_QM9EDqo8EvgJUkm0DrvJd4SGbXApygiE3kMo");

        public static void SendMessage(string message)
        {
            discord.SendMessageAsync(message);
        }
    }
}