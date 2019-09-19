using System.Threading;
using System.Threading.Tasks;
using Mmu.Mlh.RaspberryPi.Areas.SenseHats.Models;
using Mmu.Mlh.RaspberryPi.Areas.SenseHats.Models.LedMatrixs;
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
            var yellow = new RgbColor(255, 231, 14);
            var black = RgbColor.CreateBlack();

            while (true)
            {
                ReadAndShowTemperature(senseHat, yellow, black);
                Thread.Sleep(10000);
            }
        }

        private static void ReadAndShowTemperature(SenseHat senseHat, RgbColor foreground, RgbColor background)
        {
            var task = Task.Run(async () =>
            {
                var temparature = await senseHat.TemparatureSensor.ReadTemparature();
                await senseHat.LedMatrix.ShowMessage(temparature.AsDescription(), 0.2f, foreground, background);
            });

            Task.WaitAll(task);
        }
    }
}