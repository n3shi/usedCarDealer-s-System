
--------------------------------------------------
--			NOWO ZBUDOWANE TRANSAKCJE I INSERTY
--************************************************

--W TABELI LOGINSETUP JEST 6 FUNKCJONALNOŒCI DO KTÓRYCH DOSTÊP MA PRACOWNIK
--KA¯DA Z NICH ODNOSI SIÊ DO TABEL I WI¥¯E SIÊ Z DOSTÊPEM DO 3 OPCJI WYBORU: DODANIE, AKTUALIZACJA, USUNIÊCIE
--Potrzebne wiêc jest stworzenie 18 zapytañ realizuj¹cych wszystkie dostêpne zmiany dokonane przez pracownika


--********************
--INSERTY
--********************
exec dbo.addCommision @streetAddres = 'A2', @cityAddres = 'To2', @postalCodeAddres = 55555, @countr ='juzTak?3';


	select * from COMMISION
	select * from LOCATION
	select * from commision inner join location on location.Location_ID = COMMISION.Location_ID;
GO

--**************************
	--1. Dodawanie nowego komisu
	--**************************
CREATE OR ALTER PROCEDURE addCommision @streetAddres varchar(30), @cityAddres varchar(30), @postalCodeAddres numeric(5),  @countr varchar(30), @id int
AS
BEGIN
	BEGIN TRAN addCommision

	declare @streetAddress varchar(30)= @streetAddres;
	declare @cityAddress varchar(30) = @cityAddres;
	declare @postalCodeAddress numeric(5) = @postalCodeAddres;
	declare @country varchar(30) 
	SET @country = @countr;

	--do location_id_value dostajemy informacje o tym jakie id lokacji przypisac, jesli nei ma to trzeba taka lokacje najpierw utworzyc
	declare @location_id_value int;
	SET @location_id_value = (SELECT location_id from location where Street_address = @streetAddress and city = @cityAddress and Country = @country);


		if(@location_id_value is null)
		begin
			INSERT INTO LOCATION (Street_address, Postal_code, City, Country)
			values (@streetAddress,@postalCodeAddress,@cityAddress,@country);
			SET @location_id_value = (SELECT location_id from location where Street_address = @streetAddress and city = @cityAddress and Country = @country);
		end

		declare @does_commision_exists int;
		SET @does_commision_exists = (select commision_id from commision where Location_ID = @location_id_value);

		if(@does_commision_exists is null)
			begin
			INSERT INTO COMMISION
			(Location_ID, Commision_manager_ID)
			values (@location_id_value, @id)
			end

	COMMIT TRAN addCommision
END

select * from location




select * from LOGINSETUP
Inner join EMPLOYEES ON EMPLOYEES.Employee_id = LOGINSETUP.Employee_id
inner join users on users.User_id = EMPLOYEES.User_id
inner join LOCATION ON location.Location_ID = Users.Address_id;

declare @username varchar(30) = '';

select * from loginsetup where User_username = @username;

select * from car

EXEC sp_addrolemember N'db_datareader', N'Ma³y'	

grant all privilages

BEGIN
END
GO;
--****************************
--2. dodanie nowego u¿ytkownika
-- ***************************
SELECT first_name, secound_name, employees.Commision_id 
From employees 
Inner join users ON users.user_id = employees.user_id 
Inner join jobs ON jobs.job_id = employees.job_id 
inner join COMMISION on COMMISION.Commision_ID = EMPLOYEES.employee_id 
inner join location on location.location_id = commision.commision_id 

where employees.job_id = 1 AND employees.commision_id = (select commision_id 
														from COMMISION 
															inner join location ON location.Location_ID = COMMISION.Location_ID 
															where location.location_id = 16);

drop user MisioMisio2692
delete from LOGINSETUP where User_username = 'admin';
select * from LOGINSETUP
EXEC addNewEmployee @streetAddres = '1', @postalCode = 66666, @cityName = '1', @countryName = '1',
									@firstName = 'Jan', @secound_name = 'Kowalski', @mail = '', 

									@jobId = 3, @commisionId = 2, @salaryValue = 300, @managerId = 2, 
									@userUsername = 'admin', @userPassword = 'siema', @jobsManager = 1, @employeesManager = 1,
									@usersManager = 0, @comissionManager = 0, @transactionManager = 0, @carsManager = 0;

select * from LOGINSETUP
select * from users


