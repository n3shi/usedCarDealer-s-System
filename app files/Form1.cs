using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;

namespace BD2_Komisy_samochodowe_DesktopApp
{
    public partial class Form1 : Form
    {
        //INFO
        //Wszystkie funkcje w kodzie występują kolejno
        //CZĘŚĆ PIERWSZA
        //1. Dodanie komisu
        //2. dodanie użytkownika
        //3. Dodanie nowego samochodu
        //4. Dodanie stanowiska pracy
        //5. dodanie nowego klienta
        //6. dodanie nowej transacji

        //CZĘŚĆ DRUGA
        //1. Aktualizacja/usunięcie komisu
        //2. Aktualizacja/usunięcie użytkownika
        //3. Aktualizacja/usunięcie nowego samochodu
        //4. Aktualizacja/usunięcie stanowiska pracy
        //5. Aktualizacja/usunięcie nowego klienta
        //6. Aktualizacja/usunięcie nowej transacji

        public static string password, login, ipAddress;
        public Form1()
        {
            
            InitializeComponent();

            //FormBorderStyle = FormBorderStyle.None;
            //WindowState = FormWindowState.Maximized;
            //TopMost = true;
            this.AutoScroll = true;
            gbAvailableCars.Hide();
            tbAllProces.AppendText("Zarządzanie procesami");
            HideAll();

            btAdTranGetClientId.Hide();
            btAdTranGetCarId.Hide();
            gbAdTranCarPrize.Hide();

            

        }

        string[] name55 = new string[2];
        public void setLoggedUserInfo(string login)
        {
            string printOutput = "";
            try
            {

                if (con.State == ConnectionState.Open) con.Close();

            cmd = new SqlCommand("printEmployeeData", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("usernamev2", login));

            con.Open();
            con.InfoMessage += (object obj, SqlInfoMessageEventArgs w) =>
            {
                printOutput += w.Message;
            };
            cmd.ExecuteNonQuery();
            con.Close();

            string[] newString = printOutput.Split(';');

            //name55[0] = newString[0];
            //name55[1] = newString[1];

            //Do testow aplikacji przypisanie do admina stalych wartosci

            name55[0] = "Jan";
            name55[1] = "Kowalski";

            Console.WriteLine(login + " " + newString[0] + " " + newString[1] + " " + newString[2]);

            //labelUserNick.Text = "Użytkownik: " + login;
            //labelUserName.Text = "Imie i nazwisko: " + newString[0] + " " +  newString[1];
            //labelUserJob.Text = "Stanowisko: " + newString[2];

            labelUserNick.Text = "Użytkownik: " + "admin";
            labelUserName.Text = "Imie i nazwisko: " + name55[0] + " " + name55[1];
            labelUserJob.Text = "Stanowisko: " + "TurboAdmin";
            }
            catch
            {
                MessageBox.Show("Pogladowy system zarzadzania siecia komisow samochodowych");

                //MessageBox.Show("Błąd ładowania danych, teraz nastąpi wyjście z aplikacji");
                MessageBox.Show("ERROR \n\nNiektore funkcje moga byc wylaczone! \n\n Mozliwe przypadki: \n -brak praw (skontaktuj sie z administratorem) \n- wersja demo programu(brak mozliwosci usuwania, tworzenia pracownikow i transakcji)");
                //System.Windows.Forms.Application.Exit();
                name55[0] = "Jan";
                name55[1] = "Kowalski";
                labelUserNick.Text = "Użytkownik: " + "admin";
                labelUserName.Text = "Imie i nazwisko: " + name55[0] + " " + name55[1];
                labelUserJob.Text = "Stanowisko: " + "TurboAdmin";
            }
        }

        void HideAll() //do wyłączenia każdego groupboxa na start
        {
            gbAddCommision.Hide();
            gbAddEmployee.Hide();
            gbAddCar.Hide();
            gbAddJob.Hide();
            gbAdClient.Hide();
            gbAdTran.Hide();

            gbSearchCar.Hide();
        }







        SqlConnection con;

        string connectString;
        public string getId(string login, string password, string ipAddress)
        {
            //connectString = "Data Source= " + ipAddress + ",1433; Network Library=DBMSSOCN; Initial Catalog =" + '"' + "BD2 komisy samochodowe" + '"' + "; User ID = " + login + "; Password=" + password + ";";
            //connectString = "Data Source= " + ipAddress + ",1433; Network Library=DBMSSOCN; Initial Catalog =" + '"' + "komis" + '"' + "; User ID = " + login + "; Password=" + password + ";";
            //connectString = @"Server=tcp:komisserver.database.windows.net,1433;Initial Catalog=komisdb;Persist Security Info=False;User ID=" + login + ";Password=" + password + ";MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Authentication=Active Directory Password;";
            //connectString = "Server=tcp:myserver.database.windows.net,1433;Database=myDataBase;User ID=login@myserver;Password=myPassword;Trusted_Connection=False;Encrypt=True;";
            connectString = "Server = tcp:komisserver.database.windows.net,1433; Initial Catalog = komisdb; Persist Security Info = False; User ID =" + login + "; Password = " + password + "; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30;";
            Console.WriteLine(connectString);
            return connectString;
            
        }


        public void StartConnection()
        {
            //try
            //{
            con = new SqlConnection(connectString);
            con.Open();
            con.Close();
            //}catch( AggregateException e)
            //{
            //    Console.WriteLine(e);
            //}

        }

        //DEKLAROWANIE WSZYSTKICH ZMIENNYCH
  
        SqlCommand cmd;

        //do dodania pracownika
        string locationIdEmployee;
        string userIdEmployee;
        
        int temporaryInt;
        string setId;

        string date;

        //Zwraca aktualną godzinę
        string ActualData()
        {
            date = DateTime.Now.ToString("HH:mm:ss");
            tbAllProces.AppendText("\r\n" + date + " Proces: ");
            return date;
        }

        //Po podaniu tekstu wypisuje godzinę i proces
        void writeProcess(string text) 
        {
            ActualData();
            tbAllProces.AppendText(text);
        }

        int test; //do okreslenia które wypełnić
        int x = 0;




        //*****************************************************
        //ZARZĄDZANIE POJAWIANIEM SIĘ ODPOWIEDNICH GROUP BOXÓW
        //*****************************************************


        //-----------------
        //DODAWANIE
        //-----------------
        //1. Dodanie komisu
        private void buttonAddCommision_Click(object sender, EventArgs e)
        {
            HideAll();
            gbAddCommision.Show();
            fillComAdManCb();


            ActualData();
            tbAllProces.AppendText("Załadowanie funkcji dodającej komis.");

            
            fillGridView("selectCommision");
            test = 1;
        }
        private void komisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HideAll();
            gbAddCommision.Show();
            fillComAdManCb();

            ActualData();
            tbAllProces.AppendText("Załadowanie funkcji dodającej komis.");

            fillGridView("selectCommision");
            test = 2;

        }



        //2. dodanie użytkownika
        private void buttonAddEmployee_Click(object sender, EventArgs e)
        {
            HideAll();
            gbAddEmployee.Show();
            readAddEmployee();
            fillGridView("selectEmployee");

            ActualData();
            tbAllProces.AppendText("Załadowanie funkcji dodającej użytkownika.");
            test = 2;
        }
        private void użytkownikaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HideAll();
            gbAddEmployee.Show();
            readAddEmployee();
            fillGridView("selectEmployee");

