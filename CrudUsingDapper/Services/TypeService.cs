using CrudUsingDapper.Common;
using CrudUsingDapper.IServices;
using CrudUsingDapper.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CrudUsingDapper.Services
{
    public class TypeService : IType
    {
        Type_client _type = new Type_client();
        List<Type_client> _types = new List<Type_client>();

        public string Delete(int typeId)
        {
            string message = "";
            try
            {
                _type = new Type_client()
                {
                    Id = typeId
                };

                using (IDbConnection con = new SqlConnection(Global.ConnectionString))
                {
                    if (con.State == ConnectionState.Closed) con.Open();

                    var types = con.Query<Type_client>("P_Type_client", this.SetParameters(_type, (int)OperationType.Delete), commandType: CommandType.StoredProcedure);

                    message = "Tipo de Id:" + _type.Id + "Deletado";
                }

            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return message;
        }

        public Type_client Get(int typeId)
        {
            _type = new Type_client();
            using (IDbConnection con = new SqlConnection(Global.ConnectionString))
            {
                if (con.State == ConnectionState.Closed) con.Open();

                var types = con.Query<Type_client>("SELECT * FROM Type_client WHERE ID = " + typeId).ToList();

                if (types != null && types.Count() > 0)
                {
                    _type = types.SingleOrDefault();
                }
            }
            return _type;
        }

        public List<Type_client> Gets()
        {
            _types = new List<Type_client>();
            using (IDbConnection con = new SqlConnection(Global.ConnectionString))
            {
                if (con.State == ConnectionState.Closed) con.Open();

                var types = con.Query<Type_client>("SELECT * FROM Type_client").ToList();

                if (types != null && types.Count() > 0)
                {
                    _types = types;
                }
            }
            return _types;
        }

        public Type_client Insert(Type_client type)
        {
            _type = new Type_client();
            try
            {
                int operationType = Convert.ToInt32(type.Id == 0 ? OperationType.Insert : OperationType.Update);

                using (IDbConnection con = new SqlConnection(Global.ConnectionString))
                {
                    if (con.State == ConnectionState.Closed) con.Open();

                    var types = con.Query<Type_client>("P_Type_client", this.SetParameters(type, operationType), commandType: CommandType.StoredProcedure);

                    if (types != null && types.Count() > 0)
                    {
                        _type = types.FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                _type.Message = ex.Message;
            }
            return _type;
        }

        private DynamicParameters SetParameters(Type_client type, int operationType)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@ID", type.Id);
            parameters.Add("@Type_des", type.Type_des);
            return parameters;
        }
    }
}
