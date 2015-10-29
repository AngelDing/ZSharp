namespace ZSharp.Framework.Logging.Configuration
{
    public  interface IConfigurationReader
    {
        object GetSection(string sectionName);
    }
}
 