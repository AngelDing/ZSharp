
using System;

namespace BuilderPattern.Equipment.Implementation3
{
    public class EQPBuilder
    {
        protected Equipment m_equipment;
        protected Machine m_machine;

        public EQPBuilder()
        {
            m_equipment = new Equipment();
        }

        public virtual void BuildMachine(EquipmentType type)
        {
            var name = type.ToString();
            m_machine = new Machine(name);
            m_equipment.Name = name;
            m_equipment.Machine = m_machine;
        }

        public virtual Equipment GetEQP()
        {
            return m_equipment;
        }

        internal virtual void AddInputPort()
        {
            Port port = new InputPort();
            m_equipment.AddPort(port);
        }

        internal virtual void AddOutputPort()
        {
            Port port = new OutputPort();
            m_equipment.AddPort(port);
        }
    }

    public class SupperEQPBuilder : EQPBuilder
    {
    }


    public static class LCDFactory
    {
        public static Equipment CreateEQP(EQPBuilder builder, EquipmentType type)
        {
            builder.BuildMachine(type);
            switch (type)
            {
                case EquipmentType.InputEQP:
                    builder.AddInputPort();
                    break;
                case EquipmentType.OutputEQP:
                    builder.AddOutputPort();
                    break;
                case EquipmentType.IOPutEQP:
                    builder.AddInputPort();
                    builder.AddOutputPort();
                    break;
                default:
                    return null;
            }
            return builder.GetEQP();
        }
    }   
}
