﻿using System;
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
        public int Direction = 0, Food = 0;
        public int[] dy = { -1, 0, 1, 0 };
        public int[] dx = { 0, 1, 0, -1 };
        Pair Head, Tail;
        private void Form1_Load(object sender, EventArgs e)
        {
            Width = width * k;
            Height = height * k;
            Head = new Pair() { x = width / 2, y = height / 2 };
            gr = Graphics.FromImage(field);
            gr.Clear(Color.Black);
            // заносим начальные значения координат клеток змейки в очередь
            for (int i = 4; i >= 0; i--)
            {
                int x = width / 2, y = height / 2 + i;
                q.Enqueue(new Pair() { x = x, y = y });//добавляем элемент
                gr.FillRectangle(Brushes.Lime, x * k, y * k, k, k);//клетка
                gr.DrawRectangle(Pens.Black, x * k, y * k, k, k);//границы
            }
            pictureBox1.Image = field;//привязываем картинку, которую мы сделали, и запускаем
            timer1.Start();
        }

      
        public Form1()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Head.x += dx[Direction]; Head.y += dy[Direction];
            if (Head.x < 0) Head.x = height;
            if (Head.y < 0) Head.y = width;
            if (Head.x > width) Head.x = 0;
            if (Head.y > height) Head.y = 0;
            q.Enqueue(new Pair() { x = Head.x, y = Head.y });
            gr.FillRectangle(Brushes.Lime, Head.x * k, Head.y * k, k, k);
            gr.DrawRectangle(Pens.Black, Head.x * k, Head.y * k, k, k);
            Tail = q.Dequeue();
            gr.FillRectangle(Brushes.Black, Tail.x * k, Tail.y * k, k, k);
            pictureBox1.Image = field;
        }
    }
}
