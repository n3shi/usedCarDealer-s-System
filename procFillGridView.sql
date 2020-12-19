--PROCEDURY DO WCZYTYWANIA DANYCH DO GRID VIEW

CREATE OR ALTER PROCEDURE selectCommision
AS
BEGIN
	select Commision_ID, commision.Commision_manager_ID, location.Street_address, LOCATION.City 
	from commision 
	inner join location on location.Location_ID = COMMISION.Location_ID;
END

GO
CREATE OR ALTER PROCEDURE selectEmployee
AS
BEGIN
select Login_setup_ID Id, User_username 'Nazwa u¿ytkownika', Commision_id as 'Id komisu', Manager_id as 'Id managera', Job_title as Stanowisko from LOGINSETUP
Inner join EMPLOYEES ON EMPLOYEES.Employee_id = LOGINSETUP.Employee_id
inner join users on users.User_id = EMPLOYEES.User_id
inner join LOCATION ON location.Location_ID = Users.Address_id
inner join jobs on jobs.Job_ID = EMPLOYEES.Job_id;
END

exec selectEmployee

GO
Create or alter procedure getAllCars
AS
BEGIn

	BEGIN
		select car_id as 'Id pojazdu', COMMISION_id as 'Id komisu', brand Marka,name Nazwa,is_Sold 'Czy sprzedany', type Typ, date_production as Rocznik 
		from Car
		inner join specification on specification.Specification_ID = car.Specification_ID;
	END
END

exec getAllCars
exec getAvailableCars

GO
Create or alter procedure getAvailableCars
AS
BEGIN
	begin
		select car_id as 'Id pojazdu',COMMISION_id as 'Id komisu', brand Marka,name Nazwa,is_Sold 'Czy sprzedany', type Typ, date_production as Rocznik 
		from Car
		inner join specification on specification.Specification_ID = car.Specification_ID
		WHERE is_sold = 0;
	end
END

GO
Create or alter procedure getSoldCars
AS
BEGIN
	begin
		select car_id as 'Id pojazdu', COMMISION_id as 'Id komisu', brand Marka,name Nazwa,is_Sold 'Czy sprzedany', type Typ, date_production as Rocznik 
		from Car
		inner join specification on specification.Specification_ID = car.Specification_ID
		WHERE is_sold = 1;
	end
END

GO
Create or alter procedure selectJob
AS
BEGIN
SELECT job_id as 'Id stanowiska', job_title as Stanowisko, min_salary as 'Minimalna pensja', Max_salary as 'Maksymalna pensja' FROM JOBS;
END








