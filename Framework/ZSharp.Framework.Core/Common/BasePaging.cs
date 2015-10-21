
namespace ZSharp.Framework.Common
{
    public class BasePaging : IPaging
    {
        public BasePaging()
        {
            PageSize = 20;
        }

        private int pageIndex = 1;

        /// <summary>
        /// 获取或设置分頁頁索引（從1開始）
        /// </summary>
        public int PageIndex
        {
            get
            {
                if (pageIndex == 0)
                {
                    pageIndex = 1;
                }
                return pageIndex;
            }
            set
            {
                pageIndex = value;
            }
        }

        /// <summary>
        /// 获取或设置符合查詢條件的種記錄數
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 获取或设置分頁頁面大小
        /// </summary>
        public int PageSize { get; set; }
    }
}
