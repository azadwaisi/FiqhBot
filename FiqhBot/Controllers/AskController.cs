using FiqhBot.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;

namespace FiqhBot.Controllers
{
	public class AskController : Controller
	{
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        public AskController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }
        public IActionResult Index()
        {
            var model = new ChatViewModel
            {
                Messages = GetMessagesFromSession()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(ChatViewModel model)
        {
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(model.Message))
            {
                model.Messages = GetMessagesFromSession();
                return View("Index", model);
            }

            // اضافه کردن پیام کاربر به سشن
            AddMessageToSession(model.Message, true);

            try
            {
                // ارسال درخواست به Google AI Studio
                var aiResponse = await SendToGoogleAI(model.Message);

                // اضافه کردن پاسخ AI به سشن
                AddMessageToSession(aiResponse, false);
            }
            catch (Exception ex)
            {
                // در صورت خطا، پیام خطا را اضافه کن
                AddMessageToSession($"خطا در دریافت پاسخ: {ex.Message}", false);
            }

            return RedirectToAction("Index");
        }

        private async Task<string> SendToGoogleAI(string message)
        {
            var httpClient = _httpClientFactory.CreateClient();

            // دریافت API Key از appsettings.json
            var apiKey = _configuration["GoogleAI:ApiKey"];

            if (string.IsNullOrEmpty(apiKey))
            {
                throw new Exception("API Key تنظیم نشده است");
            }

            // آدرس API Google Gemini
            var apiUrl = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={apiKey}";

            // ساخت درخواست
            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = message }
                        }
                    }
                }
            };

            var jsonContent = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // ارسال درخواست
            var response = await httpClient.PostAsync(apiUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"خطا در API: {response.StatusCode} - {errorContent}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var aiResponse = JsonSerializer.Deserialize<GoogleAIResponse>(responseContent);

            return aiResponse?.candidates?.FirstOrDefault()?.content?.parts?.FirstOrDefault()?.text ?? "پاسخی دریافت نشد";
        }

        private List<ChatMessage> GetMessagesFromSession()
        {
            var messagesJson = HttpContext.Session.GetString("ChatMessages");
            if (string.IsNullOrEmpty(messagesJson))
            {
                return new List<ChatMessage>();
            }

            return JsonSerializer.Deserialize<List<ChatMessage>>(messagesJson) ?? new List<ChatMessage>();
        }

        private void AddMessageToSession(string message, bool isFromUser)
        {
            var messages = GetMessagesFromSession();
            messages.Add(new ChatMessage
            {
                Text = message,
                IsFromUser = isFromUser,
                Timestamp = DateTime.Now
            });

            var messagesJson = JsonSerializer.Serialize(messages);
            HttpContext.Session.SetString("ChatMessages", messagesJson);
        }
    }

    // کلاس‌های کمکی برای پاسخ Google AI
    public class GoogleAIResponse
    {
        public Candidate[]? candidates { get; set; }
    }

    public class Candidate
    {
        public Content? content { get; set; }
    }

    public class Content
    {
        public Part[]? parts { get; set; }
    }

    public class Part
    {
        public string? text { get; set; }
    }
}
