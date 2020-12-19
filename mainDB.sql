
drop view if exists transakcje;

alter table commision
drop constraint commision_manager_id_fk;

alter table commision
drop constraint location_id_fk;

drop table if exists transactions;
drop table if exists car;
drop table if exists specification;
drop table if exists LOGINSETUP;
drop table if exists employees;
drop table if exists jobs;
drop table if exists commision;
drop table if exists Users;
drop table if exists location;
drop table if exists TRANSACTIONS_history;
drop table if exists log;



-- TWORZENIE TABEL
create table COMMISION ( Commision_ID int primary key identity (1,1),
                        Location_ID int not null,
                        Commision_manager_ID int 
                        );
                        
create table CAR (Car_ID int primary key identity (1,1),
                Brand varchar(30) not null,
                Name varchar(30) not null,
                Commision_ID int not null,
                Specification_ID int not null,
                Is_sold bit not null --do sprawdzenia czy mamy auto na stanie
                    );
                    
create table SPECIFICATION (
                            Specification_ID int primary key identity (1,1),
                            Type varchar(20),
                            Milage numeric(7) not null,
                            Date_production numeric(4) check (Date_production between 1950 and 2020),
                            Air_conditioning bit not null,
                            Car_drive bit not null ,
                            Gear_case varchar(20) not null,
                            Engine_name varchar(30) not null,
                            Engine_power numeric(3) not null
                            );

create table LOCATION (
                        Location_ID int primary key identity (1,1),
                        Street_address varchar(30) not null,
                        Postal_code numeric(5) not null,
                        City varchar(30) not null,
                        Country varchar(30) not null
                        );
                        


create table EMPLOYEES(
			Employee_id int primary key identity (1,1),
			Job_id int not null,
			Commision_id int not null,
			Salary numeric(7,2) not null,
			Manager_id int not null,
			User_id int not null
			);

create table Users(    User_id int primary key identity (1,1) ,
			First_name varchar(30) not null,
			Secound_name varchar(30) not null,
			Email varchar(30),
			Address_id int not null,
			isWorker bit not null
			);
			
                        
create table JOBS (
                    Job_ID int primary key identity (1,1),
                    Job_title varchar(30) not null unique ,
                    Min_salary numeric (7,2) not null,
                    Max_salary numeric (7,2) not null
                    );                        
                        
                        
create table TRANSACTIONS(
                        Transactions_ID int not null identity (1,1),
                        Customer_ID int not null,
                        Employee_ID int not null,
                        Transaction_type bit not null,--0 sprzedaz, 1 kupno
                        Car_ID int not null,
                        Prize int not null,
                        Transaction_data date not null 
                        );


create table LOGINSETUP(Login_setup_ID int not null primary key identity (1,1),
						Employee_id int not null,
						User_username varchar(20) not null unique,
						User_password varchar(20) not null,
						Jobs_manager bit not null,
						Employees_manager bit not null,
						Users_manager  bit not null,
						Commision_manager  bit not null,
						Transactions_manager bit not null,
						Cars_manager bit not null)

CREATE  TABLE log ( id int identity(1,1),
				   active_user nvarchar(30) not null,
                   action_name nvarchar(255),
                   time date not null) ;



CREATE TABLE TRANSACTIONS_HISTORY ( tranactions_history_id int identity (1,1)not null,
									active_user nvarchar(30) not null,
									file_name nvarchar(30) not null,
									file_path nvarchar(255) not null,
									time date not null);
									
--TRIGER DO DODAWANIA TRANSACTION_HISTORY
GO
CREATE OR alter TRIGGER tranactions_history_log ON Transactions AFTER UPDATE, INSERT, DELETE
AS
BEGIN

DECLARE @action_name varchar(127);
SET @action_name = (CASE WHEN EXISTS(SELECT * FROM INSERTED)
                         AND EXISTS(SELECT * FROM DELETED)
                        THEN 'Job record updated'  -- Set Action to Updated.
                        WHEN EXISTS(SELECT * FROM INSERTED)
                        THEN 'Job record inserted'  -- Set Action to Insert.
                        WHEN EXISTS(SELECT * FROM DELETED)
                        THEN 'Job record deleted'  -- Set Action to Deleted.
                        ELSE NULL -- Skip. It may have been a "failed delete".   
                    END)

  INSERT INTO TRANSACTIONS_HISTORY VALUES( 'username',@action_name, 'filepath', sysdatetime() );
