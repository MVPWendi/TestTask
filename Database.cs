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
                        string phone = (string)reader.GetValue(0);
                        string name = (string)reader.GetValue(1);
                        string surname = (string)reader.GetValue(2);
                        string fathersname = (string)reader.GetValue(3);
                        string position = (string)reader.GetValue(4);
                        DateTime firstinterview = (DateTime)reader.GetValue(5);
                        string interviewerSurname = (string)reader.GetValue(6);
                        string interviewerPosition = (string)reader.GetValue(7);
                        DateTime DateToComplete = (DateTime)reader.GetValue(8);
                        DateTime DateWhenComplete = (DateTime)reader.GetValue(9);
                        string StructureDirector = (string)reader.GetValue(10);
                        if(reader.GetValue(11)!=DBNull.Value)
                        {
                            int ResultScore = (int)reader.GetValue(11);
                        }                     
                        string TaskStatus = (string)reader.GetValue(12);
                        int Score = (int)reader.GetValue(13);
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
                        candidate.ResultScore = (byte)UseStoredProcedure("CountScore", new List<CommandParameter> { new CommandParameter("phone", phone, System.Data.SqlDbType.NVarChar) });
                        candidates.Add(candidate);
                    }
                }

                reader.Close();
            }
            return candidates;
        }
    }
}
