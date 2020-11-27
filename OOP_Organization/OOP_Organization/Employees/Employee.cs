using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace OOP_Organization
{
    public class Employee : IEquatable<Employee>, IComparable<Employee>
    {
        #region Fields;

        private static int staticID = 0; //Static ID to give for all Employees
        protected int employeeID; //Number to get the Employee status (0 - Head Of Department, 1 - Worker or Intern)
        protected string employeeName; //Name of The Employee
        protected string lastName; //Last Name of the Employee
        protected int age; //Age of the Employee
        protected string department; //Department of the Employee
        protected float salary; //Salary of the Employee
        protected int daysWorked; //How many days Employee worked at the Company
        protected string position;
        protected Repository repository; //Repository with all Company DATA

        #endregion Fields

        #region Constuctor;

        /// <summary>
        /// Consturctor for MAIN Employee Class
        /// </summary>
        /// <param name="EmployeeName">Employee Name</param>
        /// <param name="LastName">Employee Last Name</param>
        /// <param name="Age">Employee Age</param>
        /// <param name="Department">Employee Department</param>
        /// <param name="DaysWorked">Days Worked by Employee</param>
        public Employee(string EmployeeName,
                        string LastName,
                        int Age,
                        string Department,
                        int DaysWorked)
        {
            this.employeeID = Employee.NextID();
            this.employeeName = EmployeeName;
            this.lastName = LastName;
            this.age = Age;
            this.department = Department;
            this.daysWorked = DaysWorked;
            this.position = this.MyPosition();
        }

        #endregion Constuctor        

        #region Properties;

        private static int NextID()
        {
            staticID++;
            return staticID;
        }

        private string MyPosition()
        {
            string pos;
            pos = Regex.Replace(this.GetType().Name, "([a-z])([A-Z])", "$1 $2");
            return pos;
        }

        public int EmployeeID //Number Property
        {
            get { return this.employeeID; }
        }

        public string EmployeeName //Name Property
        {
            get { return this.employeeName; }
            set { this.employeeName = value; }
        }

        public string LastName //Last Name Property
        {
            get { return this.lastName; }
            set { this.lastName = value; }
        }

        public int Age //Age Property
        {
            get { return this.age; }
            set { this.age = value; }
        }

        public string Department //Department Property
        {
            get { return this.department; }
            set { this.department = value; }
        }

        public virtual float Salary //Salary Property
        {
            get { return this.salary; }
            set { this.salary = value; }
        }

        public virtual int DaysWorked //Days Worked Property
        {
            get { return this.daysWorked; }
            set { this.daysWorked = value; }
        }

        public string Position //Days Worked Property
        {
            get { return this.position; }
        }

        public Repository Repository //Repository Property
        {
            get { return this.repository; }
            set { this.repository = value; }
        }

        #endregion Properties

        #region Interfaces;

        /// <summary>
        /// Method for IEquatable<Employee> to .Contains
        /// </summary>
        /// <param name="other">Employee to CHECK</param>
        /// <returns></returns>
        public bool Equals(Employee other)
        {
            return this.employeeName == other.EmployeeName
                   && this.lastName == other.lastName
                   && this.age == other.age
                   && this.department == other.department
                   && this.employeeID == other.employeeID;
        }

        /// <summary>
        /// Method for IComparable<Employee> to .Sort by Age
        /// </summary>
        /// <param name="employee">Employee to CHECK</param>
        /// <returns></returns>
        public int CompareTo(Employee employee)
        {
            if (this.age > employee.age) return 1;
            else if (this.age < employee.age) return -1;
            else return 0;
        }

        /// <summary>
        /// Class to .Sort by Name
        /// </summary>
        public class SortByName : IComparer<Employee>
        {
            public int Compare(Employee x, Employee y)
            { 
                return String.Compare(x.employeeName, y.employeeName);
            }           
        }

        /// <summary>
        /// Class to .Sort by LastName
        /// </summary>
        public class SortByLastName : IComparer<Employee>
        {
            public int Compare(Employee x, Employee y)
            {       
                return String.Compare(x.lastName, y.lastName);
            }
        }

        /// <summary>
        /// Class to .Sort by Salary
        /// </summary>
        public class SortBySalary : IComparer<Employee>
        {
            public int Compare(Employee x, Employee y)
            {
                if (x.Salary > y.Salary) return -1;
                else if (x.Salary < y.Salary) return 1;
                else return 0;
            }
        }

        /// <summary>
        /// Class to .Sort by Position  
        /// </summary>
        public class SortByPosition : IComparer<Employee>
        {
            public int Compare(Employee x, Employee y)
            {
                if (x is HeadOfOrganization) return -1;
                else if (x is HeadOfDepartment)
                    if (y is HeadOfOrganization) return 1;
                    else if (y is HeadOfDepartment) return 0;
                    else return -1;
                else if (x is Worker)
                    if (y is Intern) return -1;
                    else if (y is Worker) return 0;
                    else return 1;
                else if (x is Intern) return 1;
                else return 0;
            }
        }

        #endregion Interfaces
    }
}
