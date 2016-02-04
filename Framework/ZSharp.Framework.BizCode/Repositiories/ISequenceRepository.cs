using System;
using System.Collections.Generic;
using ZSharp.Framework.Repositories;

namespace ZSharp.Framework.BizCode
{
    public interface ISequenceRepository : IRepository<SequenceEntity, Guid>
    {
        IList<SequenceSettingEntity> GetSettingList(string sequenceName);

        //.FindAll<SequenceSettingEntity>(v => v.SequenceName == _context.SequenceName).OrderBy(v => v.RuleOrder).ToList()
    }
}
