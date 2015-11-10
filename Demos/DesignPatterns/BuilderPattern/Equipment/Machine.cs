using System;

namespace BuilderPattern.Equipment
{
    public class Machine
    {
        private string m_name;
        private string m_portType;

        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        public string PortType
        {
            get { return m_portType; }
            set { m_portType = value; }
        }

        public Machine(string name)
        {
            m_name = name;
        }

        public void Run()
        {
            Console.WriteLine("The machine {0} is running!", m_name);
        }
    }
}
