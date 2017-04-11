using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using GShine.Data;

namespace GShine.Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string conStr = ConfigurationManager.ConnectionStrings["conStr"].ConnectionString;

        private void button1_Click(object sender, EventArgs e)
        {
            InsertCommand inCommand = new InsertCommand("TestUser", conStr);
            int count = inCommand.AddParameter("birthday", DateTime.Now).AddParameter("go",DateTime.Now).Insert();

            Tips(count, "插入方法一");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DeleteCommand delCommand = new DeleteCommand("testuser", conStr);
            int count = delCommand.LikeWhere(LogicOperator.And, "name", "m", Wildcard.All).Where(LogicOperator.Or, "name", ValueOperator.Equal, "Jack").Delete();
            Tips(count, "按条件");

            //以下使用也可以，即分开调用
            //delCommand.LikeWhere(LogicOperator.And, "name", "x", Wildcard.Back).Where(LogicOperator.Or, "name", ValueOperator.Equal, "fuck");
            //int count = delCommand.Delete();
            //Tips(count, "按条件");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DeleteCommand command = new DeleteCommand("testuser", conStr);
           int count = command.Delete();
           Tips(count, "全部删除");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            UpdateCommand command = new UpdateCommand("testuser", conStr);
            command.Record["Age"].Value = 18;
            command.Record["namE"].Value = "pig";
           int count = command.Where(LogicOperator.And, "name", ValueOperator.Equal, "Jack").NullWhere(LogicOperator.Or,"Birthday",NullValueOperator.Is).Update();
           Tips(count, "更新方法一");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            InsertCommand inCommand = new InsertCommand("TestUser", conStr);
            inCommand.Record["go"].Value = DateTime.Now;
            inCommand.Record["name"].Value = "Jack";
            inCommand.Record["age"].Value = 20;
            inCommand.Record["birthday"].Value = DateTime.Now;
           int count = inCommand.Insert();
           Tips(count, "插入方法二");
        }

        private void Tips(int count,string operation)
        {
            if (count > 0)
            {
                MessageBox.Show("执行" + operation + "成功");
            }
            else
            {
                MessageBox.Show("执行" + operation + "失败");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            InsertCommand command = new InsertCommand("TestUser", conStr);
           int count = command.Insert(new string[] {"name","birthday" }, new object[] {"Xiaoli",DateTime.Now });
           Tips(count, "插入方法三");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            UpdateCommand command = new UpdateCommand("testuser", conStr);
            int count = command.Update(new string[] { "name" }, new object[] { "bug" },"and birthday is null ");

            Tips(count, "更新方法二");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            UpdateCommand command = new UpdateCommand("testuser", conStr);
            int count = command.Update(new string[] { "name" }, new object[] { "bug" }, "or name=@0 ",new object[]{"jack"});

            Tips(count, "更新方法三");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            SelectCommand command = new SelectCommand(conStr);
            DataTable tb;
            tb = command.SelectDataTable("testuser");
         
            dataGridView1.DataSource = tb;

            //测试返回数组，取消注释加断点
            //object[][] rows = command.SelectArray("testuser");
            
        }

        private void button10_Click(object sender, EventArgs e)
        {
            SelectCommand command = new SelectCommand(conStr);
            DataTable tb;
            tb = command.SelectDataTable("testuser",new string[]{ "name", "age"});
            dataGridView1.DataSource = tb;

            //测试返回数组，取消注释加断点
            //object[][] rows = command.SelectArray("testuser", "name", "age");
        }

        private void button11_Click(object sender, EventArgs e)
        {
            SelectCommand command = new SelectCommand(conStr);
            DataTable tb = command.Table("testuser").GroupWrapLeft(LogicOperator.And).Where(LogicOperator.And, "name", ValueOperator.Equal, "jack").NullWhere(LogicOperator.Or,"name",NullValueOperator.Is).GroupWrapRight().SelectDataTable();
            dataGridView1.DataSource = tb;

            //测试返回数组，取消注释加断点
            //object[][] rows = command.Table("testuser").Where(LogicOperator.And, "name", ValueOperator.Equal, "jack").SelectArray();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            SelectCommand command = new SelectCommand(conStr);
            DataTable tb = command.SelectDataTable("select name,age from testuser where name=@0", "jack");
            dataGridView1.DataSource = tb;

            // 测试返回数组，取消注释加断点
            //object[][] rows = command.SelectArray("select name,age from testuser where name=@0", "jack");
        }
    }
}