GO
CREATE OR ALTER PROCEDURE addNewEmployee @streetAddres varchar(30), @postalCode numeric(5), @cityName varchar(30), @countryName varchar(30),

										@firstName varchar(30), @secound_name varchar(30), @mail varchar(30),

										@jobId int, @commisionId int, @salaryValue numeric(6), @managerId int,

										@userUsername varchar(20), @userPassword varchar(20), @jobsManager bit, @employeesManager bit,
										@usersManager bit, @comissionManager bit, @transactionManager bit, @carsManager bit
AS
BEGIN


	BEGIN TRAN tranAddNewEmployeeToLoginSetup 

								

	--dodanie nowego pracownika, przypisuj¹c mu odpowiednie id u¿ytkownika, który ma odpowiedni¹ lokalizacjê"
	--dodanie loginu i has³a i przypisanie wskazane id nowego pracownika	

	--location
	declare @street_address_value varchar(30);
	SET @street_address_value = @streetAddres;
	declare @postal_code_value numeric(5);
	SET @postal_code_value = @postalCode;
	declare @city_value varchar(30);
	SET @city_value = @cityName;
	declare @country_value varchar(30);
	SET @country_value = @countryName;

	
				

	--users First_name, Secound_name, Email, Address_id, isWorker
	declare @first_name_value varchar(30);
	SET @first_name_value = @firstName;
	declare @secound_name_value varchar(30);
	SET @secound_name_value = @secound_name;
	declare @email varchar(30);
	SET @email = @mail;
	if(@email = '')set @email = null;

	
	declare @address_id_value int; 
	declare @is_Worker_value bit;
	set @is_Worker_value = 1;
	

	

	--employees (te wartoœci wybiera siê z aplikacji desktopowej)
	declare @job_id_value int;
	SET @job_id_value = @jobId;
	declare @Commision_id_value int;
	SET @Commision_id_value = @commisionId;
	declare @salary_value numeric(6);
	SET @salary_value = @salaryValue;
	declare @manager_id_value int;
	SET @manager_id_value = @managerId;
	declare @user_id_value int;

	--loginSetup
	declare @employee_id_value int;
	declare @user_username_value varchar(20);
	SET @user_username_value = @userUsername;
	declare @user_password_value varchar(20);
	SET @user_password_value= @userPassword;
	declare @jobs_manager_value bit;
	SET @jobs_manager_value = @jobsManager;
	declare @employees_manager_value bit;
	SET @employees_manager_value= @employeesManager;
	declare @users_manager_value bit;
	SET @users_manager_value = @usersManager;
	declare @commision_manager_value bit;
	SET @commision_manager_value = @comissionManager;
	declare @transactions_manager_value bit;
	SET @transactions_manager_value = @transactionManager;
	declare @cars_manager_value bit;
	SET @cars_manager_value = @carsManager;

	

	--co wiêcej, jeœli za pierwszym razem dodam lokacjê, to nie muszê sprawdzaæ i zapamiêtywaæ za ka¿dym razem u¿ytkownika i pracownika, bo i tak wiem
	-- ¿e bêdê musia³ ich stworzyæ. Przypisujê wiêc im wartoœæ bitow¹, jeœli bêdê musia³ stworzyæ odpowiednie rekordy w tabeli to wiem, ¿e pozosta³e rekordy
	-- równie¿ nie maj¹ szansy na bycie odnalezionym przez zapytanie select, st¹d na pocz¹tku nale¿y sprawdziæ czy dodano wskazane parametry, czy nie
	declare @new_location_id bit;
	set @new_location_id = 0;
	declare @new_user_id bit;
	set @new_user_id = 0;

	

	--dodanie pracownika wiaze sie z kaskadowym dodaniem w nastêpuj¹cej kolejnoœci
	--add to: location -> users -> employees -> loginsetup

	--0. Sprawdzenie czy pracownik bêdzie mia³ unikaln¹ nazwê (w przypadku jeœli nie sprawdzi³bym tego najpierw, to musia³bym kaskadowo wszystko dodaæ a w przypadku 
	-- trafienia na unikaln¹ nazwê zrobiæ rollback wszystkiego. Jedno sprawdzenie daje nam gwarancjê, ¿e na pewno uda siê stworzyæ nowe konto.

	-- W przypadku otrzymania informacji zwrotnej o id_employee is not null mamy gwarancjê, i¿ pracownik wczeœniej mia³ konto. Zamiast tworzyæ nowego
	-- u¿ytkownika nale¿y zrobiæ update na istniej¹cym przypisuj¹c now¹ nazwê, has³o i uprawnienia

	declare @what_id_has_username int;
	set @what_id_has_username = (select Login_setup_ID from LOGINSETUP where User_username = @user_username_value);

	if(@what_id_has_username is null)
	begin

		--1. sprawdzam czy jest lokacja w bazie, jeœli tak, to zapamiêtuje jej id, jesli nie, tworzê wskazan¹ lokacjê i zapamiêtuje jej id

		SET @address_id_value = (SELECT location_id FROM location WHERE  
								street_address = @street_address_value  AND postal_code = @postal_code_value  
								AND city = @city_value AND country = @country_value)

		if ( @address_id_value is null)
			begin
				INSERT INTO location (street_address, postal_code, city, country) 
				values(@street_address_value, @postal_code_value, @city_value, @country_value);

				SET @address_id_value = (SELECT location_id FROM location WHERE  
									street_address = @street_address_value  AND postal_code = @postal_code_value  
									AND city = @city_value AND country = @country_value);
			end

		--2. sprawdzam czy jest u¿ytkownik w bazie, jeœli tak, to zapamiêtuje jego id, jeœli nie, tworze wskazanego u¿ytkownika i zapamiêtuje jego id

		SET @user_id_value = (select user_id 
								from Users
								where First_name = @first_name_value  AND Secound_name = @secound_name_value and Address_id = @address_id_value);

		if (@user_id_value is null)
			begin
				INSERT INTO Users (First_name, Secound_name, Email, Address_id, isWorker)
				values(@first_name_value, @secound_name_value, @email, @address_id_value, @is_Worker_value);

				SET @user_id_value = (select user_id 
									from Users
									where First_name = @first_name_value  AND Secound_name = @secound_name_value and Address_id = @address_id_value);
			end


		--3. profilaktycznie sprawdzam czy nie ma pracownika w bazie. W teorii jesli byl kiedys pracownikiem i potem nie, to nalezalo usunac go z bazy,
		--jednak odbywa sie to recznie, nie automatycznie stad mozliwe przeoczenie.

		SET @employee_id_value = (select employee_id from EMPLOYEES
								  where 
								  job_id = @job_id_value 
								  and Commision_id = @Commision_id_value
								  and salary = @salary_value
								  and manager_id = @manager_id_value 
								  and user_id = @user_id_value);
	
		if (@employee_id_value is not null)
			begin
			
				--usun poprzednie konto
				declare @previousAccount varchar(30) = (select user_username from LOGINSETUP where Employee_id = @employee_id_value);

				declare @sqlDropExistingUser varchar(255) = 'drop user if exists ' + @previousAccount;
				exec(@sqlDropExistingUser);

				--zamien user_username i user_password
					Update LOGINSETUP
					set User_username = @user_username_value, User_password = @user_password_value
					where employee_id = @employee_id_value;

				-- stworz nowe konto
				declare @sqlDynamicQueryExecute varchar (200);
				SET @sqlDynamicQueryExecute = 'CREATE USER ' + @user_username_value + ' WITH PASSWORD = ''' + @user_password_value  + '''';
				exec(@sqlDynamicQueryExecute);
			end

		else
			begin
				INSERT INTO employees (Job_id,Commision_id,Salary,Manager_id,User_id)
				values (@job_id_value, @Commision_id_value, @salary_value, @manager_id_value, @user_id_value);

				SET @employee_id_value = (select employee_id from EMPLOYEES
									  where 
									  job_id = @job_id_value 
									  and Commision_id = @Commision_id_value
									  and salary = @salary_value
									  and manager_id = @manager_id_value 
									  and user_id = @user_id_value);


				-- 4. Nie ma jeszcze takiego loginu i jest utworzone juz wszystko pod pracownika. Mozna ostatecznie bez sprawdzania
				-- stworzyc login, has³o pracownika i zapisac mu w tabelce uprawnienia

				INSERT INTO LOGINSETUP (Employee_id, User_username, User_password, Jobs_manager, 
										Employees_manager, Users_manager, Commision_manager, Transactions_manager, 
										Cars_manager)																			
				values (@employee_id_value, @user_username_value, @user_password_value, @jobs_manager_value, 
						@employees_manager_value, @users_manager_value, @commision_manager_value, @transactions_manager_value,
						@cars_manager_value);
			

			
				SET @sqlDynamicQueryExecute = 'CREATE USER ' + @user_username_value + ' WITH PASSWORD = ''' + @user_password_value  + '''';
				exec(@sqlDynamicQueryExecute);

			end

			--nadanie dostêpu do odczytu dla typka:

			--**************************
			--TRZEBA PODMIENIC NA KOD PONIZEJ
			--******************************

		--			GRANT EXEC ON SCHEMA::dbo TO John-- Grant perpmission on all procedures in
		--                                      the dbo schema
		--GRANT EXEC TO John-- Grant EXEC permission all procedures in the database

				declare @grantReadPermission varchar (100) = 'EXEC sp_addrolemember N' + '''' + 'db_datareader' + '''' + ', N' + '''' + @user_username_value + '''';
				exec(@grantReadPermission);

				if (@jobs_manager_value = 1) 
				begin  
					declare @grantJobPrivilage varchar (300);
					SET @grantJobPrivilage = 'GRANT INSERT, UPDATE, DELETE on dbo.JOBS to ' + @user_username_value;
					exec(@grantJobPrivilage);
				end

				if (@employees_manager_value = 1)
				begin
					declare @grantEmployeesPrivilage varchar (300);
					SET @grantEmployeesPrivilage = 'GRANT INSERT, UPDATE, DELETE on EMPLOYEES to ' + @user_username_value;
					exec (@grantEmployeesPrivilage);

					declare @grantLoginPrivilage varchar (300);
					SET @grantEmployeesPrivilage = 'GRANT INSERT, UPDATE, DELETE on LOGINSETUP to ' + @user_username_value;
					exec (@grantEmployeesPrivilage);

				end

				if(@users_manager_value = 1 or @employees_manager_value = 1)
				begin
					declare @grantUserPrivilage varchar(300);
					SET @grantUserPrivilage = 'GRANT INSERT, UPDATE, DELETE on USERS to ' + @user_username_value;
					exec (@grantUserPrivilage);
				end

				if(@commision_manager_value = 1)
				begin
					declare @grantCommisionPrivilage varchar (300);
					SET @grantCommisionPrivilage = 'GRANT INSERT, UPDATE, DELETE on commision to ' + @user_username_value;
					exec (@grantCommisionPrivilage);
				end

				if(@transactions_manager_value = 1)
				begin
					declare @grantTransactionPrivilage varchar (300);
					SET @grantTransactionPrivilage = 'GRANT INSERT, UPDATE, DELETE on transactions to ' + @user_username_value;
					exec (@grantTransactionPrivilage);
				end

				if(@cars_manager_value = 1)
				begin
					declare @grantCarsPrivilage varchar (300);
					SET @grantCarsPrivilage = 'GRANT INSERT, UPDATE, DELETE on Car to ' + @user_username_value;
					exec (@grantCarsPrivilage);

					declare @grantSpecificationPrivilage varchar (300);
					SET @grantCarsPrivilage = 'GRANT INSERT, UPDATE, DELETE on Specification to ' + @user_username_value;
					exec (@grantCarsPrivilage);
				end

				--pod lokacje
				if(@employees_manager_value = 1 or @users_manager_value = 1 or @cars_manager_value = 1 or @transactions_manager_value = 1)
				begin
					declare @grantLocationPrivilage varchar (300);
					SET @grantLocationPrivilage = 'GRANT INSERT, UPDATE, DELETE on location to ' + @user_username_value;
					exec (@grantLocationPrivilage);
				end

				begin try
					declare @testQuery varchar(100);
					set @testQuery = 'GRANT EXECUTE TO ' + @user_username_value;
					exec(@testQuery);
				end try
				begin catch
				end catch

	end --koniec dla warunku "nie ma takiego u¿ytkownika

	COMMIT TRAN tranAddNewEmployeeToLoginSetup

END




--******************
--3. Dodanie nowego samochodu
-- *****************



EXEC addCar @brand_val = 'Marka', @name_val = 'Nazwa', @commision_id = 1,
			@type_val = 'Kombi', @mil_val = 200000, @date_val = 2004, @air_cond_val = 1, @car_driv_val = 1,
			@gear_case_val = 'Manual', @engine_name_val = 'V8', @eng_pow_val = 100;

select *from SPECIFICATION;

			select * from location where Street_address = 'Parkowa 3/7';
GO
CREATE OR ALTER PROCEDURE addCar @brand_val varchar(20), @name_val varchar(20),

								@commision_id int,--@street_adres varchar(30), @city_val varchar(30),

								@type_val varchar(30), @mil_val numeric(6), @date_val numeric(4), @air_cond_val bit,
								@car_driv_val bit, @gear_case_val varchar(20), @engine_name_val varchar(20), @eng_pow_val numeric(3)
AS
BEGIN
	BEGIN TRAN tranAddCar

	

	--addToCar
	declare @brand_value varchar(20) 
	set @brand_value= @brand_val;
	declare @name_value varchar(20) 
	set @name_value = @name_val;
	declare @commision_id_val int;
	SET @commision_id_val = @commision_id;
	declare @specification_id_value int;
	declare @is_sold_value bit;
	SET @is_sold_value = 0;


	declare @type_value varchar(30) = @type_val;
	declare @milage_value numeric(6) = @mil_val;
	declare @date_production_value numeric(4) = @date_val;
	declare @air_conditioning_value bit = @air_cond_val;
	declare @car_drive_value bit = @car_driv_val;
	declare @gear_case_value varchar(20) = @gear_case_val;
	declare @engine_name_value varchar(20) = @engine_name_val;
	declare @engine_power_value numeric (3) = @eng_pow_val;

	SET @specification_id_value = (Select specification_id 
								  from SPECIFICATION
								  where type = @type_value and milage = @milage_value and Date_production = @date_production_value and Air_conditioning = @air_conditioning_value
								  and Car_drive = @car_drive_value and Gear_case = @gear_case_value
								  and engine_name = @engine_name_value and Engine_power = @engine_power_value)

		if(@specification_id_value is null)
		begin

		insert into SPECIFICATION(type,milage,Date_production,Air_conditioning,Car_drive,Gear_case,Engine_name,Engine_power)
		values (@type_value, @milage_value, @date_production_value,@air_conditioning_value,@car_drive_value, @gear_case_value, @engine_name_value, @engine_power_value);

		SET @specification_id_value = (Select specification_id 
								  from SPECIFICATION
								  where type = @type_value and milage = @milage_value and Date_production = @date_production_value and Air_conditioning = @air_conditioning_value
								  and Car_drive = @car_drive_value and Gear_case = @gear_case_value
								  and engine_name = @engine_name_value and Engine_power = @engine_power_value)

		end

	INSERT INTO CAR (brand,name,Commision_ID,Specification_ID,Is_sold)
	values (@brand_value, @name_value, @commision_id_val, @specification_id_value, @is_sold_value);

	COMMIT TRAN tranAddCar
END

SELECT * FROM JOBS;
--***************************
--4. DODANIE STANOWISKA PRACY
--***************************

exec addJob @jobTitle = 'Test2222', @minSal = 1200, @maxSal = 13000;
select * from jobs

GO
CREATE OR ALTER PROCEDURE addJob @jobTitle varchar(30), @minSal numeric(7,2), @maxSal numeric(7,2)
AS
BEGIN


    declare @job_id int;
	set @job_id = (select Job_ID from jobs where Job_title = @jobTitle);
	
	if(@job_id is null)
	begin
		INSERT INTO jobs (Job_title, Min_salary, Max_salary)
		values  
		(@jobTitle, @minSal, @maxSal);
	end

	--else
	--begin
	--update JOBS
	--SET Job_title = @jobTitle, Min_salary = @minSal, Max_salary = @maxSal
	--where Job_title = @jobTitle;
	--end
END

SELECT * FROM JOBS

--*****************************
--5. DODANIE NOWEGO U¯YTKOWNIKA
--*****************************

exec addUser @street_val = 'Test', @post_cod_val = 69696, @city_val = 'Test', @countryy = 'Test', @first_nam_val = 'Imie',
			@secound_nam_val = 'Nazwisko', @mail = 'mail';

SELECT * FROM users inner join location on location.Location_ID = users.Address_id;



GO
CREATE OR ALTER PROCEDURE addUser @street_val varchar(30), @post_cod_val numeric(5), @city_val varchar(30), @countryy varchar(30),

								  @first_nam_val varchar(30), @secound_nam_val varchar(30), @mail varchar(30)
AS
	BEGIN
	BEGIN TRAN tranAddNewUsers

	--dodanie nowego pracownika, przypisuj¹c mu odpowiednie id u¿ytkownika, który ma odpowiedni¹ lokalizacjê"
	--dodanie loginu i has³a i przypisanie wskazane id nowego pracownika	

	--location
	declare @street_address_value varchar(30);
	SET @street_address_value = @street_val;
	declare @postal_code_value numeric(5);
	SET @postal_code_value= @post_cod_val;
	declare @city_value varchar(30);
	SET @city_value = @city_val;
	declare @country_value varchar(30);
	SET @country_value = @countryy;

	--users First_name, Secound_name, Email, Address_id, isWorker
	declare @first_name_value varchar(30);
	SET @first_name_value = @first_nam_val;
	declare @secound_name_value varchar(30);
	SET @secound_name_value = @secound_nam_val;
	declare @email varchar(30)
	SET @email = @mail;
	if(@email = '')set @email = null;
	declare @address_id_value int; 
	declare @is_Worker_value bit;
	SET @is_Worker_value = 0;

	--dodanie u¿ytkownika wiaze sie z kaskadowym dodaniem w nastêpuj¹cej kolejnoœci
	--add to: location -> users

		--1. sprawdzam czy jest lokacja w bazie, jeœli tak, to zapamiêtuje jej id, jesli nie, tworzê wskazan¹ lokacjê i zapamiêtuje jej id

		SET @address_id_value = (SELECT location_id FROM location WHERE  
								street_address = @street_address_value  AND postal_code = @postal_code_value  
								AND city = @city_value AND country = @country_value)

		if ( @address_id_value is null)
			begin
				INSERT INTO location (street_address, postal_code, city, country) 
				values(@street_address_value, @postal_code_value, @city_value, @country_value);

				SET @address_id_value = (SELECT location_id FROM location WHERE  
									street_address = @street_address_value  AND postal_code = @postal_code_value  
									AND city = @city_value AND country = @country_value);
			end

		--2. sprawdzam czy jest u¿ytkownik w bazie, jeœli tak, to zapamiêtuje jego id, jeœli nie, tworze wskazanego u¿ytkownika i zapamiêtuje jego id
		declare @user_id_value int;

		SET @user_id_value = (select user_id 
								from Users
								where First_name = @first_name_value  AND Secound_name = @secound_name_value and Address_id = @address_id_value);

		if (@user_id_value is null)
			begin
				INSERT INTO Users (First_name, Secound_name, Email, Address_id, isWorker)
				values(@first_name_value, @secound_name_value, @email, @address_id_value, @is_Worker_value);

				SET @user_id_value = (select user_id 
									from Users
									where First_name = @first_name_value  AND Secound_name = @secound_name_value and Address_id = @address_id_value);
			end
	COMMIT TRAN tranAddNewUsers
END


SELECT * FROM TRANSACTIONS;
--************************
--6. Dodaj now¹ transakcje
--************************

select * from transakcje
exec addTransaction @street_val = 'Komputer1', @post_cod_val = 88888, @city_val = 'Komputer2', @countryy = 'Komputer3',

								  @first_nam_val = 'Komputer4', @secound_nam_val = 'Komputer5', @mail = 'Komputer6', 
								  
								  @username = 'admin', @tranType = 0, @prize = 222555,

								  --adToCar
								  @brand = 'Komputer8', @name = 'Komputer9', @isSold = 0, @type = 'Komputer10',
								  @millage = 55555, @date = 2002, @AirCon = 1, @carDrive = 0,
								  @gearCase = 'Komputer12', @engine = 'Komputer11', @engPow = 512

GO
CREATE OR ALTER PROCEDURE addTransaction @street_val varchar(30), @post_cod_val numeric(5), @city_val varchar(30), @countryy varchar(30),

								  @first_nam_val varchar(30), @secound_nam_val varchar(30), @mail varchar(30), 
								  
								  @username varchar(30), @tranType bit, @prize numeric(10,2),

								  --adToCar
								  @brand varchar(30), @name varchar(30), @isSold bit, @type varchar(20),
								  @millage numeric(7), @date numeric(4), @AirCon bit, @carDrive bit,
								  @gearCase varchar(20), @engine varchar(30), @engPow numeric(3)

								  


								  


AS
BEGIN

	BEGIN TRAN addNewTransaction
	declare @street_address_value varchar(30);
	set @street_address_value = @street_val;
	declare @postal_code_value numeric(5);
	set @postal_code_value = @post_cod_val;
	declare @city_value varchar(30);
	set @city_value =  @city_val;
	declare @country_value varchar(30);
	set @country_value = @countryy;

	--users First_name, Secound_name, Email, Address_id, isWorker
	declare @first_name_value varchar(30);
	set @first_name_value = @first_nam_val;
	declare @secound_name_value varchar(30);
	set @secound_name_value = @secound_nam_val;
	declare @email varchar(30);
	set @email = @mail;
	if(@email = '')set @email = null;
	declare @address_id_value int; 
	declare @is_Worker_value bit;
	SET @is_Worker_value = 0;


	SET @address_id_value = (SELECT location_id FROM location WHERE  
								street_address = @street_address_value  AND postal_code = @postal_code_value  
								AND city = @city_value AND country = @country_value)

		if ( @address_id_value is null)
			begin
				INSERT INTO location (street_address, postal_code, city, country) 
				values(@street_address_value, @postal_code_value, @city_value, @country_value);

				SET @address_id_value = (SELECT location_id FROM location WHERE  
									street_address = @street_address_value  AND postal_code = @postal_code_value  
									AND city = @city_value AND country = @country_value);
			end

		--2. sprawdzam czy jest u¿ytkownik w bazie, jeœli tak, to zapamiêtuje jego id, jeœli nie, tworze wskazanego u¿ytkownika i zapamiêtuje jego id
		DECLARE @user_id_value int;

		SET @user_id_value = (select user_id 
								from Users
								where First_name = @first_name_value  AND Secound_name = @secound_name_value and Address_id = @address_id_value);

		if (@user_id_value is null)
			begin
				INSERT INTO Users (First_name, Secound_name, Email, Address_id, isWorker)
				values(@first_name_value, @secound_name_value, @email, @address_id_value, @is_Worker_value);

				SET @user_id_value = (select user_id 
									from Users
									where First_name = @first_name_value  AND Secound_name = @secound_name_value and Address_id = @address_id_value);
			end

	--3. mamy ju¿ id pracownika, employee_id zostanie pobrane przez aplikacjê w momencie podania loginu oraz has³a
	-- transaction_data zaktualizuje sie sama poprzez funkcje get_date, transaction_type oraz prize bêd¹ wpisane z poziomu aplikacji, 
	--samo car_id bêdzie przyznane po wskazaniu odpowiedniego samochodu z bazy

	declare @comId int;
	set @comId = (Select Commision_id 
					from loginsetup 
					inner join employees on EMPLOYEES.Employee_id = LOGINSETUP.Employee_id
					where User_username = @username)

	declare @emplId int;
	set @emplId = (Select Login_setup_ID
					from loginsetup 
					where User_username = @username)

	declare @carId int;
	set @carId = (Select Car_ID 
					from car inner join SPECIFICATION 
					on SPECIFICATION.Specification_ID = car.Specification_ID
					where brand = @brand and Name = @name and Commision_ID = @comId and Is_sold = @tranType
					and type = @type and Milage = @millage and Date_production = @date and Air_conditioning = @airCon
					and Car_drive = @carDrive and Gear_case = @gearCase and Engine_name = @engine 
					and Engine_power = @engPow)

	if(@carId is null)
	begin
		
		begin try
			EXEC addCar @brand_val = @brand, @name_val = @name, @commision_id = @comId,
						@type_val = @type, @mil_val = @millage, @date_val = @date, @air_cond_val = @AirCon,
						@car_driv_val = @carDrive,
						@gear_case_val = @gearCase, @engine_name_val = @engine, @eng_pow_val = @engPow;

			set @carId = (Select Car_ID 
						from car inner join SPECIFICATION 
						on SPECIFICATION.Specification_ID = car.Specification_ID
						where brand = @brand and Name = @name and Commision_ID = @comId and Is_sold = @tranType
						and type = @type and Milage = @millage and Date_production = @date and Air_conditioning = @airCon
						and Car_drive = @carDrive and Gear_case = @gearCase and Engine_name = @engine 
						and Engine_power = @engPow)

		end try
		begin catch
		end catch
			
	end

	INSERT
	INTO transactions (Customer_ID, Employee_ID, Transaction_type, Car_ID, Prize, Transaction_data)
	values
	(@user_id_value, @emplId, @tranType,@carId, @prize, GETDATE());

	COMMIT TRAN addNewTransaction;
END