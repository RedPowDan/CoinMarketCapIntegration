namespace CryptoUpdaterWorker.Installers
{
    using Autofac;
    using Domain.Services;
    using Domain.Services.Interfaces;

    public class ServicesInstaller : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CryptoService>().As<ICryptoService>().SingleInstance();
            builder.RegisterType<MetadataService>().As<IMetadataService>().SingleInstance();
        }
    }
}