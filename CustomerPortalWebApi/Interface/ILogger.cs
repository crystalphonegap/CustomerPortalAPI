using System;

namespace CustomerPortalWebApi.Interface
{
    public interface ILogger
    {
        void Log(Exception exception);

        void LogToDB(string log);
    }
}