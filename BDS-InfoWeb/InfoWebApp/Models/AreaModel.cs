using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InfoWebApp.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class AreaModel
    {
        /// <summary>
        /// 
        /// </summary>
        public AreaModel() {
            Id = 0;
            Name = String.Empty;
            ParentId = 0;
        }

        /// <summary>
        /// Get Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Get Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Get Parent Id
        /// </summary>
        public int ParentId { get; set; }
    }
}