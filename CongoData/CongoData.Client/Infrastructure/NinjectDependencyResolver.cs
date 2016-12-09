using CongoData.DataAccess.Abstract;
using CongoData.DataAccess.Concrete;
using Ninject;
using Ninject.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Dependencies;

namespace CongoData.Client.Infrastructure {
    public class NinjectDependencyScope : IDependencyScope {
        private IResolutionRoot resolver;

        public NinjectDependencyScope(IResolutionRoot resolver) {
            this.resolver = resolver;
        }

        public void Dispose() {
            IDisposable disposable = resolver as IDisposable;

            if (disposable != null) {
                disposable.Dispose();
            }

            resolver = null;
        }

        public object GetService(Type serviceType) {
            CheckForScope();
            return resolver.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType) {
            CheckForScope();
            return resolver.GetAll(serviceType);
        }

        private void CheckForScope() {
            if (resolver == null) {
                throw new ObjectDisposedException("this", "This scope has been disposed");
            }
        }
    }

    public class NinjectDependencyResolver : NinjectDependencyScope, IDependencyResolver {
        private readonly IKernel kernel;

        public NinjectDependencyResolver(IKernel kernel) : base(kernel) {
            this.kernel = kernel;
            AddBindings();
        }

        public IDependencyScope BeginScope() {
            return new NinjectDependencyScope(kernel.BeginBlock());
        }

        private void AddBindings() {
            kernel.Bind<ICongoRepository>().To<EfCongoRepository>();
        }
    }
}