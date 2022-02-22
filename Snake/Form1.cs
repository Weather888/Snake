using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
    struct Pair
    {
        public int x, y;//координаты клетки
    }
    public partial class Form1 : Form
    {
        // ширина и высота поля
        public const int width = 20, height = 20, k = 30;
        // само поле, хранится в двумерном массиве
        public int[,] Map = new int[width + 1, height + 1];
        // полотно для хранения изображкения поля
        public Bitmap field = new Bitmap(k * (width + 1) + 1, k * (height + 1) + 1);
        public Graphics gr;// для соединения 
        // очередь для хранения клеток змейки
        Queue<Pair> q = new Queue<Pair>();
        public int Direction = 0, Food = 0;//Direction направление куда ползет змейка/Food = 0 для роста змейки
        public int[] dy = { -1, 0, 1, 0 };//массивы смещений
        public int[] dx = { 0, 1, 0, -1 };
        Pair Head, Tail;
        Random Rnd = new Random(DateTime.Now.Millisecond);

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                // движение змейки влево
                case Keys.A:
                case Keys.Left:
                    Direction = 3;
                    break;
                // движение змейки вправо
                case Keys.D:
                case Keys.Right:
                    Direction = 1;
                    break;
                // движение змейки вверх
                case Keys.W:
                case Keys.Up:
                    Direction = 0;
                    break;
                // движение змейки вниз
                case Keys.S:
                case Keys.Down:
                    Direction = 2;
                    break;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Width = (width + 1) * k + 20;
            Height = (height + 1) * k + 40;
            Head = new Pair() { x = width / 2, y = height / 2 };
            gr = Graphics.FromImage(field);
            gr.Clear(Color.Black);
            // заносим начальные значения координат клеток змейки в очередь
            for (int i = 4; i >= 0; i--)
            {
                int x = width / 2, y = height / 2 + i;
                q.Enqueue(new Pair() { x = x, y = y });//добавляем элемент
                Map[x, y] = 1;//запоминаем точку на карте
                gr.FillRectangle(Brushes.Lime, x * k, y * k, k, k);//клетка
                gr.DrawRectangle(Pens.Black, x * k, y * k, k, k);//границы
            }
            AddFood();
            pictureBox1.Image = field;//привязываем картинку, которую мы сделали, и запускаем
            timer1.Start();
        }

      
        public Form1()
        {
            InitializeComponent();
        }
        public void AddFood()
        {
            int x, y;
            do
            {
                x = 1 + Rnd.Next(height - 1);
                y = 1 + Rnd.Next(width - 1);
            } while (Map[x, y] != 0);
            Map[x, y] = 2;
            gr.FillEllipse(Brushes.Yellow, x * k + 2, y * k + 2, k - 4, k - 4);
            pictureBox1.Image = field;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            Head.x += dx[Direction]; Head.y += dy[Direction];//изменится голова змейки
            if (Head.x < 0) Head.x = height;//если выползает вверх, появляется снизу
            if (Head.y < 0) Head.y = width;
            if (Head.x > width) Head.x = 0;
            if (Head.y > height) Head.y = 0;
            if (Map[Head.x, Head.y] == 1)//если клетка занята останавливаем таймер
             // q.Enqueue(new Pair() { x = Head.x, y = Head.y });//в очередь добавляем новую координату змейки
             //gr.FillRectangle(Brushes.Lime, Head.x * k, Head.y * k, k, k);//рисуем змейку на новой координате
             //gr.DrawRectangle(Pens.Black, Head.x * k, Head.y * k, k, k);
             //Tail = q.Dequeue();//хвост
             //gr.FillRectangle(Brushes.Black, Tail.x * k, Tail.y * k, k, k);//зарисовываем черным
             //pictureBox1.Image = field;
             
            {
                gr.FillRectangle(Brushes.OrangeRed, Head.x * k, Head.y * k, k, k);
                gr.DrawRectangle(Pens.Black, Head.x * k, Head.y * k, k, k);
                timer1.Stop();
            }
            else
            {
                if (Map[Head.x, Head.y] == 2)
                {
                    AddFood();
                    Food += 3;
                }
                q.Enqueue(new Pair() { x = Head.x, y = Head.y });
                Map[Head.x, Head.y] = 1;
                gr.FillRectangle(Brushes.Lime, Head.x * k, Head.y * k, k, k);
                gr.DrawRectangle(Pens.Black, Head.x * k, Head.y * k, k, k);
                if (Food > 0)
                {
                    Food--;
                }
                else
                {
                    Tail = q.Dequeue();
                    Map[Tail.x, Tail.y] = 0;// удаляем
                    gr.FillRectangle(Brushes.Black, Tail.x * k, Tail.y * k, k, k);
                }
            }
            pictureBox1.Image = field;
        }
    }
}
