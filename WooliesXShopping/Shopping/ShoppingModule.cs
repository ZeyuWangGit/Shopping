using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Shopping.Services;

namespace Shopping
{
    public class ShoppingModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TrolleyService>().As<ITrolleyService>();
            builder.RegisterType<ResourceHttpClientService>().As<IResourceHttpClientService>();
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<ProductService>().As<IProductService>();

        }
    }
}
