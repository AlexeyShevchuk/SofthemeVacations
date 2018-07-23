CREATE DATABASE Vacations

USE Vacations

CREATE TABLE EmployeeStatus
(
	EmployeeStatusID uniqueidentifier PRIMARY KEY,
	Name nvarchar(50)
);

CREATE TABLE JobTitle
(
	JobTitleID uniqueidentifier PRIMARY KEY,
	Name nvarchar(50)
);

CREATE TABLE Role
(
	RoleID uniqueidentifier PRIMARY KEY,
	Name nvarchar(50)
);

CREATE TABLE Team
(
	TeamID uniqueidentifier PRIMARY KEY,
	TeamLeadID uniqueidentifier,
	Name nvarchar(100)
);

CREATE TABLE Employee
(
	EmployeeID uniqueidentifier PRIMARY KEY,
	Name nvarchar(50),
	Surname nvarchar(50), --50
	WorkEmail nvarchar(256), --256
	TelephoneNumber nvarchar(20), --20
	Birthday date,
	Skype nvarchar(50),
	StartDate date,
	EmployeeStatusID uniqueidentifier, --Отдельная таблица
	EndDate date,
	JobTitleID uniqueidentifier, --Отдельная таблица
	Balance int,
	CONSTRAINT Employee_EmployeeStatusID_FK FOREIGN KEY (EmployeeStatusID) REFERENCES EmployeeStatus(EmployeeStatusID),
	CONSTRAINT Employee_JobTitleID_FK FOREIGN KEY (JobTitleID) REFERENCES JobTitle(JobTitleID)
);

ALTER TABLE Team
ADD CONSTRAINT Team_TeamLeadID_FK FOREIGN KEY (TeamLeadID) REFERENCES Employee(EmployeeID)

CREATE TABLE [User]
(
	UserID uniqueidentifier PRIMARY KEY,
	EmployeeID uniqueidentifier UNIQUE,
	Password nvarchar(300),
	PersonalEmail nvarchar(256) NOT NULL, 
	RoleID uniqueidentifier,
	CONSTRAINT User_RoleID_FK FOREIGN KEY (RoleID) REFERENCES [Role](RoleID),
	CONSTRAINT User_EmployeeID_FK FOREIGN KEY (EmployeeID) REFERENCES Employee(EmployeeID)
);

CREATE TABLE EmployeeTeam
(
	EmployeeID uniqueidentifier,
	TeamID uniqueidentifier,
	CONSTRAINT EmployeeTeam_EmployeeID_FK FOREIGN KEY (EmployeeID) REFERENCES Employee(EmployeeID),
	CONSTRAINT EmployeeTeam_TeamID_FK FOREIGN KEY (TeamID) REFERENCES Team(TeamID),
	CONSTRAINT EmployeeTeam_EmployeeID_TeamID_Unique UNIQUE(EmployeeID, TeamID)
)

CREATE TABLE TransactionType
(
	TransactionTypeID uniqueidentifier PRIMARY KEY,
	Name nvarchar(50) 
);

CREATE TABLE [Transaction]
(
	TransactionID uniqueidentifier PRIMARY KEY,
	TransactionTypeID uniqueidentifier,
	EmployeeID uniqueidentifier, --Отдельная таблица
	Days int,
	Comment nvarchar(200),
	CONSTRAINT Transaction_TransactionTypeID_FK FOREIGN KEY (TransactionTypeID) REFERENCES TransactionType(TransactionTypeID),
	CONSTRAINT Transaction_EmployeeID_FK FOREIGN KEY (EmployeeID) REFERENCES Employee(EmployeeID)
);

CREATE TABLE VacationStatus
(
	VacationStatusID uniqueidentifier PRIMARY KEY,
	Name nvarchar(50)
);

CREATE TABLE Vacation
(
	VacationID uniqueidentifier PRIMARY KEY,
	StartVocationDate date,
	EndVocationDate date,
	VacationStatusID uniqueidentifier,  --Отдельная таблица
	Comment nvarchar(200),
	EmployeeID uniqueidentifier, 
	CONSTRAINT Vacation_EmployeeID_FK FOREIGN KEY (EmployeeID) REFERENCES Employee(EmployeeID),
	CONSTRAINT Vacation_VacationStatusID_FK FOREIGN KEY (VacationStatusID) REFERENCES VacationStatus(VacationStatusID)
);
