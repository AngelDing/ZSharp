using System.Collections.Generic;
using ZSharp.Framework.Common;

namespace ZSharp.Framework.Results
{
    public class PagingResult<T> : BasePaging, IPagingResult<T>
    {
        /// <summary>
        /// 初始化分頁結果的新實例
        /// </summary>
        public PagingResult()
        {
        }

        /// <summary>
        /// 初始化分頁結果的新實例
        /// </summary>
        /// <param name="totalCount">符合查詢條件的種記錄數</param>
        /// <param name="entities">分頁查詢結果集</param>
        public PagingResult(int totalCount, IEnumerable<T> datas)
        {
            this.TotalCount = totalCount;
            this.Datas = datas;
        }

        /// <summary>
        /// 初始化分頁結果的新實例
        /// </summary>
        /// <param name="totalCount">符合查詢條件的種記錄數</param>
        /// <param name="entities">分頁查詢結果集</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        public PagingResult(int totalCount, int pageIndex, int pageSize, IEnumerable<T> datas)
        {
            this.TotalCount = totalCount;
            this.Datas = datas;
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
        }

        /// <summary>
        /// 获取或设置分頁查詢結果集
        /// </summary>
        public IEnumerable<T> Datas { get; set; }
    }
}
