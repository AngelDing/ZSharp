using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace ZSharp.Framework.BizCode
{
    public interface ISequence
    {
        ISequence SetDbContext(ISequenceRepository repo);

        ISequence SetTenantID(string tenantId);

        ISequence SetValues(Dictionary<string, object> row);

        ISequence SetValues(JToken row);

        ISequence SetValue(string name, object value);

        string Next();

        string Next(int qty);
    }
}
