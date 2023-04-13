using CrudUsingDapper.Common;
using CrudUsingDapper.IServices;
using CrudUsingDapper.Models;
using Dapper;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CrudUsingDapper.Services
{
    public class ClientService: IClient
    {
        Client _client = new Client();
        List<Client> _clients = new List<Client>();

        #region Methods
        public string Delete(int clientId)
        {
            string message = "";
            try
            {
                _client = new Client()
                {
                    Id = clientId
                };

                using (IDbConnection con = new SqlConnection(Global.ConnectionString))
                {
                    if (con.State == ConnectionState.Closed) con.Open();

                    var clients = con.Query<Client>("P_Client", this.SetParameters(_client, (int)OperationType.Delete), commandType: CommandType.StoredProcedure);

                    message = "Usuário de Id: "+ _client.Id +" Deletado";
                }

            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return message;
        }

        public Client Get(int clientId)
        {
            _client = new Client();
            using (IDbConnection con = new SqlConnection(Global.ConnectionString))
            {
                if (con.State == ConnectionState.Closed) con.Open();

                var clients = con.Query<Client>("SELECT * FROM Client WHERE ID = " + clientId).ToList();

                if (clients != null && clients.Count() > 0)
                {
                    _client = clients.SingleOrDefault();
                }
            }
            return _client;
        }

        public List<Client> Gets()
        {
            _clients = new List<Client>();
            using (IDbConnection con = new SqlConnection(Global.ConnectionString))
            {
                if (con.State == ConnectionState.Closed) con.Open();

                var clients = con.Query<Client>("SELECT * FROM Client").ToList();

                if (clients != null && clients.Count() > 0)
                {
                    _clients = clients;
                }
            }
            return _clients;
        }

        public Client Insert(Client client)
        {
            string message;

            _client = new Client();

            try
            {
                if (!ValidateInsertClientData(client, out message))
                {
                    _client.Message = message;
                    return _client;
                }
                using (IDbConnection con = new SqlConnection(Global.ConnectionString))
                {
                    if (con.State == ConnectionState.Closed) con.Open();

                    var clients = con.Query<Client>("P_Client", this.SetParameters(client, (int)OperationType.Insert), commandType: CommandType.StoredProcedure);

                    if (clients != null && clients.Count() > 0)
                    {
                        _client = clients.FirstOrDefault();
                        _client.Message = "Cliente criado com sucesso!";
                    }
                }
            }
            catch (Exception ex)
            {
                _client.Message = ex.Message;
            }
            return _client;
        }

        public Client Update(Client client) 
        {
            string message;
            _client = new Client();
            try
            {
                if (!ValidateUpdateClientData(client, out message))
                {
                    _client.Message = message;
                    return _client;
                }
                using (IDbConnection con = new SqlConnection(Global.ConnectionString))
                {
                    if (con.State == ConnectionState.Closed) con.Open();

                    var clients = con.Query<Client>("P_Client", this.SetParameters(client, (int)OperationType.Update), commandType: CommandType.StoredProcedure);

                    if (clients != null && clients.Count() > 0)
                    {
                        _client = clients.FirstOrDefault();
                        _client.Message = "Cliente editado com sucesso!";
                    }
                }
            }
            catch (Exception ex)
            {
                _client.Message = ex.Message;
            }
            return _client;
        }
        #endregion Methods

        #region Dynamic Parameters
        private DynamicParameters SetParameters(Client client, int operationType)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@ID", client.Id);
            parameters.Add("@Name", client.Name);
            parameters.Add("@Cpf", client.Cpf);
            parameters.Add("@Type", client.Type);
            parameters.Add("@Gender", client.Gender);
            parameters.Add("@Situation", client.Situation);
            parameters.Add("@OperationType", operationType);
            return parameters;
        }
        #endregion Dynamic Parameters

        #region Validation Methods

        public bool ValidateUpdateClientData(Client client, out string message)
        {
            Client validationClient = new Client();

            using (IDbConnection con = new SqlConnection(Global.ConnectionString))
            {
                if (con.State == ConnectionState.Closed) con.Open();

                var clients = con.Query<Client>("SELECT * FROM Client WHERE Cpf = '" + client.Cpf + "' AND ID != " + client.Id.ToString()).ToList();

                if (clients != null && clients.Count() > 0)
                {
                    message = "CPF existente!";
                    return false;
                }
            }

            if (string.IsNullOrEmpty(client.Name))
            {
                message = "Campo Nome obrigatório!";
                return false;
            }

            if (string.IsNullOrEmpty(client.Cpf))
            {
                message = "Campo Cpf obrigatório!";
                return false;
            }

            if (!IsCpf(client.Cpf))
            {
                message = "Cpf Invalido!";
                return false;
            }

            message = string.Empty;
            return true;
        }

        public bool ValidateInsertClientData(Client client, out string message)
        {
            Client validationClient = new Client();

            using (IDbConnection con = new SqlConnection(Global.ConnectionString))
            {
                if (con.State == ConnectionState.Closed) con.Open();

                var clients = con.Query<Client>("SELECT * FROM Client WHERE Cpf = '" + client.Cpf + "'").ToList();

                if (clients != null && clients.Count() > 0)
                {
                    message = "CPF existente!";
                    return false;
                }
            }

            using (IDbConnection con = new SqlConnection(Global.ConnectionString))
            {
                if (con.State == ConnectionState.Closed) con.Open();

                var types = con.Query<Type_client>("SELECT * FROM Type_client WHERE ID = " + client.Type.ToString()).ToList();

                if (types.Count() <= 0)
                {
                    message = "Tipo inválido!";
                    return false;
                }
            }

            using (IDbConnection con = new SqlConnection(Global.ConnectionString))
            {
                if (con.State == ConnectionState.Closed) con.Open();

                var situations = con.Query<Client>("SELECT * FROM Situation_client WHERE ID = " + client.Situation.ToString()).ToList();

                if (situations.Count() <= 0)
                {
                    message = "Situação inálida";
                    return false;
                }
            }

            if (string.IsNullOrEmpty(client.Name))
            {
                message = "Campo Nome obrigatório!";
                return false;
            }

            if (string.IsNullOrEmpty(client.Cpf))
            {
                message = "Campo Cpf obrigatório!";
                return false;
            }

            if (!IsCpf(client.Cpf))
            {
                message = "Cpf Invalido!";
                return false;
            }

            message = string.Empty;
            return true;
        }

        public static bool IsCpf(string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");
            if (cpf.Length != 11)
                return false;
            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cpf.EndsWith(digito);
        }


        #endregion Validation Methods
    }
}
