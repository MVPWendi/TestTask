using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TestTask.Model;

namespace TestTask.Controllers
{
    [ApiController]
    
    public class API : ControllerBase
    {


        private readonly ILogger<API> _logger;

        public API(ILogger<API> logger)
        {
            _logger = logger;
        }


        // 2022-07-27T00:00:00
        /// <summary>
        /// ����� ��� �������� ������� (���� ��� ������)
        /// </summary>
        [HttpPost]
        [Route("[controller]/CreateTable")]
        public void CreateTable()
        {
            Database.UseStoredProcedure("CreateTable");
        }
        /// <summary>
        /// ����� ��� �������� ������ (��� ������)
        /// </summary>
        [HttpPost]
        [Route("[controller]/CountScore")]
        public int CountScore(string phoneNumber)
        {
            return Database.UseStoredProcedure("CountScore", new List<CommandParameter> { new CommandParameter("phone", phoneNumber, System.Data.SqlDbType.NVarChar) });
        }
        /// <summary>
        /// ����� ��� ��������� ������
        /// </summary>
        [HttpPost]
        [Route("[controller]/GetResult")]
        public string GetResult(DateTime DateOne, DateTime DateTwo)
        {
            try
            {
                var candidates = Database.GetCandidates(DateOne, DateTwo);
                foreach (var cand in candidates)
                {
                    if(cand.DateWhenCompleteTask==cand.FirstInterviewDate)
                    {
                        cand.ResultScore = -1;
                    }
                }
                return JsonConvert.SerializeObject(candidates);
            }
            catch(Exception ex)
            {
                Response.StatusCode = 500;
                _logger.LogInformation(ex.Message);
                return "��������� ������";
            }
        }

        /// <summary>
        /// ����� ��� ���������� ���������
        /// </summary>
        [HttpPost]
        [Route("[controller]/TakeTask")]
        public void AddCandidate(string phoneNumber, DateTime whenTakeTask, string structDirector, byte score)
        {
            try
            {
                var value = Database.UseStoredProcedure("TakeTask", CommandParameter.GetParametersForTask(phoneNumber, whenTakeTask, structDirector, score));
                if(value==0)
                {
                    throw new Exception();
                }
                HTTP.SendHTTP(phoneNumber, "������� ���������� ������ �����������");
            }
            catch(Exception e)
            {
                Response.StatusCode = 500;
                _logger.LogInformation(e.Message);
            }
        }
        /// <summary>
        /// ����� ��� ���������� ������ ���������
        /// </summary>
        [HttpPost]
        [Route("[controller]/addcandidate")]
        public void AddCandidate(string name, string surname, string fathersName, string phoneNumber, string position, DateTime firstInterviewDate, string interviewerSurname, string interviewerPosition, byte daysToCompleteTask)
        {
            try
            {
                Candidate candidate = new Candidate(name, surname, fathersName, phoneNumber, position, firstInterviewDate, interviewerSurname, interviewerPosition, daysToCompleteTask);
                if (Database.UseStoredProcedure("AddCandidate", CommandParameter.GetParametersForNewUser(candidate)) == 0)
                {

                    throw new Exception(message: "There is already candidate with this number");
                }
            }
            catch(Exception e)
            {
                Response.StatusCode = 500;
                _logger.LogInformation(e.Message);
            }

        }
    }
}