using System;
using System.ServiceModel;
using KSR_WCF1;
using WCF_LAB1_1.ServiceReference1;

namespace WCF_LAB1_1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var factory = new ChannelFactory<IZadanie1>(
                new NetNamedPipeBinding(),
                new EndpointAddress("net.pipe://localhost/ksr-wcf1-test"));

            var channel = factory.CreateChannel();

            Console.WriteLine(channel.Test("WCF zadanie 1"));

            try
            {
                channel.RzucWyjatek(true);
            }
            catch (FaultException<KSR_WCF1.Wyjatek> e)
            {
                channel.OtoMagia(e.Detail.magia);
            }
            
            ((IDisposable)channel).Dispose();

            factory.Close();


            var channel2 = new ServiceReference1.Zadanie7Client("NetNamedPipeBinding_IZadanie7");

            try
            {
                channel2.RzucWyjatek7("test", 123);
            }
            catch (FaultException<Wyjatek7> e)
            {
                Console.WriteLine($"Wyjatek zadanie 7: {e.Detail.opis}, {e.Detail.a}, {e.Detail.b}");
            }
        }
    }
}
