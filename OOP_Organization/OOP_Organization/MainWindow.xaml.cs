using System.Windows;
using System.Windows.Controls;
using System.Linq;

namespace OOP_Organization
{
    public partial class MainWindow : Window
    {
        #region Fields;

        const string path = "new.xml"; //Path to Comapny DATA

        Repository repository; //Repository for Company DATA 

        private bool comboBoxNotEmpty => cbDepartments.SelectedIndex > -1; //Bool to check if Combo Box is EMPTY

        #endregion Fields

        #region Constructor;

        /// <summary>
        /// Main Initializator
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            repository = new Repository(path, this);

            LoadDpartmentsToComboBox();

            LoadDepartmentsToTreeView();            
        }

        #endregion Constructor

        #region Elements' Methods;

        /// <summary>
        /// Method to UPDATE Departments in Tree View on Combo Box change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadEmployeesToListView();
        }

        /// <summary>
        /// Method to UPDATE Departments in Combo Box on Tree View change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TvDepartments_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            cbDepartments.SelectedIndex = repository.DepartmentsDb.IndexOf((tvDepartments.SelectedItem as Department));
        }

        /// <summary>
        /// Button method to ADD Employee
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddEmployee(object sender, RoutedEventArgs e)
        {
            AddWindow addWindow = new AddWindow(repository, this);
            addWindow.Show();
        }

        /// <summary>
        /// Button method to EDIT Employee
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditEmployee(object sender, RoutedEventArgs e)
        {
            if (lvEmployees.SelectedItem != null)
            {
                AddWindow addWindow = new AddWindow(repository, this, lvEmployees.SelectedItem as Employee);
                addWindow.Show();
            }
        }

        /// <summary>
        /// Button method to REMOVE Employee
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemoveEmployee(object sender, RoutedEventArgs e)
        {
            if (lvEmployees.SelectedItem != null)
                repository.RemoveEmployee(lvEmployees.SelectedItem as Employee);
        }

        /// <summary>
        /// Button method to ADD Department
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddDepartment(object sender, RoutedEventArgs e)
        {
            DeptWindow deptWindow = new DeptWindow(repository, this);
            deptWindow.Show();
        }

        /// <summary>
        /// Button method to EDIT Department
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditDepartment(object sender, RoutedEventArgs e)
        {
            if (cbDepartments.SelectedIndex > -1)
            {
                DeptWindow deptWindow = new DeptWindow(repository, this, cbDepartments.SelectedItem as Department);
                deptWindow.Show();
            }
        }

        /// <summary>
        /// Button method to REMOVE Department
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemoveDepartment(object sender, RoutedEventArgs e)
        {
            if (comboBoxNotEmpty)
            {
                RemoveWindow removeWindow = new RemoveWindow(repository, this, cbDepartments.SelectedItem as Department);
                removeWindow.Show();
            }
        }

        /// <summary>
        /// Method to SORT Employee by Name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SortByName(object sender, RoutedEventArgs e)
        {
            repository.EmployeesDB.Sort(new Employee.SortByName());
            LoadEmployeesToListView();
        }

        /// <summary>
        /// Method to SORT Employee by Last Name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SortByLastName(object sender, RoutedEventArgs e)
        {
            repository.EmployeesDB.Sort(new Employee.SortByLastName());
            LoadEmployeesToListView();
        }

        /// <summary>
        /// Method to SORT Employee by Salary
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SortBySalary(object sender, RoutedEventArgs e)
        {
            repository.EmployeesDB.Sort(new Employee.SortBySalary());
            LoadEmployeesToListView();
        }

        /// <summary>
        /// Method to SORT Employee by Position
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SortByPosition(object sender, RoutedEventArgs e)
        {
            repository.EmployeesDB.Sort(new Employee.SortByPosition());
            LoadEmployeesToListView();
        }

        /// <summary>
        /// Method to SORT Employee by Age
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SortByAge(object sender, RoutedEventArgs e)
        {
            repository.EmployeesDB.Sort();
            LoadEmployeesToListView();
        }

        #endregion Elements' Methods

        #region Methods;

        /// <summary>
        /// Method to LOAD Employees to List View
        /// </summary>
        public void LoadEmployeesToListView()
        {
            if (cbDepartments.SelectedIndex > -1)
                lvEmployees.ItemsSource = repository.EmployeesDB.Where(GetAllEmployeesOfSelectedDepartment);
            else
                lvEmployees.ItemsSource = null;

            repository.SetSalaryToHeads();
        }

        /// <summary>
        /// Bool to GET all Employees of selected Department
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private bool GetAllEmployeesOfSelectedDepartment(Employee arg)
        {
            return arg.Department == (cbDepartments.SelectedItem as Department).DepartmentName;
        }

        /// <summary>
        /// Method to LOAD Departments in Combo Box
        /// </summary>
        public void LoadDpartmentsToComboBox()
        {
            cbDepartments.ItemsSource = repository.DepartmentsDb;
            cbDepartments.Items.Refresh();

            repository.SetSalaryToHeads();
        }

        /// <summary>
        /// Method to LOAD Departments in Tree View
        /// </summary>
        public void LoadDepartmentsToTreeView()
        {
            tvDepartments.Items.Refresh();

            tvDepartments.ItemsSource = repository.company;

            repository.DepartmentsDb.Sort();
        }

        #endregion Methods
    }
}
