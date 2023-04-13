using CrudUsingDapper.Models;
using System.Collections.Generic;

namespace CrudUsingDapper.IServices
{
    public interface IType
    {
        Type_client Insert(Type_client type_Client);
        List<Type_client> Gets();
        Type_client Get(int typeId);
        string Delete(int typeId);
    }
}
