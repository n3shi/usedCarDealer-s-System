--1. Komis
GO
create or alter procedure updateCommision @com_id int, @street varchar(30), @city varchar(30), @postCod numeric(5), @country varchar(30), @manId int
AS
BEGIN

UPDATE location
SET Street_address = @street, City = @city, Postal_code = @postCod, Country = @country
WHERE location_id = (select Location_ID from COMMISION where Commision_ID = @com_id);

UPDATE COMMISION
SET Commision_manager_ID = @manId
WHERE location_id = (select Location_ID from COMMISION where Commision_ID = @com_id);


END

--2. U¿ytkownika
exec selectEmployee
exec updateEmployee @loginId = 10, @user_username = 'Testowniczek', @user_password = 'Testowniczek', @first_name = 'Testowniczek', 
	@Secound_name = 'Testowniczek', @Email = 'Testowniczek', @Street_address = 'Testowniczek', @Postal_code = 55555, 
	@City = 'Testowniczek', @country = 'Testowniczek', @Commision_id = 4, @Manager_id = 2, 
	@Job_id = 3, @Salary = 21553.24 , @Jobs_manager = 1, @Employees_manager = 0, 
	@Users_manager = 1, @Commision_manager = 0, @Transactions_manager = 1, @Cars_manager = 0;


GO
CREATE OR ALTER PROCEDURE updateEmployee @loginId int, @user_username nvarchar(30), @user_password nvarchar(30),
	@first_name nvarchar(30), @Secound_name nvarchar(30),
	@Email nvarchar(30), @Street_address nvarchar(30), @Postal_code numeric(7,2), @City nvarchar(30), @country nvarchar(30),
	@Commision_id int, @Manager_id int, @Job_id int, @Salary int,
	@Jobs_manager bit, @Employees_manager bit, @Users_manager bit, @Commision_manager bit,
	@Transactions_manager bit, @Cars_manager bit

AS
BEGIN
	UPDATE LOGINSETUP
	SET User_username = @user_username, User_password = @user_password, Jobs_manager =@Jobs_manager, Employees_manager = @Employees_manager,
	Users_manager = @Users_manager,Commision_manager = @Commision_manager, Transactions_manager = @Transactions_manager, Cars_manager = @Cars_manager
	WHERE Login_setup_ID = @loginId;

	declare @emplId int;
	SET @emplId = (select Employee_id from LOGINSETUP where Login_setup_ID = @loginId);

	declare @usersId int;
	SET @usersId = (select User_id from EMPLOYEES where Employee_id = @emplId);

	declare @locId int;
	SET @locId = (select Address_id from Users where User_id = @usersId);
	
	update EMPLOYEES
	set Commision_id = @Commision_id, Manager_id = @Manager_id, Job_id = @Job_id, Salary = @Salary
	where Employee_id = @emplId;

	update Users
	SET First_name = @first_name, Secound_name = @Secound_name, Email = @Email, isWorker = 1
	where User_id = @usersId

	update location
	set Street_address = @Street_address, Postal_code = @Postal_code, City = @City, Country = @country
	where Location_ID = @locId;
END

--3. samochów
go
create or alter procedure updateCar @getId int, @comId int, @brand varchar(30), @name varchar(30), @type varchar(30),
@millage varchar(30), @date varchar(30), @airCon bit, @carDrive bit, @gearCase varchar(30), @eng varchar(30), 
@engVal numeric(5)
AS
BEGIN
	Update CAR 
	SET Brand = @brand, Name = @name, Commision_ID = @comId
	where Car_ID = @getId;

	update SPECIFICATION
	set Type = @type, Milage = @millage, Date_production = @date, Air_conditioning = @airCon, Car_drive = @carDrive,
	Gear_case = @gearCase, Engine_name = @eng, Engine_power = @engVal
	where Specification_ID = (select Specification_ID 
							from car 
							where car_id = @getId)
END
--4. stanowisko
GO
create or alter procedure updateJob @job_id int, @job_name varchar(30), @job_min numeric(7,2), @job_max numeric(7,2)
AS
BEGIN
	if @job_max > @job_min
	begin
		UPDATE JOBS
		SET Job_title = @job_name, Min_salary = @job_min, Max_salary = @job_max
		WHERE job_id = @job_id 
	END
END

select * from jobs;
exec updateJob @job_id = 6, @job_name = 'NowyTest', @job_min = 100, @job_max = 180;


--5. klienta
GO
CREATE OR ALTER procedure updateClient @user_id int, @name varchar(30), @secName varchar(30), @email varchar(30),
										 @street varchar(30), @city varchar(30), @postCod numeric(5), @country varchar(30)
AS
BEGIN

	BEGIN TRANSACTION 
		UPDATE Users
		set First_name = @name, Secound_name = @secName, Email = @email
		WHERE User_id = @user_id;

		UPDATE location
	SET Street_address = @street, City = @city, Postal_code = @postCod, Country = @country
		WHERE location_id = (select Location_ID from COMMISION where Commision_ID = @user_id);

	COMMIT TRANSACTION
END




--***********************
--DO WYSZUKANIA

select * from loginsetup inner join employees on employees.Employee_id = LOGINSETUP.Employee_id;
exec getEmplInfo @userID = 2;
GO

CREATE OR ALTER Procedure getEmplInfo @userID int
AS
BEGIN
	select user_username, user_password, first_name, Secound_name,
	Email, Street_address, Postal_code, City, country,
	Commision_id, Manager_id, employees.Job_id, Salary,
	Jobs_manager, Employees_manager, Users_manager, Commision_manager,
	Transactions_manager, Cars_manager
	 from LOGINSETUP
	Inner join EMPLOYEES ON EMPLOYEES.Employee_id = LOGINSETUP.Employee_id
	inner join users on users.User_id = EMPLOYEES.User_id
	inner join LOCATION ON location.Location_ID = Users.Address_id
	inner join jobs on jobs.Job_ID = EMPLOYEES.Job_id
	where Login_setup_ID = @userID;
END