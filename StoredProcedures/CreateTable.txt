--- Процедура Создания таблицы


CREATE PROCEDURE CreateTable

AS
	CREATE TABLE Candidates
	(
		PhoneNumber NVARCHAR(11) PRIMARY KEY NOT NULL,
		Name NVARCHAR(max) NOT NULL,
		Surname NVARCHAR(max) NOT NULL,
		FathersName NVARCHAR(max),
		Position NVARCHAR(max) NOT NULL,

		FirstInterviewDate DATETIME NOT NULL,
		InterviewerSurname NVARCHAR(max) NOT NULL,
		InterviewerPosition NVARCHAR(max) NOT NULL,

		DateToCompleteTask DATETIME NOT NULL,
		DateWhenCompleteTask DATETIME,
		StructureDirector NVARCHAR(max),
		ResultScore INT,
		TaskStatus NVARCHAR(max),
		Score INT,
	);
RETURN 0