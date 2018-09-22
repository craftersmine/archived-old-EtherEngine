using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using craftersmine.EtherEngine.Content;
using craftersmine.EtherEngine.Core;
using craftersmine.EtherEngine.Core.Math;
using craftersmine.EtherEngine.Input;
using craftersmine.EtherEngine.Objects;
using craftersmine.EtherEngine.Utils;

namespace TestApp
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Window gw = new Window();
            gw.SetScene(0);
            GameApplication.Run(gw, @"craftersmine\EtherEngineTest");
        }
    }

    public class TestUIControl : UIControl
    {
        public Pen drawRectPen = new Pen(Color.Red);
        public TestUIControl() : base(20, 10)
        {
            SetPosition(30, 30);
        }

        public override void DrawMethod(Graphics graphics)
        {
            graphics.DrawRectangle(drawRectPen, 5, 5, 10, 2);
        }
    }

    public class Window : GameWindow
    {
        GameObject testGameObj = new GameObject();
        GameObject testGameObj1 = new GameObject();
        GameObject cameraBinded = new GameObject();
        Timer timer = new Timer() { Interval = 2000, Enabled = true };
        Scene scene1 = new Scene() { BackgroundColor = Color.Green };
        public Window() : base("GameWindow", new WindowSize(WindowSizePresets.SVGA), true)
        {
            timer.Tick += Timer_Tick;
            Scene scene = new Scene() { BackgroundColor = Color.Red };
            this.AddScene(0, scene);
            this.AddScene(1, scene1);
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            this.SetTextureInterpolationMode(System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor);
            this.SetScene(1);
            timer.Stop();
        }

        public override void OnCreate()
        {
            GameApplication.SetLogger("Logs", "log");
            GameApplication.Log(LogEntryType.Debug, "GameWindow Created!", true);
            this.SetTextureInterpolationMode(System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor);
            testGameObj.Transform.Place(30, 30);
            testGameObj.Transform.SetSize(32, 32);
            Texture bg = new Texture(Image.FromFile(@"G:\Родион\Images\_PNG\12.png"), TextureLayout.Default);
            scene1.BackgroundTexture = bg;
            scene1.LightValue = 0.8f;
            Audio audio = new Audio(WaveFileReaderFromBytesConverter.ByteArrayToWaveFileReader(File.ReadAllBytes(@"D:\Родион\Desktop\testAssets\aud.wav")));
            AudioChannel audioChannel = new AudioChannel("test", audio);
            Texture texture = new Texture(Image.FromFile(@"D:\Родион\VS\MyGame2\resources\raw\maps\textures\overworld_base_river.png"), TextureLayout.Default);
            Texture texturetiled = new Texture(Image.FromFile(@"D:\Родион\VS\MyGame2\resources\raw\maps\textures\overworld_base_river.png"), TextureLayout.Tile);
            testGameObj.SetTexture(texture);
            testGameObj.CollisionBox.SetCollisionBox(0, 0, 32, 32);
            scene1.AddGameObject(testGameObj);
            testGameObj1.Transform.Place(80, 30);
            testGameObj1.Transform.SetSize(32, 32);
            testGameObj1.CollisionBox.SetCollisionBox(0, 0, 32, 32);
            testGameObj1.SetTexture(texture);
            Animation animation = new Animation(new Texture(Image.FromFile(@"D:\Родион\Desktop\testAssets\anim.png"), TextureLayout.Stretch), 6, 10, 32);
            testGameObj.Animation = animation;
            testGameObj.Animate();
            cameraBinded.Transform.Place((scene1.Width / 2) - (cameraBinded.Width / 2), (scene1.Height / 2) - (cameraBinded.Height / 2));
            cameraBinded.Transform.SetSize(48, 48);
            cameraBinded.SetTexture(texturetiled);
            cameraBinded.CollisionBox.SetCollisionBox(2, 2, 28, 28);
            scene1.AddGameObject(testGameObj);
            scene1.AddGameObject(testGameObj1);
            scene1.AddGameObject(cameraBinded);
            TestObj tobj = new TestObj(cameraBinded);
            scene1.AddGameObject(tobj);
            TestUIControl control = new TestUIControl();
            Texture buttonTex = new Texture(Image.FromFile(@"D:\Родион\fankit\TestButtonTexture.png"), TextureLayout.Stretch);
            UIButton button = new UIButton(100, 32, buttonTex, 32);
            scene1.AddUIControl(control);
            button.Transform.Place(100, 100);
            scene1.AddUIControl(button);
            //cameraBinded.IsCameraSticked = true;
            audioChannel.ChannelVolume = 0.1f;
            audioChannel.Play();
            double[,] perlinData = new double[16, 16];
            int xArr = 0, yArr = 0;
            for (double x = 0; x < 1.0d; x += 0.0625d)
            {
                for (double y = 0; y < 1.0d; y += 0.0625d)
                {
                    perlinData[xArr, yArr] = PerlinNoise.GetNoiseData(x, y, 0);
                    yArr++;
                }
                xArr++;
                yArr = 0;
            }
            for (int x = 0; x < 16; x++)
            {
                string arrCtor = "";
                for (int y = 0; y < 16; y++)
                {
                    arrCtor += Math.Round(perlinData[x, y], 2) + " ";
                }
                GameApplication.Log(LogEntryType.Info, arrCtor);
            }
        }

        public int objInitialSpeed = 5;

        public override void OnUpdate()
        {
            Sticks sticks = Gamepad.GetSticks(Player.First);
            Vector2 moveVec = new Vector2((int)(objInitialSpeed * sticks.LeftStickAxisX), (int)(objInitialSpeed * sticks.LeftStickAxisY * -1));
            Vector2 moveCamVec = new Vector2((int)(objInitialSpeed * sticks.LeftStickAxisX * -1), (int)(objInitialSpeed * sticks.LeftStickAxisY));
            cameraBinded.Transform.Move(moveVec);
            scene1.Camera.MoveCamera(moveCamVec);
        }
    }

    public class TestObj : GameObject
    {
        private GameObject collidableObj;

        public TestObj(GameObject collidable)
        {
            collidableObj = collidable;
            Texture texture = new Texture(Image.FromFile(@"D:\Родион\VS\MyGame2\resources\raw\maps\textures\overworld_base_river.png"), TextureLayout.Default);
            this.SetTexture(texture);
            this.Transform.SetSize(32, 32);
            this.CollisionBox.SetCollisionBox(0, 0, 32, 32);
        }

        public override void OnCollision(GameObject gameObject)
        {
            base.OnCollision(gameObject);
            if (gameObject == collidableObj)
                GameApplication.Log(LogEntryType.Info, "Collided with object");
        }
    }
}
