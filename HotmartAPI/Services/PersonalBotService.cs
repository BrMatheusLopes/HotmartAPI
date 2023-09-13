using System.IO;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace HotmartAPI.Services
{
    public class PersonalBotService
    {
        public const string BotToken = "6320322272:AAHiyUAhkkugzrxqQLK5CkXv8ICh5fiT4i0";
        public const long PersonalChatId = 1045332664;

        private readonly ITelegramBotClient _botClient;

        public PersonalBotService(ITelegramBotClient botClient)
        {
            _botClient = botClient;
        }

        public async Task SendMessageAsync(string text, ParseMode parseMode = ParseMode.Html)
        {
            await _botClient.SendTextMessageAsync(PersonalChatId, 
                text: text, 
                parseMode: parseMode);
        }

        public async Task SendDocumentAsync(string data, string name, string caption = null)
        {
            var dataStream = new MemoryStream(Encoding.UTF8.GetBytes(data));
            await _botClient.SendDocumentAsync(PersonalChatId, 
                document: InputFile.FromStream(dataStream, name), 
                caption: caption);
        }
    }
}
