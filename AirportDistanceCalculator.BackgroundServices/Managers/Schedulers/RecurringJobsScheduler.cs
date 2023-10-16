using AirportDistanceCalculator.BackgroundServices.Managers.RecurringJobs;
using Hangfire;

namespace AirportDistanceCalculator.BackgroundServices.Managers.Schedulers
{
    public class RecurringJobsScheduler
    {
        public static void CacheAirportJob()
        {
            RecurringJob.RemoveIfExists(nameof(CacheAirportsJobManager));
            RecurringJob.AddOrUpdate<CacheAirportsJobManager>(nameof(CacheAirportsJobManager),
                job => job.Perform(),
                "0 19 * * *",
                new RecurringJobOptions { TimeZone = TimeZoneInfo.Local }
                );
                
        }

    }
}
