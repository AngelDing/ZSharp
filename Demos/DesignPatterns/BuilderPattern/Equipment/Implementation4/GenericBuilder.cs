using System.Collections.Generic;

namespace BuilderPattern.Equipment.Implementation4
{
    public delegate void BuildStepHandler();

    public interface IBuilder<T> where T : class
    {
        void AddSteps(IEnumerable<BuildStepHandler> setps);

        void BuildUp();

        T GetResult();

        void TearDown();
    }

    public abstract class BaseBuilder<T> : IBuilder<T> where T : class, new()
    {
        private List<BuildStepHandler> stepHandlers = new List<BuildStepHandler>();

        /// <summary>
        /// 此為通用意義上的待構造的對象，不是電商系統中的具體商品
        /// </summary>
        protected T Product { get; private set; }

        public BaseBuilder()
            : this(null)
        {
        }

        public BaseBuilder(T product)
        {
            this.Product = product;
            if (this.Product == null)
            {
                this.Product = new T();
            }
        }

        public virtual void AddSteps(IEnumerable<BuildStepHandler> setps)
        {
            stepHandlers.AddRange(setps);
        }

        public virtual void BuildUp()
        {
            foreach (BuildStepHandler step in stepHandlers)
            {
                step();
            }
        }

        public virtual T GetResult()
        {
            return Product;
        }

        public virtual void TearDown()
        {
        }
    }

    public class BuildDirector<T> where T : class
    {
        public virtual void Construct(IBuilder<T> builder)
        {
            builder.BuildUp();
        }
    }
}
