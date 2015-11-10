
using BuilderPattern;

namespace BuilderPattern.Equipment
{
    public abstract class EQPBuilder
    {
        protected Equipment m_equipment;
        protected Machine m_machine;

        public EQPBuilder()
        {
            m_equipment = new Equipment();
        }

        public abstract void BuildPort();

        public virtual void BuildMachine(string name)
        {
            m_machine = new Machine(name);
            m_equipment.Name = name;
            m_equipment.Machine = m_machine;
        }

        public virtual Equipment GetEQP()
        {
            return m_equipment;
        }
    }

    public class InputEQPBuilder : EQPBuilder
    {
        public override void BuildPort()
        {
            Port port = new InputPort();
            m_equipment.AddPort(port);
        }
        public override void BuildMachine(string name)
        {
            base.BuildMachine(name);
            m_machine.PortType = "Input";
        }
    }

    public class OutputEQPBuilder : EQPBuilder
    {
        public override void BuildPort()
        {
            Port port = new OutputPort();
            m_equipment.AddPort(port);
        }
        public override void BuildMachine(string name)
        {
            base.BuildMachine(name);
            m_machine.PortType = "Output";
        }
    }

    public class IOPutEQPBuilder : EQPBuilder
    {
        public override void BuildPort()
        {
            Port inputPort = new InputPort();
            m_equipment.AddPort(inputPort);
            Port outputPort = new OutputPort();
            m_equipment.AddPort(outputPort);
        }
        public override void BuildMachine(string name)
        {
            base.BuildMachine(name);
            m_machine.PortType = "InputOutput";
        }
    }

    public static class LCDFactory
    {
        public static Equipment CreateEQP(EQPBuilder buider, string name)
        {
            buider.BuildPort();
            buider.BuildMachine(name);
            return buider.GetEQP();
        }
    }
}
