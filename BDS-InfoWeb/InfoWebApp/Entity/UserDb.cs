using InfoWebApp.Generate;
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
    public class UserDb
    {
        public UserDb() {
            UserId = 0;
            Password = String.Empty;
            FullName = String.Empty;
            PhoneNumber = String.Empty;
            Email = String.Empty;
            Address = String.Empty;
            Permission = String.Empty;
            Status = String.Empty;
        }

        #region Propeties
        public int UserId { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Permission { get; set; }
        public string Status { get; set; }
        #endregion

        public LoginUserModel Login(LoginUserModel model) {
            var _conn = new SqlConnection(ConfigurationManager.ConnectionStrings["InfoWebAppDbStr"].ConnectionString);
            if (_conn.State == ConnectionState.Closed)
                _conn.Open();
            //Create command store procedure
            var command = new SqlCommand("LoginUser");
            command.Connection = _conn;
            command.CommandType = CommandType.StoredProcedure;
            try
            {
                var PhoneNumberParam = new SqlParameter("@PhoneNumber", model.PhoneNumber);
                PhoneNumberParam.Direction = ParameterDirection.Input;
                command.Parameters.Add(PhoneNumberParam);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (model.Password == Crypto.DecryptStringAES(reader.GetString(1))) {
                            model.UserId = reader.GetInt32(0);
                            model.FullName = reader.GetString(2);
                            model.Permission = reader.GetString(6);
                            model.Status = reader.GetString(7);
                        }
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

        public void ChangePassword(ChangePasswordModel model) {
            var _conn = new SqlConnection(ConfigurationManager.ConnectionStrings["InfoWebAppDbStr"].ConnectionString);
            if (_conn.State == ConnectionState.Closed)
                _conn.Open();
            //Create command store procedure
            var command = new SqlCommand("ChangePassword");
            command.Connection = _conn;
            command.CommandType = CommandType.StoredProcedure;
            try
            {
                var PasswordParam = new SqlParameter("@Password", Crypto.EncryptStringAES(model.NewPassword));
                PasswordParam.Direction = ParameterDirection.Input;
                var PhoneNumberParam = new SqlParameter("@PhoneNumber", model.PhoneNumber);
                command.Parameters.Add(PasswordParam);
                command.Parameters.Add(PhoneNumberParam);
                command.ExecuteNonQuery();
            }
            finally
            {
                command.Connection.Close();
                command.Connection.Dispose();
            }
        }

        public bool CheckPhoneNumberExist(String PhoneNumber) {
            var status = false;
            var _conn = new SqlConnection(ConfigurationManager.ConnectionStrings["InfoWebAppDbStr"].ConnectionString);
            if (_conn.State == ConnectionState.Closed)
                _conn.Open();
            //Create command store procedure
            var command = new SqlCommand("CheckPhoneNumber");
            command.Connection = _conn;
            command.CommandType = CommandType.StoredProcedure;
            try
            {
                var IdParam = new SqlParameter("@PhoneNumber", PhoneNumber);
                IdParam.Direction = ParameterDirection.Input;
                command.Parameters.Add(IdParam);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    status = true;
                    reader.Close();
                }
            }
            finally
            {
                command.Connection.Close();
                command.Connection.Dispose();
            }
            return status;
        }

        public int Register(RegisterUserModel model) {
            var _conn = new SqlConnection(ConfigurationManager.ConnectionStrings["InfoWebAppDbStr"].ConnectionString);
            if (_conn.State == ConnectionState.Closed)
                _conn.Open();
            //Create command store procedure
            var command = new SqlCommand("Add_tblUser_Register");
            command.Connection = _conn;
            command.CommandType = CommandType.StoredProcedure;
            try
            {
                var PasswordParam = new SqlParameter("@Password", Crypto.EncryptStringAES(model.Password));
                PasswordParam.Direction = ParameterDirection.Input;
                var FullNameParam = new SqlParameter("@FullName", model.FullName);
                FullNameParam.Direction = ParameterDirection.Input;
                var PhoneNumberParam = new SqlParameter("@PhoneNumber", model.PhoneNumber);
                PhoneNumberParam.Direction = ParameterDirection.Input;
                var EmailParam = new SqlParameter("@Email", model.Email);
                EmailParam.Direction = ParameterDirection.Input;
                var AddressParam = new SqlParameter("@Address", model.Address);
                AddressParam.Direction = ParameterDirection.Input;

                command.Parameters.Add(PasswordParam);
                command.Parameters.Add(FullNameParam);
                command.Parameters.Add(PhoneNumberParam);
                command.Parameters.Add(EmailParam);
                command.Parameters.Add(AddressParam);
                model.Id = int.Parse(command.ExecuteScalar().ToString());
            }
            finally
            {
                command.Connection.Close();
                command.Connection.Dispose();
            }
            return model.Id;
        }

        public InfoUserModel GetUserInfo(String PhoneNumber) {
            var model = new InfoUserModel();
            var _conn = new SqlConnection(ConfigurationManager.ConnectionStrings["InfoWebAppDbStr"].ConnectionString);
            if (_conn.State == ConnectionState.Closed)
                _conn.Open();
            //Create command store procedure
            var command = new SqlCommand("UserInfo");
            command.Connection = _conn;
            command.CommandType = CommandType.StoredProcedure;
            try
            {
                var IdParam = new SqlParameter("@PhoneNumber", PhoneNumber);
                IdParam.Direction = ParameterDirection.Input;
                command.Parameters.Add(IdParam);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        model.Id = reader.GetInt32(0);
                        model.FullName = reader.GetString(2);
                        model.PhoneNumber = reader.GetString(3);
                        model.Email = reader.GetString(4);
                        model.Address = reader.GetString(5);
                        model.Permission = reader.GetString(6);
                        model.Status = reader.GetString(7);
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

        public void UpdateInfo(InfoUserModel model) {
            var _conn = new SqlConnection(ConfigurationManager.ConnectionStrings["InfoWebAppDbStr"].ConnectionString);
            if (_conn.State == ConnectionState.Closed)
                _conn.Open();
            //Create command store procedure
            var command = new SqlCommand("Update_UserInfo");
            command.Connection = _conn;
            command.CommandType = CommandType.StoredProcedure;
            try
            {
                var FullNameParam = new SqlParameter("@FullName", model.FullName);
                FullNameParam.Direction = ParameterDirection.Input;
                var EmailParam = new SqlParameter("@Email", model.Email);
                EmailParam.Direction = ParameterDirection.Input;
                var AddressParam = new SqlParameter("@Address", model.Address);
                AddressParam.Direction = ParameterDirection.Input;
                var PhoneNumberParam = new SqlParameter("@PhoneNumer", model.PhoneNumber);
                PhoneNumberParam.Direction = ParameterDirection.Input;
                command.Parameters.Add(FullNameParam);
                command.Parameters.Add(EmailParam);
                command.Parameters.Add(AddressParam);
                command.Parameters.Add(PhoneNumberParam);
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