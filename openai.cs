using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media.Animation;
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Images;
using OpenAI_API.Models;
namespace Cure_WPF
{
    internal class Openai
    {
        public static string apikey = "sk-proj-qo71ZyOS2ra4UkO6hWDqT3BlbkFJ3tdAK2PM6YFg9vdVIU4n";
        public static OpenAIAPI openai = new OpenAIAPI(apikey);
        public static OpenAI_API.Chat.Conversation chatgpt = openai.Chat.CreateConversation();
        public string SystemMessage { get; set; }
        public string UserRequest {  get; set; }
        public string ImagePrompt { get; set; }
        public async Task<string> MakeRequest()
        {
            chatgpt.Model=Model.ChatGPTTurbo_1106;
            chatgpt.AppendSystemMessage(SystemMessage);
            chatgpt.AppendUserInput(UserRequest);
            var respond=await chatgpt.GetResponseFromChatbotAsync();
            return respond;
        }
        public async Task<string> GetImage()
        {
            ImageGenerationRequest request = new ImageGenerationRequest() {Prompt=$"generate health images based on: {ImagePrompt}" };
            var image=await openai.ImageGenerations.CreateImageAsync(request);
            return image.ToString();
        }
       
    }
}
