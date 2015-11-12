using Common;
using System;
using System.Collections.Generic;

namespace BuilderPattern.Equipment.Implementation4
{
    public class EquipmentBuilder : BaseBuilder<Equipment>
    {
        private Machine m_machine;
        internal EquipmentType EquipmentType { get; private set; }

        public EquipmentBuilder(EquipmentType type)
        {
            this.EquipmentType = type;
            BuildMachine();
        }

        private void BuildMachine()
        {
            var name = EquipmentType.ToString();
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
        public override void Construct(IBuilder<Equipment> builder)
        {
            var eqpBuilder = builder as EquipmentBuilder;
            switch (eqpBuilder.EquipmentType)
            {
                case EquipmentType.InputEQP:
                    eqpBuilder.AddSteps(new List<Action> { eqpBuilder.AddInputPort });
                    break;
                case EquipmentType.OutputEQP:
                    eqpBuilder.AddSteps(new List<Action> { eqpBuilder.AddOutputPort });
                    break;
                case EquipmentType.IOPutEQP:
                    var steps = new List<Action>
                    {
                        eqpBuilder.AddInputPort,
                        eqpBuilder.AddOutputPort
                    };
                    eqpBuilder.AddSteps(steps);
                    break;
                default:
                    break;
            }

            base.Construct(eqpBuilder);
        }

        public static Equipment CreateEQP(EquipmentBuilder builder)
        {
            var director =  new LCDDirector();
            director.Construct(builder);
            return builder.GetResult();
        }
    }
}
