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
    /// Area db
    /// </summary>
    public class AreaDb
    {
        /// <summary>
        /// 
        /// </summary>
        public AreaDb()
        {
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

        /// <summary>
        /// Get List Area by Id
        /// </summary>
        /// <param name="ParentId"></param>
        /// <returns></returns>
        public List<AreaModel> GetListAreaById(int ParentId)
        {
            var list = new List<AreaModel>();
            var areaModel = new AreaModel();
            areaModel.Id = 0;
            areaModel.Name = "--Chọn quận huyện--";
            list.Add(areaModel);
            var _conn = new SqlConnection(ConfigurationManager.ConnectionStrings["InfoWebAppDbStr"].ConnectionString);
            if (_conn.State == ConnectionState.Closed)
                _conn.Open();
            //Create command store procedure
            var command = new SqlCommand("GetAreaByParentId");
            command.Connection = _conn;
            command.CommandType = CommandType.StoredProcedure;
            try
            {
                var IdParam = new SqlParameter("@ParentId", ParentId);
                IdParam.Direction = ParameterDirection.Input;
                command.Parameters.Add(IdParam);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var model = new AreaModel();
                        model.Id = reader.GetInt32(0);
                        model.Name = reader.GetString(1);
                        model.ParentId = reader.GetInt32(2);
                        list.Add(model);
                    }
                    reader.Close();
                }
            }
            finally
            {
                command.Connection.Close();
                command.Connection.Dispose();
            }
            return list;
        }

        /// <summary>
        /// Get all item from database
        /// </summary>
        /// <returns></returns>
        public List<AreaModel> GetAll()
        {
            var list = new List<AreaModel>();
            //--Start Get/List--//
            var _conn = new SqlConnection(ConfigurationManager.ConnectionStrings["InfoWebAppDbStr"].ConnectionString);
            if (_conn.State == ConnectionState.Closed)
                _conn.Open();
            //Create command store procedure
            var command = new SqlCommand("GetArea");
            command.Connection = _conn;
            command.CommandType = CommandType.StoredProcedure;
            try
            {
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var item = new AreaModel();
                        item.Id = reader.GetInt32(0);
                        item.Name = reader.GetString(1);
                        item.ParentId = reader.GetInt32(2);
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
            return list;
        }
    }
}