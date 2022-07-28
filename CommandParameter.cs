using System.Data;
using TestTask.Model;

namespace TestTask
{
    public class CommandParameter
    {
        public string Name;
        public Object Value;
        public SqlDbType Type;


        public CommandParameter(string name, Object value, SqlDbType type)
        {
            Name = name;
            Value = value;
            Type = type;
        }
        public static List<CommandParameter> GetParametersForTask(string phone, DateTime date, string structDirector, byte score)
        {
            var list = new List<CommandParameter>();
            list.Add(new CommandParameter("Phone", phone, SqlDbType.NVarChar));
            list.Add(new CommandParameter("WhenTakeTask", date, SqlDbType.DateTime));
            list.Add(new CommandParameter("Score", score, SqlDbType.Int));
            list.Add(new CommandParameter("StructDirector", structDirector, SqlDbType.NVarChar));
            return list;
        }
        public static List<CommandParameter> GetParametersForNewUser(Candidate candidate)
        {
            var list = new List<CommandParameter>();
            list.Add(new CommandParameter("Name", candidate.Name, SqlDbType.NVarChar));
            list.Add(new CommandParameter("Surname", candidate.Surname, SqlDbType.NVarChar));
            list.Add(new CommandParameter("FathersName", candidate.FathersName, SqlDbType.NVarChar));
            list.Add(new CommandParameter("PhoneNumber", candidate.PhoneNumber, SqlDbType.NVarChar));
            list.Add(new CommandParameter("Position", candidate.Position, SqlDbType.NVarChar));
            list.Add(new CommandParameter("FirstInterviewDate", candidate.FirstInterviewDate, SqlDbType.DateTime));
            list.Add(new CommandParameter("InterviewerSurname", candidate.InterviewerSurname, SqlDbType.NVarChar));
            list.Add(new CommandParameter("InterviewerPosition", candidate.InterviewerPosition, SqlDbType.NVarChar));
            list.Add(new CommandParameter("DateToComplete", candidate.DateToCompleteTask, SqlDbType.DateTime));
            return list;
        }
    }
}
