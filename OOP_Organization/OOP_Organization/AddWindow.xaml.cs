using System;
using System.Windows;

namespace OOP_Organization
{
    public partial class AddWindow : Window
    {
        #region Fields;

        Repository repository; //Repository for Company DATA

        MainWindow mainWindow; //MainWindow reference

        Employee employee; //Temporarily Employee (with Data gotten from TextBoxes)

        int age; //Number to OUT from TryParse Employee Age checker

        /// <summary>
        /// Bool to CHECK if Input Data is correct
        /// </summary>
        bool inputDataIsCorrect => tbAddName.Text != ""
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
                repository.AddEmployee(tbAddName.Text, tbAddLastName.Text, age, (cbAddEmployeeDepartment.SelectedItem as Department).DepartmentName);
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

                repository[employee.EmployeeName,
                           employee.LastName,
                           employee.Age,
                           employee.Department] = newEnployeeData;

                CloseWindow();
            }
            else
                MessageBox.Show("The DATA you are entering is wrong!",
                                $"{AddWindow.TitleProperty.Name}",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
        }

        #endregion Elements' Methods

        #region Methods;

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
