using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IILab1
{
    internal class CustomButton: Button
    {
        public int Index = 0;

        Field field = Field.getInstance();
        public CustomButton(int index)
        {
            this.Background = Brushes.White;
            this.Click += ClickEvent;
            Index = index;
        }
        public int ChangeColor()
        {
            if(this.Background != Brushes.White)
            {
                this.Background = Brushes.White;
                return 1;
            }
            else
            {
                this.Background = Brushes.Black;
                return 0;
            }
        }
        new protected void ClickEvent(object sender, RoutedEventArgs e)
        {
            field.OnClick(Index);
        }
    }
}
