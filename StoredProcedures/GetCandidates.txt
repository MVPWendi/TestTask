--- Процедура получения канидатов, которые еще не выполнили задание, для фонового процесса

CREATE PROCEDURE GetCandidates
AS
	IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Candidates'))
		EXEC CreateTable
	SELECT * FROM Candidates WHERE DateWhenCompleteTask IS NULL
RETURN 0