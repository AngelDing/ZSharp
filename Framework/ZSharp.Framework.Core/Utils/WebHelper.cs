using System.Web;

namespace ZSharp.Framework.Utils
{
    public partial class WebHelper
    {
        private static AspNetHostingPermissionLevel? s_trustLevel = null;

        /// <summary>
		/// Finds the trust level of the running application
        /// http://blogs.msdn.com/dmitryr/archive/2007/01/23/finding-out-the-current-trust-level-in-asp-net.aspx
		/// </summary>
		/// <returns>The current trust level.</returns>
		public static AspNetHostingPermissionLevel GetTrustLevel()
		{
			if (!s_trustLevel.HasValue)
			{
				//set minimum
				s_trustLevel = AspNetHostingPermissionLevel.None;

				//determine maximum
				foreach (AspNetHostingPermissionLevel trustLevel in
						new AspNetHostingPermissionLevel[] {
                                AspNetHostingPermissionLevel.Unrestricted,
                                AspNetHostingPermissionLevel.High,
                                AspNetHostingPermissionLevel.Medium,
                                AspNetHostingPermissionLevel.Low,
                                AspNetHostingPermissionLevel.Minimal 
                            })
				{
					try
					{
						new AspNetHostingPermission(trustLevel).Demand();
						s_trustLevel = trustLevel;
						break; //we've set the highest permission we can
					}
					catch (System.Security.SecurityException)
					{
						continue;
					}
				}
			}
			return s_trustLevel.Value;
		}
    }
}
