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
    public class PrivateNewsDb
    {
        public PrivateNewsDb() {
            Id = 0;
            Status = 0;
            Title = String.Empty;
            Address = String.Empty;
            Dientich = String.Empty;
            Price = String.Empty;
            PhoneNumer = String.Empty;
            NewsContent = String.Empty;
            Status = 0;
            Datetime = new DateTime();
        }

        #region Propeties

        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

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
        /// Datetime
        /// </summary>
        public DateTime Datetime { get; set; }

        #endregion

        /// <summary>
        /// Tim kiem tin tuc
        /// </summary>
        /// <param name="Count"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="TitleSearch"></param>
        /// <param name="Address"></param>
        /// <param name="Price"></param>
        /// <param name="PageNumber"></param>
        /// <param name="RowsPage"></param>
        /// <returns></returns>
        public List<PrivateNewsModel> SearchNews(
            out int Count,
            DateTime StartDate,
            DateTime EndDate,
            String TitleSearch = "",
            String Address = "",
            String Price = "",
            int PageNumber = 0,
            int RowsPage = 0,
            int Status = 0)
        {
            var listSearch = new List<PrivateNewsModel>();
            var _conn = new SqlConnection(ConfigurationManager.ConnectionStrings["InfoWebAppDbStr"].ConnectionString);
            if (_conn.State == ConnectionState.Closed)
                _conn.Open();
            //Create command store procedure
            var command = new SqlCommand("Search_tblPrivateNews");
            command.Connection = _conn;
            command.CommandType = CommandType.StoredProcedure;

            var CountTitleParam = new SqlParameter("@CountTitle", SqlDbType.Int);
            CountTitleParam.Direction = ParameterDirection.Output;
            var TitleSearchParam = new SqlParameter("@TitleSearch", TitleSearch);
            TitleSearchParam.Direction = ParameterDirection.Input;
            var PriceParam = new SqlParameter("@Price", Price);
            PriceParam.Direction = ParameterDirection.Input;
            var PageNumberParam = new SqlParameter("@PageNumber", PageNumber);
            PageNumberParam.Direction = ParameterDirection.Input;
            var RowspPageParam = new SqlParameter("@RowspPage", RowsPage);
            RowspPageParam.Direction = ParameterDirection.Input;
            var AddressParam = new SqlParameter("@Address", Address);
            AddressParam.Direction = ParameterDirection.Input;
            var StatusParam = new SqlParameter("@Status", Status);
            StatusParam.Direction = ParameterDirection.Input;
            if (StartDate != DateTime.MinValue) {
                var StartDateParam = new SqlParameter("@DateStart", StartDate);
                StartDateParam.Direction = ParameterDirection.Input;
                command.Parameters.Add(StartDateParam);
            }
            if (EndDate != DateTime.MinValue)
            {
                var EndDateParam = new SqlParameter("@DateEnd", EndDate);
                EndDateParam.Direction = ParameterDirection.Input;
                command.Parameters.Add(EndDateParam);
            }

            command.Parameters.Add(TitleSearchParam);
            command.Parameters.Add(PriceParam);
            command.Parameters.Add(PageNumberParam);
            command.Parameters.Add(RowspPageParam);
            command.Parameters.Add(CountTitleParam);
            command.Parameters.Add(AddressParam);
            command.Parameters.Add(StatusParam);
            try
            {
                Count = 0;
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var model = new PrivateNewsModel();
                        model.Number = (int)reader.GetInt64(0);
                        model.Id = reader.GetInt32(1);
                        model.Title = reader.GetString(2);
                        model.Price = reader.GetString(3);
                        model.PhoneNumer = reader.GetString(4);
                        model.Status = reader.GetInt32(5);
                        listSearch.Add(model);
                    }
                    reader.Close();
                }
            }
            catch (Exception ex) {
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
        /// Lay ra tin bai chi tiet
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public PrivateNewsModel GetDetail(int Id) {
            var model = new PrivateNewsModel();
            var _conn = new SqlConnection(ConfigurationManager.ConnectionStrings["InfoWebAppDbStr"].ConnectionString);
            if (_conn.State == ConnectionState.Closed)
                _conn.Open();
            //Create command store procedure
            var command = new SqlCommand("GetPrivateNewsById");
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
                        model.NewsContent = reader.GetString(2);
                        model.Address = reader.GetString(3);
                        model.Dientich = reader.GetString(4);
                        model.Price = reader.GetString(5);
                        model.PhoneNumer = reader.GetString(6);
                        model.Datetime = reader.GetDateTime(7);
                        model.Status = reader.GetInt32(8);
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
    }


}