END

GO



-- wszystkie trigery: jest ich tyle co opcji w loginsetup: dla zmiany w tabeli: job/employees/users/commision/transactions/car, czyli 6       
GO
CREATE or alter TRIGGER log_job on jobs AFTER UPDATE, INSERT, delete
AS
BEGIN

DECLARE @action_name varchar(127);

    SET @action_name = (CASE WHEN EXISTS(SELECT * FROM INSERTED)
                         AND EXISTS(SELECT * FROM DELETED)
                        THEN 'Job record updated'  -- Set Action to Updated.
                        WHEN EXISTS(SELECT * FROM INSERTED)
                        THEN 'Job record inserted'  -- Set Action to Insert.
                        WHEN EXISTS(SELECT * FROM DELETED)
                        THEN 'Job record deleted'  -- Set Action to Deleted.
                        ELSE NULL -- Skip. It may have been a "failed delete".   
                    END)

  INSERT INTO log VALUES( 'login_name',@action_name, sysdatetime() );

END;
GO                       

--addEmployeeLog
GO
CREATE or alter TRIGGER log_employee on Employees AFTER UPDATE, INSERT, delete
AS
BEGIN

DECLARE @action_name varchar(127);

    SET @action_name = (CASE WHEN EXISTS(SELECT * FROM INSERTED)
                         AND EXISTS(SELECT * FROM DELETED)
                        THEN 'Employee record updated'  -- Set Action to Updated.
                        WHEN EXISTS(SELECT * FROM INSERTED)
                        THEN 'Employee record inserted'  -- Set Action to Insert.
                        WHEN EXISTS(SELECT * FROM DELETED)
                        THEN 'Job record deleted'  -- Set Action to Deleted.
                        ELSE NULL -- Skip. It may have been a "failed delete".   
                    END)

  INSERT INTO log VALUES( 'login_name',@action_name, sysdatetime() );

END;
GO                       


--addUsersLog
GO
CREATE or alter TRIGGER log_user on users AFTER UPDATE, INSERT, delete
AS
BEGIN

DECLARE @action_name varchar(127);

    SET @action_name = (CASE WHEN EXISTS(SELECT * FROM INSERTED)
                         AND EXISTS(SELECT * FROM DELETED)
                        THEN 'Users record updated'  -- Set Action to Updated.
                        WHEN EXISTS(SELECT * FROM INSERTED)
                        THEN 'Users record inserted'  -- Set Action to Insert.
                        WHEN EXISTS(SELECT * FROM DELETED)
                        THEN 'Users record deleted'  -- Set Action to Deleted.
                        ELSE NULL -- Skip. It may have been a "failed delete".   
                    END)

  INSERT INTO log VALUES( 'login_name',@action_name, sysdatetime() );

END;
GO                       


--addCommisionLog
GO
CREATE or alter TRIGGER log_commision on commision AFTER UPDATE, INSERT, delete
AS
BEGIN

DECLARE @action_name varchar(127);

    SET @action_name = (CASE WHEN EXISTS(SELECT * FROM INSERTED)
                         AND EXISTS(SELECT * FROM DELETED)
                        THEN 'Commision record updated'  -- Set Action to Updated.
                        WHEN EXISTS(SELECT * FROM INSERTED)
                        THEN 'Commision record inserted'  -- Set Action to Insert.
                        WHEN EXISTS(SELECT * FROM DELETED)
                        THEN 'Commision record deleted'  -- Set Action to Deleted.
                        ELSE NULL -- Skip. It may have been a "failed delete".   
                    END)

  INSERT INTO log VALUES( 'login_name',@action_name, sysdatetime() );

END;
GO                       

GO
CREATE or alter TRIGGER log_transactions on transactions AFTER UPDATE, INSERT
AS
BEGIN

