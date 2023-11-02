//#define DEMO

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SRWords
{
    public partial class InitForm : Form
    {
        public InitForm()
        {
            InitializeComponent();
        }

        
        private void InitForm_Load(object sender, EventArgs e)
        {
            #if (!DEMO)
            string login = SerialNum.GetLogin();
            if (String.IsNullOrEmpty(login))
            {
                _loginTextBox.ReadOnly = false;
                _genButton.Visible = true;

                for (int i = 1; i <= 1; i++)
                {
                    int k = _groupBox.Controls.IndexOfKey("_keyTextBox" + i.ToString());
                    (_groupBox.Controls[k] as TextBox).Enabled = false; 
                }
                _checkKeyButton.Enabled = false;
            }
            else
            {
                _loginTextBox.Text = login;
                _loginTextBox.ReadOnly = true;
                _genButton.Visible = false;
                GenCodeToSend();
            }
            #endif
        }

        private void InitForm_Shown(object sender, EventArgs e)
        {
            if (_loginTextBox.ReadOnly)
                _keyTextBox1.Focus();
        }

        private void _genButton_Click(object sender, EventArgs e)
        {
            string t = _loginTextBox.Text;
            if (!(t.Length >= 5 && t.Length <= 10))
            {
                MessageBox.Show("Длина логина должна быть от 5 до 10 символов!", "Внимание!",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _loginTextBox.Focus();
                return;
            }

            if (MessageBox.Show("Сформировать персональный код, используя введённый логин?", "Внимание!",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                GenCodeToSend();
            }
        }

        private void GenCodeToSend()
        {
            #if (!DEMO)
            string t = _loginTextBox.Text;

            try
            {
                Cursor = Cursors.WaitCursor;
                string result;
                int loginLength;
                SerialNum.LoginToHex(t, out result, out loginLength);

                ADSData.MdFiveSetLogin(loginLength, (loginLength + 5).ToString("x"), result);

                string info = SerialNum.RestansDay();
                _codeTextBox.Text = info;
                _loginTextBox.ReadOnly = true;
                _genButton.Visible = false;

                for (int i = 1; i <= 1; i++)
                {
                    int k = _groupBox.Controls.IndexOfKey("_keyTextBox" + i.ToString());
                    (_groupBox.Controls[k] as TextBox).Enabled = true;
                }
                _checkKeyButton.Enabled = true;
                _keyTextBox1.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                Cursor = Cursors.Default;
            }
            #endif
        }

        private void _cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void _loginTextBox_TextChanged(object sender, EventArgs e)
        {
            _codeTextBox.Text = "";
        }

        private void _keyTextBox1_TextChanged(object sender, EventArgs e)
        {
            
            TextBox tb = (sender as TextBox);
            /*
            int j = int.Parse(tb.Name.Substring(tb.Name.Length - 1, 1)) + 1;
            int k = 0;
            if (j < 9)
                k = _groupBox.Controls.IndexOfKey("_keyTextBox" + j.ToString());

            string text = tb.Text;
            text = text.Replace(" ", "");

            if (tb.Text.Length > 4)
            {
                tb.Text = tb.Text.Substring(0, 4);
                if (j < 9)
                {
                    _groupBox.Controls[k].Focus();
                    (_groupBox.Controls[k] as TextBox).Text = text.Substring(4);
                }
                else
                    _checkKeyButton.Focus();
            }
            else if (tb.Text.Length == 4)
            {
                if (j < 9)
                    _groupBox.Controls[k].Focus();
                else
                    _checkKeyButton.Focus();
            }
            */ 
        }

        private void _keyTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            /*
            const string allowStr = "abcdefABCDEF0123456789 "; 
            if (e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Delete)
            {
                if (!allowStr.Contains(e.KeyChar.ToString()))
                    e.Handled = true;
            }
            */ 
        }

        private void _checkKeyButton_Click(object sender, EventArgs e)
        {
            #if (!DEMO)
            string _key = _keyTextBox1.Text.Replace(" ", "").ToLower();

            //for (int i = 1; i <= 8; i++)
            //{
            //    int k = _groupBox.Controls.IndexOfKey("_keyTextBox" + i.ToString());
            //    _key += (_groupBox.Controls[k] as TextBox).Text.Trim().ToLower();
            //}
            //if (_key.Length != 32)
            //{
            //    MessageBox.Show("Вероятно, ключ введён не полностью.", "Внимание!",
            //        MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    _keyTextBox1.Focus();
            //    return;
            //}

            try
            {
                Cursor = Cursors.WaitCursor;
                // Рассчитать ключ
                string computedKey = 
                    SerialNum.MakeKeyByLoginInfo(SerialNum.GetLoginHex(), SerialNum.DensityWeber(), false);

                // Сравнить ключи
                if (!String.Equals(_key, computedKey))
                {
                    MessageBox.Show("Введён неправильный ключ.", "Внимание!",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    _keyTextBox1.Focus();
                    return;
                }
                else
                {
                    try
                    {
                        // Сохранить ключ
                        ADSData.MdFiveSetKey(_key);

                        // Считать и проверить ключ
                        if (!SerialNum.CompareKey())
                        {
                            MessageBox.Show("Ошибка идентификации ключа.", "Внимание!",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        else
                        {
                            MessageBox.Show("Идентификация ключа успешно завершена.", "Внимание!",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Application.Restart();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            finally
            {
                Cursor = Cursors.Default;
            }
            #endif
        }

    }
}