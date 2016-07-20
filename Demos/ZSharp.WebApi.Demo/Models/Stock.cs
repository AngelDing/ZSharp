using System.ComponentModel.DataAnnotations;
using ZSharp.WebApi.Demo.Common.DataAnnotations;

namespace ZSharp.WebApi.Demo.Models
{
    /// <summary>
    /// 股票信息
    /// </summary>
    public class Stock
    {
        /// <summary>
        /// 股票代码
        /// </summary>
        [Required]
        public string Symbol { get; set; }

        /// <summary>
        /// 股票名称
        /// </summary>
        [Required]
        [MaxWords(2)]
        public string Name { get; set; }

        /// <summary>
        /// 交易所
        /// </summary>
        [Required]
        public string Exchange { get; set; }
    }
}