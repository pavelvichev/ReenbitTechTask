namespace BlobTask
{
    public class AppSettings
    {
        public string CONFIGURE_STRING_STORAGE { get;  }

        public AppSettings(string config)
        {
            CONFIGURE_STRING_STORAGE = config;
        }
    }
}
