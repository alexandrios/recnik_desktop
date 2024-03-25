using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;

namespace SRWords
{
    public partial class InitOnlineForm : Form
    {
        public InitOnlineForm()
        {
            InitializeComponent();
        }

        
        private void InitOnlineForm_Load(object sender, EventArgs e)
        {
            /*
            #if (!DEMO)
            string login = SerialNum.GetLogin();
            if (String.IsNullOrEmpty(login))
            {
                _loginTextBox.ReadOnly = false;
                _genButton.Visible = true;
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
            */
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
                String text = "Ваш email: " + _emailTextBox.Text + Environment.NewLine +
                    "Логин: " + _loginTextBox.Text + Environment.NewLine + Environment.NewLine +
                    "Продолжить регистрацию?";
                if (MessageBox.Show(text, "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    GeneratePersonalCode();
                }
            }
        }

        private bool DataValidate()
        {
            _emailTextBox.Text = _emailTextBox.Text.Trim();
            if (String.IsNullOrEmpty(_emailTextBox.Text))
            {
                MessageBox.Show("Введите Ваш email!", "Внимание!",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _emailTextBox.Focus();
                return false;
            }
            if (!IsValidEmail(_emailTextBox.Text))
            {
                MessageBox.Show("Неверный формат email!", "Внимание!",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _emailTextBox.Focus();
                return false;
            }

            _loginTextBox.Text = _loginTextBox.Text.Trim();
            string t = _loginTextBox.Text;
            if (!(t.Length >= 5 && t.Length <= 10))
            {
                MessageBox.Show("Длина логина должна быть от 5 до 10 символов!", "Внимание!",
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
        /// Формирование персонального кода.
        /// </summary>
        private void GeneratePersonalCode()
        {
            #if (!DEMO)
            try
            {
                Cursor = Cursors.WaitCursor;
                string result;
                int loginLength;

                SerialNum.LoginToHex(_loginTextBox.Text, out result, out loginLength);

                // Сохранение логина
                ADSData.MdFiveSetLogin(loginLength, (loginLength + 5).ToString("x"), result);

                // Персональный код (отсылается для генерации ключа на сервере)
                string personalCode = SerialNum.RestansDay();
                _codeTextBox.Text = personalCode;

                // Рассчитать ключ
                string computedKey = SerialNum.MakeKeyByLoginInfo(SerialNum.GetLoginHex(), SerialNum.DensityWeber(), false);
                _keyTextBox.Text = computedKey;

                // Проверить интернет
                if (!ConnectionAvailable("http://google.com"))
                {
                    MessageBox.Show("Проверьте соединение с интернетом!");
                    return;
                }


                // Отослать логин, email, персональный код на сервер
                string parameters = String.Format("login={0}&email={1}&perscode={2}", _loginTextBox.Text, _emailTextBox.Text, personalCode);
                ScanWord.Rest rest = new ScanWord.Rest();
                string response = rest.InsertRegistrInfo(parameters);
                if (response != "OK")
                {
                    MessageBox.Show(response, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // Убедиться, что информация записана на сервере в БД
                parameters = String.Format("login={0}&email={1}&perscode={2}", _loginTextBox.Text, _emailTextBox.Text, personalCode);
                List<ScanWord.UserDonation> donation = rest.GetUserDonation(parameters);
                if (donation.Count == 0)
                {
                    // запись не найдена на сервере
                }
                else
                {
                    DateTime date = DateTime.ParseExact(donation[0].date, "yyyy-MM-dd HH:mm:ss",
                                       System.Globalization.CultureInfo.InvariantCulture);
                    int donate = int.Parse(donation[0].donate);
                    string note = donation[0].note;

                    int interval = Convert.ToInt32(Math.Floor((DateTime.Now - date).TotalDays));
                    
                }


                // Сохранить ключ
                ADSData.MdFiveSetKey(computedKey);

                // Считать и проверить ключ
                if (SerialNum.CompareKey())
                {
                    Application.Restart();
                }
                else
                {
                    MessageBox.Show("Ошибка идентификации ключа.", "Внимание!",
                                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
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

        /*
                string code = SerialNum.Stores(_codeTextBox.Text);
                string loginHex;
                int loginLength;
                SerialNum.LoginToHex(_loginTextBox.Text, out loginHex, out loginLength);
                    string computedKey = SerialNum.MakeKeyByLoginInfo(loginHex, code, true);
                _keyTextBox.Text = computedKey;

                                string outerKey = computedKey;
                        try
                        {
                            // Сохранить ключ
                            ADSData.MdFiveSetKey(outerKey);

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
        */


        public bool ConnectionAvailable(string strServer)
        {
            try
            {
                HttpWebRequest reqFP = (HttpWebRequest)HttpWebRequest.Create(strServer);
                HttpWebResponse rspFP = (HttpWebResponse)reqFP.GetResponse();
                if (HttpStatusCode.OK == rspFP.StatusCode)
                {
                    // HTTP = 200 - Интернет безусловно есть! 
                    rspFP.Close();
                    return true;
                }
                else
                {
                    // сервер вернул отрицательный ответ, инета нет
                    rspFP.Close();
                    return false;
                }
            }
            catch (WebException)
            {
                // Ошибка, интернета у нас нет.
                return false;
            }
        }
    }
}