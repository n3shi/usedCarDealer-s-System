using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD2_Komisy_samochodowe_DesktopApp
{
    class AddEmployee
    {
        AddEmployee()
        {

        }
        //public void getJobId();

        public void addEmployee()
        {
            string query = " BEGIN TRAN tranAddNewUser " +

                " begin try" +
                " INSERT INTO employees (Job_id,Commision_id,Salary,Manager_id,User_id) " +
                " values (2, 4, 3500, 2, (select user_id " +
                "                       from Users" +
                "                       where First_name = 'Grzegorz'  AND Secound_name = 'Budyń' and Address_id = ( SELECT location_id FROM location WHERE  " +
                "                                                                                               street_address = 'Adres'  AND postal_code = 12345  " +
                "                                                                                               AND city = 'miasto' AND country = 'panstwo')));" +
                "" +
                " INSERT INTO LOGINSETUP (Employee_id, User_username, User_password, Jobs_manager, Employees_manager, Users_manager, Commision_manager, Transactions_manager, Cars_manager)" +
                "values ((select employee_id from EMPLOYEES" +
                "       where job_id = 2 " +
                "       and Commision_id = 4 " +
                "and salary = 3500 " +
                "and manager_id = 2 " +
                "and user_id = (select user_id " +
                "from Users" +
                "where First_name = 'Grzegorz'  " +
                "AND Secound_name = 'Budyń' " +
                "and Address_id = ( SELECT location_id FROM location WHERE  " +
                "street_address = 'Adres'  AND postal_code = 12345  " +
                "AND city = 'miasto' AND country = 'panstwo'))), " +
                "'', " +// --User_username" +
                "'', " +//  --User_username" +
                "0, " +// --Jobs_manager" +
                "0, " +// --Employees_manager" +
                "0, " +// --Users_manager" +
                "0, " +// --Commision_manager" +
                "0, " +// --Transactions_manager" +
                "0 " +// --Cars_manager  " +
                "" +
                ");";
        }

    }

    //void getJobId() : this.AddEmployee
    //{

    //}
        
    

    

    
}
