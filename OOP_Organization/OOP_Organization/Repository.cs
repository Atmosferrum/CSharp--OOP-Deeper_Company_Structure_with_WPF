using System;
using System.Collections.Generic;
using System.Xml;
using System.Diagnostics;
using System.Collections;
using System.Linq;

namespace OOP_Organization
{
    public class Repository : ICloneable, IEnumerable<Employee>, IEnumerable<Department>
    {
        #region Fields;

        private string path; //PATH to file

        private MainWindow mainWindow; //MainWidnow reference

        public List<Employee> EmployeesDB { get; set; } //Workers Database

        public List<Department> DepartmentsDb { get; set; } //Departments Database

        #endregion Fields

        #region Constructor;

        /// <summary>
        /// Constructor for Repository
        /// </summary>
        /// <param name="Path">Path to file to construct</param>
        public Repository(string Path,
                          MainWindow MainWindow)
        {
            this.path = Path; //Set Path to DB (XML File)
            this.mainWindow = MainWindow; //Set Reference to MainWindow
            EmployeesDB = new List<Employee>(); //Set new Employees Database
            DepartmentsDb = new List<Department>(); //Set new Departments Database

            AutoDesiarilizationXML(path);

            SetSalaryToHeads();
        }

        #endregion Constructor;

        #region Methods;

        /// <summary>
        /// Load & Show Company from XML File
        /// </summary>
        /// <param name="path">Path to Company XML File</param>
        void AutoDesiarilizationXML(string path)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(path);

            XmlElement xRoot = xDoc.DocumentElement;

            XmlNode xCompany = xRoot;

            XMLNodesDesiarilization(xCompany);
        }

        /// <summary>
        /// SERIALIZE XML Nodes to appropriate Class
        /// </summary>
        /// <param name="xNode">Node to Serialize</param>
        void XMLNodesDesiarilization(XmlNode xNode)
        {
            switch (xNode.Name)
            {
                case "Company":
                    DefineDepartmentClass(xNode, new Company());
                    break;
                case "Bureau":
                    DefineDepartmentClass(xNode, new Bureau());
                    break;
                case "Division":
                    DefineDepartmentClass(xNode, new Division());
                    break;
                case "HeadOfOrganization":
                    DefineEmployeeClass(xNode, new HeadOfOrganization());
                    break;
                case "HeadOfDepartment":
                    DefineEmployeeClass(xNode, new HeadOfDepartment());
                    break;
                case "Worker":
                    DefineEmployeeClass(xNode, new Worker());
                    break;
                default:
                    DefineEmployeeClass(xNode, new Intern());
                    break;
            }

            if (xNode.HasChildNodes)
                foreach (XmlNode x in xNode.ChildNodes)
                    XMLNodesDesiarilization(x);

            DepartmentsDb.Sort();            
        }

        /// <summary>
        /// SET Department Values for Properties
        /// </summary>
        /// <param name="node">XML node to get Values</param>
        /// <param name="dept">Department to add Values</param>
        void DefineDepartmentClass(XmlNode node, Department dept)
        {
            dept.DepartmentName = Convert.ToString(node.Attributes.GetNamedItem("departmentName").Value);
            dept.ParentDepartment = Convert.ToString(node.Attributes.GetNamedItem("parentDepartment").Value);
            dept.Repository = this;

            DepartmentsDb.Add(dept);
        }

        /// <summary>
        /// SET Employee Values for Properties
        /// </summary>
        /// <param name="node">XML node to get Values</param>
        /// <param name="dept">Department to add Values</param>
        void DefineEmployeeClass(XmlNode node, Employee emply)
        {
            emply.EmployeeName = Convert.ToString(node.Attributes.GetNamedItem("employeeName").Value);
            emply.LastName = Convert.ToString(node.Attributes.GetNamedItem("lastName").Value);
            emply.Age = Convert.ToInt32(node.Attributes.GetNamedItem("age").Value);
            emply.Department = Convert.ToString(node.Attributes.GetNamedItem("department").Value);
            emply.DaysWorked = Convert.ToInt32(node.Attributes.GetNamedItem("daysWorked").Value);
            emply.Repository = this;

            EmployeesDB.Add(emply);
        }

        /// <summary>
        /// Method to SET Salary to HeadOfDepartment and HeadOfOrganization
        /// </summary>
        void SetSalaryToHeads()
        {
            foreach (Employee head in EmployeesDB)
            {
                if (head is HeadOfDepartment)
                {
                    foreach (Employee e in EmployeesDB)
                        if (e.Department == head.Department) head.Salary += (int)(e.Salary * 0.13f);

                    if (head.Salary < 1300) head.Salary = 1300;
                }    
            }

            foreach(Employee head in EmployeesDB)
            {
                if (head is HeadOfOrganization)
                {
                    foreach (Employee e in EmployeesDB)
                        head.Salary += (int)(e.Salary * 0.1f);
                }
            }           
        }

