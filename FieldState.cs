using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace IILab1
{
    class FieldState
    {
        public int [] cells = new int[Field.numberOfCells];
        public string id = "1";
        public string parent = "0";
        public int value;
        public int depth=0;
        public FieldState() 
        {
            id = new string('0', Field.numberOfCells);
            depth = 0;
        }
        public FieldState(string pID, int index,int depth=0)
        {
            id = "";
            this.depth = depth;
            for (int i = 0; i < this.cells.Length; i++)
            {
                if (pID[i].ToString() == "0") this.cells[i] = 0;
                else this.cells[i] = 1;
            }
            parent = pID;
            
            this.cells[index] = (this.cells[index]+1)%2;
            //Верх клик
            if ((index - Field.sizeOfField) >= 0)
            {
                this.cells[index - Field.sizeOfField] = (this.cells[index- Field.sizeOfField] + 1) % 2;
            }
            //Низ клик
            if ((index + Field.sizeOfField) < Field.numberOfCells)
            {
                this.cells[index + Field.sizeOfField] = (this.cells[index+ Field.sizeOfField] + 1) % 2;
            }
            //Лево клик
            if ((index - 1) % Field.sizeOfField != Field.sizeOfField - 1 && (index - 1) >= 0)
            {
                this.cells[index - 1] = (this.cells[index-1] + 1) % 2;
            }
            //Право клик
            if ((index + 1) % Field.sizeOfField != 0)
            {
                this.cells[index + 1] = (this.cells[index+1] + 1) % 2;
            }
            //Создаем id
            for (int i = 0; i < this.cells.Length; i++)
            {
                id += this.cells[i].ToString();
            }
        }
        static public bool IsSollution(FieldState fs)
        {
            for (int i = 0; i < Field.numberOfCells; i++)
            {
                if (fs.cells[i] != 1)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
