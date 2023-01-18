using Polly;

namespace Coupon.API.Middlewares
{
    public static class WebHostExtensions
    {
        public static IWebHost SeedDatabase<TContext>(this IWebHost host, Action<TContext> seeder)
        {
            using (var scope = host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<TContext>();

                var policy = Policy.Handle<Exception>()
                    .WaitAndRetry(new TimeSpan[]
                    {
                        TimeSpan.FromSeconds(3),
                        TimeSpan.FromSeconds(5),
                        TimeSpan.FromSeconds(8),
                    });

                policy.Execute(() =>
                {
                    seeder.Invoke(context);
                });
            }

            return host;
        }
    }
}
