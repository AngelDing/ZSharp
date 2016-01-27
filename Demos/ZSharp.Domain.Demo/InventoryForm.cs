using System;
using System.Windows.Forms;
using ZSharp.Framework.Infrastructure;
using ZSharp.Framework.Domain;

namespace ZSharp.Domain.Demo
{
    public partial class InventoryForm : Form
    {
        private IReadModelFacade readmodel;
        private ICommandBus commandBus;

        public InventoryForm()
        {
            readmodel = ServiceLocator.GetInstance<IReadModelFacade>();
            commandBus = ServiceLocator.GetInstance<ICommandBus>();
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var name = this.txtName.Text;
            var command = new CreateInventoryItem(Guid.NewGuid(), name);
            commandBus.Send(command);
        }

        private void btnRename_Click(object sender, EventArgs e)
        {
            var id = Guid.NewGuid(); //TODO:获取已经存在的Id
            var newName = this.txtName.Text;
            var command = new RenameInventoryItem(id, newName);
            commandBus.Send(command);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            var id = Guid.NewGuid(); //TODO:获取已经存在的Id
            var number = Convert.ToInt32(this.txtNumber.Text);
            commandBus.Send(new RemoveItemsFromInventory(id, number));
        }

        private void btnCheckIn_Click(object sender, EventArgs e)
        {
            var id = Guid.NewGuid(); //TODO:获取已经存在的Id
            var number = Convert.ToInt32(this.txtNumber.Text);
            commandBus.Send(new CheckInItemsToInventory(id, number));
        }

        private void listShow_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnDeactivate_Click(object sender, EventArgs e)
        {
            var id = Guid.NewGuid(); //TODO:获取已经存在的Id
            commandBus.Send(new DeactivateInventoryItem(id));
        }
    }
}
