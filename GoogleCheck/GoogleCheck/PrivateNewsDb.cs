using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCheck
{
    public class PrivateNewsDb
    {
        public PrivateNewsDb()
        {
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
        /// 
        /// </summary>
        /// <returns></returns>
        public List<PrivateNewsModel> GetValuesForGoogle()
        {
            var listSearch = new List<PrivateNewsModel>();
            var _conn = new SqlConnection(ConfigurationManager.ConnectionStrings["InfoWeb"].ConnectionString);
            if (_conn.State == ConnectionState.Closed)
                _conn.Open();
            //Create command store procedure
            var command = new SqlCommand("GetValuesForGoogle");
            command.Connection = _conn;
            command.CommandType = CommandType.StoredProcedure;
            try
            {
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var model = new PrivateNewsModel();
                        model.Id = reader.GetInt32(0);
                        model.PhoneNumer = reader.GetString(6);
                        listSearch.Add(model);
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Khong the ket noi den server! Vui long kiem tra cau hinh va ket noi mang...");
                Console.Read();
            }
            finally
            {
                command.Connection.Close();
                command.Connection.Dispose();
            }

            return listSearch;
        }

        /// <summary>
        /// Update new item and return success of fail
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Status"></param>
        public void Update(int Id, int Status)
        {
            var _conn = new SqlConnection(ConfigurationManager.ConnectionStrings["InfoWeb"].ConnectionString);
            if (_conn.State == ConnectionState.Closed)
                _conn.Open();
            //Create command store procedure
            var command = new SqlCommand("Update_tblPrivateNewsViaGoogle");
            command.Connection = _conn;
            command.CommandType = CommandType.StoredProcedure;
            try
            {
                var StatusParam = new SqlParameter("@Status", Status);
                StatusParam.Direction = ParameterDirection.Input;
                var IdParam = new SqlParameter("@Id", Id);
                IdParam.Direction = ParameterDirection.Input;
                command.Parameters.Add(StatusParam);
                command.Parameters.Add(IdParam);
                command.ExecuteScalar();
            }
            finally
            {
                command.Connection.Close();
                command.Connection.Dispose();
            }
        }
    }
}
