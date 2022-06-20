using NUnit.Framework;
using RestSharp;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using TaskBoardRestSharpTests;

namespace RestSharpTests
{
	public class TaskBoardRestSharpTests
	{
		private RestClient clint;
		private RestRequest request;
		//enter different baseUrl
		private const string baseUrl = "https://taskboard.nakov.repl.co";
		//enter different path and rename the var
		private const string allTasks = "/api/tasks";
		private const string tasksByKeyword = "/api/tasks/search/:{keyword}";


		[SetUp]
		public void Setup()
		{
			this.clint = new RestClient(baseUrl);

		}

		[Test]
		public async Task Test_TaskBoard_Get_API_Request()
		{
			this.request = new RestRequest(allTasks);

			var response = await this.clint.ExecuteAsync(this.request, Method.Get);

			Assert.IsNotNull(response.Content);
		}

		[Test]
		public async Task Test_TaskBoard_Task_Lists()
		{
			this.request = new RestRequest(allTasks);

			var response = await this.clint.ExecuteAsync(this.request, Method.Get);

			var tasks = JsonSerializer.Deserialize<List<Tasks>>(response.Content);

			foreach (var task in tasks)
			{
				Assert.Greater(task.id, 0);
				Assert.IsNotEmpty(task.title);

			}
		}

		[Test]
		public async Task Test_TaskBoard_TaskLists_Check_Title()
		{
			this.request = new RestRequest(allTasks);
			
			var response = await this.clint.ExecuteAsync(this.request, Method.Get);

			var tasks = JsonSerializer.Deserialize<List<Tasks>>(response.Content);

			foreach (var task in tasks)
			{
				if (task.id == 1)
				{
					Assert.That(task.title.Contains("Project"));
					break;
				}
			}
		}

		[Test]
		public async Task Test_TaskBoard_FindTasks_By_Keyword()
		{
			this.request = new RestRequest(tasksByKeyword);
			request.AddUrlSegment("keyword", "home");
			var response = await this.clint.ExecuteAsync(this.request, Method.Get);

			var tasks = JsonSerializer.Deserialize<List<Tasks>>(response.Content);

			foreach (var task in tasks)
			{
				if (task.id == 1)
				{
					Assert.That(task.title.Contains("Home"));
					break;
				}
			}
		}
	}
}