            test = 2;
            ActualData();
            tbAllProces.AppendText("Załadowanie funkcji dodającej użytkownika.");
           
        }


        //3. Dodanie nowego samochodu
        private void nowySamochódToolStripMenuItem_Click(object sender, EventArgs e)
        {
            

            gbAvailableCars.Show();
            gbSearchCar.Show();
            HideAll();
            gbAddCar.Show();
            
            rbCarSelectAll.Checked = true;
            fillCommisionComboBox(cbAdCarCommision);
            fillAdCarBox();

            test = 3;
            fillGridView("getAllCars");

            ActualData();
            tbAllProces.AppendText("Załadowanie funkcji dodającej samochód.");
        }

        private void buttonAddCar_Click(object sender, EventArgs e)
        {
            rbCarSelectAll.Checked = true;
            
            HideAll();
            gbSearchCar.Show();
            gbAvailableCars.Show();
            gbAddCar.Show();

            test = 3;

            fillCommisionComboBox(cbAdCarCommision);
            fillAdCarBox();


            fillGridView("getAllCars");

            ActualData();
            tbAllProces.AppendText("Załadowanie funkcji dodającej samochód.");
        }

        //4. Dodanie stanowiska pracy

        private void stanowiskoPracyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gbAvailableCars.Hide();
            if (con.State == ConnectionState.Open) con.Close();
            HideAll();
            gbAddJob.Show();
            fillGridView("selectJob");

            ActualData();
            tbAllProces.AppendText("Załadowanie funkcji dodającej stanowiska.");
            test = 4;
        }


        private void buttonAddJob_Click(object sender, EventArgs e)
        {
            gbAvailableCars.Hide();
            if (con.State == ConnectionState.Open) con.Close();
            HideAll();
            gbAddJob.Show();
            fillGridView("selectJob");

            ActualData();
            tbAllProces.AppendText("Załadowanie funkcji dodającej stanowiska.");
            test = 4;
        }


        //5. dodanie nowego klienta
        private void użytkownikaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            gbAvailableCars.Hide();
            fillGridView("selectClient");
            HideAll();
            gbAdClient.Show();


            ActualData();
            tbAllProces.AppendText("Załadowanie funkcji dodającej klienta.");
            test = 5;
        }

        private void buttonAddClient_Click(object sender, EventArgs e)
        {
            gbAvailableCars.Hide();
            fillGridView("selectClient");
            HideAll();
            gbAdClient.Show();
            

            ActualData();
            tbAllProces.AppendText("Załadowanie funkcji dodającej klienta.");
            test = 5;
        }





        //6. dodanie nowej transacji
        private void transakcjeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            test = 6;
            HideAll();
            gbAdTran.Show();
            dataGridView.DataSource = null;

        }

        private void buttonAddTransaction_Click(object sender, EventArgs e)
        {
            test = 6;
            HideAll();
            gbAdTran.Show();
            dataGridView.DataSource = null;
        }




        //użyte funkcje:

        void fillCommisionComboBox (ComboBox combobox)
        {
            if (con.State == ConnectionState.Open) con.Close();

            ActualData();
            tbAllProces.AppendText("Załadowanie funkcji dodającej pracowników.");

            string selectAllCommisions = "SELECT commision.commision_id, l.city, l.STREET_ADDRESS " +
                "FROM commision " +
                "INNER JOIN location l on l.Location_ID = COMMISION.Location_ID";

            cmd = new SqlCommand(selectAllCommisions, con);
            con.Open();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            sda.Fill(ds);
            combobox.Items.Clear();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                combobox.Items.Add(ds.Tables[0].Rows[i][0] + ". " + ds.Tables[0].Rows[i][1] + ", " + ds.Tables[0].Rows[i][2]);
            }
            combobox.SelectedIndex = 0;
            ActualData();
            tbAllProces.AppendText("Przypisanie odpowiednich komisów");
            con.Close();

        }

        void fillSalaryCombobox(ComboBox comboBox)
        {
            if (con.State == ConnectionState.Open) con.Close();
            string querySelectJobTitle = "Select job_id, job_title, min_salary, max_salary FROM jobs";

            cmd = new SqlCommand(querySelectJobTitle, con);
            SqlDataAdapter dataAdapter = new SqlDataAdapter(querySelectJobTitle, con);
            con.Open();
            DataSet data = new DataSet();

            dataAdapter.Fill(data);
            comboBoxJobTitle.Items.Clear();
            
            for (int i = 0; i < data.Tables[0].Rows.Count; i++)
            {
                comboBoxJobTitle.Items.Add(data.Tables[0].Rows[i][0] + ". " + data.Tables[0].Rows[i][1] + "   Pensja: < " + data.Tables[0].Rows[i][2] + ",  " + data.Tables[0].Rows[i][3] + " >");
            }
            comboBoxJobTitle.SelectedIndex = 0;
            con.Close();
        }


        void readAddEmployee()
        {
            fillCommisionComboBox(comboBoxCommision);
            fillSalaryCombobox(comboBoxJobTitle);
        }

        void fillAdCarBox()
        {

            tbAdCarBrand.Clear();
            tbAdCarDate.Clear();
            tbAdCarEngine.Clear();
            tbAdCarEnginePower.Clear();
            tbAdCarMillage.Clear();
            tbAdCarName.Clear();

            string[] Text = new string[] { "Sedan", "Hatchback", "Liftback", "Kombi", "VAN", "Minivan", "Kabriolet", "Kabriolet" };
            cbAdCarType.Items.Clear();
            for (int i = 0; i < 8; i++)
            {
                cbAdCarType.Items.Add(Text[i]);
                cbSelectCarType.Items.Add(Text[i]);
            }
            cbAdCarType.SelectedIndex = 0;
            cbSelectCarType.SelectedIndex = -1;

            cbAdCarGearCase.Items.Clear();
            cbAdCarGearCase.Items.Add("Automatyczna");
            cbAdCarGearCase.Items.Add("Manualna");
            cbAdCarGearCase.SelectedIndex = 0;


        }


        void fillGridView(string command)
        {
            if (con.State == ConnectionState.Open) con.Close();
            //try
            //{
                dataGridView.DataSource = null;
            con.Open();
                SqlDataAdapter sqlDA = new SqlDataAdapter(command, con);
                DataTable dtbl = new DataTable();
                sqlDA.Fill(dtbl);

                dataGridView.DataSource = dtbl;
                con.Close();
            //}
            //catch
            //{
            //    MessageBox.Show("Błąd podglądu danych");
            //}
        }

        void villGridView(string command, string parametrName, string parametrValue)
        {
            if (con.State == ConnectionState.Open) con.Close();
            try
            {
                dataGridView.DataSource = null;
                con.Open();
                SqlDataAdapter sqlDA = new SqlDataAdapter(command, con);
                DataTable dtbl = new DataTable();
                sqlDA.Fill(dtbl);

                dataGridView.DataSource = dtbl;
                con.Close();
            }
            catch
            {
                MessageBox.Show("Błąd podglądu danych");
            }
        }

        void fillComAdManCb()
        {
            try
            {
                if (con.State == ConnectionState.Open) con.Close();
                string query = "SELECT Employee_id, First_name, Secound_name from employees inner join users on users.User_id = EMPLOYEES.User_id  where Job_id = 1;";


                cmd = new SqlCommand(query, con);
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, con);
                con.Open();
                DataSet data = new DataSet();

                dataAdapter.Fill(data);
                cbAdComManager.Items.Clear();

                for (int i = 0; i < data.Tables[0].Rows.Count; i++)
                {
                    cbAdComManager.Items.Add(data.Tables[0].Rows[i][0] + ". " + data.Tables[0].Rows[i][1] + "  " + data.Tables[0].Rows[i][2]);
                }
                cbAdComManager.SelectedIndex = 0;
                con.Close();

            }
            catch
            {
                MessageBox.Show("Errorek");
            }
        }



















        private void buttonConfirmAddJob_Click(object sender, EventArgs e)
        {
            if (con.State == ConnectionState.Open) con.Close();
            float minSalary = 0.00f;
            float maxSalary = 0.00f;


            if (textBoxJobTitle.Text != "" && textBoxJobMinSalary.Text != "" && textBoxJobMaxSalary.Text != "")
            {
                try
                {
                    if (con.State == ConnectionState.Open) con.Close();

                    minSalary = float.Parse(textBoxJobMinSalary.Text);
                    maxSalary = float.Parse(textBoxJobMaxSalary.Text);

                    
                    
                    if (minSalary < maxSalary)
                    {
                        con.Open();

                        string command = "exec addJob @jobTitle = " + textBoxJobTitle.Text+ "," +
                            " @minSal = " + minSalary +
                            ", @maxSal = " + maxSalary + ";";
                        Console.WriteLine(command);

                        cmd = new SqlCommand(command, con);


                        Console.WriteLine(textBoxJobMinSalary.Text + "  " + textBoxJobMinSalary.Text);
                        cmd.ExecuteNonQuery();
                        con.Close();

                        fillGridView("selectJob");

                        ActualData();
                        tbAllProces.AppendText("Poprawne wprowadzenie danych");
                    }

                    else
                    {
                        ActualData();
                       MessageBox.Show("ERROR: Minimalne wynagrodzenie nie może być większe od maksymalnego");
                        tbAllProces.AppendText("ERROR: Blad danych (wynagrodzenie min > wynagrodzenie max)");
                    }
                }
                catch (Exception)
                {
                    ActualData();
                    MessageBox.Show("ERROR: Nieprawidłowy format danych");
                    tbAllProces.AppendText("ERROR: Nieprawidłowy format danych / brak praw do wykonania funkcji");
                }
            }
            else
            {
                ActualData();
                MessageBox.Show("ERROR: Wszystkie dane z * muszą zostać wypełnione");
                tbAllProces.AppendText("ERROR: Wszystkie dane z * muszą zostać wypełnione");
            }
        }

        private void buttonConfirmAddEmployee_Click(object sender, EventArgs e)
        {
            if (con.State == ConnectionState.Open) con.Close();

            if (tbEmplCity.Text != "" && tbEmplCountry.Text != "" && tbEmplLastName.Text != "" && tbEmplName.Text != "" && tbEmplPostalCode.Text != "" && tbEmplStreet.Text != "")
            {
                try
                {
                    int jobValue, emplValue, userValue, commisionValue, transactionValue, carsValue;

                    if (rbAdEmpJobYes.Checked) jobValue = 1;
                    else jobValue = 0;

                    if (rbAdEmpEmplYes.Checked) emplValue = 1;
                    else emplValue = 0;

                    if (rbAdEmpClientYes.Checked) userValue = 1;
                    else userValue = 0;

                    if (rbAdEmpCommYes.Checked) commisionValue = 1;
                    else commisionValue = 0;

                    if (rbAdEmpTransYes.Checked) transactionValue = 1;
                    else transactionValue = 0;

                    if (rbAdEmpCarsYes.Checked) carsValue = 1;
                    else carsValue = 0;

                    string commisionID =  comboBoxCommision.Text;
                    string jobID = comboBoxJobTitle.Text;
                    string managerID = comboBoxManager.Text;

                    string[] comId = commisionID.Split('.');
                    string[] jobbId = jobID.Split('.');
                    string[] managId = managerID.Split('.');

                    //do sprawdzenia czy wpisana kwota jest pomiędzy tym co można
                    string printOutput = "";
                    cmd = new SqlCommand("printMinMaxSalary", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("job_id", jobbId[0]));

                    con.Open();
                    con.InfoMessage += (object obj, SqlInfoMessageEventArgs w) =>
                    {
                        printOutput += w.Message;
                    };
                    cmd.ExecuteNonQuery();
                    con.Close();

                    float checkMin = 0, checkMax = 100000;

                    string[] newText = printOutput.Split(' ');
                    checkMin = float.Parse(newText[0]);
                    checkMax = float.Parse(newText[1]);

                    float test = float.Parse(tbEmplSalary.Text);
                    TestButton.Text = test.ToString();
                    //Console.WriteLine(checkMin + "    " + checkMax + "    " + int.Parse(tbEmplSalary.Text));
                    if (float.Parse(tbEmplSalary.Text) >= checkMin && float.Parse(tbEmplSalary.Text) <= checkMax)
                    {
                        
                        cmd = new SqlCommand("addNewEmployee", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("streetAddres", tbEmplStreet.Text));
                        cmd.Parameters.Add(new SqlParameter("postalCode", tbEmplPostalCode.Text));
                        cmd.Parameters.Add(new SqlParameter("cityName", tbEmplCity.Text));
                        cmd.Parameters.Add(new SqlParameter("countryName", tbEmplCountry.Text));
                        cmd.Parameters.Add(new SqlParameter("firstName", tbEmplName.Text));
                        cmd.Parameters.Add(new SqlParameter("secound_name", tbEmplLastName.Text));
                        cmd.Parameters.Add(new SqlParameter("mail", tbEmplEmail.Text));
                        cmd.Parameters.Add(new SqlParameter("jobId", jobbId[0]));
                        cmd.Parameters.Add(new SqlParameter("commisionId", comId[0]));
                        cmd.Parameters.Add(new SqlParameter("salaryValue", float.Parse(tbEmplSalary.Text)));
                        cmd.Parameters.Add(new SqlParameter("managerId", managId[0]));
                        cmd.Parameters.Add(new SqlParameter("userUsername", tbEmplUsername.Text));
                        cmd.Parameters.Add(new SqlParameter("userPassword", tbEmplPassword.Text));
                        cmd.Parameters.Add(new SqlParameter("jobsManager", jobValue));
                        cmd.Parameters.Add(new SqlParameter("employeesManager", emplValue));
                        cmd.Parameters.Add(new SqlParameter("usersManager", userValue));
                        cmd.Parameters.Add(new SqlParameter("comissionManager", commisionValue));
                        cmd.Parameters.Add(new SqlParameter("transactionManager", transactionValue));
                        cmd.Parameters.Add(new SqlParameter("carsManager", carsValue));

                        if (con.State == ConnectionState.Open) con.Close();

                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();

                        fillGridView("selectEmployee");

                        ActualData();
                        tbAllProces.AppendText("Poprawne wprowadzenie danych");
                    }
                    else MessageBox.Show("Pensja musi być liczbą z przedziału dozwolonych wartości (patrz wartości stanowiska)");
                }
                catch (Exception)
                {
                    ActualData();
                    MessageBox.Show("ERROR: Niewłaściwy format danych w lokacji. Bład w dodaniu pracownika do lokacji");
                    tbAllProces.AppendText("ERROR: Niewłaściwy format danych w lokacji. Bład w dodaniu pracownika do lokacji / brak wymaganych praw");
                }
            }

            else MessageBox.Show("ERROR: Pola z * muszą być wypełnione");
           
        }

        string GetLocationEmplId()
        {


            string selectChoosenLoc = "SELECT location_id FROM location WHERE " +
        "street_address = '" + tbEmplStreet.Text + "' AND postal_code = '" + tbEmplPostalCode.Text + "' AND city = '" + tbEmplCity.Text + "' AND country = '" + tbEmplCountry.Text + "';";
            SqlCommand command = new SqlCommand(selectChoosenLoc, con);


            try
            {
                con.Open();
                temporaryInt = (int)command.ExecuteScalar();
                locationIdEmployee = temporaryInt.ToString();

            }
            catch (Exception)
            {
                locationIdEmployee = "null";
            }
            con.Close();
            return locationIdEmployee;
        }


 

        private void buttonAddCountry_Click(object sender, EventArgs e)
        {
            try
            {
                if (tbAdComStreet.Text != "" && tbAdComCity.Text != "" && tbAdComStreet.Text != "" &&
                    tbAdComPostalCode.Text != "" && tbAdComCountry.Text != "")
                {

                    if (con.State == ConnectionState.Open) con.Close();
                    int postalCode = 0;
                    try
                    {
                         postalCode = Int32.Parse(tbAdComPostalCode.Text);
                    }

                    catch
                    {
                        MessageBox.Show("Niewłaściwy format kodu pocztowego. Powinien zawierać 5 cyfr");
                    }
                    string getText = cbAdComManager.Text;
                    string[] getId = getText.Split('.');

                    cmd = new SqlCommand("addCommision", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("streetAddres", tbAdComStreet.Text));
                    cmd.Parameters.Add(new SqlParameter("cityAddres", tbAdComCity.Text));
                    cmd.Parameters.Add(new SqlParameter("postalCodeAddres", postalCode));
                    cmd.Parameters.Add(new SqlParameter("countr", tbAdComCountry.Text));
                    cmd.Parameters.Add(new SqlParameter("id", getId[0]));

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    fillGridView("selectCommision");
                    ActualData();
                    tbAllProces.AppendText("Komis został dodany pomyślnie");
                }
                else MessageBox.Show("Pola z * muszą być wypełnione");
        }
            catch
            {
                MessageBox.Show("Nie udało się utworzyć komis. Sprawdź poprawność danych ");
            }
}

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (tbAdCarBrand.Text == "" && tbAdCarName.Text == "" && cbAdCarType.Text == "" && tbAdCarMillage.Text == "" &&
                    tbAdCarDate.Text == "" && cbAdCarGearCase.Text == "" && tbAdCarEngine.Text == "" && tbAdCarEnginePower.Text == "")
                    MessageBox.Show("Pola z * muszą być wypełnione");
                else
                {
                    if (con.State == ConnectionState.Open) con.Close();

                    string text = cbAdCarCommision.Text;
                    string[] comID = text.Split('.');

                    int airVal, carDriveVal;

                    if (rbAdCarAirCondX2.Checked) airVal = 0;
                    else airVal = 1;

                    if (rbAdCarGearCaseYes.Checked) carDriveVal = 1;
                    else carDriveVal = 0;

                    cmd = new SqlCommand("addCar", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("brand_val", tbAdCarBrand.Text));
                    cmd.Parameters.Add(new SqlParameter("name_val", tbAdCarName.Text));
                    cmd.Parameters.Add(new SqlParameter("commision_id", comID[0]));
                    cmd.Parameters.Add(new SqlParameter("type_val", cbAdCarType.Text));
                    cmd.Parameters.Add(new SqlParameter("mil_val", int.Parse(tbAdCarMillage.Text)));
                    cmd.Parameters.Add(new SqlParameter("date_val", int.Parse(tbAdCarDate.Text)));
                    cmd.Parameters.Add(new SqlParameter("air_cond_val", airVal));
                    cmd.Parameters.Add(new SqlParameter("car_driv_val", carDriveVal));
                    cmd.Parameters.Add(new SqlParameter("gear_case_val", cbAdCarGearCase.Text));
                    cmd.Parameters.Add(new SqlParameter("engine_name_val", tbAdCarEngine.Text));
                    cmd.Parameters.Add(new SqlParameter("eng_pow_val", int.Parse(tbAdCarEnginePower.Text)));

                    Console.WriteLine(int.Parse(tbAdCarMillage.Text));
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    fillGridView("getAllCars");
                    ActualData();
                    tbAllProces.AppendText("Poprawne wprowadzenie danych");

                    if (dataGridView.Enabled == false) dataGridView.Enabled = true;
                }
            }
            catch
            {
                MessageBox.Show("Nie udało się dodać samochodu. Sprawdź poprawność danych");
            }
        }

        private void buttonAdClient_Click(object sender, EventArgs e)
        {
            try
            {
                if (tbAdClientCity.Text == "" || tbAdClientCountry.Text == "" || tbAdClientFirstName.Text == "" || tbAdClientMail.Text == "" || tbAdClientPostalCode.Text == "" || tbAdClientSecoundName.Text == "" || tbAdClientStreet.Text == "")
                {
                     MessageBox.Show("Pola z * muszą być wypełnione");
                }
                else
                {
                    if (con.State == ConnectionState.Open) con.Close();

                    cmd = new SqlCommand("addUser", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("street_val", tbAdClientStreet.Text));
                    cmd.Parameters.Add(new SqlParameter("post_cod_val", tbAdClientPostalCode.Text));
                    cmd.Parameters.Add(new SqlParameter("city_val", tbAdClientCity.Text));
                    cmd.Parameters.Add(new SqlParameter("countryy", tbAdClientCountry.Text));
                    cmd.Parameters.Add(new SqlParameter("first_nam_val", tbAdClientFirstName.Text));
                    cmd.Parameters.Add(new SqlParameter("secound_nam_val", tbAdClientSecoundName.Text));
                    cmd.Parameters.Add(new SqlParameter("mail", tbAdClientMail.Text));

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    fillGridView("selectClient");

                    ActualData();
                    tbAllProces.AppendText("Poprawne wprowadzenie danych");
                }
            }
            catch { MessageBox.Show("Błąd w dodaniu danych. Sprawdź poprawność wprowadzonych danych"); }
        }



        string GetUserId()
        {
            string selectChoosenUser = "SELECT user_id FROM Users WHERE" +
                "First_name = '" + tbEmplName.Text + "' AND Secound_name = '" + tbEmplLastName.Text + "' AND Email = '" + tbEmplEmail.Text + "';";
            cmd = new SqlCommand(selectChoosenUser, con);

            try
            {
                con.Open();
                temporaryInt = (int)cmd.ExecuteScalar();
                userIdEmployee = temporaryInt.ToString();

            }
            catch (Exception)
            {
                userIdEmployee = "null";
            }
            con.Close();

            return userIdEmployee;
        }


        //przycisk do testowania czy dziala cos
        private void TestButton_Click(object sender, EventArgs e)
        {
            if (con.State == ConnectionState.Open) con.Close();

        }



        //funkcja do pobierania danych z serwera (print)
        static void connection_InfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            // this gets the print statements (maybe the error statements?)
            var outputFromStoredProcedure = e.Message;
        }

        int id;

        //okresla id tego w co klikne i przypisuje wedlug tego info w odpowiednie miejsce
        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
            int index = e.RowIndex;
            DataGridViewRow selectedRow = dataGridView.Rows[index];
            dataGridView.Rows[index].Selected = true;
            
            switch(test)
            {
                case 1:
                    if (con.State == ConnectionState.Open) con.Close();

                    id = Int32.Parse(selectedRow.Cells[0].Value.ToString());
                   
                    try
                    {
                        if (con.State == ConnectionState.Open) con.Close();
                        cmd = new SqlCommand("getComInfo", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("comId", id));
                        con.Open();
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                               tbAdComStreet.Text = dr[0].ToString();
                               tbAdComCity.Text = dr[2].ToString();
                               tbAdComPostalCode.Text = dr[1].ToString();
                               tbAdComCountry.Text = dr[3].ToString();

                                choseIdAndGetIndexOfComboBox(cbAdComManager, dr[4].ToString());
                            }
                        }
                        
                        con.Close();
                    }
                    catch { MessageBox.Show("Error"); }
                    break;
                case 2:
                    if (con.State == ConnectionState.Open) con.Close();

                    id = Int32.Parse(selectedRow.Cells[0].Value.ToString());

                    try
                    {
                        if (con.State == ConnectionState.Open) con.Close();
                        cmd = new SqlCommand("getEmplInfo", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("userID", id));
                        con.Open();
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                            string[] test = new string[19];
                                for(int i=0;i<19; i++)
                                {
                                test[i] = rewriteCorrectId(dr[i].ToString());
                                }
                            tbEmplUsername.Text = test[0];
                            tbEmplPassword.Text = test[1];
                            tbEmplName.Text = test[2];
                            tbEmplLastName.Text = test[3];
                            tbEmplEmail.Text = test[4];
                            tbEmplStreet.Text = test[5];
                            tbEmplPostalCode.Text = test[6];
                            tbEmplCity.Text = test[7];
                            tbEmplCountry.Text = test[8];


                            choseIdAndGetIndexOfComboBox(comboBoxCommision, rewriteCorrectId(test[9]));
                            choseIdAndGetIndexOfComboBox(comboBoxManager, rewriteCorrectId(test[10]));
                            choseIdAndGetIndexOfComboBox(comboBoxJobTitle, rewriteCorrectId(test[11]));

                            tbEmplSalary.Text = test[12];



                            if (Int32.Parse(rewriteCorrectId(test[13])) == 0) rbAdEmpEmplNo.Checked = true;
                            else rbAdEmpJobYes.Checked = true;

                            if (Int32.Parse(rewriteCorrectId(test[14])) == 0) rbAdEmpEmplNo.Checked = true;
                            else rbAdEmpEmplYes.Checked = true;

                            if (Int32.Parse(rewriteCorrectId(test[15])) == 0) rbAdEmpClientNo.Checked = true;
                            else rbAdEmpClientYes.Checked = true;

                            if (Int32.Parse(rewriteCorrectId(test[16])) == 0) rbAdEmpCommNo.Checked = true;
                            else rbAdEmpCommYes.Checked = true;

                            if (Int32.Parse(rewriteCorrectId(test[17])) == 0) rbAdEmpTransNo.Checked = true;
                            else rbAdEmpTransYes.Checked = true;

                            if (Int32.Parse(rewriteCorrectId(test[18])) == 0) rbAdEmpCarsNo.Checked = true;
                            else rbAdEmpCarsYes.Checked = true;
                            }
                        }

                        con.Close();
                    }
                    catch {
                    }

                    break;
                case 3:
                    if (con.State == ConnectionState.Open) con.Close();

                    id = Int32.Parse(selectedRow.Cells[0].Value.ToString());

                    try
                    {
                        if (con.State == ConnectionState.Open) con.Close();
                        cmd = new SqlCommand("getCarInfo", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("id", id));
                        con.Open();
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                choseIdAndGetIndexOfComboBox(cbAdCarCommision, dr[1].ToString());
                                tbAdCarBrand.Text = dr[2].ToString();
                                tbAdCarName.Text = dr[3].ToString();
                                choseIdAndGetIndexOfComboBox(cbAdCarType, dr[4].ToString());
                                tbAdCarMillage.Text = dr[5].ToString();
                                tbAdCarDate.Text = dr[6].ToString();

                                if (Int32.Parse(rewriteCorrectId(dr[7].ToString())) == 0) rbAdCarAirCondX2.Checked = true;
                                else rbAdCarAirCondX4.Checked = true;

                                if (Int32.Parse(rewriteCorrectId(dr[8].ToString())) == 0) rbAdCarGearCaseNo.Checked = true;
                                else rbAdCarGearCaseYes.Checked = true;
                                choseIdAndGetIndexOfComboBox(cbAdCarGearCase, dr[9].ToString());
                                tbAdCarEngine.Text = dr[10].ToString();
                                tbAdCarEnginePower.Text = dr[11].ToString();

                            }
                        }

                        if (con.State == ConnectionState.Open) con.Close();
                     }
                    catch { MessageBox.Show("Error"); }

            break;
                case 4:
                    id = Int32.Parse(selectedRow.Cells[0].Value.ToString());
                    textBoxJobTitle.Text = selectedRow.Cells[1].Value.ToString();
                    textBoxJobMinSalary.Text = selectedRow.Cells[2].Value.ToString();
                    textBoxJobMaxSalary.Text = selectedRow.Cells[3].Value.ToString();
                    
                    break;
                case 5:
                    id = Int32.Parse(selectedRow.Cells[0].Value.ToString());

                    try
                    {
                        if (con.State == ConnectionState.Open) con.Close();
                        cmd = new SqlCommand("getClientInfo", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("client_id", id));
                        con.Open();
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                tbAdClientFirstName.Text = dr[0].ToString();
                                tbAdClientSecoundName.Text = dr[1].ToString();
                                tbAdClientMail.Text = dr[2].ToString();
                                tbAdClientStreet.Text = dr[3].ToString();
                                tbAdClientPostalCode.Text = dr[4].ToString();
                                tbAdClientCity.Text = dr[5].ToString();
                                tbAdClientCountry.Text = dr[6].ToString();
                            }
                        }

                        con.Close();
                    }
                    catch { MessageBox.Show("Error"); }
                    break;
                case 6:
                    Console.WriteLine("Case 2");
                    break;
            }
            
        }

        //************************
        //AKTUALIZACJA WSZYSTKIEGO
        //**********************

        #region Aktualizacja wszystkiego
        //1. KOMIS
        //*******************
        private void buttonUpdateCom_Click(object sender, EventArgs e)
        {
            string caption = "Error Detected in Input";
            string message = "Czy na pewno chcesz zaktualizować ten komis?";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;

            result = MessageBox.Show(message, caption, buttons);

            if (result == System.Windows.Forms.DialogResult.Yes)
            {

                try
                {
                    string text = cbAdComManager.Text;
                  string[] manId = text.Split('.');

                    cmd = new SqlCommand("updateCommision", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("com_id", id));
                    cmd.Parameters.Add(new SqlParameter("street", tbAdComStreet.Text));
                    cmd.Parameters.Add(new SqlParameter("city", tbAdComCity.Text));
                    cmd.Parameters.Add(new SqlParameter("postCod", tbAdComPostalCode.Text));
                    cmd.Parameters.Add(new SqlParameter("country", tbAdComCountry.Text));
                    cmd.Parameters.Add(new SqlParameter("manID", manId[0]));

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    fillGridView("selectCommision");
                    MessageBox.Show("Pomyślnie zaktualizowano komis");
                }
                catch
                {
                    MessageBox.Show("Nie udało się zaktualizować komisu");
                }


            }

        }

        
        //*******************
        //2. Aktualizacja pracownika

        private void buttonUpdateEmpl_Click(object sender, EventArgs e)
        {
            if (con.State == ConnectionState.Open) con.Close();

            if (tbEmplCity.Text != "" && tbEmplCountry.Text != "" && tbEmplLastName.Text != "" && tbEmplName.Text != "" && tbEmplPostalCode.Text != "" && tbEmplStreet.Text != "")
            {
                try
                {
                    int jobValue, emplValue, userValue, commisionValue, transactionValue, carsValue;

                    if (rbAdEmpJobYes.Checked) jobValue = 1;
                    else jobValue = 0;

                    if (rbAdEmpEmplYes.Checked) emplValue = 1;
                    else emplValue = 0;

                    if (rbAdEmpClientYes.Checked) userValue = 1;
                    else userValue = 0;

                    if (rbAdEmpCommYes.Checked) commisionValue = 1;
                    else commisionValue = 0;

                    if (rbAdEmpTransYes.Checked) transactionValue = 1;
                    else transactionValue = 0;

                    if (rbAdEmpCarsYes.Checked) carsValue = 1;
                    else carsValue = 0;

                    string commisionID = comboBoxCommision.Text;
                    string jobID = comboBoxJobTitle.Text;
                    string managerID = comboBoxManager.Text;

                    string[] comId = commisionID.Split('.');
                    string[] jobbId = jobID.Split('.');
                    string[] managId = managerID.Split('.');

                    //do sprawdzenia czy wpisana kwota jest pomiędzy tym co można
                    string printOutput = "";
                    cmd = new SqlCommand("printMinMaxSalary", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("job_id", jobbId[0]));

                    con.Open();
                    con.InfoMessage += (object obj, SqlInfoMessageEventArgs w) =>
                    {
                        printOutput += w.Message;
                    };
                    cmd.ExecuteNonQuery();
                    con.Close();

                    float checkMin = 0, checkMax = 100000;

                    string[] newText = printOutput.Split(' ');
                    checkMin = float.Parse(newText[0]);
                    checkMax = float.Parse(newText[1]);
                    //float test = float.Parse(tbEmplSalary.Text);
                    string[] temp = tbEmplSalary.Text.Split(',');
                    float test = float.Parse(temp[0]);
                    TestButton.Text = test.ToString();




                    if (float.Parse(tbEmplSalary.Text) >= checkMin && float.Parse(tbEmplSalary.Text) <= checkMax)
                    {
                    
                    cmd = new SqlCommand("updateEmployee", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("loginId", id));
                        cmd.Parameters.Add(new SqlParameter("user_username", tbEmplUsername.Text));
                        cmd.Parameters.Add(new SqlParameter("user_password", tbEmplPassword.Text));
                        cmd.Parameters.Add(new SqlParameter("first_name", tbEmplName.Text));
                        cmd.Parameters.Add(new SqlParameter("Secound_name", tbEmplLastName.Text));
                        
                        cmd.Parameters.Add(new SqlParameter("Email", tbEmplEmail.Text));
                        cmd.Parameters.Add(new SqlParameter("Street_address", tbEmplStreet.Text    ));
                        cmd.Parameters.Add(new SqlParameter("Postal_code", tbEmplPostalCode.Text    ));
                        cmd.Parameters.Add(new SqlParameter("City", tbEmplCity.Text));
                        cmd.Parameters.Add(new SqlParameter("country", tbEmplCountry.Text));
                        
                        cmd.Parameters.Add(new SqlParameter("Commision_id", comId[0]));
                        cmd.Parameters.Add(new SqlParameter("Manager_id", managId[0]));
                        cmd.Parameters.Add(new SqlParameter("Job_id", jobbId[0]));
                        cmd.Parameters.Add(new SqlParameter("Salary", tbEmplSalary.Text));
                        cmd.Parameters.Add(new SqlParameter("Jobs_manager", jobValue));
                        
                        cmd.Parameters.Add(new SqlParameter("Employees_manager", emplValue));
                        cmd.Parameters.Add(new SqlParameter("Users_manager", userValue ));
                        cmd.Parameters.Add(new SqlParameter("Commision_manager", commisionValue));
                        cmd.Parameters.Add(new SqlParameter("Transactions_manager", transactionValue));
                        cmd.Parameters.Add(new SqlParameter("Cars_manager", carsValue));



                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();

                        fillGridView("selectEmployee");
                        MessageBox.Show("Pomyślnie zaktualizowano pracownika");


                    }
                    else MessageBox.Show("Pensja musi być liczbą z przedziału dozwolonych wartości (patrz wartości stanowiska)");
            }
                catch (Exception)
            {
                ActualData();
                tbAllProces.AppendText("ERROR: Niewłaściwy format danych w lokacji. Bład w dodaniu pracownika do lokacji");
                }
            }

            else writeProcess("ERROR: Pola z * muszą być wypełnione");
        }

        //****************
        //3. Aktualizacja pojazdu

        private void buttonUpdateCar_Click(object sender, EventArgs e)
        {
            try
            {
                string message = "Czy na pewno chcesz zaktualizowac ten samochod?";
                string caption = "Error Detected in Input";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result;

                result = MessageBox.Show(message, caption, buttons);

                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    if (con.State == ConnectionState.Open) con.Close();

                    string text = cbAdCarCommision.Text;
                    string[] comID = text.Split('.');

                    int airVal, carDriveVal;

                    if (rbAdCarAirCondX2.Checked) airVal = 0;
                    else airVal = 1;

                    if (rbAdCarGearCaseYes.Checked) carDriveVal = 1;
                    else carDriveVal = 0;



                    cmd = new SqlCommand("updateCar", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("getId", id));
                    cmd.Parameters.Add(new SqlParameter("brand", tbAdCarBrand.Text));
                    cmd.Parameters.Add(new SqlParameter("name", tbAdCarName.Text));
                    cmd.Parameters.Add(new SqlParameter("comId", comID[0]));
                    cmd.Parameters.Add(new SqlParameter("type", cbAdCarType.Text));
                    cmd.Parameters.Add(new SqlParameter("millage", int.Parse(tbAdCarMillage.Text)));
                    cmd.Parameters.Add(new SqlParameter("date", int.Parse(tbAdCarDate.Text)));
                    cmd.Parameters.Add(new SqlParameter("airCon", airVal));
                    cmd.Parameters.Add(new SqlParameter("carDrive", carDriveVal));
                    cmd.Parameters.Add(new SqlParameter("gearCase", cbAdCarGearCase.Text));
                    cmd.Parameters.Add(new SqlParameter("eng", tbAdCarEngine.Text));
                    cmd.Parameters.Add(new SqlParameter("engVal", int.Parse(tbAdCarEnginePower.Text)));

                    Console.WriteLine(int.Parse(tbAdCarMillage.Text));
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    fillGridView("getAllCars");
                    ActualData();
                    tbAllProces.AppendText("Poprawne wprowadzenie danych");
                }
                }
            catch
            {
                MessageBox.Show("Nie udało się dodać samochód.");
            }
         
        }


        //***********
        //4. Stanowiska pracy
        private void buttonUpdateJob_Click(object sender, EventArgs e)
        {
            string message = "Czy na pewno chcesz zaktualizować to stanowisko?";
            string caption = "Error Detected in Input";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;

            result = MessageBox.Show(message, caption, buttons);

            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                float min = 0.00f, max = 0.00f;
                try
                {
                    min = float.Parse(textBoxJobMinSalary.Text);
                    max = float.Parse(textBoxJobMaxSalary.Text);
                }
                catch { MessageBox.Show("Błedny zapis wartości"); }

                if (min < max)
                {
                    try
                    {


                        cmd = new SqlCommand("updateJob", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("job_id", id));
                        cmd.Parameters.Add(new SqlParameter("job_name", textBoxJobTitle.Text));
                        cmd.Parameters.Add(new SqlParameter("job_min", min));
                        cmd.Parameters.Add(new SqlParameter("job_max", max));

                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();

                        fillGridView("selectJob");
                        MessageBox.Show("Pomyślnie zaktualizowano stanowisko");
                    }
                    catch
                    {
                        MessageBox.Show("Nie udało się zaktualizować stanowisko");
                    }
                }
                else MessageBox.Show("Pensja min musi być mniejsza od pensji max");
            }

        }


        //***********
        //5. Klienta
        private void buttonUpdateClient_Click(object sender, EventArgs e)
        {
            string message = "Czy na pewno chcesz zaktualizować dane tego klienta?";
            string caption = "Error Detected in Input";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;

            result = MessageBox.Show(message, caption, buttons);

            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                try
                {

                    cmd = new SqlCommand("updateClient", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("user_id", id));
                    cmd.Parameters.Add(new SqlParameter("name", tbAdClientFirstName.Text));
                    cmd.Parameters.Add(new SqlParameter("secName", tbAdClientSecoundName.Text));
                    cmd.Parameters.Add(new SqlParameter("email", tbAdClientMail.Text));
                    cmd.Parameters.Add(new SqlParameter("street", tbAdClientStreet.Text));
                    cmd.Parameters.Add(new SqlParameter("city", tbAdClientCity.Text));
                    cmd.Parameters.Add(new SqlParameter("postCod", tbAdClientPostalCode.Text));
                    cmd.Parameters.Add(new SqlParameter("country", tbAdClientCountry.Text));

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    fillGridView("[dbo].[selectClient]");
                    MessageBox.Show("Pomyślnie zaktualizowano dane klienta");
                }
                catch
                {
                    MessageBox.Show("Nie udało się zaktualizować danych klienta");
                }
            }
        }


        //***********
        //6. Transakcji

        #endregion



        //****************************
        //USUWANIE
        //****************************
        #region

        //***********
        //2. PRACOWNIKA
        private void buttonDeleteEmpl_Click(object sender, EventArgs e)
        {
            string message = "Czy na pewno chcesz usunąć tego pracownika?";
            string caption = "Error Detected in Input";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;

            result = MessageBox.Show(message, caption, buttons);

            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                try
                {
                    cmd = new SqlCommand("deleteWorker", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("loginId", id));


                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    fillGridView("selectEmployee");
                    MessageBox.Show("Pomyślnie usunięto pracownika");
                }
                catch
                {
                    MessageBox.Show("Nie udało się usunąć pracownika");
                }

            }
        }

        private void buttonDeleteCom_Click(object sender, EventArgs e)
        {
            string message = "Czy na pewno chcesz zaktualizować ten komis?";
            string caption = "Error Detected in Input";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;

            result = MessageBox.Show(message, caption, buttons);

            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                try
                {
                    cmd = new SqlCommand("deleteCommision", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("commisionID", id));


                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    fillGridView("selectCommision");
                    MessageBox.Show("Pomyślnie usunięto komis");
                }
                catch
                {
                    MessageBox.Show("Nie udało się usunąć komisu");
                }

            }
        }

       

        private void buttonDeleteJob_Click(object sender, EventArgs e)
        {
          string message = "Czy na pewno chcesz usunąć to stanowisko?";
            string caption = "Error Detected in Input";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;

            result = MessageBox.Show(message, caption, buttons);

            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                    try
                    {

                        cmd = new SqlCommand("deleteJob", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("job_id", id));

                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();

                        fillGridView("selectJob");
                        MessageBox.Show("Pomyślnie usunięto stanowisko");
                    }
                    catch
                    {
                        MessageBox.Show("Nie udało się usunąć stanowiska");
                    }
                
            }

        }

        

        private void buttonDeleteClient_Click(object sender, EventArgs e)
        {
            string message = "Czy na pewno chcesz usunąć klienta?";
            string caption = "Error Detected in Input";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;

            result = MessageBox.Show(message, caption, buttons);

            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                try
                {

                    cmd = new SqlCommand("deleteUser", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("user_id", id));

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    fillGridView("[dbo].[selectClient]");
                    MessageBox.Show("Pomyślnie zaktualizowano klienta");
                }
                catch
                {
                    MessageBox.Show("Nie udało się zaktualizować klienta");
                }
            }
        }
        #endregion

        private void comboBoxCommision_SelectedIndexChanged(object sender, EventArgs e)
        {
            getManagerOfCommision(comboBoxCommision, comboBoxManager);
        }

        

        void getManagerOfCommision(ComboBox commisionBox, ComboBox managerBox)
        {
            
                if (con.State == ConnectionState.Open) con.Close();
                managerBox.Items.Clear();
                managerBox.SelectedIndex = -1;

                string allAddres = commisionBox.Text;
                string[] words = allAddres.Split('.');//potrzebne do inicjowaniania zapytania

                //wyswietlanie wszystkich menagerow danego wybranego komisu(wybieram id komisu)
                string quere = "SELECT Login_setup_ID, first_name, secound_name, employees.Commision_id "
                         + "From LOGINSETUP "
                         + "inner join EMPLOYEES on EMPLOYEES.Employee_id = LOGINSETUP.Employee_id "
                         + "Inner join users ON users.user_id = employees.user_id "
                         + "Inner join jobs ON jobs.job_id = employees.job_id "
                         + "where employees.job_id = 1 AND (employees.commision_id = " + words[0]
                     + "or employees.employee_id = (select Commision_manager_ID from COMMISION where Commision_id = " + words[0] + ")); ";

                try
                {
                    Console.WriteLine(quere);

                    con.Open();

                    SqlDataAdapter sda = new SqlDataAdapter(quere, con);
                    DataSet ds = new DataSet();

                    try
                    {
                        sda.Fill(ds);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Nie znaleziono managera");
                    }

                    managerBox.Items.Clear();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        managerBox.Items.Add(ds.Tables[0].Rows[i][0] + ". " + ds.Tables[0].Rows[i][1] + " " + ds.Tables[0].Rows[i][2]);
                    }
                    ActualData();
                    Console.WriteLine("dziala 4");
                    tbAllProces.AppendText("Przypisanie odpowiednich komisów");
                    con.Close();

                    managerBox.SelectedIndex = 0;
                }
                catch (Exception)
                {
                    MessageBox.Show("Brak managera w tym komisie. Przypisz managera do wskazanego komisu lub skontaktuj się z administratorem");
                }
            
        }
        private void buttonClose_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void btSearchCar_Click(object sender, EventArgs e)
        {
            try
            {
                fillGridView("getAllCars");

                if (tbSearchCarBrand.Text != "")
                    for(int v = 0; v < dataGridView.Rows.Count; v++)
                    {
                        if (dataGridView[2, v].Value.ToString() != tbSearchCarBrand.Text)
                        {
                        dataGridView.Rows.RemoveAt(v);
                        v--;
                        }
                    }

                if (tbSearchCarName.Text != "")
                    for (int v = 0; v < dataGridView.Rows.Count; v++)
                    {
                        if (dataGridView[3, v].Value.ToString() != tbSearchCarName.Text)
                        {
                            dataGridView.Rows.RemoveAt(v);
                            v--;
                        }
                    }

                int temp = 0;
                try
                {
                    temp = Int32.Parse(tbSearchCarDate.Text);

                    if (temp > 1990 && temp < 2020)
                        for (int v = 0; v < dataGridView.Rows.Count; v++)
                        {
                            if (dataGridView[6, v].Value.ToString() != temp.ToString())
                            {
                                dataGridView.Rows.RemoveAt(v);
                                v--;
                            }
                        }
                }
                catch
                {

                }

                if (cbSelectCarType.Text != "")
                    for (int v = 0; v < dataGridView.Rows.Count; v++)
                    {
                        if (dataGridView[5, v].Value.ToString() != cbSelectCarType.Text)
                        {
                            dataGridView.Rows.RemoveAt(v);
                            v--;
                        }
                    }

                if (rbCarSelectAvailable.Checked)
                {
                    for (int v = 0; v < dataGridView.Rows.Count; v++)
                    {
                        if (dataGridView[4, v].Value.ToString() != "False")
                        {
                            dataGridView.Rows.RemoveAt(v);
                            v--;
                        }
                    }
                }
                else if (rbCarSelectSold.Checked)
                {
                    for (int v = 0; v < dataGridView.Rows.Count; v++)
                    {
                        if (dataGridView[4, v].Value.ToString() != "True")
                        {
                            dataGridView.Rows.RemoveAt(v);
                            v--;
                        }
                    }
                }

            }
            catch
            {
                MessageBox.Show("Nie udalo sie cholibka");
            }
        }

        private void btResetCar_Click(object sender, EventArgs e)
        {
            fillGridView("getAllCars");
            tbSearchCarName.Text = "";
            tbSearchCarBrand.Text = "";
            tbSearchCarDate.Text = "";
            cbSelectCarType.Text = "";

            rbCarSelectAll.Checked = true;
        }

        void getAvailableCar()
        {
            fillGridView("getAllCars");

            for (int v = 0; v < dataGridView.Rows.Count; v++)
            {
                if (dataGridView[4, v].Value.ToString() != "False")
                {
                    dataGridView.Rows.RemoveAt(v);
                    v--;
                }
            }
        }

        int tranType = 0;
        
        
        
        
        void setClient()
        {
            gbAdTran.Hide();
            gbAddCar.Show();

            

            btAdTranGetCarId.Show();

            string[] Text = new string[] { "Sedan", "Hatchback", "Liftback", "Kombi", "VAN", "Minivan", "Kabriolet", "Kabriolet" };
            cbAdCarType.Items.Clear();
            for (int i = 0; i < 8; i++)
            {
                cbAdCarType.Items.Add(Text[i]);
                cbSelectCarType.Items.Add(Text[i]);
            }

            cbAdCarGearCase.Items.Add("Automatyczna");
            cbAdCarGearCase.Items.Add("Manualna");
            cbAdCarType.SelectedIndex = 0;
            cbAdCarGearCase.SelectedIndex = 0;


        }
        
        private void btAdTranBuy_Click(object sender, EventArgs e)
        {
            tranType = 0;
            setClient();

            buttonAdCar.Enabled = false;
            buttonUpdateCar.Enabled = false;
            buttonDeleteCar.Enabled = false;

            test = 2;

        }

        string name, brand, type, millage, newDate, gearCase, engName, engPow;

        private void btTranEnd_Click(object sender, EventArgs e)
        {
            gbAdTranCarPrize.Hide();
            buttonAdCar.Enabled = true;
            buttonUpdateCar.Enabled = true;
            buttonDeleteCar.Enabled = true;

        }

        private void btAdTranSell_Click(object sender, EventArgs e)
        {
            test = 3;
            tranType = 0;
            fillGridView("[dbo].[getAvailableCars]");
            setClient();
        }

        private void btAdTranGetCarId_Click(object sender, EventArgs e)
        {

            //dataGridView.Rows.Clear();
            dataGridView.DataSource = null;
            dataGridView.Enabled = true;

            name = tbAdCarName.Text;
            brand = tbAdCarBrand.Text;
            type = cbAdCarType.Text;
            millage = tbAdCarMillage.Text;



            if (rbAdCarAirCondX2.Checked) airVal = 0;
            else airVal = 1;

            if (rbAdCarGearCaseYes.Checked) carDriveVal = 1;
            else carDriveVal = 0;


            newDate = tbAdCarDate.Text;
            gearCase = cbAdCarGearCase.Text;
            engName = tbAdCarEngine.Text;
            engPow = tbAdCarEnginePower.Text;

            btAdTranGetCarId.Hide();
            HideAll();
            gbAdClient.Show();
            fillGridView("selectClient");
            btAdTranGetClientId.Show();

            //buttonAdClient.Enabled = false;
            //buttonUpdateClient.Enabled = false;
            //buttonDeleteClient.Enabled = false;

            test = 5;
        }




        string street, postalCode, cityVal, country, firstName, secName, mail;
        private void btAdTranGetClientId_Click(object sender, EventArgs e)
        {
            street = tbAdClientStreet.Text;
            postalCode = tbAdClientPostalCode.Text;
            cityVal = tbAdClientCity.Text;
            country = tbAdClientCountry.Text;
            firstName = tbAdClientFirstName.Text;
            secName = tbAdClientSecoundName.Text;
            mail = tbAdClientMail.Text;

            btAdTranGetClientId.Hide();
            HideAll();


            lbClientName.Text = "Imię: " + firstName;
            lbClientLastName.Text = "Nazwisko: " + secName;
            lbClientMail.Text = "E-mail: " + mail;

            lbClientStreet.Text = "Adres: " + street;
            lbClientPostCode.Text = "Kod pocztowy: " + postalCode;
            lbClientCity.Text = "Miasto: " + cityVal;
            lbClientCountry.Text = "Państwo: " + country;



            lbCarBrand.Text = "Marka: " + name;
            lbCarName.Text = "Nazwa: " + brand;
            lbCarType.Text = "Typ: " + type;
            lbCarMilage.Text = "Przebieg: " + millage;
            lbCarDate.Text = "Rocznik: " + newDate;

            string a, b;
            if (airVal == 1) a = "Przód-tył";
            else a = "Przód";

            if (carDriveVal == 1) b = "Tak";
            else b = "Nie";
            lbCarAirCon.Text = "Poduszki: " + a;
            lbCarDriveCase.Text = "Wspomaganie: " + b;
            lbCarGearCase.Text = "Skrzynia biegów: " + gearCase;
            lbCarEngName.Text = "Nazwa silnika: " + engName;
            lbCarEngPow.Text = "Moc silnika: " + engPow;

            lbEmplName.Text = "Imię: " + name55[0];
            lbEmplLastName.Text = "Nazwisko: " + name55[1];

            gbAdTranCarPrize.Show();
            fillGridView("selectTransaction");

            //MessageBox.Show(Int32.Parse(login).ToString());
        }


        private void btConfirmAdTran_Click(object sender, EventArgs e)
        {
            try
            {

                cmd = new SqlCommand("addTransaction", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("street_val", street));
                cmd.Parameters.Add(new SqlParameter("post_cod_val", Int32.Parse(postalCode)));
                cmd.Parameters.Add(new SqlParameter("city_val", cityVal));
                cmd.Parameters.Add(new SqlParameter("countryy", country));

                cmd.Parameters.Add(new SqlParameter("first_nam_val", firstName));
                cmd.Parameters.Add(new SqlParameter("secound_nam_val", secName));
                cmd.Parameters.Add(new SqlParameter("mail", mail));


                string[] newString = labelUserNick.Text.Split(' ');
                cmd.Parameters.Add(new SqlParameter("username", newString[1]));
                cmd.Parameters.Add(new SqlParameter("tranType", tranType));
                cmd.Parameters.Add(new SqlParameter("prize", float.Parse(adTranPrize.Text)));

                cmd.Parameters.Add(new SqlParameter("brand", brand));
                cmd.Parameters.Add(new SqlParameter("name", name));
                cmd.Parameters.Add(new SqlParameter("isSold", tranType));
                cmd.Parameters.Add(new SqlParameter("type", type));

                cmd.Parameters.Add(new SqlParameter("millage", Int32.Parse(millage)));
                cmd.Parameters.Add(new SqlParameter("date", newDate));
                cmd.Parameters.Add(new SqlParameter("AirCon", airVal));
                cmd.Parameters.Add(new SqlParameter("carDrive", carDriveVal));

                cmd.Parameters.Add(new SqlParameter("gearCase", gearCase));
                cmd.Parameters.Add(new SqlParameter("engine", engName));
                cmd.Parameters.Add(new SqlParameter("engPow", engPow));


                if (con.State == ConnectionState.Open) con.Close();

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                fillGridView("selectTransaction");

                ActualData();
                tbAllProces.AppendText("Poprawne wprowadzenie danych");

        }
            catch (Exception)
            {
                ActualData();
            MessageBox.Show("ERROR: Niewłaściwy format danych. Bład w dodaniu transakcji");
            }
}

        int airVal, carDriveVal;

      


        //ROZNE FUNKCJE

        void choseIdAndGetIndexOfComboBox(ComboBox comboBox, string choseId)//choseId to string z id ktorego szukamy w formacie id. costam
        {
            int i = 1;
            comboBox.SelectedIndex = 0;
            try
            {
                //MessageBox.Show(comboBox.Items.Count.ToString());
                for (; i < comboBox.Items.Count; i++)
                {
                    string value = comboBox.GetItemText(comboBox.Items[i]);
                    string[] newValue = value.Split('.');
                    // MessageBox.Show("newValue[0] = " + newValue[0] + "   choseId = " + choseId);
                    if (newValue[0] == choseId)
                    {
                        comboBox.SelectedIndex = i;
                        //break;
                    }
                }
            }
            catch { }
        }

        string rewriteCorrectId(string wrongId)
        {
            string correctId = wrongId;
            if (correctId == "False") correctId = "0";
            if (correctId == "True") correctId = "1";


            return correctId;
        }

    }


}





