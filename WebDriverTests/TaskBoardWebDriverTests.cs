using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Linq;

namespace WebDriverTests
{
	public class TaskBoardWebDriverTests
	{
		private WebDriver driver;
		private const string url = "https://taskboard.nakov.repl.co/";

		[SetUp]
		public void OpenAndNavigate()
		{
			this.driver = new ChromeDriver();
			driver.Url = url;
			driver.Manage().Window.Maximize();
		}

		[TearDown]
		public void ShutDown()
		{
			driver.Quit();
		}

		[Test]
		public void Test_TaskBoard_Done_Section()
		{
			var taskBoardButton = driver.FindElement(By.CssSelector("a:nth-of-type(1) > .icon"));

			taskBoardButton.Click();

			var doneTable = driver.FindElements(By.CssSelector("div:nth-of-type(3) > h1"));

			foreach (var task in doneTable)
			{
				if (task.Text.Contains("Project"))
				{
					var actualTitle = driver.FindElement(By.CssSelector("div:nth-of-type(3) > table:nth-of-type(1)  .title > td"));
					Assert.That(actualTitle, Is.EqualTo("Project skeleton"));
					break;
				}
			}

		}

		[Test]
		public void Test_TaskBoard_Section()
		{
			var taskBoardButton = driver.FindElement(By.CssSelector("a:nth-of-type(1) > .icon"));

			taskBoardButton.Click();

			var title = driver.FindElement(By.CssSelector("div:nth-of-type(3) > table:nth-of-type(1)  .title > td")).Text;

			Assert.That(title, Is.EqualTo("Project skeleton"));
		}

		[Test]
		public void Test_Search_Existing_Task()
		{
			var searchTasksButton = driver.FindElement(By.CssSelector("a:nth-of-type(3) > .icon"));
			searchTasksButton.Click();

			var keywordField = driver.FindElement(By.Id("keyword"));
			var searchButton = driver.FindElement(By.Id("search"));

			keywordField.SendKeys("home");
			searchButton.Click();

			var actualTitle = driver.FindElement(By.CssSelector(".title > td")).Text;
			var expectedTitle = "Home page";

			Assert.AreEqual(expectedTitle, actualTitle);
		}

		[Test]
		public void Test_Search_Not_Existing_Task()
		{
			var searchTasksButton = driver.FindElement(By.CssSelector("a:nth-of-type(3) > .icon"));
			searchTasksButton.Click();

			var keywordField = driver.FindElement(By.Id("keyword"));
			var searchButton = driver.FindElement(By.Id("search"));

			keywordField.SendKeys("missing{randnum}” ");
			searchButton.Click();

			var actualSearchResult = driver.FindElement(By.Id("searchResult")).Text;
			var expectedTitle = "No tasks found.";

			Assert.AreEqual(actualSearchResult, expectedTitle);
		}

		[Test]
		public void Test_Create_NewTask_With_InvalidData()
		{
			var newTaskButton = driver.FindElement(By.CssSelector("a:nth-of-type(2) > span:nth-of-type(2)"));
			newTaskButton.Click();

			var createButton = driver.FindElement(By.Id("create"));
			createButton.Click();

			var actualError = driver.FindElement(By.CssSelector(".err")).Text;
			var expectedError = "Error: Title cannot be empty!";

			Assert.That(actualError, Is.EqualTo(expectedError));
		}

		[Test]
		public void Test_Create_NewTask_With_ValidData()
		{
			var newTaskButton = driver.FindElement(By.CssSelector("a:nth-of-type(2) > span:nth-of-type(2)"));
			newTaskButton.Click();

			var titleField = driver.FindElement(By.Id("title"));
			var uniqueTitle = "NewTaskTitle" + DateTime.Now.Ticks.ToString();
			
			titleField.SendKeys(uniqueTitle);
			
			var createButton = driver.FindElement(By.Id("create"));
			createButton.Click();

			var openIssues = driver.FindElements(By.CssSelector("div:nth-of-type(1) > h1"));

			foreach (var task in openIssues)
			{
				if (task.Text.StartsWith("NewTaskTitle"))
				{
					Assert.That(task.Text.Contains(uniqueTitle));
					break;
				}
			}

		}
	}
}