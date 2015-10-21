using ZSharp.Framework.Entities;
using ObjectMapper;
using ZSharp.Framework.Common;

namespace ZSharp.Framework.Infrastructure
{
    /// <summary>
    /// 採用ObjectMapper作為對象映射組件，不採用AutoMap組件，原因如下：
    /// 1.性能比ObjectMapper較差；
    /// 2.AutoMap使用前，需要事先做好對象映射，而ObjectMapper不需要；
    /// 3.使用約定 : 只映射兩個對象間的字段相同的數據；
    /// </summary>
    public static class MappingExtensions
    {
        #region Common Map

        /// <summary>
        /// 將特定對象的數據，映射到另一個的對象上, 目標對象新創建對象 
        /// </summary>
        /// <typeparam name="TTarget">目標對象類型</typeparam>
        /// <param name="source">源對象</param>
        /// <returns>目標對象</returns>
        public static TTarget Map<TTarget>(this object source)
        {
            if (source == null)
            {
                return default(TTarget);
            }
            return ObjectMapper.ObjectMapper.Default.Map<TTarget>(source);
        }

        /// <summary>
        /// 將特定對象的數據，映射到另一個的對象上, 目標對象字段不同的數據會保留
        /// </summary>
        /// <typeparam name="TSource">源對象類型</typeparam>
        /// <typeparam name="TTarget">目標對象類型</typeparam>
        /// <param name="source">源對象</param>
        /// <param name="target">目標對象</param>
        public static void Map<TSource, TTarget>(this TSource source, TTarget target)
        {
            Mapper.Map(source, target);
        }

        #endregion

        #region To DTO

        /// <summary>
        /// 將數據庫實體轉換成相關的模型數據，比如DTO，ViewModel，BizMode等
        /// </summary>
        /// <typeparam name="TEntity">數據庫實體類型</typeparam>
        /// <typeparam name="TDto">模型實體類型</typeparam>
        /// <param name="entity">要轉換的數據庫實體</param>
        /// <returns>模型數據</returns>
        public static TDto ToDto<TEntity, TDto>(this TEntity entity)
            where TDto : class
            where TEntity : IEntity
        {
            return Mapper.Map<TEntity, TDto>(entity);
        }

        /// <summary>
        /// 將數據庫實體轉換成相關的模型數據，比如DTO，ViewModel，BizMode等
        /// </summary>
        /// <typeparam name="TDto">模型實體類型</typeparam>
        /// <param name="entity">要轉換的數據庫實體</param>
        /// <returns>模型數據</returns>
        public static TDto ToDto<TDto>(this IEntity entity)
            where TDto : IDto
        {
            if (entity == null)
            {
                return default(TDto);
            }
            return Map<TDto>(entity);
        }

        #endregion

        #region To Entity

        /// <summary>
        /// 將模型數據，如DTO，ViewModel等轉換成數據庫實體
        /// </summary>
        /// <typeparam name="TModel">模型實體類型</typeparam>
        /// <typeparam name="TEntity">數據庫實體類型</typeparam>
        /// <param name="model">要轉換的模型數據</param>
        /// <returns>數據庫實體</returns>
        public static TEntity ToEntity<TModel, TEntity>(this TModel model)
            where TModel : class
            where TEntity : IEntity
        {
            return Mapper.Map<TModel, TEntity>(model);
        }

        /// <summary>
        /// 將模型數據，如DTO，ViewModel等轉換成數據庫實體
        /// </summary>
        /// <typeparam name="TEntity">數據庫實體類型</typeparam>
        /// <param name="model">要轉換的模型數據</param>
        /// <returns>數據庫實體</returns>
        public static TEntity ToEntity<TEntity>(this IDto model)
            where TEntity : IEntity
        {
            return Map<TEntity>(model);
        }

        #endregion
    }
}
