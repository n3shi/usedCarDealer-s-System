
--^^^^^^^^^^^^^^^^^^^^^^
--**********************
--WSZYSTKIE DELETE
--**********************
--vvvvvvvvvvvvvvvvvvvvvv

--delete polega na wybraniu z aplikacji interesuj¹cego nas rekordu do usuniêcia z combobox, lub innej listy a nastêpnie dokonania próby kaskadowego usuniêcia tam gdzie potrzeba


--1. delete from jobs
exec deleteJob '6';
select * from jobs;

CREATE OR ALTER PROCEDURE deleteJob @job_id int
AS 
Begin
	--przypisz nieokreslony job wszystkim pracownikom ktorzy maj¹ aktualne stanowisko. Nieokreslonego nie mozna usunac
	update employees
	set job_id = '0'
	where job_id = @job_id;

	Delete from jobs where job_id = @job_id;
end



--2. delete from loginsetup
exec deleteWorker @loginId = 4

select * from log
create or alter procedure deleteWorker @loginId int
AS
	BEGIN 
	begin tran deleteWorker
		--loginsetup ustawiony bêdzie z poziomu aplikacji z combobox. Najpierw zostanie wywo³ane zapytanie odnoœnie znalezienia odpowiedniego pracownika, potem zostanie zczytane jego id i 
		-- zapamietane przez aplikacje. Kolejnym krokiem bêdzie przypisanie odpowiedniego id a dalej kaskadowe usuniecie

		declare @loginsetup_id_toDelete int
		set @loginsetup_id_toDelete = @loginId;
		declare @employee_id_toDelete int;
		declare @user_id_toDelete int;
		declare @location_id_toDelete int;

		SET @employee_id_toDelete = (select employee_id from LOGINSETUP where Login_setup_ID = @loginsetup_id_toDelete);
		SET @user_id_toDelete = (select user_id from EMPLOYEES where Employee_id = @employee_id_toDelete );
		SET @location_id_toDelete = (select address_id from users where user_id = @user_id_toDelete);

		declare @user_usernameV1 varchar(30);
		set @user_usernameV1 = (select user_username from LOGINSETUP where Login_setup_ID = @loginId)

		declare @dropQuery varchar(30);
		set @dropQuery = 'drop user ' + @user_usernameV1; 

		begin try
		exec(@dropQuery);
		end try

		begin catch
		end catch

		update COMMISION
		set Commision_manager_ID = null
		where Commision_manager_ID = @employee_id_toDelete;

		delete from LOGINSETUP where Login_setup_ID = @loginsetup_id_toDelete;
		delete from EMPLOYEES where Employee_id = @employee_id_toDelete;
		delete from Users where user_id = @user_id_toDelete;

			begin try
				delete from location where location_id = @location_id_toDelete;
			end try

			begin catch
			end catch
	commit tran deleteWorker
END



--3. delete user

select * from Users inner join location on LOCATION.Location_ID = users.Address_id;


exec deleteUser @user_id = 15;
create or alter procedure deleteUser @user_id int
AS
BEGIN 

	begin tran deleteUser

		--podobnie jak w przypadku pracownika id uzytkownika zostanie do³o¿one z poziomu aplikacji poprzez odpowiedni wybór
		declare @user_id_toDelete int;
		set @user_id_toDelete = @user_id;
		declare @location_id_toDelete int;

		SET @location_id_toDelete = (select address_id from users where user_id = @user_id_toDelete);

		begin try
			delete from Users where user_id = @user_id_toDelete;

			begin try
				delete from location where location_id = @location_id_toDelete;
			end try

			begin catch
			end catch
		end try

		begin catch
		end catch

	commit tran deleteUser
END

--4. delete commision
--dane pobrane z aplikacji

select * from commision inner join location on location.Location_ID = COMMISION.Location_ID;
exec deleteCommision @commisionID = 5;

CREATE OR ALTER PROCEDURE deleteCommision @commisionID int
AS
BEGIN 

	

	--do location_id_value dostajemy informacje o tym jakie id lokacji przypisac
	declare @location_id_value int;
	SET @location_id_value = (SELECT location_id from COMMISION where Commision_ID = @commisionID);

	
	begin try
	delete from location where Location_ID = @location_id_value;
	delete from COMMISION where Commision_ID = @commisionID;
	end try

	begin catch
	delete from COMMISION where Commision_ID = @commisionID;
	end catch
END

--5. delete transaction
CREATE OR ALTER PROCEDURE deleteTransaction @transactionId int
AS 
BEGIN
	declare @setTransactionIdValue int = @transactionId;
	delete from transactions where transactions_id = @setTransactionIdValue;
END

--6. delete cars
-- id samochodu zostanie wczytane z programu

CREATE OR ALTER PROCEDURE deleteCars @carId int
AS
	BEGIN
	begin tran deleteCar

		declare @setCarIdValue int = @carId;

		declare @setSpecificationIdValue int = (select specification_id from Car where Car_ID = @setCarIdValue);

		begin try
			delete from specification where Specification_ID = @setSpecificationIdValue;
			delete from car where car_id = @setCarIdValue;
		end try

		begin catch 
			delete from car where car_id = @setCarIdValue;
		end catch

	commit tran deleteCar
END;