DECLARE @action_name varchar(127);

    SET @action_name = (CASE WHEN EXISTS(SELECT * FROM INSERTED)
                         AND EXISTS(SELECT * FROM DELETED)
                        THEN 'Transactions record updated'  -- Set Action to Updated.
                        WHEN EXISTS(SELECT * FROM INSERTED)
                        THEN 'Transactions record inserted'  -- Set Action to Insert.
                        WHEN EXISTS(SELECT * FROM DELETED)
                        THEN 'Transactions record deleted'  -- Set Action to Deleted.
                        ELSE NULL -- Skip. It may have been a "failed delete".   
                    END)

  INSERT INTO log VALUES( 'login_name',@action_name, sysdatetime() );

END;
GO                       

GO
CREATE or alter TRIGGER log_car on car AFTER UPDATE, INSERT
AS
BEGIN

DECLARE @action_name varchar(127);

    SET @action_name = (CASE WHEN EXISTS(SELECT * FROM INSERTED)
                         AND EXISTS(SELECT * FROM DELETED)
                        THEN 'Car record updated'  -- Set Action to Updated.
                        WHEN EXISTS(SELECT * FROM INSERTED)
                        THEN 'Car record inserted'  -- Set Action to Insert.
                        WHEN EXISTS(SELECT * FROM DELETED)
                        THEN 'Car record deleted'  -- Set Action to Deleted.
                        ELSE NULL -- Skip. It may have been a "failed delete".   
                    END)

  INSERT INTO log VALUES( 'login_name',@action_name, sysdatetime() );

END;
GO                       

-- CONSTRAINY                        

ALTER TABLE Commision
add CONSTRAINT Location_ID_FK FOREIGN KEY (Location_ID) 
REFERENCES Location(Location_ID);

ALTER TABLE Commision
ADD CONSTRAINT Commision_Manager_ID_FK FOREIGN KEY (commision_manager_ID)
REFERENCES Employees(employee_ID);

ALTER TABLE Car
ADD CONSTRAINT Commision_ID_FK FOREIGN KEY(Commision_ID)
REFERENCES   Commision(Commision_ID);

ALTER TABLE CAR
ADD CONSTRAINT Specification_ID_FK FOREIGN KEY(Specification_ID)
REFERENCES  Specification(Specification_ID);

ALTER TABLE EMPLOYEES
ADD CONSTRAINT     Job_id_FK FOREIGN KEY(Job_ID)
REFERENCES  JOBS(Job_ID);

ALTER TABLE Employees
ADD CONSTRAINT  Commision_ID2_FK FOREIGN KEY(Commision_ID)
REFERENCES  Commision (Commision_ID);

ALTER TABLE Employees
ADD CONSTRAINT User_id_fk FOREIGN KEY(User_id)
REFERENCES Users (User_id);

Alter TABLE Employees
ADD CONSTRAINT Manager_id_fk FOREIGN KEY(manager_id)
REFERENCES Employees (employee_id);

ALTER TABLE Users
ADD CONSTRAINT locations_id_fk FOREIGN KEY (address_id)
REFERENCES Location (location_id);

ALTER TABLE TRANSACTIONS
ADD CONSTRAINT Customer_ID_FK FOREIGN KEY (Customer_ID)
REFERENCES Users (User_id);

ALTER TABLE Transactions
ADD CONSTRAINT Employee_ID_FK FOREIGN KEY (Employee_ID)
REFERENCES Employees(Employee_ID);

ALTER TABLE TRANSACTIONS
ADD CONSTRAINT CAR_ID_FK FOREIGN KEY (Car_ID)
REFERENCES CAR(Car_ID);

ALTER TABLE LOGINSETUP
ADD CONSTRAINT LOGIN_SETUP_ID_FK FOREIGN KEY (Employee_id)
REFERENCES EMPLOYEES(Employee_id);



--modyfikacje
--alter table car
--add constraint unique_specifiaction_id unique (specification_id);

alter table commision
add constraint unique_lociation_id unique (location_id);

