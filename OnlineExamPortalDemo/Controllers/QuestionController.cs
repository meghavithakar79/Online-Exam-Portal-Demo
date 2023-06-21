using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.JSInterop;
using OnlineExamPortalDemo.Data;
using OnlineExamPortalDemo.Models;
using OnlineExamPortalDemo.ViewModel;
using System.Collections;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace OnlineExamPortalDemo.Controllers
{

	public class QuestionController : Controller
	{
		private readonly ExamDbContext mvcDemoDbContext;
		private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
		private readonly ISession _session;
		private readonly IConfiguration _configuration;
		private readonly IHttpContextAccessor _httpContextAccessor;
		public QuestionController(ExamDbContext mvcDemoDbContext, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
		{
			this.mvcDemoDbContext = mvcDemoDbContext;
			_hostingEnvironment = hostingEnvironment;
			_session = httpContextAccessor.HttpContext.Session;
			_configuration = configuration;
			_httpContextAccessor = httpContextAccessor;
		}
		// GET: Questions
		[Authorize(Roles = "admin")]
		public async Task<ActionResult> Index()
		{
			var que = await mvcDemoDbContext.question.ToListAsync();
			return View(que);
		}
		// GET: QuestionController/Create
		[Authorize(Roles = "admin")]
		public IActionResult create()
		{
			return View();
		}
		// POST: QuestionController/Create
		[Authorize(Roles = "admin")]
		[HttpPost]
		public async Task<IActionResult> create(Question model, List<IFormFile> files)
		{
			string comb = null;
			string webroot = _hostingEnvironment.WebRootPath;
			foreach (var file in files)
			{
				if (file != null && file.Length > 0)
				{
					var fileName = file.FileName;
					var filePath = Path.Combine(webroot, "Uploads", fileName);
					using (var stream = new FileStream(filePath, FileMode.Create))
					{
						await file.CopyToAsync(stream);
					}
					comb = comb + "/Uploads/" + fileName + "";
				}
			}
			model.Filepath = comb;
			var que = new Question()
			{
				Question1 = model.Question1,
				Option1 = model.Option1,
				Option2 = model.Option2,
				Option3 = model.Option3,
				Option4 = model.Option4,
				Correctans = model.Correctans,
				Filepath = model.Filepath
			};
			await mvcDemoDbContext.question.AddAsync(que);
			await mvcDemoDbContext.SaveChangesAsync();
			return RedirectToAction("Index");
		}
		// GET:edit
		[Authorize(Roles = "admin")]
		public async Task<ActionResult> Edit(int id)
		{
			var que = await mvcDemoDbContext.question.FirstOrDefaultAsync(o => o.Id == id);
			if (que != null)
			{
				string ext = Path.GetExtension(que.Filepath);
				var viewModel = new Question()
				{
					Id = que.Id,
					Question1 = que.Question1,
					Option1 = que.Option1,
					Option2 = que.Option2,
					Option3 = que.Option3,
					Option4 = que.Option4,
					Correctans = que.Correctans,
					Filepath = que.Filepath,
					ext = ext
				};
				return View(viewModel);
			}
			return RedirectToAction("Index");
		}
		// POST: edit
		[HttpPost]
		[Authorize(Roles = "admin")]
		public async Task<ActionResult> Edit(Question model, List<IFormFile> files)
		{
			string comb = null;
			string webroot = _hostingEnvironment.WebRootPath;
			foreach (var file in files)
			{
				if (file != null && file.Length > 0)
				{
					var fileName = file.FileName;
					var filePath = Path.Combine(webroot, "Uploads", fileName);
					using (var stream = new FileStream(filePath, FileMode.Create))
					{
						await file.CopyToAsync(stream);
					}
					comb = comb + "/Uploads/" + fileName + "";
				}
			}
			model.Filepath = comb;
			var que = await mvcDemoDbContext.question.FindAsync(model.Id);
			string ext = Path.GetExtension(que.Filepath);
			if (que != null)
			{
				que.Question1 = model.Question1;
				que.Option1 = model.Option1;
				que.Option2 = model.Option2;
				que.Option3 = model.Option3;
				que.Option4 = model.Option4;
				que.Correctans = model.Correctans;
				que.Filepath = model.Filepath;
				que.ext = model.ext;
				await mvcDemoDbContext.SaveChangesAsync();
				return RedirectToAction("Index");
			}
			return RedirectToAction("Index");
		}
		//delete question
		[Authorize(Roles = "admin")]
		public async Task<IActionResult> Delete(int id)
		{
			var delRecord = mvcDemoDbContext.question.FirstOrDefault(x => x.Id == id);

			if (delRecord != null)
			{
				mvcDemoDbContext.Remove(delRecord);
				await mvcDemoDbContext.SaveChangesAsync();
				TempData["Success"] = "Data Deteted Successfully";
			}
			return RedirectToAction("Index");
		}
		//get user Index of Questions
		[HttpGet]
		[Authorize(Roles = "user")]
		public async Task<ActionResult<PageList<Question>>> userquestions([FromQuery] Question model)
		{

			var query = mvcDemoDbContext.question.AsQueryable();
			var questions = await PageList<Question>.ToPagedList(query, model.PageNumber, model.PageSize);
			return View(questions);
		}
		//user Instruction
		[Authorize(Roles = "user")]
		public async Task<IActionResult> Instruction()
		{
			return View();
		}
		//user exam selection
		[Authorize(Roles = "user")]
		public async Task<IActionResult> ExamSelection()
		{
			return View();
		}
		//nextbtn
		public IActionResult IsAnswerCorrect(int id, string answer, int userId)
		{
			List<UserAnswer> userAnswers;
			if (!HttpContext.Session.TryGetValue("userAnswers", out byte[] value))
			{
				userAnswers = new List<UserAnswer>();
			}
			else
			{
				userAnswers = JsonSerializer.Deserialize<List<UserAnswer>>(value);
			}
			UserAnswer userans = new UserAnswer
			{
				UserId = userId,
				QuestionId = id,
				SelectedAnswer = answer
			};
			UserAnswer recordToRemove = userAnswers.FirstOrDefault(u => u.QuestionId == id);
			if (recordToRemove != null)
			{
				userAnswers.Remove(recordToRemove);
			}
			userAnswers.Add(userans);
			HttpContext.Session.SetString("userAnswers", JsonSerializer.Serialize(userAnswers));
			id = id + 1;
			string previousUserAnswer = userAnswers.FirstOrDefault(x => x.QuestionId == id)?.SelectedAnswer;
			if (previousUserAnswer != null)
			{
				HttpContext.Session.SetString("uans", previousUserAnswer);
			}
			return Json(true);
		}
		//prevButton
		public IActionResult PreviousPage(int id)
		{
			byte[] sessionData = HttpContext.Session.Get("userAnswers");
			if (sessionData != null)
			{
				List<UserAnswer> userAnswers = JsonSerializer.Deserialize<List<UserAnswer>>(sessionData);
				if (userAnswers != null && userAnswers.Count > 0)
				{
					string previousUserAnswer = userAnswers.FirstOrDefault(x => x.QuestionId == id)?.SelectedAnswer;
					if (previousUserAnswer != null)
					{
						HttpContext.Session.SetString("uans", previousUserAnswer);
					}
					HttpContext.Session.SetString("userAnswers", JsonSerializer.Serialize(userAnswers));
				}
			}
			return Json(true);
		}
		//resultPage
		public ActionResult ResultView()
		{
			var quizResults = mvcDemoDbContext.QuizResults.OrderBy(x => x.Id).LastOrDefault();
			return View(quizResults);
		}
		//[HttpPost]
		//submitButton
		public ActionResult SubmitQuiz(int id, string answer, int userId)
		{
			List<UserAnswer> userAnswers;
			if (!HttpContext.Session.TryGetValue("userAnswers", out byte[] value))
			{
				userAnswers = new List<UserAnswer>();
			}
			else
			{
				userAnswers = JsonSerializer.Deserialize<List<UserAnswer>>(value);
			}
			UserAnswer userans = new UserAnswer
			{
				UserId = userId,
				QuestionId = id,
				SelectedAnswer = answer
			};
			userAnswers.Add(userans);
			HttpContext.Session.SetString("userAnswers", JsonSerializer.Serialize(userAnswers));
			id = id + 1;
			string previousUserAnswer = userAnswers.FirstOrDefault(x => x.QuestionId == id)?.SelectedAnswer;
			if (previousUserAnswer != null)
			{
				HttpContext.Session.SetString("uans", previousUserAnswer);
			}
			DataTable dataTable = new DataTable();
			dataTable.Columns.Add("Id", typeof(int));
			dataTable.Columns.Add("UserId", typeof(int));
			dataTable.Columns.Add("QuestionId", typeof(int));
			dataTable.Columns.Add("SelectedAnswer", typeof(string));
			foreach (UserAnswer userAnswer in userAnswers)
			{
				DataRow dataRow = dataTable.NewRow();
				dataRow["Id"] = userAnswer.Id;
				dataRow["UserId"] = userAnswer.UserId;
				dataRow["QuestionId"] = userAnswer.QuestionId;
				dataRow["SelectedAnswer"] = userAnswer.SelectedAnswer;
				dataTable.Rows.Add(dataRow);
			}
			string ConnectionString = "server=DESKTOP-VQTK0O0\\SQLEXPRESS; database=ExamDb; Trusted_connection=true; TrustServerCertificate=True";
			using (SqlConnection connection = new SqlConnection(ConnectionString))
			{
				connection.Open();
				using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
				{
					bulkCopy.DestinationTableName = "dbo.UserAnswers";
					bulkCopy.WriteToServer(dataTable);
				}
			}
			id = id - 1;
			int score = mvcDemoDbContext.UserAnswers
			.Join(mvcDemoDbContext.question,
				ua => ua.QuestionId,
				q => q.Id,
				(ua, q) => new { UserAnswer = ua, Question = q })
			.Where(uq => uq.UserAnswer.UserId == userId && uq.UserAnswer.SelectedAnswer == uq.Question.Correctans)
			.Count();
			QuizResult quizResult = new QuizResult
			{
				UserId = userId,
				Score = score
			};
			mvcDemoDbContext.QuizResults.Add(quizResult);
			mvcDemoDbContext.SaveChanges();
			return Json(true);
		}
	}
}
