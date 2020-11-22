using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_Organization
{
    class Company : Department
    {

        #region Constructor;

        /// <summary>
        /// Constructor DERIVED from Department Class
        /// </summary>
        /// <param name="Name">Name of Bureau</param>
        /// <param name="ParentDepartment">Parent Department of Bureau</param>
        public Company(string DepartmentName,                      
                       string ParentDepartment)
            : base(DepartmentName,
                  ParentDepartment)
        { }

        /// <summary>
        /// Default Constructor for Bureau
        /// </summary>
        public Company() : this("", "") { }

        #endregion Constructor
    }
}

