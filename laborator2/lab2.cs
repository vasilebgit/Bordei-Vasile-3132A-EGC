using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenTK.Platform;

/**
    Aplicația utilizează biblioteca OpenTK v2.0.0 (stable) oficială și OpenTK. GLControl v2.0.0
    (unstable) neoficială. Aplicația fiind scrisă în modul consolă nu va utiliza controlul WinForms
    oferit de OpenTK!
    Tipul de ferestră utilizat: GAMEWINDOW. Se demmonstrează modul imediat de randare (vezi comentariu!)...
**/
namespace lab2 {
    class SimpleWindow : GameWindow {

        private List<MyObject> objects = new List<MyObject>();
        private Vector2 objectPosition = new Vector2(0.0f, 0.0f);
        private float objectSpeed = 0.05f;
        private bool useMouseControl = false;

        public void AddObject(Vector2 position, Color color)
        {
            objects.Add(new MyObject(position, color));
        }
        public void RenderObjects()
        {
            foreach (var obj in objects)
            {
                // Update the object's position based on keyboard input
                obj.Position = objectPosition;
                obj.Render();
            }
        }

        // Constructor.
        public SimpleWindow() : base(800, 600) {
            KeyDown += Keyboard_KeyDown;
        }

        // Tratează evenimentul generat de apăsarea unei taste. Mecanismul standard oferit de .NET
        // este cel utilizat.
        void Keyboard_KeyDown(object sender, KeyboardKeyEventArgs e) {
            if (e.Key == Key.Escape)
                this.Exit();

            if (e.Key == Key.F11)
                if (this.WindowState == WindowState.Fullscreen)
                    this.WindowState = WindowState.Normal;
                else
                    this.WindowState = WindowState.Fullscreen;
        }

        // Setare mediu OpenGL și încarcarea resurselor (dacă e necesar) - de exemplu culoarea de
        // fundal a ferestrei 3D.
        // Atenție! Acest cod se execută înainte de desenarea efectivă a scenei 3D.
        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(Color.White);
            GL.Enable(EnableCap.DepthTest);  // Enable depth testing
        }

        // Inițierea afișării și setarea viewport-ului grafic. Metoda este invocată la redimensionarea
        // ferestrei. Va fi invocată o dată și imediat după metoda ONLOAD!
        // Viewport-ul va fi dimensionat conform mărimii ferestrei active (cele 2 obiecte pot avea și mărimi 
        // diferite). 
        protected override void OnResize(EventArgs e) {
            GL.Viewport(0, 0, Width, Height);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(-1.0, 1.0, -1.0, 1.0, 0.0, 4.0);
        }

        // Secțiunea pentru "game logic"/"business logic". Tot ce se execută în această secțiune va fi randat
        // automat pe ecran în pasul următor - control utilizator, actualizarea poziției obiectelor, etc.
        protected override void OnUpdateFrame(FrameEventArgs e) {
            var keyboardState = OpenTK.Input.Keyboard.GetState();

            if (keyboardState.IsKeyDown(Key.M))
            {
                // Toggle between mouse control and keyboard control
                useMouseControl = !useMouseControl;
            }

            if (useMouseControl)
            {
                var mouseState = OpenTK.Input.Mouse.GetCursorState();
                var mousePosition = new Vector2(mouseState.X, mouseState.Y);

                // Convert mouse position to a range of -1 to 1
                float mouseX = (mousePosition.X / Width) * 2 - 1;
                float mouseY = 1 - (mousePosition.Y / Height) * 2;

                objectPosition = new Vector2(mouseX, mouseY);
            }
            else
            {
                if (keyboardState.IsKeyDown(Key.Left))
                {
                    objectPosition.X -= objectSpeed;
                }

                if (keyboardState.IsKeyDown(Key.Right))
                {
                    objectPosition.X += objectSpeed;
                }

                if (keyboardState.IsKeyDown(Key.Up))
                {
                    objectPosition.Y += objectSpeed;
                }

                if (keyboardState.IsKeyDown(Key.Down))
                {
                    objectPosition.Y -= objectSpeed;
                }
            }
        }

        // Secțiunea pentru randarea scenei 3D. Controlată de modulul logic din metoda ONUPDATEFRAME.
        // Parametrul de intrare "e" conține informatii de timing pentru randare.
        protected override void OnRenderFrame(FrameEventArgs e) {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Render your objects
            RenderObjects();

            this.SwapBuffers();
            // Sfârșitul modului imediat!


        }

        [STAThread]
        static void Main(string[] args) {

            // Utilizarea cuvântului-cheie "using" va permite dealocarea memoriei o dată ce obiectul nu mai este
            // în uz (vezi metoda "Dispose()").
            // Metoda "Run()" specifică cerința noastră de a avea 30 de evenimente de tip UpdateFrame per secundă
            // și un număr nelimitat de evenimente de tip randare 3D per secundă (maximul suportat de subsistemul
            // grafic). Asta nu înseamnă că vor primi garantat respectivele valori!!!
            // Ideal ar fi ca după fiecare UpdateFrame să avem si un RenderFrame astfel încât toate obiectele generate
            // în scena 3D să fie actualizate fără pierderi (desincronizări între logica aplicației și imaginea randată
            // în final pe ecran).
            using (SimpleWindow example = new SimpleWindow())
            {
                // Add a red object at position (0.5, 0.0)
                example.AddObject(new Vector2(0.5f, 0.0f), Color.Red);

                // Run the application
                example.Run(30.0, 30.0);
            }
        }
    }
}
