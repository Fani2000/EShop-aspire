using Microsoft.Extensions.AI;
using OllamaSharp.Models.Chat;
using ChatRole = Microsoft.Extensions.AI.ChatRole;

namespace Catelog.Services;

public class ProductAIService(IChatClient client)
{
    public async Task<string?> SupportAsync(string query)
    {
        var systemPrompt = """
                           You are a useful assistant.
                           You always reply with a short and funny message.
                           If you do not know an answer, you say 'I don't know that.' 
                           You only answer questions related to outdoor camping products.
                           For any other type of questions, explain to the user that you only answer outdoor camping questions.
                           At the end, offer one of our products: Hiking poles, Outdoor Rain Jacket, Outdoor Backpack, Camping Cookware, Camping Stove, Camping Lantern, Camping Tent.
                           Do not store memory of the chat conversation.
                           """;

        var chatHistory = new List<ChatMessage>
        {
            new ChatMessage(ChatRole.System, systemPrompt), 
            new ChatMessage(ChatRole.User, query)
        };

        try
        {
            var resultPrompt = await client.GetResponseAsync(chatHistory);
            
            return resultPrompt.Messages[0].Contents[0].ToString() ?? "No response from the AI.";
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"HTTP Request Error: {ex.Message}");
            return "There was an issue connecting to the AI service.";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected Error: {ex.Message}");
            return "An unexpected error occurred.";
        }
    }
}