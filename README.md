# TestTask
Тестовое задание, Лимончело
### AddCandidate
процедура на добавление нового кандидата
CREATE PROCEDURE AddCandidate
	@Name nvarchar(max),
	@Surname nvarchar(max),
	@FathersName nvarchar(max),
	@PhoneNumber nvarchar(11),
	@Position nvarchar(max),
	@FirstInterviewDate datetime,
	@InterviewerSurname nvarchar(max),
	@InterviewerPosition nvarchar(max),
	@DateToComplete datetime
AS
BEGIN
	IF NOT EXISTS (SELECT * FROM Candidates Where PhoneNumber = @PhoneNumber)
	BEGIN
	INSERT INTO Candidates (Name, Surname, FathersName, PhoneNumber, Position, FirstInterviewDate, InterviewerPosition,InterviewerSurname, DateToCompleteTask, TaskStatus)
	VALUES (@Name, @Surname, @FathersName, @PhoneNumber, @Position, @FirstInterviewDate, @InterviewerPosition, @InterviewerSurname, @DateToComplete, N'Задание получено');
	return 1
	END;
    ELSE
	return 0
END;

### CountScore

### CreateTable

### GetCandidates

### TakeTask
