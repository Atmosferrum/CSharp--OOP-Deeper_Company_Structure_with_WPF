using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace OOP_Organization
{
    public partial class DeptWindow : Window
    {
        #region Fields;

        Repository repository; //Repository for Company DATA

        MainWindow mainWindow; //MainWindow reference

        Department department; //Temporarily Department (with Data gotten from TextBoxes)

        private List<string> parent = new List<string>() { "Company", "Bureau", "Division"}; //Department status to SELECT

        private string parentName;

        /// <summary>
        /// Bool to CHECK if Input Data is Correct
        /// </summary>
        bool inputDataIsCorrect => tbAddName.Text != ""; 

        #endregion Fields

        #region Constructor;

        /// <summary>
        /// Constructor with Repository & MainWindow
        /// </summary>
        /// <param name="Repository"></param>
        /// <param name="MainWindow"></param>
        public DeptWindow(Repository Repository,
                         MainWindow MainWindow)
        {
            InitializeComponent();

            this.repository = Repository;
            this.mainWindow = MainWindow;

            cbAddParentDepartment.ItemsSource = repository.DepartmentsDb;            

            btnEditDepartment.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Constructor with Repository, MainWindow & Department
        /// </summary>
        /// <param name="Repository"></param>
        /// <param name="MainWindow"></param>
        /// <param name="Employee"></param>
        public DeptWindow(Repository Repository,
                         MainWindow MainWindow,
                         Department Department)
        {
            InitializeComponent();

            this.repository = Repository;
            this.mainWindow = MainWindow;
            this.department = Department;          

            tbAddName.Text = department.DepartmentName;

            cbAddParentDepartment.ItemsSource = repository.DepartmentsDb;
            cbAddParentDepartment.ItemsSource = repository.DepartmentsDb.Where(ExcludeSelf);
            cbAddParentDepartment.SelectedItem = repository.DepartmentsDb.Find(x => x.DepartmentName == department.ParentDepartment);

            if (department.DepartmentName == repository.DepartmentsDb.Find(x => x is Company).DepartmentName)
            {
                cbAddParentDepartment.Visibility = Visibility.Hidden;
                tbAddParentDepartment.Visibility = Visibility.Hidden;
            }
            else
            {
                cbAddParentDepartment.Visibility = Visibility.Visible;
                tbAddParentDepartment.Visibility = Visibility.Visible;
            }


            btnAddDepartment.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Bool to EXCLUDE chosen Department from Combo Box
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private bool ExcludeSelf(Department arg)
        {
            return arg.DepartmentName != department.DepartmentName
                && arg.ParentDepartment != department.DepartmentName
                && GetChildren(arg);
        }

        /// <summary>
        /// Exclude CHILDREN Departments
        /// </summary>
        /// <param name="dept"></param>
        /// <returns></returns>
        private bool GetChildren(Department dept)
        {
            Department tempParent = repository.DepartmentsDb.Find(x => x.DepartmentName == dept.ParentDepartment);

            if (tempParent != null)
            {
                if (tempParent.ParentDepartment != department.DepartmentName)
                {
                    return GetChildren(tempParent);
                }
                else if (tempParent.ParentDepartment == department.DepartmentName)
                    return false;
                else return true;
            }
            else return true;
        }


        /// <summary>
        /// Method to GET correct Index to Employee Position
        /// </summary>
        /// <returns></returns>
        private int GetParentIndexFromComboBox()
        {
            if (cbAddParentDepartment.SelectedIndex > -1)
            {
                var tempDept = repository.DepartmentsDb.Find(x => x.DepartmentName == (cbAddParentDepartment.SelectedItem as Department).DepartmentName);
                if (tempDept is Company)
                    return 1;
                else return 2;
            }
            else return 0;            
        }

        #endregion Constructor

        #region Elements' Methods;

        /// <summary>
        /// Button Method to ADD Department
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAddDepartment(object sender, RoutedEventArgs e)
        {
            if (inputDataIsCorrect)
            {
                repository.AddDepartment(tbAddName.Text, 
                                        (cbAddParentDepartment.SelectedItem as Department).DepartmentName);
                CloseWindow();
            }
            else
                MessageBox.Show("The DATA you are entering is wrong!",
                                $"{AddWindow.TitleProperty.Name}",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
        }

        /// <summary>
        /// Button Method to EDIT Department
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnEditDepartment(object sender, RoutedEventArgs e)
        {       
            if (inputDataIsCorrect)
            {
                if (department.DepartmentName != repository.DepartmentsDb.Find(x => x is Company).DepartmentName)
                {
                    parentName = (cbAddParentDepartment.SelectedItem as Department).DepartmentName;
                }
                else parentName = "";                

                var newDepartmentData = new Department(tbAddName.Text,
                                                      parentName);

                switch (GetParentIndexFromComboBox())
                {
                    case 0: newDepartmentData = new Company(); break;
                    case 1: newDepartmentData = new Bureau(); break;
                    default: newDepartmentData = new Division(); break;
                }

                DefineDepartmentClass(newDepartmentData);

                foreach(Department d in repository.DepartmentsDb)
                {
                    if(d.ParentDepartment == department.DepartmentName)
                    {
                        d.ParentDepartment = tbAddName.Text;                        
                    }
                }

                repository[department.DepartmentName,
                          department.ParentDepartment] = newDepartmentData;

                repository.DepartmentsDb.Sort(new Department.SortByPosition());

                foreach(Department d in repository.DepartmentsDb)
                    d.innerDepartments.Clear();

                repository.company.Clear();

                foreach (Department d in repository.DepartmentsDb)
                {
                    if (d.ParentDepartment != "")
                    {
                        var newParent = repository.DepartmentsDb.Find(x => x.DepartmentName == d.ParentDepartment);
                        newParent.innerDepartments.Add(d);
                    }
                    else
                        repository.company.Add(d);
                }

                CloseWindow();
            }
        }

        /// <summary>
        /// Method to CHECK for children to Move
        /// </summary>
        /// <param name="dept"></param>
        void CheckNextChild(Department dept)
        {
            foreach (Department d in dept.innerDepartments)
            {
                if (dept.DepartmentName != department.DepartmentName)
                    CheckNextChild(d);
                else
                {
                    dept.DepartmentName = tbAddName.Text;
                    foreach (Department childDept in dept.innerDepartments)
                        childDept.ParentDepartment = tbAddName.Text;                    
                }                    
            }
        }

        #endregion Elements' Methods

        #region Methods;

        /// <summary>
        /// Method to DEFINE Department Class
        /// </summary>
        /// <param name="dept"></param>
        private void DefineDepartmentClass(Department dept)
        {
            dept.DepartmentName = tbAddName.Text;
            dept.ParentDepartment = parentName;
        }

        /// <summary>
        /// Method to CLOSE this Window
        /// </summary>
        private void CloseWindow()
        {
            mainWindow.LoadEmployeesToListView();
            mainWindow.LoadDpartmentsToComboBox();
            mainWindow.LoadDepartmentsToTreeView();
            this.Close();
        } 

        #endregion Methods
    }
}
