using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Collections.Generic;

namespace OOP_Organization
{
    public partial class MainWindow : Window
    {
        #region Fields;

        const string path = "new.xml"; //Path to Comapny DATA

        Repository repository; //Repository for Company DATA 

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

        private void CbDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadEmployeesToListView();
        }

        private void btnAddEmployee(object sender, RoutedEventArgs e)
        {
            AddWindow addWindow = new AddWindow(repository, this);
            addWindow.Show();
        }

        private void btnEditEmployee(object sender, RoutedEventArgs e)
        {
            if (lvEmployees.SelectedItem != null)
            {
                AddWindow addWindow = new AddWindow(repository, this, lvEmployees.SelectedItem as Employee);
                addWindow.Show();
            }
        }

        private void btnRemoveEmployee(object sender, RoutedEventArgs e)
        {
            if (lvEmployees.SelectedItem != null)
                repository.RemoveEmployee(lvEmployees.SelectedItem as Employee);
        }

        private void btnAddDepartment(object sender, RoutedEventArgs e)
        {
            DeptWindow deptWindow = new DeptWindow(repository, this);
            deptWindow.Show();
        }

        private void btnEditDepartment(object sender, RoutedEventArgs e)
        {
            if (cbDepartments.SelectedIndex > -1)
            {
                DeptWindow deptWindow = new DeptWindow(repository, this, cbDepartments.SelectedItem as Department);
                deptWindow.Show();
            }
        }

        private void btnRemoveDepartment(object sender, RoutedEventArgs e)
        {
            if (cbDepartments.SelectedIndex > -1)
            {
                RemoveWindow removeWindow = new RemoveWindow(repository, this, cbDepartments.SelectedItem as Department);
                removeWindow.Show();
            }
        }

        private void SortByName(object sender, RoutedEventArgs e)
        {
            repository.EmployeesDB.Sort(new Employee.SortByName());
            LoadEmployeesToListView();
        }

        private void SortByLastName(object sender, RoutedEventArgs e)
        {
            repository.EmployeesDB.Sort(new Employee.SortByLastName());
            LoadEmployeesToListView();
        }

        private void SortBySalary(object sender, RoutedEventArgs e)
        {
            repository.EmployeesDB.Sort(new Employee.SortBySalary());
            LoadEmployeesToListView();
        }


        private void SortByPosition(object sender, RoutedEventArgs e)
        {
            repository.EmployeesDB.Sort(new Employee.SortByPosition());
            LoadEmployeesToListView();
        }

        private void SortByAge(object sender, RoutedEventArgs e)
        {
            repository.EmployeesDB.Sort();
            LoadEmployeesToListView();
        }        

        #endregion Elements' Methods

        #region Methods;

        public void LoadEmployeesToListView()
        {
            if (cbDepartments.SelectedIndex > -1)
                lvEmployees.ItemsSource = repository.EmployeesDB.Where(GetAllEmployeesOfSelectedDepartment);
            else
                lvEmployees.ItemsSource = null;

            repository.SetSalaryToHeads();
        }

        private bool GetAllEmployeesOfSelectedDepartment(Employee arg)
        {
            return arg.Department == (cbDepartments.SelectedItem as Department).DepartmentName;
        }

        public void LoadDpartmentsToComboBox()
        {
            cbDepartments.ItemsSource = repository.DepartmentsDb;
            cbDepartments.Items.Refresh();

            repository.SetSalaryToHeads();
        }

        public void LoadDepartmentsToTreeView()
        {
            tvDepartments.Items.Clear();

            List<TreeViewItem> tvItems = new List<TreeViewItem>();

            foreach(Department d in repository.DepartmentsDb)
            {                
                TreeViewItem item = new TreeViewItem
                {
                    Header = d.DepartmentName
                };

                tvItems.Add(item);               
            }

            foreach(Department d in repository.DepartmentsDb)
            {
                if (d.ParentDepartment == "")                                    
                    tvDepartments.Items.Add(tvItems.Find(x => (string)x.Header == d.DepartmentName));                
                else
                {
                    var parent = tvItems.Find(x => (string)x.Header == d.ParentDepartment);
                    parent.Items.Add(tvItems.Find(x => (string)x.Header == d.DepartmentName));
                }
            }

            repository.DepartmentsDb.Sort();
        }
        #endregion Methods

    }
}
