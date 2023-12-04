using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace IILab1
{
    internal class Field:UniformGrid
    {
        public static int sizeOfField = 4;
        public static int numberOfCells = sizeOfField * sizeOfField;
        public CustomButton [] _buttons = new CustomButton[Field.numberOfCells];
        public FieldState fieldState = new FieldState();
        
        // 0 1 2 3
        // 4 5 6 7
        // 8 9 10 11
        // 12 13 14 15

        private static Field instance;
        private Field() 
        {

        }
        public void CreateButtons()
        {
            for (int i = 0; i < Field.numberOfCells; i++)
            {
                var button = new CustomButton(i);
                //???????????????????????????????????????????????///
                button.Background = Brushes.White;
                _buttons[i] = button;
                instance.Children.Add(button);
                instance.fieldState.cells[i] = 1;
            }
        }
        public static Field getInstance()
        {
            if (instance == null)
                instance = new Field();
            return instance;
        }
        public void Clear()
        {
            fieldState = new FieldState();
        }
        public void OnClick(int index)
        {
           //При нажатии вручную
            fieldState.cells[index] = _buttons[index].ChangeColor();
            if ((index-Field.sizeOfField)>=0)
            {
                fieldState.cells[index - Field.sizeOfField] =_buttons[index - Field.sizeOfField].ChangeColor();
            }
            if((index+ Field.sizeOfField) <Field.numberOfCells)
            {
                fieldState.cells[index + Field.sizeOfField] =_buttons[index + Field.sizeOfField].ChangeColor();
            }
            if((index-1)% Field.sizeOfField != Field.sizeOfField - 1 && (index - 1)>=0)
            {
                fieldState.cells[index - 1] = _buttons[index - 1].ChangeColor();
            }
            if((index+1) % Field.sizeOfField != 0)
            {
                fieldState.cells[index + 1] = _buttons[index + 1].ChangeColor();
            }

            fieldState.id = "";
            //Создаем id
            for (int i = 0; i < fieldState.cells.Length; i++)
            {
                fieldState.id += fieldState.cells[i].ToString();
            }
        }
        public void CreateRandomField()
        {
            fieldState.id = "";
            Random random = new Random();
            for (int i = 0; i < Field.numberOfCells; i++)
            {
                var indexClick = random.Next(0, Field.numberOfCells);
                this.OnClick(indexClick);
            }
        }
        public void CreateRandomField(int n)
        {
            fieldState.id = "";
            Random random = new Random();
            if (n<=6)
            {
                for (int i = 0; i < n; i++)
                {
                    var indexClick = random.Next(0, Field.numberOfCells);
                    this.OnClick(indexClick);
                }
            }
            else
            {
                for (int i = 0; i < Field.numberOfCells; i++)
                {
                    var indexClick = random.Next(0, Field.numberOfCells);
                    this.OnClick(indexClick);
                }
            }
            
        }
        public void Render()
        {
            for(int i = 0; i < fieldState.cells.Length;i++)
            {
                if (fieldState.cells[i] == 1)
                {
                    instance._buttons[i].Background = Brushes.White;
                }
                else
                {
                    instance._buttons[i].Background = Brushes.Black;
                }
            }
        }
    }
}
