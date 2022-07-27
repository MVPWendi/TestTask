CREATE PROCEDURE CreateTable

AS
	CREATE TABLE Candidates
	(
		PhoneNumber NVARCHAR(11) PRIMARY KEY NOT NULL,
		Name NVARCHAR(max) NOT NULL,
		Surname NVARCHAR(max) NOT NULL,
		FathersName NVARCHAR(max),
		Position NVARCHAR(max) NOT NULL,

		FirstInterviewDate DATE NOT NULL,
		InterviewerSurname NVARCHAR(max) NOT NULL,
		InterviewerPosition NVARCHAR(max) NOT NULL,

		DateToCompleteTask DATE NOT NULL,
		DateWhenCompleteTask DATE,
		StructureDirector NVARCHAR(max),
		Score INT,
	);
RETURN 0

CREATE PROCEDURE TakeTask
	@Phone nvarchar(11),
	@WhenTakeTask datetime,
	@StructDirector nvarchar(max),
	@Score int
AS
BEGIN
	IF NOT EXISTS (SELECT * FROM Candidates Where PhoneNumber = @Phone)
	return 0
	ELSE
	BEGIN
	UPDATE Candidates
	SET DateWhenCompleteTask = @WhenTakeTask
	WHERE PhoneNumber = @Phone

	UPDATE Candidates
	SET StructureDirector = @StructDirector
	WHERE PhoneNumber = @Phone

	UPDATE Candidates
	SET Score = @Score
	WHERE PhoneNumber = @Phone
RETURN 1
END;
END;

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
	INSERT INTO Candidates (Name, Surname, FathersName, PhoneNumber, Position, FirstInterviewDate, InterviewerPosition,InterviewerSurname, DateToCompleteTask)
	VALUES (@Name, @Surname, @FathersName, @PhoneNumber, @Position, @FirstInterviewDate, @InterviewerPosition, @InterviewerSurname, @DateToComplete);
	return 1
	END;
	ELSE
	return 0
END;

CREATE PROCEDURE CountScore
	@phone nvarchar(11)
AS
	BEGIN
	DECLARE @WhenComplete datetime
	DECLARE @ResultScore INT
	DECLARE @WhenShouldComplete datetime
	DECLARE @TempTimeScore INT
	DECLARE @Score INT
	DECLARE @DayForOneValue FLOAT
	DECLARE @WhenGive datetime
	SET @WhenComplete = (SELECT DateWhenCompleteTask FROM Candidates Where PhoneNumber = @phone)
	SET @WhenShouldComplete = (SELECT DateToCompleteTask FROM Candidates Where PhoneNumber = @phone)
	SET @WhenGive = (SELECT FirstInterviewDate FROM Candidates Where PhoneNumber = @phone)
	SET @Score = (SELECT Score FROM Candidates Where PhoneNumber = @phone)
		


	IF @WhenComplete IS NULL
		return 500
	ELSE
		BEGIN
	    IF DATEDIFF(DAY, @WhenShouldComplete, @WhenComplete)>0 ---Если закончил позже чем должен был
			BEGIN
			RETURN 0;
			END
		ELSE
			BEGIN
			SET @DayForOneValue = DATEDIFF(DAY,@WhenGive, @WhenShouldComplete)/5; --- считаем "ценность одного дня"

			SET @TempTimeScore = CEILING(6 - CEILING(DATEDIFF(DAY, @WhenGive, @WhenComplete)/@DayForOneValue)); --- считаем оценку за время
			SET @ResultScore = CEILING((@Score+@TempTimeScore)/2);
			RETURN @ResultSCore;
			END;
		END;
END;


