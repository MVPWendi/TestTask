--- Процедура подсчёта результирующей оценки

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