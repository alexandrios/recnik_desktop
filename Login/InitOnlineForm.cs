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
                    String text = "Ваш email: " + _emailTextBox.Text + Environment.NewLine +
                        "Логин: " + _loginTextBox.Text + Environment.NewLine + Environment.NewLine +
                        "Продолжить регистрацию?";
                    if (MessageBox.Show(text, "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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
                        // Записать логин и email в бд
                        SerialNum.SaveLogin(login);
                        SerialNum.SaveEmail(email);
                        SerialNum.SaveDonate("0");
                        SerialNum.SaveFails("0");

                        // TODO: сделать форму с информацией о реквизитах или со ссылкой на реквизиты. Сказать, что регистрация закончится после того, как будет сделан донат
                        MessageBox.Show("Регистрация прошла успешно.\nНе забудьте сделать донат!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
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
                MessageBox.Show("Длина логина должна быть от 5 до 10 символов!", "Внимание!",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _loginTextBox.Focus();
                return false;
            }

            if (!IsAllowedChars(_loginTextBox.Text))
            {
                MessageBox.Show("В строке логина присутствуют недопустимые символы!" + Environment.NewLine +
                    "Разрешены латинские буквы, цифры и знак подчеркивания.", "Внимание!",
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
                MessageBox.Show("Введите E-mail!", "Внимание!",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _emailTextBox.Focus();
                return false;
            }

            if (!IsValidEmail(_emailTextBox.Text))
            {
                MessageBox.Show("Неверный формат E-mail!", "Внимание!",
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
        /// Регистрация нового пользователя.
        /// </summary>
        private bool Registration()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                // Отослать логин, email, персональный код на сервер
                if (!SerialNum.SendRegistration(login, email))
                {
                    return false;
                }

                // Убедиться, что информация записана на сервере в БД
                if (!SerialNum.ReadRegistration(login, email))
                { 
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("Введите ключ!", "Внимание!",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _keyTextBox.Focus();
                return;
            }

            key = key.Replace(" ", "").ToLower();

            try
            {
                Cursor = Cursors.WaitCursor;
                // Рассчитать ключ
                string computedKey = SerialNum.MakeKeyByLoginInfo(SerialNum.GetLoginHex(), SerialNum.DensityWeber(), false);

                // Сравнить ключи
                if (!String.Equals(key, computedKey))
                {
                    MessageBox.Show("Введён неправильный ключ.", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    _keyTextBox.Focus();
                    return;
                }
                else
                {
                    try
                    {
                        // Сохранить ключ
                        SerialNum.SaveKey(key);

                        // Считать и проверить ключ
                        if (!SerialNum.CompareKey())
                        {
                            MessageBox.Show("Ошибка проверки ключа.", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        else
                        {
                            // Ключи совпали - обнулить failCount
                            SerialNum.SaveFails("0");

                            // Вопрос: заполнить donate в бд или считать его с сервера?
                            // Возможно ли, что интернета на компьютере в принципе нет, а значит нет возможности подтвердить donate?
                            // В этом случае надо сделать: SerialNum.SaveDonate("1000");
                            // .................................
                            // Решено, что интернет должен быть!

                            MessageBox.Show("Проверка ключа успешно завершена.", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        }

        private void _loginTextBox_Leave(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(_loginTextBox.Text))
                LoginValidate();
        }
    }
}