using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace OOP_Organization
{
    public partial class AddWindow : Window
    {
        #region Fields;

        private Repository repository; //Repository for Company DATA

        private MainWindow mainWindow; //MainWindow reference

        private Employee employee; //Temporarily Employee (with Data gotten from TextBoxes)

        private int age; //Number to OUT from TryParse Employee Age checker

        private List<string> position = new List<string>() { "Head Of Organization", "Head Of Department", "Worker", "Intern" }; //Employee Position to SELECT

        private List<string> tempPosition = new List<string>(); //Temporary List for Employee Position

        private bool exclude => (cbAddEmployeeDepartment.SelectedItem as Department)?.DepartmentName != "Normandy"; //Bool to CHECK the Head Of Organization exclusion

        /// <summary>
        /// Bool to CHECK if Input Data is correct
        /// </summary>
        private bool inputDataIsCorrect => tbAddName.Text != ""
                       && tbAddLastName.Text != ""
                       && tbAddAge.Text != ""
                       && Int32.TryParse(tbAddAge.Text, out age)
                       && cbAddEmployeeDepartment.SelectedIndex > -1;

        #endregion Fields

        #region Constructor;

        /// <summary>
        /// Constructor with Repository & MainWindow
        /// </summary>
        /// <param name="Repository"></param>
        /// <param name="MainWindow"></param>
        public AddWindow(Repository Repository,
                         MainWindow MainWindow)
        {
            InitializeComponent();

            this.repository = Repository;
            this.mainWindow = MainWindow;

            cbAddEmployeeDepartment.ItemsSource = repository.DepartmentsDb;

            cbAddEmployeePosition.ItemsSource = GetPosition();

            btnEditEmployee.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Constructor with Repository, MainWindow & Employee
        /// </summary>
        /// <param name="Repository"></param>
        /// <param name="MainWindow"></param>
        /// <param name="Employee"></param>
        public AddWindow(Repository Repository,
                         MainWindow MainWindow,
                         Employee Employee)
        {
            InitializeComponent();

            this.repository = Repository;
            this.mainWindow = MainWindow;
            this.employee = Employee;

            cbAddEmployeeDepartment.ItemsSource = repository.DepartmentsDb;
            cbAddEmployeeDepartment.SelectedIndex = repository.DepartmentsDb.FindIndex(GetSelectedEmployeeDepartment);

            cbAddEmployeePosition.ItemsSource = GetPosition();
            cbAddEmployeePosition.SelectedIndex = GetPositionIndex();

            tbAddName.Text = employee.EmployeeName;
            tbAddLastName.Text = employee.LastName;
            tbAddAge.Text = Convert.ToString(employee.Age);

            btnAddEmployee.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// RETURNS Department for Selected Employee
        /// </summary>
        /// <param name="args">Selection argument</param>
        /// <returns></returns>
        private bool GetSelectedEmployeeDepartment(Department args)
        {
            return args.DepartmentName == employee.Department;
        }

        /// <summary>
        /// Method to GET Position for Employee Position Combo Box
        /// </summary>
        /// <returns></returns>
        private List<string> GetPosition()
        {
            if (exclude)
            {
                tempPosition = position.Where(ExcludeSelf).ToList();
                return tempPosition;
            }     
            else return position;
        }

        /// <summary>
        /// Method to GET correct default Index for Combo Box
        /// </summary>
        /// <returns></returns>
        private int GetPositionIndex()
        {
            if (employee.Department == "Normandy")
                return position.IndexOf(employee.Position);
            else
                return tempPosition.IndexOf(employee.Position);
        }
        
        /// <summary>
        /// Method to GET correct Index to Emplouee Position
        /// </summary>
        /// <returns></returns>
        private int GetPositionIndexFromComboBox()
        {
            return position.IndexOf(cbAddEmployeePosition.Text);
        }

        /// <summary>
        /// Method to EXCLUDE HeadOfOrganization from Employee Position List
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private bool ExcludeSelf(string args)
        {
            return !args.Equals(position[0]);
        }

        #endregion Constructor

        #region Elements' Methods;

        // <summary>
        /// Button Method to ADD Employee
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAddEmployee(object sender, RoutedEventArgs e)
        {
            if (inputDataIsCorrect)
            {
                repository.AddEmployee(tbAddName.Text, tbAddLastName.Text, age, (cbAddEmployeeDepartment.SelectedItem as Department).DepartmentName, cbAddEmployeePosition.SelectedIndex);
                CloseWindow();
            }
            else
                MessageBox.Show("The DATA you are entering is wrong!",
                    $"{AddWindow.TitleProperty.Name}",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
        }

        /// <summary>
        /// Button Method to EDIT Employee
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnEditEmployee(object sender, RoutedEventArgs e)
        {
            if (inputDataIsCorrect)
            {
                var newEnployeeData = new Employee(tbAddName.Text,
                                                   tbAddLastName.Text,
                                                   age,
                                                   (cbAddEmployeeDepartment.SelectedItem as Department).DepartmentName,
                                                   employee.DaysWorked);
               
                switch (GetPositionIndexFromComboBox())
                {
                    case 0: newEnployeeData = new HeadOfOrganization(); break;
                    case 1: newEnployeeData = new HeadOfDepartment(); break;
                    case 2: newEnployeeData = new Worker(); break;
                    case 3: newEnployeeData = new Intern(); break;
                    default: newEnployeeData = new Employee(tbAddName.Text,
                                                   tbAddLastName.Text,
                                                   age,
                                                   (cbAddEmployeeDepartment.SelectedItem as Department).DepartmentName,
                                                   employee.DaysWorked); break;
                }

                DefineEmployeeClass(newEnployeeData);

                repository[employee.EmployeeName,
                           employee.LastName,
                           employee.Age,
                           employee.Department,
                           GetPositionIndex()] = newEnployeeData;

                CloseWindow();
            }
            else
                MessageBox.Show("The DATA you are entering is wrong!",
                                $"{AddWindow.TitleProperty.Name}",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
        }

        /// <summary>
        /// Method to UPDATE Employee Position list on Department change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbAddEmployeeDepartment_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            cbAddEmployeePosition.ItemsSource = GetPosition();
        }

        #endregion Elements' Methods

        #region Methods;

        /// <summary>
        /// Method to DEFINE Employee Class
        /// </summary>
        /// <param name="emply"></param>
        private void DefineEmployeeClass(Employee emply)
        {
            emply.EmployeeName = tbAddName.Text;                                                  
            emply.LastName = tbAddLastName.Text;
            emply.Age = age;
            emply.Department = (cbAddEmployeeDepartment.SelectedItem as Department).DepartmentName;
            emply.DaysWorked = employee.DaysWorked;
            emply.Repository = repository;
        }

        /// <summary>
        /// Method to CLOSE this Window
        /// </summary>
        private void CloseWindow()
        {
            mainWindow.LoadEmployeesToListView();
            this.Close();
        }

        #endregion Methods

        
    }
}
