using Newtonsoft.Json;
using System.Text;

namespace LoggingExample.Models;

public class SplunkLogger
{
	private readonly string _splunkUrl;
	private readonly string _hecToken;

	public SplunkLogger(string splunkUrl, string hecToken)
	{
		_splunkUrl = splunkUrl;
		_hecToken = hecToken;
	}
	public async Task LogEventAsync(string message, string loglevel)
	{
		using (var client = new HttpClient())
		{
			client.Timeout = TimeSpan.FromMinutes(2);
			var payload = new
			{
				@event = message,
				logLevel = loglevel,
				sourcetype = "syslog"
			};

			var jsonPayload = JsonConvert.SerializeObject(payload);
			var request = new HttpRequestMessage(HttpMethod.Post, _splunkUrl)
			{
				Headers = { { "Authorization", $"Splunk {_hecToken}" } },
				Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json")
			};

			try
			{
				var response = await client.SendAsync(request);
				response.EnsureSuccessStatusCode();
				Console.WriteLine("Log successfully sent.");
			}
			catch (HttpRequestException e)
			{
				Console.WriteLine($"Request failed: {e.Message}");

				// HTTP cavab məzmununu əldə etmək üçün
				var response = e.Data["response"] as HttpResponseMessage;
				if (response != null)
				{
					Console.WriteLine($"Status Code: {response.StatusCode}");
					var responseBody = await response.Content.ReadAsStringAsync();
					Console.WriteLine($"Response Content: {responseBody}");
				}
			}
		}
	}

	//public async Task LogEventAsync(string message, string loglevel)
	//{
	//    var payload = new
	//    {
	//        @event = message,
	//        logLevel = loglevel,
	//        sourcetype = "syslog"
	//    };

	//    var jsonPayload = JsonConvert.SerializeObject(payload);

	//    using (var client = new HttpClient())
	//    {
	//        var request = new HttpRequestMessage(HttpMethod.Post, _splunkUrl)
	//        {

	//            Headers =
	//            {
	//                { "Authorization", $"Splunk {_hecToken}" }
	//            },
	//            Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json"),
	//            Version = new Version(2, 0)
	//        };

	//        var response = await client.SendAsync(request);
	//        response.EnsureSuccessStatusCode();
	//    }
	//}
	//public async Task LogEventAsync(string message, string loglevel)
	//{
	//	using (var client = new HttpClient())
	//	{
	//		client.Timeout = TimeSpan.FromMinutes(2); // Timeout müddətini artırın
	//		var payload = new
	//		{
	//			@event = message,
	//			logLevel = loglevel,
	//			sourcetype = "syslog"

	//		};

	//		var jsonPayload = JsonConvert.SerializeObject(payload);
	//		var request = new HttpRequestMessage(HttpMethod.Post, _splunkUrl)
	//		{
	//			Headers = { { "Authorization", $"Splunk {_hecToken}" } },
	//			Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json")
	//		};

	//		try
	//		{
	//			var response = await client.SendAsync(request);
	//			response.EnsureSuccessStatusCode();
	//			Console.WriteLine("Log successfully sent.");
	//		}
	//		catch (HttpRequestException e)
	//		{
	//			Console.WriteLine($"Request failed: {e.Message}");

	//			// HTTP cavab məzmununu əldə etmək üçün
	//			var response = e.Data["response"] as HttpResponseMessage;
	//			if (response != null)
	//			{
	//				Console.WriteLine($"Status Code: {response.StatusCode}");
	//				var responseBody = await response.Content.ReadAsStringAsync();
	//				Console.WriteLine($"Response Content: {responseBody}");
	//			}
	//		}
	//	}

	//}

}