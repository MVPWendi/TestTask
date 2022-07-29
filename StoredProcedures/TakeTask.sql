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
	SET DateWhenCompleteTask = @WhenTakeTask, StructureDirector = @StructDirector, Score = @Score, TaskStatus = N'Заданию выставлена оценка сотрудником'
	WHERE PhoneNumber = @Phone
RETURN 1
END;
END;