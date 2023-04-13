using CrudUsingDapper.Models;
using System.Collections.Generic;

namespace CrudUsingDapper.IServices
{
    public interface IClient
    {
        Client Insert(Client client);
        Client Update(Client client);
        List<Client> Gets();
        Client Get(int clientId);
        string Delete(int clientId);
    }
}
