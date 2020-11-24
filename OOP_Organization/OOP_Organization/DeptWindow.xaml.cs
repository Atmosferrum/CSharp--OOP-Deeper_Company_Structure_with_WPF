using System.Collections.Generic;
using System.Diagnostics;
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
            cbAddParentDepartment.SelectedIndex = repository.DepartmentsDb.FindIndex(GetParent);

            if (department.DepartmentName == "Normandy")
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

        private bool ExcludeSelf(Department arg)
        {
            return arg.DepartmentName != department.DepartmentName;
        }

        private bool GetParent(Department args)
        {
            return args.ParentDepartment == department.ParentDepartment;
        }

        /// <summary>
        /// Method to GET correct Index to Emplouee Position
        /// </summary>
        /// <returns></returns>
        private int GetParentIndexFromComboBox()
        {
            return parent.IndexOf(cbAddParentDepartment.Text);
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
                var newDepartmentData = new Department(tbAddName.Text,
                                                      (cbAddParentDepartment.SelectedItem as Department).DepartmentName);

                switch (GetParentIndexFromComboBox())
                {
                    case 0: newDepartmentData = new Bureau(); break;
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

                CloseWindow();
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
            dept.ParentDepartment = (cbAddParentDepartment.SelectedItem as Department).DepartmentName;
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
