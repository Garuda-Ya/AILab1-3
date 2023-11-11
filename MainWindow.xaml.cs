
using System;
using System.Collections;
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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using IILab1;

namespace IILab1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        int choosenHeuristic = 0;
        //View
        Field GameField = Field.getInstance();
        int stateInd = 0;
        List<FieldState> solution = new List<FieldState>();
        //counters
        int cycles = 0;
        int maxO = 0;
        int maxC = 0;
        //States
        FieldState currentState;
        FieldState currentState2;
        public MainWindow()
        {
            InitializeComponent();
            AlgoritmSelect.SelectedIndex = 0;
            GameField.CreateButtons();
            MainGrid.Children.Add(GameField);
        }
        private void NewLayout_Click(object sender, RoutedEventArgs e)
        {
            GameField.CreateRandomField();
        }
        private void StartGame_Click(object sender, RoutedEventArgs e)
        {
            if(AlgoritmSelect.SelectedValue.ToString()== "Поиск в глубину")
            {
                DepthSearch();
            }
            else if(AlgoritmSelect.SelectedValue.ToString() == "Поиск в ширину")
            {
                BreadthSearch();
            }
            else if (AlgoritmSelect.SelectedValue.ToString() == "Поиск в глубину с ограничением")
            {
               IDDFS();
            }
            else if (AlgoritmSelect.SelectedValue.ToString() == "Поиск в ширину двунаправленный")
            {
                BreadthTwoWaySearch();
            }
            else if (AlgoritmSelect.SelectedValue.ToString() == "Эвристика1")
            {
                choosenHeuristic = 0;
                solutionA();
            }
            else if (AlgoritmSelect.SelectedValue.ToString() == "Эвристика2")
            {
                choosenHeuristic = 1;
                solutionA();
            }
            else if (AlgoritmSelect.SelectedValue.ToString() == "Эвристика3")
            {
                choosenHeuristic = 2;
                solutionA();
            }
            else if (AlgoritmSelect.SelectedValue.ToString() == "Эвристика4")
            {
                choosenHeuristic = 3;
                solutionA();
            }

        }
        private void BreadthSearch()
        {
            //Счетчики
            cycles = 0;
            maxO = 0;
            maxC = 0;
            //Ф-ция поиска решения
            //Очередь для поиска в ширину и Словарь для списка О
            Queue<FieldState> listO = new Queue<FieldState>();
            //Dictionary<string,FieldState> listC = new Dictionary<string,FieldState>();
            List<FieldState> listC = new List<FieldState>();
            FieldState fs = new FieldState();

            fs.id = "";
            for (int i = 0; i < GameField.fieldState.cells.Length; i++)
            {
                fs.cells[i] = GameField.fieldState.cells[i];
                if (GameField.fieldState.cells[i] == 0)
                {
                    fs.cells[i] = 0;
                    fs.id += "0";
                }
                else
                {
                    fs.cells[i] = 1;
                    fs.id += "1";
                }
            }
            fs.parent = "0";

            listO.Enqueue(fs);

            currentState = fs;

            while (listO.Count > 0)
            {
                cycles++;
                //Взяли первый элемент
                currentState = listO.Dequeue();
                //Проверка является ли решением?
                if (FieldState.IsSollution(currentState))
                {
                    //Нашли решение
                    break;
                }

                //Добавляем в список неверных решений
                //listC.Add(currentState.id,currentState);
                listC.Add(currentState);
                
                //Добавляем все возможные варианты (Потомков)
                for (int i = 0; i < currentState.cells.Length; i++)
                {
                    FieldState newFieldState = new FieldState(currentState.id, i);
                    //Почему-то не так работает 
                    if (!listC.Any(o => o.id == newFieldState.id) && !listO.Any(o => o.id == newFieldState.id)) //Моя ф-ция
                    {
                        listO.Enqueue(newFieldState);
                    }

                }
                if (listO.Count > maxO) maxO = listO.Count;
                if (listO.Count + listC.Count > maxC) maxC = listO.Count + listC.Count;
            }
            //Очищаем решение и записываем новое
            solution.Clear();
            while (currentState.id != GameField.fieldState.id)
            {
                solution.Add(currentState);
                currentState = GetFatherFromList(listC, currentState);
            }
            solution.Add(currentState);
            solution.Reverse();

            MessageBox.Show("Решение Найдено");
            GameField.fieldState.cells = solution[0].cells;
            GameField.Render();
            SetCounters();
        }
        private void DepthSearch()
        {
            //Счетчики
            cycles = 0;
            maxO = 0;
            maxC = 0;
            //Ф-ция поиска решения
            //Очередь для поиска в ширину и Словарь для списка О
            Stack<FieldState> listO = new Stack<FieldState>();
            List<FieldState> listC = new List<FieldState>();
            FieldState fs = new FieldState();

            fs.id = "";
            for (int i = 0; i < GameField.fieldState.cells.Length; i++)
            {
                fs.cells[i] = GameField.fieldState.cells[i];
                if (GameField.fieldState.cells[i] == 0)
                {
                    fs.cells[i] = 0;
                    fs.id += "0";
                }
                else
                {
                    fs.cells[i] = 1;
                    fs.id += "1";
                }
            }
            fs.parent = "0";

            listO.Push(fs);

            currentState = fs;

            while (listO.Count > 0)
            {
                cycles++;
                //Взяли первый элемент
                currentState = listO.Pop();
                //Проверка является ли решением?
                if (FieldState.IsSollution(currentState))
                {
                    //Нашли решение
                    break;
                }

                //Добавляем в список неверных решений
                listC.Add(currentState);
                
                //Добавляем все возможные варианты (Потомков)
                for (int i = 0; i < currentState.cells.Length; i++)
                {
                    FieldState newFieldState = new FieldState(currentState.id, i);
                    if (!listC.Any(o => o.id == newFieldState.id) && !listO.Any(o=>o.id == newFieldState.id)) //Моя ф-ция
                    {
                        listO.Push(newFieldState);
                    }

                }
                if (listO.Count > maxO) maxO = listO.Count;
                if (listO.Count+listC.Count > maxC) maxC = listO.Count + listC.Count;

            }
            //Очищаем решение и записываем новое
            solution.Clear();
            while (currentState.id != GameField.fieldState.id)
            {
                solution.Add(currentState);
                currentState = GetFatherFromList(listC, currentState);
            }
            solution.Add(currentState);
            solution.Reverse();

            MessageBox.Show("Решение Найдено");
            GameField.fieldState.cells = solution[0].cells;
            GameField.Render();
            SetCounters();
        }
        private bool DLS(Stack<FieldState> listO, List<FieldState> listC, int limit)
        {
            int depth = 1;
            while (listO.Count > 0)
            {

                cycles++;
                //Взяли первый элемент
                
                currentState = listO.Pop();
                if (currentState.depth > limit)
                    continue;
                //Проверка является ли решением?
                if (FieldState.IsSollution(currentState))
                    {
                        //Нашли решение
                        return true;
                    }

                    //Добавляем в список неверных решений
                    listC.Add(currentState);
                
                //Добавляем все возможные варианты (Потомков)
                for (int i = 0; i < currentState.cells.Length; i++)
                    {
                        FieldState newFieldState = new FieldState(currentState.id, i,currentState.depth+1);
                        if (!listC.Any(o => o.id == newFieldState.id) && !listO.Any(o => o.id == newFieldState.id)) //Моя ф-ция
                        {
                            listO.Push(newFieldState);
                        }

                    }
                    depth++;
                    if (listO.Count > maxO) maxO = listO.Count;
                    if (listO.Count + listC.Count > maxC) maxC = listO.Count + listC.Count;
            }
            return false;
        }
        private void IDDFS()
        {
            //Счетчики
            int limit = 0;
            cycles = 0;
            maxO = 0;
            maxC = 0;
            //Ф-ция поиска решения
            //Очередь для поиска в ширину и Словарь для списка О
            Stack<FieldState> listO = new Stack<FieldState>();
            List<FieldState> listC = new List<FieldState>();
            FieldState fs = new FieldState();

            fs.id = "";
            for (int i = 0; i < GameField.fieldState.cells.Length; i++)
            {
                fs.cells[i] = GameField.fieldState.cells[i];
                if (GameField.fieldState.cells[i] == 0)
                {
                    fs.cells[i] = 0;
                    fs.id += "0";
                }
                else
                {
                    fs.cells[i] = 1;
                    fs.id += "1";
                }
            }
            fs.parent = "0";
            fs.depth = 0;

            listO.Push(fs);

            currentState = fs;
            while(!DLS(listO,listC,limit))
            {
                listC.Clear();
                listO.Clear();
                listO.Push(fs);
                cycles = 0;
                limit++;
            }
            
            //Очищаем решение и записываем новое
            solution.Clear();
            while (currentState.id != GameField.fieldState.id)
            {
                solution.Add(currentState);
                currentState = GetFatherFromList(listC, currentState);
            }
            solution.Add(currentState);
            solution.Reverse();

            MessageBox.Show("Решение Найдено");
            GameField.fieldState.cells = solution[0].cells;
            GameField.Render();

            SetCounters();
        }
        private void BreadthTwoWaySearch()
        {
            //Счетчики
            cycles = 0;
            maxO = 0;
            maxC = 0;
            //Очередь для поиска в ширину и Словарь для списка О
            Queue<FieldState> listO = new Queue<FieldState>();
            List<FieldState> listC = new List<FieldState>();
            FieldState fs = new FieldState();

            Queue<FieldState> listO2 = new Queue<FieldState>();
            List<FieldState> listC2 = new List<FieldState>();
            FieldState finalFS = new FieldState();
            finalFS.id = "";
            for(int i = 0;i< Field.numberOfCells;i++)
            {
                finalFS.cells[i] = 1;
                finalFS.id += 1;
            }
            finalFS.parent = "0";

            listO2.Enqueue(finalFS);

            currentState2 = finalFS;

            fs.id = "";
            for (int i = 0; i < Field.numberOfCells; i++)
            {
                fs.cells[i] = GameField.fieldState.cells[i];
                if (GameField.fieldState.cells[i] == 0)
                {
                    fs.cells[i] = 0;
                    fs.id += "0";
                }
                else
                {
                    fs.cells[i] = 1;
                    fs.id += "1";
                }
            }
            fs.parent = "0";

            listO.Enqueue(fs);

            currentState = fs;

            while (listO.Count > 0)
            {
                cycles++;
                //Взяли первый элемент
                currentState = listO.Dequeue();
                //Проверка является ли решением?
                if (FieldState.IsSollution(currentState))
                {
                    //Нашли решение
                    break;
                }

                //Добавляем в список неверных решений
                listC.Add(currentState);

                //Добавляем все возможные варианты (Потомков)
                for (int i = 0; i < Field.numberOfCells; i++)
                {
                    FieldState newFieldState = new FieldState(currentState.id, i);
                    if (!listC.Any(o => o.id == newFieldState.id) && !listO.Any(o => o.id == newFieldState.id)) //Моя ф-ция
                    {
                        listO.Enqueue(newFieldState);
                    }

                }

                    //Второй цикл с конца

                //Взяли первый элемент с конца
                currentState2 = listO2.Dequeue();
                //Проверка является ли решением?
                if (listO.Any(o=>o.id == currentState2.id))
                {
                    //Нашли решение
                    currentState = listO.First(o => o.id == currentState2.id);
                    break;
                }

                //Добавляем в список неверных решений
                listC2.Add(currentState2);

                //Добавляем все возможные варианты (Потомков)
                for (int i = 0; i < currentState2.cells.Length; i++)
                {
                    FieldState newFieldState2 = new FieldState(currentState2.id, i);
                    if (!listC2.Any(o => o.id == newFieldState2.id) && !listO2.Any(o => o.id == newFieldState2.id)) //Моя ф-ция
                    {
                        listO2.Enqueue(newFieldState2);
                    }

                }


                //Счётчики меняем
                if (listO.Count + listO2.Count > maxO) maxO = listO.Count + listO2.Count;
                if (listO.Count + listC.Count + listO2.Count + listC2.Count > maxC) maxC = listO.Count + listC.Count + listO2.Count + listC2.Count;

            }
            
            //Очищаем решение и записываем новое
            solution.Clear();

            while (currentState2.id != "1111111111111111")
            {
                solution.Add(currentState2);
                currentState2 = GetFatherFromList(listC2, currentState2);
            }
            solution.Add(currentState2);
            solution.Reverse();
            while (currentState.parent != "0")
            {
                currentState = listC.First(o => o.id == currentState.parent);
                solution.Add(currentState);
            }
            solution.Reverse();

            MessageBox.Show("Решение Найдено");
            GameField.fieldState.cells = solution[0].cells;
            GameField.Render();

            SetCounters();
        }
        private void NextState_Click(object sender, RoutedEventArgs e)
        {
            //Ф-ция вывода следующего состояния
            if (solution.Count > 0&&stateInd+1<solution.Count)
            {
                stateInd++;
                GameField.fieldState.cells = solution[stateInd].cells;
                GameField.Render();
            } 
        }
        private void PrevState_Click(object sender, RoutedEventArgs e)
        {
            //Ф-ция вывода предыдущего состояния
            if (solution.Count > 0 && stateInd > 0)
            {
                stateInd--;
                GameField.fieldState.cells = solution[stateInd].cells;
                GameField.Render();
            }
        }
        private FieldState GetFatherFromList(List<FieldState> listC, FieldState fs)
        {
            foreach (var item in listC)
            {
                if (item.id == fs.parent) return item;
            }
            return fs;
        }
        private void SetCounters()
        {
            Cycle.Text = "Итерации " + cycles;
            MaxC.Text = "MaxC " + maxC;
            MaxO.Text = "MaxO " + maxO;
            SolutionLength.Text = "Кол-во переходов " + (solution.Count-1).ToString();
        }

        private void solutionA()
        {
            //Счетчики
            cycles = 0;
            maxO = 0;
            maxC = 0;
            int depth = 0;
            //Ф-ция поиска решения
            //Очередь для поиска в ширину и Словарь для списка О
            List<FieldState> listO = new List<FieldState>();
            //Dictionary<string,FieldState> listC = new Dictionary<string,FieldState>();
            List<FieldState> listC = new List<FieldState>();
            FieldState fs = new FieldState();

            fs.id = "";
            for (int i = 0; i < GameField.fieldState.cells.Length; i++)
            {
                fs.cells[i] = GameField.fieldState.cells[i];
                if (GameField.fieldState.cells[i] == 0)
                {
                    fs.cells[i] = 0;
                    fs.id += "0";
                }
                else
                {
                    fs.cells[i] = 1;
                    fs.id += "1";
                }
            }
            fs.parent = "0";
            fs.depth = 0;

            listO.Insert(0, fs);

            currentState = fs;

            while (listO.Count > 0)
            {
                cycles++;
                //Взяли первый элемент
                currentState = listO.First();
                listO.Remove(listO.First());
                //Проверка является ли решением?
                if (FieldState.IsSollution(currentState))
                {
                    //Нашли решение
                    break;
                }

                //Добавляем в список неверных решений
                listC.Add(currentState);

                //Добавляем все возможные варианты (Потомков)
                for (int i = 0; i < currentState.cells.Length; i++)
                {
                    FieldState newFieldState = new FieldState(currentState.id, i,depth);
                    newFieldState.value = heuristicFunc(newFieldState);
                    if (!listC.Any(o => o.id == newFieldState.id) && !listO.Any(o => o.id == newFieldState.id)) 
                    {
                        listO.Add(newFieldState);
                    }
                    else
                    {
                        if(listO.Any(o => o.id == newFieldState.id))
                        {
                            var tempState = listO.First(o => o.id == newFieldState.id);
                            newFieldState.parent = tempState.parent;
                            if (tempState.value>newFieldState.value)
                            {
                                listO.Remove(tempState);
                                listO.Add(newFieldState);
                            }
                        }
                        if(listC.Any(o => o.id == newFieldState.id))
                        {
                            var tempState = listC.First(o => o.id == newFieldState.id);
                            newFieldState.parent = tempState.parent;
                            if (tempState.value > newFieldState.value)
                            {
                                listC.Remove(tempState);
                                listO.Add(newFieldState);
                            }
                        }    
                    }
                    
                }
                //Сортируем
                listO = listO.OrderBy(o => o.value).ToList();
                listC = listC.OrderBy(o => o.value).ToList();
                depth++;
                if (listO.Count > maxO) maxO = listO.Count;
                if (listO.Count + listC.Count > maxC) maxC = listO.Count + listC.Count;
            }
            //Очищаем решение и записываем новое
            solution.Clear();
            while (currentState.id != GameField.fieldState.id)
            {
                solution.Add(currentState);
                currentState = GetFatherFromList(listC, currentState);
            }
            solution.Add(currentState);
            solution.Reverse();

            MessageBox.Show("Решение Найдено");
            GameField.fieldState.cells = solution[0].cells;
            GameField.Render();
            SetCounters();
        }
        private int heuristicFunc(FieldState fieldState)
        {
            int value = 0;
            switch (choosenHeuristic)
            {
                //Количество черных (расстояние Хемминга)
                // size = 4 || numberOfCells = size*size;
                case 0:
                    value = fieldState.id.Count(f => (f == '0')) + (fieldState.depth * 1);
                    break;
                case 1:
                    //Количество черных на краю 
                    for (int i = 0;i<Field.numberOfCells;i++)
                    {  
                        //    Верх, Низ, Лево, Право
                        if (i < Field.sizeOfField || 
                            i >= Field.numberOfCells - Field.sizeOfField || 
                            i % Field.sizeOfField == 0||
                            i % Field.sizeOfField == Field.sizeOfField-1)
                        {
                            if (fieldState.cells[i] == 0)
                            {
                                value += 1 + fieldState.depth;
                            }
                        }
                    }
                    break;
                case 2:
                    //Количество черных по элементам шахматной доски (четная линия, чередуется с нечетной)
                    for (int i = 0; i < Field.numberOfCells; i++)
                    {
                        if (i%2==((i/Field.sizeOfField)%2))
                        {
                            if (fieldState.cells[i] == 0)
                            {
                                value += 1 + fieldState.depth;
                            }
                        }
                    }
                    break;
                case 3:
                    //Количество черных в центре
                    for (int i = 0; i < Field.numberOfCells; i++)
                    {
                        if (!(i < Field.sizeOfField ||
                            i >= Field.numberOfCells - Field.sizeOfField ||
                            i % Field.sizeOfField == 0 ||
                            i % Field.sizeOfField == Field.sizeOfField - 1)
                           )
                        {
                            if (fieldState.cells[i] == 0)
                            {
                                value += 1 + fieldState.depth;
                            }
                        }
                    }
                    break;
                case 9:
                    //Количество черных по диагоналям (лучше чем Хемминга, но не лучше центра и окраины)
                    for (int i = 0; i < Field.numberOfCells; i++)
                    {
                        if (i / Field.sizeOfField == i % Field.sizeOfField || i / Field.sizeOfField + i % Field.sizeOfField == 3)
                        {
                            if (fieldState.cells[i] == 0)
                            {
                                value += 1 + fieldState.depth;
                            }
                        }
                    }
                    break;
            }
            return value;
        }

    }
}


//Old Code by Old (Maksim Sheinov) P.s was deleted