--widok
GO
create view Transakcje AS
SELECT t.transaction_type as Rodzaj_transakcji, --do transakcji
u.first_name as Imie_klienta, u.secound_name as Nazwisko_klienta, -- do klienta
l.street_address as Adres_klienta, l.city as Miasto_klienta,l.country as Kraj_klienta, -- do lokacji klienta
user_e.first_name as Imie_pracownika, user_e.secound_name as Nazwisko_pracownika, --do pracownika
loc_e.street_address as Adres_pracownika, loc_e.city as Miasto_pracownika,loc_e.country as Kraj_pracownika, -- do lokacji pracownika
--l.street_address as Adres_pracownika, l.city as Miasto_pracownika,l.country as Kraj_pracownika, -- do lokacji pracownika
car.name as Nazwa_Samochodu, car.brand as Marka, -- do specyfikacji samochodu
spec.date_production as Data_produkcji_samochodu, spec.milage as Przebieg, spec.engine_name as Rodzaj_silnika, spec.engine_power as Moc_silnika, -- do informacji o aucie
loc_com.street_address as Adres_komisu, loc_com.city as Miasto_komisu, loc_com.country, -- do lokacji komisu
t.prize as Cena, t.Transaction_data as Data_zawarcia_transakcji -- do transakcji
from transactions t
join users u ON u.User_id = t.customer_id
--join customers c ON c.customer_id = t.customer_id
join location l ON l.location_id = u.address_id
join employees e on e.employee_id =t.employee_id
join users user_e ON user_e.user_id = e.user_id
join location loc_e on loc_e.location_id = user_e.address_id
join car on car.car_id = t.car_id
join commision comis ON car.commision_id = comis.commision_id
join location loc_com on loc_com.location_id = comis.location_id
join specification spec on spec.specification_id = car.specification_id;
GO

select * from SPECIFICATION




-- DODANIE DO LOKACJI 
SET IDENTITY_INSERT location ON

INSERT
INTO location (location_id, street_address, postal_code, city, country) 
values 
--klientow
(1, 'Mleczna 20/3', 50555, 'Wroc³aw', 'Polska'),
(2, 'Pomarañczowa 7/4', 58100, 'Œwidnica', 'Polska'),
(3, 'Zielona 12/6', 66170, 'Diablice', 'Polska'),
(4, 'Koœciuszki 32/3', 18354, 'Chicago', 'USA'),
(5, 'Koœcielna 2/14', 36203, 'Berlin', 'Niemcy'),
--pracowników
(6, 'Borowska 2/14', 37203, 'Berlin', 'Niemcy'),
(7, 'Bohaterów Getta 43/74', 51350, 'Kraków', 'Polska'),
(8, 'Armii Krajowej 61/4', 23635, 'Bia³ystok', 'Polska'),
(9, 'Konopnicka 15/1', 73571, 'Toruñ', 'Polska'),
(10, 'Reja 5/15', 27589, 'Szczecin', 'Polska'),
--komisów
(11, 'Parkowa 3/7', 16153, 'Gdañsk', 'Polska'),
(12, 'Karola Wielkiego 12/5', 74253, 'Gdañsk', 'Polska'),
(13, 'Jab³eczna 1/7', 51350, 'Kraków', 'Polska'),
(14, 'Malinowa 2/8', 50555, 'Wroc³aw', 'Polska'),
(15, 'Poziomkowa 15/4', 58100, 'Œwidnica', 'Polska');

SET IDENTITY_INSERT location OFF;




SET IDENTITY_INSERT  Users ON

INSERT INTO Users (User_id, First_name, Secound_name, Email, Address_id, isWorker)
values 
(1, 'Grzegorz', 'Budyñ', 'GrzegorzBudyñ@o2.pl',1,0),
(2, 'Karol', 'Gruszka', 'KarolGruszkaTo@gmail.com', 2, 0),
(3, 'Ania', 'Wzgórze', 'AniaZZielonego@wp.pl', 3, 0),
(4, 'Tadeusz', 'Ciuszek', 'TadekNiejadek@gmail.pl', 4, 0),
(5, 'Aleksandra', 'Czapla', 'OlaCzapla@o2.pl', 5, 0),
(6, 'Antoni', 'Nowak', null, 6,1  ),
(7, 'Piotr', 'Pawe³', null, 7,1 ),
(8, 'Kinga', 'Rusin', 'Rusin@super.com', 8, 1 ),
(9, 'Marta', 'Miœ', 'Ma³yMiœ@gmail.com', 9, 1),
(10, 'Marcel', 'B¹k', null, 10, 1 )
SET IDENTITY_INSERT  Users OFF



SET IDENTITY_INSERT Commision ON
INSERT INTO COMMISION (Commision_ID, Location_ID, Commision_manager_ID)
values
(1,11,null),
(2,12,null),
(3,13,null),
(4,14,null),
(5,15,null)
SET IDENTITY_INSERT Commision OFF

