using System.Net.Http.Headers;
using System.Text.Json;

public class GithubHelper
{
    private static HttpClient HttpClient = new HttpClient();

    public static async Task<string> GetUserEmailGithub(string token, string username)
	{
		// Adapted from following resources
		// https://learn.microsoft.com/en-us/dotnet/fundamentals/networking/http/httpclient
		// https://docs.github.com/en/rest/users/emails?apiVersion=2022-11-28
		using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://api.github.com/user/emails"))
		{
			requestMessage.Headers.Add("Accept", "application/vnd.github+json");
			requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
			requestMessage.Headers.Add("X-GitHub-Api-Version", "2022-11-28");

			// https://docs.github.com/en/rest/overview/resources-in-the-rest-api?apiVersion=2022-11-28#user-agent-required
			requestMessage.Headers.Add("User-Agent", username);

			try {
				HttpResponseMessage response = await HttpClient.SendAsync(requestMessage);
				
				if (response.IsSuccessStatusCode)
				{
					var emailsJson = await response.Content.ReadAsStringAsync();
                    var emails = JsonSerializer.Deserialize<List<EmailInfo>>(emailsJson);
                    var primaryEmail = emails?
                        .Where(e => e.primary == true)
                        .Select(e => e.email)
                        .SingleOrDefault();

					return primaryEmail ?? "";
				}
				else
				{
					Console.WriteLine($"An error occured getting user email: Error code {response.StatusCode}");
					return "";
				}
			} 
			catch (Exception e)
			{
				Console.WriteLine($"An error occured ${e.Message}");
				return "";
			}
		}
	}

    private record EmailInfo(string email, bool primary);
}
