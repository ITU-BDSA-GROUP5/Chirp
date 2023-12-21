using System.Net.Http.Headers;
using System.Text.Json;

/// <summary>
/// Used to retrieve a user's email using the GitHub API
/// </summary>
public class GithubHelper
{
	private static HttpClient HttpClient = new HttpClient();

	/// <param name="token">Identity provider access token</param>
	/// <param name="username">GitHub username</param>
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

			HttpResponseMessage response = await HttpClient.SendAsync(requestMessage);

			if (!response.IsSuccessStatusCode)
			{
				throw new Exception($"An error occurred getting user email: Error code {response.StatusCode}");
			}

			var emailsJson = await response.Content.ReadAsStringAsync();
			var emails = JsonSerializer.Deserialize<List<EmailInfo>>(emailsJson);
			var primaryEmail = emails?
				.Where(e => e.primary == true)
				.Select(e => e.email)
				.SingleOrDefault();

			return primaryEmail ?? throw new Exception("Error reading email or no primary email");
		}
	}

	private record EmailInfo(string email, bool primary);
}
