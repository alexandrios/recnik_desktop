using System;
using System.Windows.Forms;

namespace SRWords
{
    public partial class InitOnlineForm : Form
    {
        public enum InitType
        { 
            LOGIN,
            KEY
        }

        private bool confirm = false;
        private string login = "";
        private string email = "";

        public InitOnlineForm(InitType initType)
        {
            InitializeComponent();

            if (initType == InitType.LOGIN)
            {
                _loginGroupBox.Visible = true;
                _keyGroupBox.Visible = false;
            }
            else
            {
                _loginGroupBox.Visible = false;
                _keyGroupBox.Visible = true;
            }
        }
                
        private void InitOnlineForm_Load(object sender, EventArgs e)
        {
        }

        private void InitForm_Shown(object sender, EventArgs e)
        {
            if (_loginTextBox.ReadOnly)
                _keyTextBox.Focus();
        }

        private void _cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void _registrationButton_Click(object sender, EventArgs e)
        {
            if (DataValidate())
            {
                bool goReg = true;
                if (!confirm || _emailTextBox.Text != email || _loginTextBox.Text != login)
                {
                    String text = "��� email: " + _emailTextBox.Text + Environment.NewLine +
                        "�����: " + _loginTextBox.Text + Environment.NewLine + Environment.NewLine +
                        "���������� �����������?";
                    if (MessageBox.Show(text, "��������!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        confirm = true;
                        email = _emailTextBox.Text;
                        login = _loginTextBox.Text;
                    }
                    else
                    {
                        goReg = false;
                    }
                }

                if (goReg)
                {
                    if (Registration())
                    {
                        // �������� ����� � email � ��
                        SerialNum.SaveLogin(login);
                        SerialNum.SaveEmail(email);
                        SerialNum.SaveDonate("0");
                        SerialNum.SaveFails("0");

                        MessageBox.Show("����������� ������ �������.\n�� �������� ������� �����!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Application.Restart();
                    }
                }
            }
        }

        private bool DataValidate()
        {
            _emailTextBox.Text = _emailTextBox.Text.Trim();
            if (String.IsNullOrEmpty(_emailTextBox.Text))
            {
                MessageBox.Show("������� ��� email!", "��������!",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _emailTextBox.Focus();
                return false;
            }
            if (!IsValidEmail(_emailTextBox.Text))
            {
                MessageBox.Show("�������� ������ email!", "��������!",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _emailTextBox.Focus();
                return false;
            }

            _loginTextBox.Text = _loginTextBox.Text.Trim();
            string t = _loginTextBox.Text;
            if (!(t.Length >= 5 && t.Length <= 10))
            {
                MessageBox.Show("����� ������ ������ ���� �� 5 �� 10 ��������!", "��������!",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _loginTextBox.Focus();
                return false;
            }

            return true;
        }

        private bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// ����������� ������ ������������.
        /// </summary>
        private bool Registration()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                // �������� �����, email, ������������ ��� �� ������
                if (!SerialNum.SendRegistration(login, email))
                {
                    return false;
                }

                // ���������, ��� ���������� �������� �� ������� � ��
                if (!SerialNum.ReadRegistration(login, email))
                { 
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "������!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                Cursor = Cursors.Default;
            }

            return true;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            //SerialNum.SaveDonate(textBox1.Text);
            SerialNum.SaveFails(textBox1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //label1.Text = SerialNum.GetDonate();
            label1.Text = SerialNum.GetFails();
        }

        private void _checkKeyButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("�������� �����");
        }
    }
}