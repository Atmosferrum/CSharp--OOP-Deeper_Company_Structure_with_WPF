using System.Linq;
using System.Windows;

namespace OOP_Organization
{
    public partial class RemoveWindow : Window
    {
        #region Fields;

        Repository repository; //Repository for Company DATA

        MainWindow mainWindow; //MainWindow reference

        Department department; //Temporarily Department (with Data gotten from TextBoxes)

        /// <summary>
        /// Bool to CHECK if Input Data is Correct
        /// </summary>
        bool inputDataIsCorrect => cbAddNewDepartmentForEmployees.SelectedIndex > -1;

        #endregion Fields

        #region Constructor;

        /// <summary>
        /// Constructor with Repository, MainWindow & Department
        /// </summary>
        /// <param name="Repository"></param>
        /// <param name="MainWindow"></param>
        /// <param name="Employee"></param>
        public RemoveWindow(Repository Repository,
                         MainWindow MainWindow,
                         Department Department)
        {
            InitializeComponent();

            this.repository = Repository;
            this.mainWindow = MainWindow;
            this.department = Department;


            cbAddNewDepartmentForEmployees.ItemsSource = repository.DepartmentsDb.Where(ExcludeSelf);
        }

        /// <summary>
        /// Bool to EXCLUDE self rom ComboBox
        /// </summary>
        /// <param name="arg">Self name</param>
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

        #endregion Constructor

        #region Elements' Methods;

        /// <summary>
        /// Button Method to REMOVE Departmetn and REMOVE all it's Employees
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnFireAll_Click(object sender, RoutedEventArgs e)
        {
            repository.RemoveDepartment(department);
            CloseWindow();
        }

        /// <summary>
        /// Button Method to REMOVE Department and MOVE all it's Employees to another
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnMoveToAnotherDepartment_Click(object sender, RoutedEventArgs e)
        {
            if (cbAddNewDepartmentForEmployees.SelectedIndex > -1)
            {
                repository.RemoveDepartment(department, cbAddNewDepartmentForEmployees.SelectedItem as Department);
                CloseWindow();
            }
            else
                MessageBox.Show("CHOSE department to move Employees!",
                                $"{AddWindow.TitleProperty.Name}",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
        }

        #endregion Elements' Methods

        #region Methods;

        /// <summary>
        /// Method to CLOSE this Window and REFRESH MainWindow Employees ListView and Department ComboBox
        /// </summary>
        private void CloseWindow()
        {
            mainWindow.LoadDpartmentsToComboBox();
            mainWindow.LoadEmployeesToListView();
            this.Close();
        }

        #endregion Methods
    }
}
