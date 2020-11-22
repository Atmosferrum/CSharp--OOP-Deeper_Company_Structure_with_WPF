using System;
using System.Collections.Generic;

namespace OOP_Organization
{
    public class Department : IEquatable<Department>, IComparable<Department>
    {
        #region Fields;

        private static int staticID = 0;
        protected int departmentID { get; set; } //Department ID
        protected string departmentName { get; set; } //Name of Department
        protected DateTime dateOfCreation { get; set; } //Dato Of Department Creation
        protected int numberOfEmployees { get; set; } //Number Of Employees in Depatment
        protected int numberOfDepartments { get; set; } //Number Of Departments in Department
        protected string parentDepartment { get; set; } //Parent Department of the Department
        protected Repository repository; //Repository with all Company DATA

        #endregion Fields

        #region Constructor;

        /// <summary>
        /// Constructor for MAIN Department Class
        /// </summary>
        /// <param name="DepartmentName">Department NAME</param>
        /// <param name="ParentDepartment">Parent Departmen NAME</param>
        public Department(string DepartmentName,                          
                          string ParentDepartment)
        {
            this.departmentID = Department.NextID();
            this.departmentName = DepartmentName;
            this.dateOfCreation = DateTime.Now;
            this.NumberOfEmployees = 0;
            this.NumberOfDepartments = 0;
            this.parentDepartment = ParentDepartment;        
        }

        #endregion Constructor
           
        #region Properties;

        private static int NextID()
        {
            staticID++;
            return staticID;
        }        

        public string DepartmentName //Name Property
        {
            get { return this.departmentName; }
            set { this.departmentName = value; }
        }

        public int DepartmentID
        {
            get { return this.departmentID; }
        }

        public DateTime DateOfCreation //Date Of Creation Property
        {
            get { return this.dateOfCreation; }
        }

        public int NumberOfEmployees //Number Of Employees Property
        {
            get { return this.numberOfDepartments; }
            set { this.numberOfDepartments = value; }
        }

        public int NumberOfDepartments //Number Of Departments Property
        {
            get { return this.numberOfEmployees; }
            set { this.numberOfEmployees = value; }
        }

        public string ParentDepartment //Parent Department Property
        {
            get { return this.parentDepartment; }
            set { this.parentDepartment = value; }
        }

        public Repository Repository //Repository Property
        {
            get { return this.repository; }
            set { this.repository = value; }
        }

        #endregion Properties

        #region Interfaces;

        /// <summary>
        /// Method for IEquatable<Department> to .Contains 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Department other)
        {
            return this.departmentID == other.departmentID
                   && this.departmentName == other.departmentName
                   && this.parentDepartment == other.parentDepartment;
        }

        /// <summary>
        ///  Method for IComparable<Department> to .Sort by Name
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int CompareTo(Department other)
        {
            return String.Compare(this.DepartmentName, other.DepartmentName);
        }

        #endregion Interfaces
    }
}
