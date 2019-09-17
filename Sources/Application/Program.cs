using System;
using System.Threading.Tasks;
using System.Timers;
using Mmu.Mlh.RaspberryPi.Areas.SenseHats.Models;
using Mmu.Mlh.RaspberryPi.Areas.SenseHats.Services;
using Mmu.Mlh.ServiceProvisioning.Areas.Initialization.Models;
using Mmu.Mlh.ServiceProvisioning.Areas.Initialization.Services;
using Mmu.Mlh.ServiceProvisioning.Areas.Provisioning.Services;

namespace Mmu.Ptm
{
    public static class Program
    {
        public static void Main()
        {
            var containerConfig = ContainerConfiguration.CreateFromAssembly(typeof(Program).Assembly);
            var serviceLocator = ContainerInitializationService
                .CreateInitializedContainer(containerConfig)
                .GetInstance<IServiceLocator>();

            var senseHatFactory = serviceLocator.GetService<ISenseHatFactory>();
            var senseHat = senseHatFactory.Create();

            using (var timer = new Timer(1000))
            {
                timer.Elapsed += (object sender, ElapsedEventArgs e) => ReadAndShowTemperature(senseHat);
                timer.Start();
                Console.ReadKey();
            }
        }

        private static void ReadAndShowTemperature(SenseHat senseHat)
        {
            var task = Task.Run(async () =>
            {
                var temparature = await senseHat.TemparatureSensor.ReadTemparature();
                await senseHat.LedMatrix.ShowMessage(temparature.AsDescription());
            });

            Task.WaitAll(task);
        }
    }
}