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
                _loginPanel.Visible = true;
                _keyPanel.Visible = false;
            }
            else
            {
                _loginPanel.Visible = false;
                _keyPanel.Visible = true;
            }
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

                        // TODO: ������� ����� � ����������� � ���������� ��� �� ������� �� ���������. �������, ��� ����������� ���������� ����� ����, ��� ����� ������ �����
                        MessageBox.Show("����������� ������ �������.\n�� �������� ������� �����!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                        Application.Restart();
                    }
                }
            }
        }

        private bool LoginValidate()
        {
            _loginTextBox.Text = _loginTextBox.Text.Trim();
            string t = _loginTextBox.Text;
            if (!(t.Length >= 5 && t.Length <= 10))
            {
                MessageBox.Show("����� ������ ������ ���� �� 5 �� 10 ��������!", "��������!",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _loginTextBox.Focus();
                return false;
            }

            if (!IsAllowedChars(_loginTextBox.Text))
            {
                MessageBox.Show("� ������ ������ ������������ ������������ �������!" + Environment.NewLine +
                    "��������� ��������� �����, ����� � ���� �������������.", "��������!",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _loginTextBox.Focus();
                return false;
            }

            return true;
        }

        private bool EmailValidate()
        {
            _emailTextBox.Text = _emailTextBox.Text.Trim();
            if (String.IsNullOrEmpty(_emailTextBox.Text))
            {
                MessageBox.Show("������� E-mail!", "��������!",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _emailTextBox.Focus();
                return false;
            }

            if (!IsValidEmail(_emailTextBox.Text))
            {
                MessageBox.Show("�������� ������ E-mail!", "��������!",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _emailTextBox.Focus();
                return false;
            }

            return true;
        }

        private bool DataValidate()
        {
            if (!LoginValidate())
                return false;

            if (!EmailValidate())
                return false;

            return true;
        }

        private bool IsAllowedChars(string s)
        {
            foreach (Char c in s)
            {
                if (!Char.IsNumber(c) && c != '_' && !((int)c >= 97 && (int)c <= 122))
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

        private void _loginInfoButton_Click(object sender, EventArgs e)
        {
            _loginTextBox.Focus();
        }

        private void _emailInfoButton_Click(object sender, EventArgs e)
        {
            _emailTextBox.Focus();
        }

        private void _keyInfoButton_Click(object sender, EventArgs e)
        {
            _keyTextBox.Focus();
        }

        private void _checkKeyButton_Click(object sender, EventArgs e)
        {
            _keyTextBox.Text = _keyTextBox.Text.Trim();
            string key = _keyTextBox.Text;
            if (String.IsNullOrEmpty(key))
            {
                MessageBox.Show("������� ����!", "��������!",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _keyTextBox.Focus();
                return;
            }

            key = key.Replace(" ", "").ToLower();

            try
            {
                Cursor = Cursors.WaitCursor;
                // ���������� ����
                string computedKey = SerialNum.MakeKeyByLoginInfo(SerialNum.GetLoginHex(), SerialNum.DensityWeber(), false);

                // �������� �����
                if (!String.Equals(key, computedKey))
                {
                    MessageBox.Show("����� ������������ ����.", "��������!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    _keyTextBox.Focus();
                    return;
                }
                else
                {
                    try
                    {
                        // ��������� ����
                        SerialNum.SaveKey(key);

                        // ������� � ��������� ����
                        if (!SerialNum.CompareKey())
                        {
                            MessageBox.Show("������ �������� �����.", "��������!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        else
                        {
                            // ����� ������� - �������� failCount
                            SerialNum.SaveFails("0");

                            // ������: ��������� donate � �� ��� ������� ��� � �������?
                            // �������� ��, ��� ��������� �� ���������� � �������� ���, � ������ ��� ����������� ����������� donate?
                            // � ���� ������ ���� �������: SerialNum.SaveDonate("1000");
                            // .................................
                            // ������, ��� �������� ������ ����!

                            MessageBox.Show("�������� ����� ������� ���������.", "��������!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Application.Restart();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "������!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            finally
            {
                Cursor = Cursors.Default;
            }

        }

        private void _loginTextBox_Leave(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(_loginTextBox.Text))
                LoginValidate();
        }
    }
}