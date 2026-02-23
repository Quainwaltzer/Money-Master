
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using OpenAI.Chat;
using System.Security.Cryptography.X509Certificates;

namespace MoneyMaster.Components.Pages
{
    public partial class MoneyMasterHome
    {

        private string _userInput = "";
        private string _streamingText = "";
        private bool _isStreaming = false;
        private bool shouldPrevent = false;
        private ElementReference chatContainer;

        private List<ChatMessage> _messages = new()
        {
            new SystemChatMessage("You are a general chatbot, just do anything")
        };
        
        public async Task StartStreaming()
        {
            _isStreaming = true;
            _streamingText = "";

            var userSays = _userInput;
            _userInput = "";

            StateHasChanged();
            
            _messages.Add(new UserChatMessage(userSays));

            try
            {
                var updates = GroqClient.CompleteChatStreamingAsync(_messages);
                await foreach (var update in updates)
                {
                    foreach(var part in update.ContentUpdate)
                    {
                        _streamingText += part.Text;
                        StateHasChanged();
                        await Task.Delay(100);
                    }
                }

                _messages.Add(new AssistantChatMessage(_streamingText));
            }
            catch (Exception ex)
            {
                _messages.Add(new SystemChatMessage($"Error: {ex.Message}"));
            }
            finally
            {
                _isStreaming = false;
                _streamingText = "";
                StateHasChanged();
            }

            
        }

        public async Task HandleEvent(KeyboardEventArgs e)
        {
            if(e.Key == "Enter" && !e.ShiftKey)
            {
                shouldPrevent = true;
                if (!string.IsNullOrWhiteSpace(_userInput))
                {
                 await StartStreaming();
                }
            }
            else
            {
                shouldPrevent = false;
            }
        }
    }
}
