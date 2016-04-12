using InfoWebApp.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InfoWebApp.Entity
{
    /// <summary>
    /// Get Menu table from Database
    /// </summary>
    public class MenuDb
    {
        /// <summary>
        /// 
        /// </summary>
        public MenuDb() {
            Id = 0;
            Name = String.Empty;
            Description = String.Empty;
            ParentId = 0;
        }

        #region Properties

        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Parent Id
        /// </summary>
        public int ParentId { get; set; }

        #endregion

        /// <summary>
        /// Add new item and return Id
        /// </summary>
        /// <returns></returns>
        public MenuModels Add(MenuModels model) {
            var _conn = new SqlConnection(ConfigurationManager.ConnectionStrings["InfoWebAppDbStr"].ConnectionString);
            if (_conn.State == ConnectionState.Closed)
                _conn.Open();
            //Create command store procedure
            var command = new SqlCommand("Add_tblMenu");
            command.Connection = _conn;
            command.CommandType = CommandType.StoredProcedure;
            try
            {
                var NameParam = new SqlParameter("@Name", model.Name);
                NameParam.Direction = ParameterDirection.Input;
                var DescriptionParam = new SqlParameter("@Description", model.Description);
                DescriptionParam.Direction = ParameterDirection.Input;
                var ParentIdParam = new SqlParameter("@ParentId", model.ParentId);
                ParentIdParam.Direction = ParameterDirection.Input;

                command.Parameters.Add(NameParam);
                command.Parameters.Add(DescriptionParam);
                command.Parameters.Add(ParentIdParam);

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
        public void Update(MenuModels model)
        {
            var _conn = new SqlConnection(ConfigurationManager.ConnectionStrings["InfoWebAppDbStr"].ConnectionString);
            if (_conn.State == ConnectionState.Closed)
                _conn.Open();
            //Create command store procedure
            var command = new SqlCommand("Update_tblMenu");
            command.Connection = _conn;
            command.CommandType = CommandType.StoredProcedure;
            try
            {
                var NameParam = new SqlParameter("@Name", model.Name);
                NameParam.Direction = ParameterDirection.Input;
                var DescriptionParam = new SqlParameter("@Description", model.Description);
                DescriptionParam.Direction = ParameterDirection.Input;
                var ParentIdParam = new SqlParameter("@ParentId", model.ParentId);
                ParentIdParam.Direction = ParameterDirection.Input;
                var IdParam = new SqlParameter("@Id", model.Id);
                IdParam.Direction = ParameterDirection.Input;

                command.Parameters.Add(NameParam);
                command.Parameters.Add(DescriptionParam);
                command.Parameters.Add(ParentIdParam);
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
        /// Get menu by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public MenuModels GetMenuById(int Id)
        {
            var model = new MenuModels();
            var _conn = new SqlConnection(ConfigurationManager.ConnectionStrings["InfoWebAppDbStr"].ConnectionString);
            if (_conn.State == ConnectionState.Closed)
                _conn.Open();
            //Create command store procedure
            var command = new SqlCommand("GetMenuById");
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
                        model.Name = reader.GetString(1);
                        model.Description = reader.GetString(2);
                        model.ParentId = reader.GetInt32(3);
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
        /// Delete menu by Id
        /// </summary>
        /// <param name="Id"></param>
        public void DeleteMenuById(int Id)
        {
            var model = new MenuModels();
            var _conn = new SqlConnection(ConfigurationManager.ConnectionStrings["InfoWebAppDbStr"].ConnectionString);
            if (_conn.State == ConnectionState.Closed)
                _conn.Open();
            //Create command store procedure
            var command = new SqlCommand("DeleteMenuById");
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
        /// Get all item from database
        /// </summary>
        /// <returns></returns>
        public List<MenuDb> GetAll() {
            var list = new List<MenuDb>();
            //--Start Get/List--//
            var _conn = new SqlConnection(ConfigurationManager.ConnectionStrings["InfoWebAppDbStr"].ConnectionString);
            if (_conn.State == ConnectionState.Closed)
                _conn.Open();
            //Create command store procedure
            var command = new SqlCommand("Get_tblMenu");
            command.Connection = _conn;
            command.CommandType = CommandType.StoredProcedure;
            try
            {
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var item = new MenuDb();
                        item.Id = reader.GetInt32(0);
                        item.Name = reader.GetString(1);
                        item.Description = reader.GetString(2);
                        item.ParentId = reader.GetInt32(3);
                        list.Add(item);
                    }
                    reader.Close();
                }
            }
            finally
            {
                command.Connection.Close();
                command.Connection.Dispose();
            }
            //--End Get/List--//

            return list;
        }

        /// <summary>
        /// Get all item of parent menu
        /// </summary>
        /// <returns></returns>
        public List<MenuModels> GetParentMenu()
        {
            var listParentMenu = new List<MenuModels>();
            var itemMain = new MenuModels();
            itemMain.Name = "--Chọn Menu--";
            itemMain.Id = 0;
            listParentMenu.Add(itemMain);
            var _conn = new SqlConnection(ConfigurationManager.ConnectionStrings["InfoWebAppDbStr"].ConnectionString);
            if (_conn.State == ConnectionState.Closed)
                _conn.Open();
            //Create command store procedure
            var command = new SqlCommand("Get_ParentMenu");
            command.Connection = _conn;
            command.CommandType = CommandType.StoredProcedure;
            try
            {
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var item = new MenuModels();
                        item.Name = reader.GetString(1);
                        item.Id = reader.GetInt32(0);
                        listParentMenu.Add(item);
                    }
                    reader.Close();
                }
            }
            finally
            {
                command.Connection.Close();
                command.Connection.Dispose();
            }
            return listParentMenu;
        }
    }
}