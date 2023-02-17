using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

namespace PC_GAMING_BAZE.UI
{
    internal class UserItemSelect : TextBlock
    {
        public UserItemSelect()
        {

        }

        public static readonly DependencyProperty IdProp = DependencyProperty.Register("IsSpaceAllowed", typeof(int), typeof(UserItemSelect));

        public int Id
        {
            get
            {
                return (int)base.GetValue(IdProp);
            }
            set
            {
                base.SetValue(IdProp, value);
            }
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            /*base.OnPreviewKeyDown(e);
            if (!Id && (e.Key == Key.Space))
            {
                e.Handled = true;
            }*/
        }

    }
}