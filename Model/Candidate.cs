namespace TestTask.Model
{
    public class Candidate
    {

        // информация о соискателе
        public string Name { get;private set; }
        public string Surname { get; private set; }
        public string FathersName { get; private set; }
        public string PhoneNumber { get; private set; }
        public string Position { get; private set; }


        public DateTime FirstInterviewDate { get; private set; }
        public string InterviewerSurname { get; private set; }//Фамилия интервьюера
        public string InterviewerPosition { get; private set; } //Должность интервьюера
        public DateTime DateToCompleteTask { get; private set; }






        public string StructureDirector { get; private set; }// фамилия, кто принял задание
        public DateTime DateWhenCompleteTask { get; private set; }//когда принято задание
        public byte Score { get; private set; } //оценка от руководителя

        public byte ResultScore;

        public string TaskStatus;

        public Candidate(string phone, DateTime timeComplete)
        {
            PhoneNumber = phone;
            DateWhenCompleteTask = timeComplete;
        }
        public Candidate(string name, string surname, string fathersName, string phoneNumber, string position, DateTime firstInterviewDate, string interviewerSurname, string interviewerPosition, byte daysToCompleteTask)
        {
            if(firstInterviewDate>DateTime.Now || phoneNumber.Length!=11 || daysToCompleteTask==0)
            {
                throw new InvalidDataException();
            }
            Name = name;
            Surname = surname;
            FathersName = fathersName;
            PhoneNumber = phoneNumber;
            Position = position;
            FirstInterviewDate = firstInterviewDate;
            InterviewerSurname = interviewerSurname;
            InterviewerPosition = interviewerPosition;
            DateWhenCompleteTask = firstInterviewDate;
            TaskStatus = "Задание получено";
            DateToCompleteTask = firstInterviewDate.AddDays(daysToCompleteTask);
            
        }

        public Candidate(string name, string surname, string fathersName, string phoneNumber, string position, DateTime firstInterviewDate, string interviewerSurname, string interviewerPosition, DateTime dateToCompleteTask, string structureDirector, DateTime dateWhenCompleteTask, byte score)
        {
            Name = name;
            Surname = surname;
            FathersName = fathersName;
            PhoneNumber = phoneNumber;
            Position = position;
            FirstInterviewDate = firstInterviewDate;
            InterviewerSurname = interviewerSurname;
            InterviewerPosition = interviewerPosition;
            DateToCompleteTask = dateToCompleteTask;
            StructureDirector = structureDirector;
            DateWhenCompleteTask = dateWhenCompleteTask;
            Score = score;
        }
    }
}
