using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InfoWebApp.Models
{
    public class NewsModels
    {
        /// <summary>
        /// 
        /// </summary>
        public NewsModels()
        {
            Number = 0;
            Id = 0;
            Title = String.Empty;
            TinhThanhId = 0;
            QuanHuyenId = 0;
            Address = String.Empty;
            MenuId = 0;
            Dientich = String.Empty;
            Price = String.Empty;
            PhoneNumber = String.Empty;
            NewsContent = String.Empty;
            UserId = 0;
            Datetime = new DateTime();
            MenuIdHidden = 0;
            MenuName = string.Empty;
        }

        /// <summary>
        /// Number
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Title
        /// </summary>
        [Required(ErrorMessage = "Tiêu đề không được để trống!")]
        public string Title { get; set; }

        /// <summary>
        /// Tinh thanh
        /// </summary>
        [Required(ErrorMessage = "Tỉnh thành không được để trống")]
        public int TinhThanhId { get; set; }

        /// <summary>
        /// Quan huyen
        /// </summary>
        [Required(ErrorMessage = "Quận huyện không được để trống")]
        public int QuanHuyenId { get; set; }

        /// <summary>
        /// Address
        /// </summary>
        [Required(ErrorMessage = "Địa chỉ không được để trống")]
        public string Address { get; set; }

        /// <summary>
        /// Menu Id
        /// </summary>
        [Required(ErrorMessage = "Menu không được để trống")]
        public int MenuId { get; set; }

        /// <summary>
        /// Menu Id
        /// </summary>
        [Required(ErrorMessage = "Menu không được để trống")]
        public int MenuIdHidden { get; set; }

        /// <summary>
        /// Dientich
        /// </summary>
        [Required(ErrorMessage = "Diện tích không được để trống!")]
        public string Dientich { get; set; }

        /// <summary>
        /// Price
        /// </summary>
        [Required(ErrorMessage = "Giá tiền không được để trống!")]
        public string Price { get; set; }

        /// <summary>
        /// Phone Number
        /// </summary>
        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// News Content
        /// </summary>
        [Required(ErrorMessage = "Nội dung không được để trống")]
        public string NewsContent { get; set; }

        /// <summary>
        /// User
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Datetime
        /// </summary>
        public DateTime Datetime { get; set; }

        /// <summary>
        /// Get menu name
        /// </summary>
        public string MenuName { get; set; }
    }
}