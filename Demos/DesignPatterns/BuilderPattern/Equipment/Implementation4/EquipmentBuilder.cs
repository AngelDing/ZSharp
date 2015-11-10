

using System.Collections.Generic;

namespace BuilderPattern.Equipment.Implementation4
{
    public class EquipmentBuilder : BaseBuilder<Equipment>
    {
        private Machine m_machine;

        internal void BuildMachine(EquipmentType type)
        {
            var name = type.ToString();
            m_machine = new Machine(name);
            this.Product.Name = name;
            this.Product.Machine = m_machine;
        }

        internal virtual void AddInputPort()
        {
            Port port = new InputPort();
            this.Product.AddPort(port);
        }

        internal virtual void AddOutputPort()
        {
            Port port = new OutputPort();
            this.Product.AddPort(port);
        }
    }

    public class LCDDirector : BuildDirector<Equipment>
    {
        public void ConstructByType(EquipmentBuilder builder, EquipmentType type)
        {
            builder.BuildMachine(type);
            switch (type)
            {
                case EquipmentType.InputEQP:
                    builder.AddSteps(new List<BuildStepHandler> { builder.AddInputPort });
                    break;
                case EquipmentType.OutputEQP:
                    builder.AddSteps(new List<BuildStepHandler> { builder.AddOutputPort });
                    break;
                case EquipmentType.IOPutEQP:
                    var steps = new List<BuildStepHandler>
                    {
                        builder.AddInputPort,
                        builder.AddOutputPort
                    };
                    builder.AddSteps(steps);
                    break;
                default:
                    break;
            }

            base.Construct(builder);
        }

        public static Equipment CreateEQP(EquipmentBuilder builder, EquipmentType type)
        {
            var director =  new LCDDirector();
            director.ConstructByType(builder, type);
            return builder.GetResult();
        }
    }
}
