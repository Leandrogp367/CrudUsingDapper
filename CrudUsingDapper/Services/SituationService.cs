using CrudUsingDapper.Common;
using CrudUsingDapper.IServices;
using CrudUsingDapper.Models;
using Dapper;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;


namespace CrudUsingDapper.Services
{
    public class SituationService : ISituation
    {
        Situation_client _situation = new Situation_client();
        List<Situation_client> _situations = new List<Situation_client>();

        public string Delete(int situationId)
        {
            string message = "";
            try
            {
                _situation = new Situation_client()
                {
                    Id = situationId
                };

                using (IDbConnection con = new SqlConnection(Global.ConnectionString))
                {
                    if (con.State == ConnectionState.Closed) con.Open();

                    var situations = con.Query<Situation_client>("P_Situation_client", this.SetParameters(_situation, (int)OperationType.Delete), commandType: CommandType.StoredProcedure);

                    message = "Situacao de Id:" + _situation.Id + "Deletado";
                }

            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return message;
        }

        public Situation_client Get(int situationId)
        {
            _situation = new Situation_client();
            using (IDbConnection con = new SqlConnection(Global.ConnectionString))
            {
                if (con.State == ConnectionState.Closed) con.Open();

                var situations = con.Query<Situation_client>("SELECT * FROM Situation_client WHERE ID = " + situationId).ToList();

                if (_situations != null && _situations.Count() > 0)
                {
                    _situation = _situations.SingleOrDefault();
                }
            }
            return _situation;
        }

        public List<Situation_client> Gets()
        {
            _situations = new List<Situation_client>();
            using (IDbConnection con = new SqlConnection(Global.ConnectionString))
            {
                if (con.State == ConnectionState.Closed) con.Open();

                var situations = con.Query<Situation_client>("SELECT * FROM Situation_client").ToList();

                if (situations != null && situations.Count() > 0)
                {
                    _situations = situations;
                }
            }
            return _situations;
        }

        public Situation_client Insert(Situation_client situation)
        {
            _situation = new Situation_client();
            try
            {
                int operationType = Convert.ToInt32(situation.Id == 0 ? OperationType.Insert : OperationType.Update);

                using (IDbConnection con = new SqlConnection(Global.ConnectionString))
                {
                    if (con.State == ConnectionState.Closed) con.Open();

                    var situations = con.Query<Situation_client>("P_Situation_client", this.SetParameters(situation, operationType), commandType: CommandType.StoredProcedure);

                    if (situations != null && situations.Count() > 0)
                    {
                        _situation = situations.FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                _situation.Message = ex.Message;
            }
            return _situation;
        }

        private DynamicParameters SetParameters(Situation_client situation, int operationType)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@ID", situation.Id);
            parameters.Add("@Situation_des", situation.Situation_des);
            return parameters;
        }
    }
}
