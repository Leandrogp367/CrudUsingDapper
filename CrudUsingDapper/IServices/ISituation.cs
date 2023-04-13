using CrudUsingDapper.Models;
using System.Collections.Generic;

namespace CrudUsingDapper.IServices
{
    public interface ISituation
    {
        Situation_client Insert(Situation_client situation_Client);
        List<Situation_client> Gets();
        Situation_client Get(int situationId);
        string Delete(int situationId);
    }
}
