using System;

namespace GetInfoWeb
{
    public class PrivateNews
    {
        public PrivateNews() {
            Id = 0;
            Title = String.Empty;
            Address = String.Empty;
            Dientich = String.Empty;
            Price = String.Empty;
            PhoneNumber = String.Empty;
            NewsContent = String.Empty;
            Status = 2;
            Datetime = new DateTime();
        }

        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

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
        public string PhoneNumber { get; set; }

        /// <summary>
        /// News Content
        /// </summary>
        public string NewsContent { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Datetime
        /// </summary>
        public DateTime Datetime { get; set; }
    }
}
