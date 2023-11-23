using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace SRWords
{
    /// <summary>
    /// Текстбокс, который распознает нажатие клавиши TAB.
    /// </summary>

    public class MyTextBox : TextBox
    {
        protected override bool IsInputKey(Keys keyData)
        {
            if (keyData == Keys.Tab || keyData == Keys.Up || keyData == Keys.Down
                || keyData == Keys.PageUp || keyData == Keys.PageDown)
            {
                return true;
            }
            else
            {
                return base.IsInputKey(keyData);
            }
        }
    }
}
