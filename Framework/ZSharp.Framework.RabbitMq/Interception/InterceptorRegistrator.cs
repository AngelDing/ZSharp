﻿using ZSharp.Framework.Dependency;

namespace ZSharp.Framework.RabbitMq
{
    public interface IInterceptorRegistrator
    {
        void Add(IProduceConsumeInterceptor interceptor);
    }

    public class InterceptorRegistrator : IInterceptorRegistrator
    {
        private readonly CompositeInterceptor compositeInterceptor;
        private readonly IServiceRegister serviceRegister;
    
        public InterceptorRegistrator(IServiceRegister serviceRegister)
        {
            this.serviceRegister = serviceRegister;
            compositeInterceptor = new CompositeInterceptor();
        }

        public IServiceRegister Register()
        {
            serviceRegister.Register<IProduceConsumeInterceptor>(_ => compositeInterceptor);
            return serviceRegister;
        }

        public void Add(IProduceConsumeInterceptor interceptor)
        {
            compositeInterceptor.Add(interceptor);
        }
    }
}