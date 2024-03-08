using Core.Interfaces;
using Hangfire;

namespace WebAPI
{
    public static class JobConfigurator
    {
        public static void AddJobs()
        {
            RemoveExpiredRefreshTokensJob();
        }
        public static void RemoveExpiredRefreshTokensJob()
        {
            RecurringJob.AddOrUpdate<IAccountsService>(
                nameof(RemoveExpiredRefreshTokensJob),
                service => service.RemoveExpiredRefreshTokens(),
                Cron.Weekly);
        }
    }
}
