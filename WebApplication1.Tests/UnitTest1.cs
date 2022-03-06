using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Xunit;

namespace WebApplication1.Tests
{
	public class UnitTest1
	{
		[Fact]
		public async Task Test()
		{
			IWebHostBuilder builder = new WebHostBuilder();
			builder.UseStartup<Startup>();
			TestServer server = new TestServer(builder);

			var client = server.CreateClient();

			var requestBody = new StringContent(JsonConvert.SerializeObject(new ClientQueryRequest()
			{
				Query = @"mutation AddBook($input: BookInput!) {
								addBook(input: $input) {
									title
									author
									genre
								}
					}",
				Variables = new Dictionary<string, object>()
				{
					{
						"input",
						new Dictionary<string, object>()
						{
							{ "title", "C# in depth"},
							{ "author", "Jon Skeet"},

							// UNCOMMENT TO MAKE TEST PASS
							// { "genre", "FICTION"} 
						}
					}
				},
				OperationName = "AddBook"
			}));
			requestBody.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
			var result =
				await client.SendAsync(new HttpRequestMessage()
				{
					Method = HttpMethod.Post,
					RequestUri = new Uri($"{client.BaseAddress}graphql"),
					Content = requestBody
				});

			var responseBody = await result.Content.ReadAsStringAsync();
			
			// assert
			Assert.True(result.IsSuccessStatusCode);
		}
	}

	public class ClientQueryRequest
	{
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("operationName")]
		public string OperationName { get; set; }

		[JsonProperty("query")]
		public string Query { get; set; }

		[JsonProperty("variables")]
		public Dictionary<string, object> Variables { get; set; }

		[JsonProperty("extensions")]
		public Dictionary<string, object> Extensions { get; set; }

		public override string ToString()
		{
			var query = new StringBuilder();

			if (Id is not null)
			{
				query.Append($"id={Id}");
			}

			if (Query is not null)
			{
				if (Id is not null)
				{
					query.Append("&");
				}
				query.Append($"query={Query.Replace("\r", "").Replace("\n", "")}");
			}

			if (OperationName is not null)
			{
				query.Append($"&operationName={OperationName}");
			}

			if (Variables is not null)
			{
				query.Append("&variables=" + JsonConvert.SerializeObject(Variables));
			}

			if (Extensions is not null)
			{
				query.Append("&extensions=" + JsonConvert.SerializeObject(Extensions));
			}

			return query.ToString();
		}
	}
}
