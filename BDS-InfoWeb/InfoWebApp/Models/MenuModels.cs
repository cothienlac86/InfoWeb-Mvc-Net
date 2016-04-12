using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InfoWebApp.Models
{
    public class MenuModels
    {
        public MenuModels() {
            Name = String.Empty;
            Description = String.Empty;
            ParentId = 0;
            Id = 0;
        }
        public int Id { get; set; }
        [Required(ErrorMessage="Tên menu không được để trống")]
        public string Name { get; set; }
        public string Description { get; set; }
        public int ParentId { get; set; }
    }

    public class MenuListModels 
    {
        public MenuListModels()
        {
            MenuName = String.Empty;
            MenuId = 0;
            Title = String.Empty;
            Price = String.Empty;
            TinhThanhId = 0;
            QuanHuyenId = 0;
            CurrentPage = 0;
            RowsPage = 0;
            PageNumber = 0;
            newsModel = new List<NewsModels>();
        }

        public string MenuName { get; set; }
        public int MenuId { get; set; }
        public string Title { get; set; }
        public string Price { get; set; }
        public int TinhThanhId { get; set; }
        public int QuanHuyenId { get; set; }
        public int CurrentPage { get; set; }
        public int RowsPage { get; set; }
        public int PageNumber { get; set; }
        public List<NewsModels> newsModel { get; set; }
    }
}