using InfoWebApp.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace InfoWebApp.Entity
{
    /// <summary>
    /// 
    /// </summary>
    public class NewsDb
    {
        /// <summary>
        /// 
        /// </summary>
        public NewsDb() {
            Id = 0;
            Title = String.Empty;
            TinhThanhId = 0;
            QuanHuyenId = 0;
            Address = String.Empty;
            MenuId = 0;
            Dientich = String.Empty;
            Price = String.Empty;
            PhoneNumer = String.Empty;
            NewsContent = String.Empty;
            UserId = 0;
            Datetime = new DateTime();
        }

        #region Propeties

        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Tinh thanh
        /// </summary>
        public int TinhThanhId { get; set; }

        /// <summary>
        /// Quan huyen
        /// </summary>
        public int QuanHuyenId { get; set; }

        /// <summary>
        /// Address
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Menu Id
        /// </summary>
        public int MenuId { get; set; }

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
        /// User
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Datetime
        /// </summary>
        public DateTime Datetime { get; set; }

        #endregion
        
        /// <summary>
        /// Add news model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public NewsModels Add(NewsModels model) {
            var _conn = new SqlConnection(ConfigurationManager.ConnectionStrings["InfoWebAppDbStr"].ConnectionString);
            if (_conn.State == ConnectionState.Closed)
                _conn.Open();
            //Create command store procedure
            var command = new SqlCommand("Add_tblNews");
            command.Connection = _conn;
            command.CommandType = CommandType.StoredProcedure;
            try
            {
                var TitleParam = new SqlParameter("@Title", model.Title);
                TitleParam.Direction = ParameterDirection.Input;
                var TinhThanhIdParam = new SqlParameter("@TinhThanhId", model.TinhThanhId);
                TinhThanhIdParam.Direction = ParameterDirection.Input;
                var QuanHuyenIdParam = new SqlParameter("@QuanHuyenId", model.QuanHuyenId);
                QuanHuyenIdParam.Direction = ParameterDirection.Input;
                var AddressParam = new SqlParameter("@Address", model.Address);
                AddressParam.Direction = ParameterDirection.Input;
                var MenuIdParam = new SqlParameter("@MenuId", model.MenuId);
                MenuIdParam.Direction = ParameterDirection.Input;
                var DientichParam = new SqlParameter("@Dientich", model.Dientich);
                DientichParam.Direction = ParameterDirection.Input;
                var PriceParam = new SqlParameter("@Price", model.Price);
                PriceParam.Direction = ParameterDirection.Input;
                var PhoneNumberParam = new SqlParameter("@PhoneNumber", model.PhoneNumber);
                PhoneNumberParam.Direction = ParameterDirection.Input;
                var NewsContentParam = new SqlParameter("@NewsContent", model.NewsContent);
                NewsContentParam.Direction = ParameterDirection.Input;
                var UserIdParam = new SqlParameter("@UserId", model.UserId);
                UserIdParam.Direction = ParameterDirection.Input;

                command.Parameters.Add(TitleParam);
                command.Parameters.Add(TinhThanhIdParam);
                command.Parameters.Add(QuanHuyenIdParam);
                command.Parameters.Add(AddressParam);
                command.Parameters.Add(MenuIdParam);
                command.Parameters.Add(DientichParam);
                command.Parameters.Add(PriceParam);
                command.Parameters.Add(PhoneNumberParam);
                command.Parameters.Add(NewsContentParam);
                command.Parameters.Add(UserIdParam);

                model.Id = int.Parse(command.ExecuteScalar().ToString());
            }
            finally
            {
                command.Connection.Close();
                command.Connection.Dispose();
            }
            return model;
        }

        /// <summary>
        /// Update new item and return success of fail
        /// </summary>
        /// <returns></returns>
        public void Update(NewsModels model)
        {
            var _conn = new SqlConnection(ConfigurationManager.ConnectionStrings["InfoWebAppDbStr"].ConnectionString);
            if (_conn.State == ConnectionState.Closed)
                _conn.Open();
            //Create command store procedure
            var command = new SqlCommand("Update_tblNews");
            command.Connection = _conn;
            command.CommandType = CommandType.StoredProcedure;
            try
            {
                var TitleParam = new SqlParameter("@Title", model.Title);
                TitleParam.Direction = ParameterDirection.Input;
                var TinhThanhIdParam = new SqlParameter("@TinhThanhId", model.TinhThanhId);
                TinhThanhIdParam.Direction = ParameterDirection.Input;
                var QuanHuyenIdParam = new SqlParameter("@QuanHuyenId", model.QuanHuyenId);
                QuanHuyenIdParam.Direction = ParameterDirection.Input;
                var AddressParam = new SqlParameter("@Address", model.Address);
                AddressParam.Direction = ParameterDirection.Input;
                var MenuIdParam = new SqlParameter("@MenuId", model.MenuId);
                MenuIdParam.Direction = ParameterDirection.Input;
                var DientichParam = new SqlParameter("@Dientich", model.Dientich);
                DientichParam.Direction = ParameterDirection.Input;
                var PriceParam = new SqlParameter("@Price", model.Price);
                PriceParam.Direction = ParameterDirection.Input;
                var PhoneNumberParam = new SqlParameter("@PhoneNumber", model.PhoneNumber);
                PhoneNumberParam.Direction = ParameterDirection.Input;
                var NewsContentParam = new SqlParameter("@NewsContent", model.NewsContent);
                NewsContentParam.Direction = ParameterDirection.Input;
                var UserIdParam = new SqlParameter("@UserId", model.UserId);
                UserIdParam.Direction = ParameterDirection.Input;
                var IdParam = new SqlParameter("@Id", model.Id);
                IdParam.Direction = ParameterDirection.Input;

                command.Parameters.Add(TitleParam);
                command.Parameters.Add(TinhThanhIdParam);
                command.Parameters.Add(QuanHuyenIdParam);
                command.Parameters.Add(AddressParam);
                command.Parameters.Add(MenuIdParam);
                command.Parameters.Add(DientichParam);
                command.Parameters.Add(PriceParam);
                command.Parameters.Add(PhoneNumberParam);
                command.Parameters.Add(NewsContentParam);
                command.Parameters.Add(UserIdParam);
                command.Parameters.Add(IdParam);
                command.ExecuteScalar();
            }
            finally
            {
                command.Connection.Close();
                command.Connection.Dispose();
            }
        }

        /// <summary>
        /// Delete itemn
        /// </summary>
        /// <param name="Id"></param>
        public void Delete(int Id) {
            var model = new NewsModels();
            var _conn = new SqlConnection(ConfigurationManager.ConnectionStrings["InfoWebAppDbStr"].ConnectionString);
            if (_conn.State == ConnectionState.Closed)
                _conn.Open();
            //Create command store procedure
            var command = new SqlCommand("DeleteNewsById");
            command.Connection = _conn;
            command.CommandType = CommandType.StoredProcedure;
            try
            {
                var IdParam = new SqlParameter("@Id", Id);
                IdParam.Direction = ParameterDirection.Input;
                command.Parameters.Add(IdParam);
                command.ExecuteScalar();
            }
            finally
            {
                command.Connection.Close();
                command.Connection.Dispose();
            }
        }

        /// <summary>
        /// Get News by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public NewsModels GetNewsById(int Id) {
            var model = new NewsModels();
            var _conn = new SqlConnection(ConfigurationManager.ConnectionStrings["InfoWebAppDbStr"].ConnectionString);
            if (_conn.State == ConnectionState.Closed)
                _conn.Open();
            //Create command store procedure
            var command = new SqlCommand("GetNewsById");
            command.Connection = _conn;
            command.CommandType = CommandType.StoredProcedure;
            try
            {
                var IdParam = new SqlParameter("@Id", Id);
                IdParam.Direction = ParameterDirection.Input;
                command.Parameters.Add(IdParam);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        model.Id = reader.GetInt32(0);
                        model.Title = reader.GetString(1);
                        model.TinhThanhId = reader.GetInt32(2);
                        model.QuanHuyenId = reader.GetInt32(3);
                        model.Address = reader.GetString(4);
                        model.MenuId = reader.GetInt32(5);
                        model.MenuIdHidden = reader.GetInt32(5);
                        model.Dientich = reader.GetString(6);
                        model.Price = reader.GetString(7);
                        model.PhoneNumber = reader.GetString(8);
                        model.NewsContent = reader.GetString(9);
                        model.UserId = reader.GetInt32(10);
                        model.Datetime = reader.GetDateTime(11);
                        break;
                    }
                    reader.Close();
                }
            }
            finally
            {
                command.Connection.Close();
                command.Connection.Dispose();
            }
            return model;
        }

        /// <summary>
        /// Get News by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public NewsModels GetDetailNews(int Id)
        {
            var model = new NewsModels();
            var _conn = new SqlConnection(ConfigurationManager.ConnectionStrings["InfoWebAppDbStr"].ConnectionString);
            if (_conn.State == ConnectionState.Closed)
                _conn.Open();
            //Create command store procedure
            var command = new SqlCommand("GetDetailNews");
            command.Connection = _conn;
            command.CommandType = CommandType.StoredProcedure;
            try
            {
                var IdParam = new SqlParameter("@Id", Id);
                IdParam.Direction = ParameterDirection.Input;
                command.Parameters.Add(IdParam);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        model.Id = reader.GetInt32(0);
                        model.Title = reader.GetString(1);
                        model.TinhThanhId = reader.GetInt32(2);
                        model.QuanHuyenId = reader.GetInt32(3);
                        model.Address = reader.GetString(4);
                        model.MenuId = reader.GetInt32(5);
                        model.MenuIdHidden = reader.GetInt32(5);
                        model.Dientich = reader.GetString(6);
                        model.Price = reader.GetString(7);
                        model.PhoneNumber = reader.GetString(8);
                        model.NewsContent = reader.GetString(9);
                        model.UserId = reader.GetInt32(10);
                        model.MenuName = reader.GetString(11);
                        model.Datetime = reader.GetDateTime(12);
                        break;
                    }
                    reader.Close();
                }
            }
            finally
            {
                command.Connection.Close();
                command.Connection.Dispose();
            }
            return model;
        }
        /// <summary>
        /// Tim kiem tin tuc
        /// </summary>
        /// <param name="Count">Tong so ban ghi</param>
        /// <param name="TinhThanhId">Tinh Thanh</param>
        /// <param name="QuanHuyenId">Quan Huyen</param>
        /// <param name="TitleSearch">Tu khoa tim kiem</param>
        /// <param name="Price">Gia tien</param>
        /// <param name="PageNumber">Trang thu bao nhieu</param>
        /// <param name="RowsPage">So luong dong tren 1 trang</param>
        /// <param name="MenuId">Menu</param>
        /// <returns></returns>
        public List<NewsModels> SearchNews(
            out int Count,
            int TinhThanhId = 0,
            int QuanHuyenId = 0, 
            String TitleSearch = "",
            String Price = "",
            int PageNumber = 0,
            int RowsPage = 0,
            int MenuId = 0)
            { 
            var listSearch = new List<NewsModels>();
            var _conn = new SqlConnection(ConfigurationManager.ConnectionStrings["InfoWebAppDbStr"].ConnectionString);
            if (_conn.State == ConnectionState.Closed)
                _conn.Open();
            //Create command store procedure
            var command = new SqlCommand("Search_tblNews");
            command.Connection = _conn;
            command.CommandType = CommandType.StoredProcedure;

            var CountTitleParam = new SqlParameter("@CountTitle", SqlDbType.Int);
            CountTitleParam.Direction = ParameterDirection.Output;
            var TinhThanhIdParam = new SqlParameter("@TinhThanhId", TinhThanhId);
            TinhThanhIdParam.Direction = ParameterDirection.Input;
            var QuanHuyenIdParam = new SqlParameter("@QuanHuyenId", QuanHuyenId);
            QuanHuyenIdParam.Direction = ParameterDirection.Input;
            var TitleSearchParam = new SqlParameter("@TitleSearch", TitleSearch);
            TitleSearchParam.Direction = ParameterDirection.Input;
            var PriceParam = new SqlParameter("@Price", Price);
            PriceParam.Direction = ParameterDirection.Input;
            var PageNumberParam = new SqlParameter("@PageNumber", PageNumber);
            PageNumberParam.Direction = ParameterDirection.Input;
            var RowspPageParam = new SqlParameter("@RowspPage", RowsPage);
            RowspPageParam.Direction = ParameterDirection.Input;
            var MenuIdParam = new SqlParameter("@MenuId", MenuId);
            MenuIdParam.Direction = ParameterDirection.Input;

            command.Parameters.Add(TinhThanhIdParam);
            command.Parameters.Add(QuanHuyenIdParam);
            command.Parameters.Add(TitleSearchParam);
            command.Parameters.Add(PriceParam);
            command.Parameters.Add(PageNumberParam);
            command.Parameters.Add(RowspPageParam);
            command.Parameters.Add(MenuIdParam);
            command.Parameters.Add(CountTitleParam);

            try
            {
                Count = 0;
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var model = new NewsModels();
                        model.Number = (int)reader.GetInt64(0);
                        model.Id = reader.GetInt32(1);
                        model.Title = reader.GetString(2);
                        model.NewsContent = reader.GetString(3);
                        model.Price = reader.GetString(4);
                        model.MenuName = reader.GetString(5);
                        model.Datetime = reader.GetDateTime(6);
                        model.MenuId = reader.GetInt32(7);
                        listSearch.Add(model);
                    }
                    reader.Close();
                }
            }
            finally
            {
                command.Connection.Close();
                command.Connection.Dispose();
            }

            Count = CountTitleParam.Value as int? ?? default(int); 
            return listSearch;
        }

        /// <summary>
        /// Tim kiem tin tuc
        /// </summary>
        /// <param name="Count">Tong so ban ghi</param>
        /// <param name="TinhThanhId">Tinh Thanh</param>
        /// <param name="QuanHuyenId">Quan Huyen</param>
        /// <param name="TitleSearch">Tu khoa tim kiem</param>
        /// <param name="Price">Gia tien</param>
        /// <param name="PageNumber">Trang thu bao nhieu</param>
        /// <param name="RowsPage">So luong dong tren 1 trang</param>
        /// <param name="UserId">Menu</param>
        /// <returns></returns>
        public List<NewsModels> SearchMyPost(
            out int Count,
            int TinhThanhId = 0,
            int QuanHuyenId = 0,
            String TitleSearch = "",
            String Price = "",
            int PageNumber = 0,
            int RowsPage = 0,
            int UserId = 0)
        {
            var listSearch = new List<NewsModels>();
            var _conn = new SqlConnection(ConfigurationManager.ConnectionStrings["InfoWebAppDbStr"].ConnectionString);
            if (_conn.State == ConnectionState.Closed)
                _conn.Open();
            //Create command store procedure
            var command = new SqlCommand("Search_MyPost");
            command.Connection = _conn;
            command.CommandType = CommandType.StoredProcedure;

            var CountTitleParam = new SqlParameter("@CountTitle", SqlDbType.Int);
            CountTitleParam.Direction = ParameterDirection.Output;
            var TinhThanhIdParam = new SqlParameter("@TinhThanhId", TinhThanhId);
            TinhThanhIdParam.Direction = ParameterDirection.Input;
            var QuanHuyenIdParam = new SqlParameter("@QuanHuyenId", QuanHuyenId);
            QuanHuyenIdParam.Direction = ParameterDirection.Input;
            var TitleSearchParam = new SqlParameter("@TitleSearch", TitleSearch);
            TitleSearchParam.Direction = ParameterDirection.Input;
            var PriceParam = new SqlParameter("@Price", Price);
            PriceParam.Direction = ParameterDirection.Input;
            var PageNumberParam = new SqlParameter("@PageNumber", PageNumber);
            PageNumberParam.Direction = ParameterDirection.Input;
            var RowspPageParam = new SqlParameter("@RowspPage", RowsPage);
            RowspPageParam.Direction = ParameterDirection.Input;
            var MenuIdParam = new SqlParameter("@UserId", UserId);
            MenuIdParam.Direction = ParameterDirection.Input;

            command.Parameters.Add(TinhThanhIdParam);
            command.Parameters.Add(QuanHuyenIdParam);
            command.Parameters.Add(TitleSearchParam);
            command.Parameters.Add(PriceParam);
            command.Parameters.Add(PageNumberParam);
            command.Parameters.Add(RowspPageParam);
            command.Parameters.Add(MenuIdParam);
            command.Parameters.Add(CountTitleParam);

            try
            {
                Count = 0;
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var model = new NewsModels();
                        model.Number = (int)reader.GetInt64(0);
                        model.Id = reader.GetInt32(1);
                        model.Title = reader.GetString(2);
                        model.NewsContent = reader.GetString(3);
                        model.Price = reader.GetString(4);
                        model.MenuName = reader.GetString(5);
                        model.Datetime = reader.GetDateTime(6);
                        listSearch.Add(model);
                    }
                    reader.Close();
                }
            }
            finally
            {
                command.Connection.Close();
                command.Connection.Dispose();
            }

            Count = CountTitleParam.Value as int? ?? default(int);
            return listSearch;
        }
    }
}