SET IDENTITY_INSERT jobs ON
INSERT
INTO jobs (Job_ID, Job_title, Min_salary, Max_salary)
values  
(1, 'Manager', 10000, 15000),
(2, 'Sprzedawca', 1500, 3500),
(3, 'Kierownik', 4000, 6000),
(4, 'Dyrektor', 15000, 20000),
(5, 'Praktykant', 1500,2000)
SET IDENTITY_INSERT jobs OFF


SET IDENTITY_INSERT jobs ON
INSERT
INTO jobs (Job_ID, Job_title, Min_salary, Max_salary)
values(0, 'Nieokreslono', 0, 0);
SET IDENTITY_INSERT jobs OFF

-- DODANIE DO PRACOWNIKOW
SET IDENTITY_INSERT employees ON
INSERT
INTO employees (Employee_id,Job_id,Commision_id,Salary,Manager_id,User_id)
values 
(1,  3, 2, 4700,  2, 6),
(2,  1,  1, 13400, 2, 7),
(3,  2, 1, 2500,  2, 8),
(4,  1, 5, 12950,  2, 9),
(5,  5, 5, 1500,  2, 10)
SET IDENTITY_INSERT employees OFF

--DODANIE DO SPECYFIKACJI
SET IDENTITY_INSERT specification ON
INSERT 
INTO specification (Specification_ID,Type,Milage,Date_production,Air_conditioning,Car_drive,Gear_case,Engine_name,Engine_power)
values 
(1, 'Kombi', 203000, 2005, 1, 1, 'Manualna', '2,0 TDI', 140),
(2, 'Sedan', 78000, 2014, 1, 1, 'Automatyczna', '3,0 benzyna BiTurbo', 306),
(3, 'Sedan', 200000, 2007, 1, 1, 'Manualna', '1,4 TDI', 70),
(4, 'Kombi', 182000, 2015, 1, 1, 'Automatyczna', '2,0 TDI', 150),
(5, 'Hatchback', 196000, 2009, 1, 1, 'Manualna', '1,4 benzyna', 80)
SET IDENTITY_INSERT specification OFF

--DODANIE DO SAMOCHODÓW
SET IDENTITY_INSERT car ON
INSERT 
INTO car (Car_ID,Brand,Name,Commision_ID,Specification_ID,Is_sold)
values
( 1, 'Audi', 'A4 III (B7)', 1, 1, 1),
( 2, 'BMW', '535Xi', 2,2, 0),
( 3, 'Seat', 'Cordoba II', 1, 3, 0),
( 4, 'Skoda', 'Octavia III', 5, 4, 0),
( 5, 'Volkswagen', 'Golf VI', 5,5, 1)
SET IDENTITY_INSERT car OFF


-- DODANIE DO TRANSAKCJI
SET IDENTITY_INSERT transactions ON
INSERT
INTO transactions (Transactions_ID, Customer_ID, Employee_ID, Transaction_type, Car_ID, Prize, Transaction_data)
values
(1, 3, 2, 1, 1, 24900, '2018/03/23'),
(2, 2, 4, 1, 5 , 15000,'2011/02/12'),
(3, 1, 3, 0, 1 , 29900,'2019/02/15'),
(4, 5, 5, 0, 5, 17000, '2013/05/01'),
(5, 3, 1, 1, 2, 15000, '2017/11/30'),
(6, 4, 3, 1, 3, 12500, '2018/05/27'),
(7, 4, 4, 1, 4, 32400, '2019/01/22')
SET IDENTITY_INSERT transactions OFF


SET IDENTITY_INSERT LOGINSETUP ON
INSERT
INTO LOGINSETUP (login_setup_id,employee_id, user_username, user_password, jobs_manager, Employees_manager,
users_manager, commision_manager, transactions_manager, cars_manager)
values
(1,1,'Test1','test1',1,0,1,1,1,1),
(2,2,'Test2','test2',1,1,0,1,1,1),
(3,3,'Test3','test3',1,1,1,0,1,1),
(4,4,'Test4','test4',1,1,1,1,0,1),
(5,5,'Test5','test5',1,1,1,1,1,0)
SET IDENTITY_INSERT loginsetup OFF

