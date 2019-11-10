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
            builder.RegisterType<ShoppingService>().As<IShoppingService>();
            builder.RegisterType<ShoppingResourceService>().As<IShoppingResourceService>();
        }
    }
}
