using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Runtime.Serialization;
using KSR_WCF1;

namespace WCF_LAB1_2
{

    [ServiceContract]
    public interface IZadanie7
    {
        [OperationContract]
        [FaultContract(typeof(Wyjatek7))]
        void RzucWyjatek7(string a, int b);

        
    }

    [DataContract]
    public class Wyjatek7 
    {
        public Wyjatek7(string opis, string a, int b)
        {
            this.opis = opis;
            this.a = a;
            this.b = b;
        }

        [DataMember]
        public string opis { get; set; }
        [DataMember]
        public string a { get; set; }
        [DataMember]
        public int b { get; set; }
    }

    public class Zadanie2 : IZadanie2, IZadanie7
    {
        public string Test(string arg)
        {
            return "Testowanie zadania: " + arg;
        }

        public void RzucWyjatek7(string a, int b)
        {
            var info = new Wyjatek7("Wyjatek zadanie7", a, b);
            throw new FaultException<Wyjatek7>(info);
        }
    }


    internal class Program
    {
        static void Main(string[] args)
        {
            var host = new ServiceHost(typeof(Zadanie2));

            var b = host.Description.Behaviors.Find<ServiceMetadataBehavior>();
            if (b == null) b = new ServiceMetadataBehavior();
            host.Description.Behaviors.Add(b);

            host.AddServiceEndpoint(
                ServiceMetadataBehavior.MexContractName, 
                MetadataExchangeBindings.CreateMexNamedPipeBinding(),
                "net.pipe://localhost/metadane");

            host.AddServiceEndpoint(
                typeof(IZadanie2),
                new NetNamedPipeBinding(),
                "net.pipe://localhost/ksr-wcf1-zad2");

            host.AddServiceEndpoint(
                typeof(IZadanie2),
                new NetTcpBinding(),
                "net.tcp://127.0.0.1:55765");

            host.AddServiceEndpoint(
                typeof(IZadanie7),
               new NetNamedPipeBinding(),
                "net.pipe://localhost/ksr-wcf1-zad7");

            host.Open();

            Console.ReadKey();

            host.Close();
        }
    }
}
