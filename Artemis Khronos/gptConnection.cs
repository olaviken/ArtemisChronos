using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.IO;
using System.Windows;
using Newtonsoft.Json.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace Artemis_Chronos
{
    internal class gptConnection
    {
        public static string getApiKey()
        {
            try
            {
                var config = new ConfigurationBuilder().AddUserSecrets<Artemis_Chronos.gptConnection>().Build();
                string ApiKey = config["OpenAI:ApiKey"];

                if (string.IsNullOrEmpty(ApiKey))
                {
                    throw new InvalidOperationException("API key is missing in user secrets");
                }

                return ApiKey;
            }
            catch(Exception ex)
            {
                MessageBox.Show($"API key error: can not retrieve API key: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
            
        }

        public static void setApiKeyLocally(string ApiKey)
        {
            try
            {
                string SecretsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Microsoft", "UserSecrets", "<OpenAI:ApiKey>", "secrets.json");
                JObject secrets;

                if (File.Exists(SecretsPath))
                {
                    string Content = File.ReadAllText(SecretsPath);
                    secrets = JsonConvert.DeserializeObject<JObject>(Content) ?? new JObject();
                }
                else
                {
                    secrets = new JObject();
                }

                secrets["OpenAI:ApiKey"] = ApiKey;
                File.WriteAllText(SecretsPath, secrets.ToString());
                MessageBox.Show("API key has been successfully saved", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show($"Access Denied: \n You do not have permission to modify the secrets file. \n\n Details: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
            catch (Exception ex)
            {
                MessageBox.Show("I / O Error: \n An error occurred while accessing the secrets file. \n\n Details: { ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }

        public static async Task<string> sendPromptToChatGPT(string prompt, int MaxTokens)
        { 
            string ApiKey = getApiKey();
            string ChatGptEndpoint = "https://api.openai.com/v1/chat/completions";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization= new AuthenticationHeaderValue("Bearer", ApiKey);
                var requestBody = new
                {
                    model = "gpt-4",
                    messages = new[]
                    { 
                        new {role="user", content=prompt}
                    },
                    max_tokens= MaxTokens
                };

                try
                {
                    string JsonBody = JsonConvert.SerializeObject(requestBody);
                    StringContent content = new StringContent(JsonBody, Encoding.UTF8, "application/json");


                    HttpResponseMessage response = await client.PostAsync(ChatGptEndpoint, content);
                    response.EnsureSuccessStatusCode();

                    string ResponseJson = await response.Content.ReadAsStringAsync();
                    dynamic responseObject = JsonConvert.DeserializeObject(ResponseJson);
                    string ChatResponse = responseObject.choices[0].message.content;
                    return ChatResponse;
                }
                catch(JsonException ex)
                {
                    MessageBox.Show($"JSON serialization Error: \n {ex.Message} \n Ensure the request body is correctly structured", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    throw;
                }
                catch(HttpRequestException ex)
                {
                    MessageBox.Show($"Network error while sending to ChatGpt: \n {ex.Message} \n Please check your internet connection and the API endpoint.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    throw;
                }
                catch(Exception ex)
                {
                    MessageBox.Show($"Unexpected Error: \n An unexpected error has occurred. \n Details: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    throw;
                }
            }
        }

        public static async Task<string> downloadImageFromDalle(string Prompt, string DownloadPath)
        {
            DateTime today = DateTime.Now;
            string Today = today.ToString("yyyy/MM/dd");
            string ApiKey = getApiKey();
            string DalleEndpoint = "https://api.openai.com/v1/images/generations";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApiKey);
                var requestBody = new
                {
                    prompt = Prompt,
                    n = 1,
                    size = "1024x1024"
                };

                try
                {
                    string JsonBody = JsonConvert.SerializeObject(requestBody);
                    StringContent content = new StringContent(JsonBody, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(DalleEndpoint, content);

                    string ResponseJson = await response.Content.ReadAsStringAsync();
                    dynamic responseObject = JsonConvert.DeserializeObject(ResponseJson);
                    response.EnsureSuccessStatusCode();

                    string ImageUrl = responseObject.data[0].url;
                    using (HttpClient imageClient = new HttpClient())
                    {
                        byte[] imageBytes = await imageClient.GetByteArrayAsync(ImageUrl);
                        string FilePath = Path.Combine(DownloadPath, $"{Today}_Artemis_Cronos.png");
                        await File.WriteAllBytesAsync(FilePath, imageBytes);
                        return FilePath;
                    }
                }
                catch (JsonException ex)
                {
                    MessageBox.Show($"JSON serialization Error: \n {ex.Message} \n Ensure the request body is correctly structured", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    throw;
                }
                catch (HttpRequestException ex)
                {
                    MessageBox.Show($"Network error while sending to ChatGpt: \n {ex.Message} \n Please check your internet connection and the API endpoint.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    throw;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Unexpected Error: \n An unexpected error has occurred. \n Details: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    throw;
                }
            }
        }

    }
}