        /// <summary>
        /// Method to ADD new Employee
        /// </summary>
        /// <param name="Name">New Employee Name</param>
        /// <param name="LastName">New Employee Last Name</param>
        /// <param name="Age">New Employee Age</param>
        /// <param name="Department">New Employee Department</param>
        public void AddEmployee(string Name, string LastName, int Age, string Department)
        {
            Employee employee = new Employee(Name, LastName, Age, Department, 0);
            EmployeesDB.Add(employee);
        }

        /// <summary>
        /// Method to REMOVE Employee
        /// </summary>
        /// <param name="employee">Employee to Remove</param>
        public void RemoveEmployee(Employee employee)
        {
            EmployeesDB.Remove(employee);
            mainWindow.LoadEmployeesToListView();
        }

        /// <summary>
        /// Method to ADD Department
        /// </summary>
        /// <param name="Name">New Department Name</param>
        /// <param name="ParentName">New Department Parent</param>
        public void AddDepartment(string Name, string ParentName)
        {
            Department department = new Department(Name, ParentName);
            DepartmentsDb.Add(department);
        }

        /// <summary>
        /// Method to REMOVE Department
        /// </summary>
        /// <param name="department">Department to Remove</param>
        public void RemoveDepartment(Department department)
        {
            EmployeesDB = EmployeesDB.Where(x => x.Department != department.DepartmentName).ToList();

            DepartmentsDb.Remove(department);
        }

        /// <summary>
        /// Method to REMOVE Department and MOVE all it's Employees to another
        /// </summary>
        /// <param name="departmentToRemove">Department to REMOVE</param>
        /// <param name="departmentToAddEmployees">Department in witch MOVE</param>
        public void RemoveDepartment(Department departmentToRemove, Department departmentToAddEmployees)
        {
            foreach (Employee e in EmployeesDB)
            {
                if (e.Department == departmentToRemove.DepartmentName)
                    e.Department = departmentToAddEmployees.DepartmentName;
            }

            DepartmentsDb.Remove(departmentToRemove);

            foreach (Employee e in EmployeesDB)
            {
                Debug.WriteLine(e.Department);
            }
        }

        #endregion Methods

        #region Interfaces;

        /// <summary>
        /// IEnumerator for Departments
        /// </summary>
        /// <returns></returns>
        IEnumerator<Department> IEnumerable<Department>.GetEnumerator()
        {
            return this.DepartmentsDb.GetEnumerator();
        }

        /// <summary>
        /// IEnumerator for Employees
        /// </summary>
        /// <returns></returns>
        IEnumerator<Employee> IEnumerable<Employee>.GetEnumerator()
        {
            return this.EmployeesDB.GetEnumerator();
        }

        /// <summary>
        /// Default IEnumerator
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return null;
        }

        /// <summary>
        /// Method to CLONE this Repository
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            var repo = new Repository(path, mainWindow);

            return repo;
        }

        #endregion Interfaces

        #region Indexers;
        /// <summary>
        /// GET Employee from Database by Index
        /// </summary>
        /// <param name="index">Index of the Employee in Database</param>
        /// <returns></returns>
        public Employee this[int index]
        {
            get { return this.EmployeesDB[index]; }
        }

        /// <summary>
        /// GET/SET Employee in Database by all available parameters
        /// </summary>
        /// <param name="Name">Name to get</param>
        /// <param name="LastName">Last name to get</param>
        /// <param name="Age">Age to get</param>
        /// <param name="Department">Department to get</param>
        /// <returns></returns>
        public Employee this[string Name, string LastName, int Age, string Department]
        {
            get
            {
                Employee e = null;
                foreach (var emply in this.EmployeesDB)
                {
                    if (emply.EmployeeName == Name
                       && emply.LastName == LastName
                       && emply.Age == Age
                       && emply.Department == Department)
                    { e = emply; break; }
                }
                return e;
            }

            set
            {
                for (int i = 0; i < EmployeesDB.Count; i++)
                {
                    if (EmployeesDB[i].EmployeeName == Name
                        && EmployeesDB[i].LastName == LastName
                        && EmployeesDB[i].Age == Age
                        && EmployeesDB[i].Department == Department)
                    { EmployeesDB[i] = value; break; }
                }
            }
        }

        /// <summary>
        /// GET/SET Department in Database by all available parameters
        /// </summary>
        /// <param name="Name">Name to get</param>
        /// <param name="ParentDepartment">Parent department to get</param>
        /// <returns></returns>
        public Department this[string Name, string ParentDepartment]
        {
            get
            {
                Department d = null;
                foreach (var dept in this.DepartmentsDb)
                {
                    if (dept.DepartmentName == Name
                       && dept.ParentDepartment == ParentDepartment)
                    { d = dept; break; }
                }
                return d;
            }

            set
            {
                for (int i = 0; i < DepartmentsDb.Count; i++)
                {
                    if (DepartmentsDb[i].DepartmentName == Name
                        && DepartmentsDb[i].ParentDepartment == ParentDepartment)
                    {
                        DepartmentsDb[i] = value;
                        foreach (Employee e in EmployeesDB)
                        {
                            if (e.Department == Name)
                            {
                                e.Department = DepartmentsDb[i].DepartmentName;
                            }
                        };
                        break;
                    }
                }


            }
        }

        #endregion Indexers
    }

}
