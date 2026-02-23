using OpenAI;
using OpenAI.Chat;
using System.ClientModel; // Required for ApiKeyCredential

namespace MoneyMaster.Setup
{
    public class GroqSetup
    {
        private readonly string _groqUri;
        private readonly string _groqApiKey;
        private readonly string _groqModel;

        public GroqSetup(IConfiguration config)
        {

            this._groqUri = "https://api.groq.com/openai/v1";
            this._groqApiKey = config["Groq:ApiKey"];
            this._groqModel = config["Groq:Model"] ?? "llama-3.3-70b-versatile";
        }

        public ChatClient GenerateClient()
        {

            var options = new OpenAIClientOptions
            {
                Endpoint = new Uri(_groqUri)
            };

            var client = new OpenAIClient(new ApiKeyCredential(_groqApiKey), options);

            return client.GetChatClient(_groqModel);
        }
    }
}