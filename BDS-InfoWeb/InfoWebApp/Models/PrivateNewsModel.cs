using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InfoWebApp.Models
{
    public class PrivateNewsModel
    {
        public PrivateNewsModel() {
            Id = 0;
            Status = 0;
            Title = String.Empty;
            Address = String.Empty;
            Dientich = String.Empty;
            Price = String.Empty;
            PhoneNumer = String.Empty;
            NewsContent = String.Empty;
            TinhThanhId = 0;
            StartDate = DateTime.Today.AddDays(-120) ;
            EndDate = DateTime.Now.AddDays(1);
            Datetime = DateTime.Now;
            Number = 0;
            listPrivateNews = new List<PrivateNewsModel>();
        }

        #region Propeties
        /// <summary>
        /// Number
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// TinhThanhId
        /// </summary>
        public int TinhThanhId { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Address
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Dientich
        /// </summary>
        public string Dientich { get; set; }

        /// <summary>
        /// Price
        /// </summary>
        public string Price { get; set; }

        /// <summary>
        /// Phone Number
        /// </summary>
        public string PhoneNumer { get; set; }

        /// <summary>
        /// News Content
        /// </summary>
        public string NewsContent { get; set; }

        /// <summary>
        /// Datetime
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Datetime
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime Datetime { get; set; }

        /// <summary>
        /// Get news private
        /// </summary>
        public List<PrivateNewsModel> listPrivateNews { get; set; }
        #endregion
    }

}