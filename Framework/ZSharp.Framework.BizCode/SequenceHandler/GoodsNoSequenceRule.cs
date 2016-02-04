using System;
using System.Collections.Generic;

namespace ZSharp.Framework.BizCode
{
    public class GoodsNoSequenceRule : IClassSequenceHandler
    {
        public string Handle(SequenceContext data)
        {
            if (!data.row.ContainsKey("ArtNo"))
                throw new Exception("缺少参数ArtNo");

            if (!data.row.ContainsKey("Color"))
                throw new Exception("缺少参数Color");

            if (!data.row.ContainsKey("Size"))
                throw new Exception("缺少参数Size");

            var list = new List<string>();
            list.Add(data.row["ArtNo"].ToString());
            list.Add(data.row["Color"].ToString());
            list.Add(data.row["Size"].ToString());

            return string.Join("-", list);
        }
    }
}
