using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.SqlClient;
using TestTask.Model;

namespace TestTask
{
    public static class Database
    {
        private const string ConnectionString = "Server=(localdb)\\mssqllocaldb;Database=Limonchello;Trusted_Connection=True;MultipleActiveResultSets=true";


        public static int UseStoredProcedure(string procedureName, List<CommandParameter>? parameters = null)
        {
            using (var connection = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand(procedureName, connection))
            {
                SqlDataAdapter adapt = new SqlDataAdapter(command);
                adapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                if (parameters != null)
                    foreach (var param in parameters)
                    {
                        command.Parameters.Add($"@{param.Name}", param.Type).Value = param.Value;
                    }
                SqlParameter returnValue = new SqlParameter();
                returnValue.Direction = ParameterDirection.ReturnValue;
                command.Parameters.Add(returnValue);
                connection.Open();
                command.ExecuteNonQuery();
                return (int)returnValue.Value;
            }
        }

        public static void SetExpiredTask(Candidate candidate)
        {
            string sqlExpression = $"UPDATE Candidates SET ResultScore = 0 WHERE PhoneNumber = '{candidate.PhoneNumber}'";
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(sqlExpression, connection))
                {
                    command.ExecuteNonQuery();
                }
                sqlExpression = $"UPDATE Candidates SET TaskStatus = N'Истекло время выполнения задания' WHERE PhoneNumber = '{candidate.PhoneNumber}'";
                using (var command = new SqlCommand(sqlExpression, connection))
                {
                    command.ExecuteNonQuery();
                }
                sqlExpression = $"UPDATE Candidates SET DateWhenCompleteTask = '2000-01-01T00:00:00' WHERE PhoneNumber = '{candidate.PhoneNumber}'";
                using (var command = new SqlCommand(sqlExpression, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
        public static List<Candidate> GetCandidatesWhoDontDoTask()
        {
            List<Candidate> candidates = new List<Candidate>();
            string sqlExpression = $"SELECT * FROM Candidates WHERE DateWhenCompleteTask IS NULL";
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read()) // построчно считываем данные
                    {
                        candidates.Add(new Candidate((string)reader.GetValue(0), (DateTime)reader.GetValue(8)));
                    }
                }

                reader.Close();
            }
            return candidates;
        }


        //Самая неудачная вещь во всём приложении, видимо за день устал и под конец уже все мысли путаться начали, ничего лучше не придумал

        //Ищеет возвращает списко кандитатов, во временных рамках + считает результирующую оценку, необходима для генерации конечного отчёта
        public static List<Candidate> GetCandidates(DateTime DateOne, DateTime DateTwo)
        {
            List<Candidate> candidates = new List<Candidate>();
            string sqlExpression = $"SELECT * FROM Candidates WHERE FirstInterviewDate>='{DateOne.ToString("yyyy-MM-dd HH:mm:ss")}' AND FirstInterviewDate<='{DateTwo.ToString("yyyy-MM-dd HH:mm:ss")}'";
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read()) // построчно считываем данные
                    {
                        int Score = 1;
                        string phone="", name = "", surname = "", fathersname = "", position = "", interviewerSurname = "", interviewerPosition = "", StructureDirector = "";
                        DateTime firstinterview= (DateTime)reader.GetValue(5), DateToComplete= (DateTime)reader.GetValue(5), DateWhenComplete= (DateTime)reader.GetValue(5);
                        phone = (string)reader.GetValue(0);
                        name = (string)reader.GetValue(1);
                        surname = (string)reader.GetValue(2);
                        fathersname = (string)reader.GetValue(3);
                        position = (string)reader.GetValue(4);
                        firstinterview = (DateTime)reader.GetValue(5);
                        interviewerSurname = (string)reader.GetValue(6);
                        interviewerPosition = (string)reader.GetValue(7);
                        DateToComplete = (DateTime)reader.GetValue(8);
                        if (reader.GetValue(9) != DBNull.Value)
                        {
                            DateWhenComplete = (DateTime)reader.GetValue(9);
                        }
                        
                        
                        if(reader.GetValue(10)!=DBNull.Value)
                        {
                            StructureDirector = (string)reader.GetValue(10);
                        }
                        if(reader.GetValue(11)!=DBNull.Value)
                        {
                            int ResultScore = (int)reader.GetValue(11);
                        }                     
                        string TaskStatus = (string)reader.GetValue(12);
                        if (reader.GetValue(13) != DBNull.Value)
                        {
                            Score = (int)reader.GetValue(13);
                        }
                        
                        Candidate candidate = new Candidate(
                            name,
                            surname,
                            fathersname,
                            phone,
                            position,
                            firstinterview,
                            interviewerSurname,
                            interviewerPosition,
                            DateToComplete,
                            StructureDirector,
                            DateWhenComplete,
                            (byte)Score
                            );

                        if (reader.GetValue(9) != DBNull.Value)
                        {
                            candidate.ResultScore = (byte)UseStoredProcedure("CountScore", new List<CommandParameter> { new CommandParameter("phone", phone, System.Data.SqlDbType.NVarChar) });
                        }
                        candidate.TaskStatus = TaskStatus;
                        candidates.Add(candidate);
                    }
                }

                reader.Close();
            }
            return candidates;
        }
    }
}
