using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;
using test.Models;

namespace test.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(string anw)
        {

            return View("Index",anw);
        }
        [HttpGet]
        public string CheckQuery( string Query,string Symbol,string Count)
        {
            string answer = "";
            if(Symbol==null || Symbol == "")
            {
                answer = "Не указен символ который нужно искать";
                return answer;
            }


            if (Count == null || Count == "")
            {
                answer = "Не указен количество ";
                return answer;
            }

            if(Query==null || Query == "")
            {
                answer = "Не указена строка в которой проверять ";
                return answer;
            }
            

            var grouped = Query
                        .GroupBy(x => x)
                        .Select(x => new {
                                x.Key,
                                Count = x.Count(),
                                });

            grouped = grouped.Where(x => x.Key.ToString()==Symbol);

            var CountInText= grouped.Count();

            if (CountInText > Convert.ToInt32(Count) || CountInText < 1)
            {
                answer = "Строка не валидна, количество символов " + CountInText + " больше допустимого " + Count;
            }
            else
            {
                answer = " Количество " + Symbol + " в строке -" + CountInText; 
            }
            return answer;
        }

        [HttpPost]
        public string CheckFile(IFormFile file, string Symbol, string Count)
        {
            string answer = "";
            if (Symbol == null || Symbol == "")
            {
                answer = "Не указен символ который нужно искать";
                return answer;
            }

            if (Count == null || Count == "")
            {
                answer = "Не указен количество ";
                return answer;
            }

            var result = new StringBuilder();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                    result.AppendLine(reader.ReadLine());
            }

            var grouped = result.ToString()
                        .GroupBy(x => x)
                        .Select(x => new
                        {
                            x.Key,
                            Count = x.Count(),
                        });

            grouped = grouped.Where(x => x.Key.ToString() == Symbol);

            var CountInText = grouped.Count();

            if (CountInText > Convert.ToInt32(Count) || CountInText < 1)
            {
                answer = "Строка не валидна, количество символов " + CountInText + " больше допустимого " + Count;
            }
            else
            {
                answer = " Количество " + Symbol + " в строке -" + CountInText;
            }
            
            return answer;
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}