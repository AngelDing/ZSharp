using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ZSharp.Framework.Configurations
{
    /// <summary>
    /// Setting service interface
    /// </summary>
    public partial interface ISettingService
    {
        /// <summary>
        /// Gets a setting by identifier
        /// </summary>
        /// <param name="settingId">Setting identifier</param>
        /// <returns>Setting</returns>
        ISettingEntity GetSettingById(int settingId);

        /// <summary>
        /// Get setting value by key
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">Key</param>
        /// <param name="defaultValue">Default value</param>		
        /// <returns>Setting value</returns>
		T GetSettingByKey<T>(string key, T defaultValue = default(T));

		/// <summary>
		/// Gets all settings
		/// </summary>
		/// <returns>Settings</returns>
		IList<ISettingEntity> GetAllSettings();

		/// <summary>
		/// Determines whether a setting exists
		/// </summary>
		/// <typeparam name="T">Entity type</typeparam>
		/// <typeparam name="TPropType">Property type</typeparam>
		/// <param name="settings">Settings</param>
		/// <param name="keySelector">Key selector</param>
		/// <returns>true -setting exists; false - does not exist</returns>
		bool SettingExists<T, TPropType>(T settings,Expression<Func<T, TPropType>> keySelector)
			where T : ISettings, new();

		/// <summary>
		/// Load settings
		/// </summary>
		/// <typeparam name="T">Type</typeparam>
		T LoadSetting<T>() where T : ISettings, new();

        /// <summary>
        /// Set setting value
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="clearCache">A value indicating whether to clear cache after setting update</param>
        void SetSetting<T>(string key, T value, bool clearCache = true);

        /// <summary>
        /// Save settings object
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
		/// <param name="settings">Setting instance</param>
		void SaveSetting<T>(T settings) where T : ISettings, new();

		/// <summary>
		/// Save settings object
		/// </summary>
		/// <typeparam name="T">Entity type</typeparam>
		/// <typeparam name="TPropType">Property type</typeparam>
		/// <param name="settings">Settings</param>
		/// <param name="keySelector">Key selector</param>
		/// <param name="clearCache">A value indicating whether to clear cache after setting update</param>
		void SaveSetting<T, TPropType>(T settings, Expression<Func<T, TPropType>> keySelector, bool clearCache = true) 
            where T : ISettings, new();

		void UpdateSetting<T, TPropType>(T settings, Expression<Func<T, TPropType>> keySelector) 
            where T : ISettings, new();

		void InsertSetting(ISettingEntity setting, bool clearCache = true);

		void UpdateSetting(ISettingEntity setting, bool clearCache = true);

		/// <summary>
		/// Deletes a setting
		/// </summary>
		/// <param name="setting">Setting</param>
		void DeleteSetting(ISettingEntity setting);

        /// <summary>
        /// Delete all settings
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        void DeleteSetting<T>() where T : ISettings, new();

		/// <summary>
		/// Delete settings object
		/// </summary>
		/// <typeparam name="T">Entity type</typeparam>
		/// <typeparam name="TPropType">Property type</typeparam>
		/// <param name="settings">Settings</param>
		/// <param name="keySelector">Key selector</param>
		void DeleteSetting<T, TPropType>(T settings, Expression<Func<T, TPropType>> keySelector) 
            where T : ISettings, new();

		void DeleteSetting(string key);

		/// <summary>
		/// Deletes all settings with its key beginning with rootKey.
		/// </summary>
		/// <remarks>codehint: sm-add</remarks>
		/// <returns>Number of deleted settings</returns>
		int DeleteSettings(string rootKey);

        /// <summary>
        /// Clear cache
        /// </summary>
        void ClearCache();
    }
}
