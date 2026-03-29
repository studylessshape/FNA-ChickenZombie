using FNAStart.Engine;
using FNAStart.Scripts;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;

namespace FNAStart
{
    internal class MainGame : Game
    {
        public static Game Currnet { get; private set; } = null!;

        #region Resources
        Camera camera;

        Texture2D textureHeart;                        // 生命值图标纹理
        Texture2D textureBullet;                       // 子弹纹理
        Texture2D textureBattery;                      // 炮台基座纹理
        Texture2D textureCrosshair;                    // 光标准星纹理
        Texture2D textureBackground;                   // 背景图纹理
        Texture2D textureBarrelIdle;                   // 炮管默认状态纹理

        Atlas atlasBarrelFire = new();                 // 炮管开火动画图集
        Atlas atlasChickenFast = new();                // 快速僵尸鸡动画图集
        Atlas atlasChikenMedium = new();               // 中速僵尸鸡动画图集
        Atlas atlasChikenSlow = new();                 // 慢速僵尸鸡动画图集
        Atlas atlasExplosion = new();                  // 僵尸鸡死亡爆炸动画图集

        Song musicBgm;                                 // 背景音乐
        Song musicLoss;                                // 游戏失败音乐

        SoundEffect soundHurt;                         // 生命值降低音效
        SoundEffect soundFire1;                        // 开火音效1
        SoundEffect soundFire2;                        // 开火音效2
        SoundEffect soundFire3;                        // 开火音效3
        SoundEffect soundExplosion;                    // 僵尸鸡死亡爆炸音效

        FontSystem fontSystem;                         // 字体系统
        SpriteFontBase font;                           // 得分字体文件
        #endregion

        #region GameStatus
        int hp = 10;                                   // 生命值
        int score = 0;                                 // 游戏得分
        List<Bullet> bulletList = [];                  // 子弹列表
        List<Chicken> chickenList = [];                // 僵尸鸡列表

        int numPerGen = 2;                             // 每次生成的鸡数量
        Timer timerGenerate = new();                   // 僵尸鸡生成定时器
        Timer timerIncreaseNumPerGen = new();          // 增加每次生成数量的定时器

        Vector2 posCrosshair;                          // 准星位置
        float angleBarrel = 0;                         // 炮管旋转角度
        readonly Vector2 posBattery = new(640, 600);   // 炮台基座中心位置
        readonly Vector2 posBarrel = new(640, 610);    // 炮台无旋转默认位置
        readonly Vector2 centerBarrel = new(48, 25);   // 炮管旋转中心坐标

        bool isCoolDown = true;                        // 是否冷却结束
        bool isFireKeyDown = false;                    // 开火键是否按下
        Animation animationBarrelFire = new();         // 炮管开火动画
        SpriteBatch spriteBatch;
        #endregion

        public MainGame()
        {
            Currnet = this;

            var graphicsDeviceManager = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1280,
                PreferredBackBufferHeight = 720
            };

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();

            camera = new Camera(GraphicsDevice);
            spriteBatch = new SpriteBatch(GraphicsDevice);

            timerGenerate.OneShot = false;
            timerGenerate.WaitTime = 1.5f;
            timerGenerate.OnTimeout += TimerGenerate_OnTimeout;

            timerIncreaseNumPerGen.OneShot = false;
            timerIncreaseNumPerGen.WaitTime = 8.0f;
            timerIncreaseNumPerGen.OnTimeout += () => numPerGen++;

            animationBarrelFire.IsLoop = false;
            animationBarrelFire.SetInterval(0.04f);
            animationBarrelFire.Center = centerBarrel;
            animationBarrelFire.Position = new Vector2(764, 635);
            animationBarrelFire.AddFrame(atlasBarrelFire);
            animationBarrelFire.OnFinished += () => isCoolDown = true;

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(musicBgm);
        }

        private void TimerGenerate_OnTimeout()
        {
            for (int i = 0; i < numPerGen; i++)
            {
                int val = Random.Shared.Next(100);
                Chicken chicken = val switch
                {
                    < 50 => new ChickenSlow(),
                    < 80 => new ChickenMedium(),
                    _ => new ChickenFast(),
                };
                chickenList.Add(chicken);
            }
        }

        protected override void LoadContent()
        {
            textureHeart = Content.Load<Texture2D>("heart.png");
            textureBullet = Content.Load<Texture2D>("bullet.png");
            textureBattery = Content.Load<Texture2D>("battery.png");
            textureCrosshair = Content.Load<Texture2D>("crosshair.png");
            textureBackground = Content.Load<Texture2D>("background.png");
            textureBarrelIdle = Content.Load<Texture2D>("barrel_idle.png");

            atlasBarrelFire.Load("barrel_fire_{0}.png", 3);
            atlasChickenFast.Load("chicken_fast_{0}.png", 4);
            atlasChikenMedium.Load("chicken_medium_{0}.png", 6);
            atlasChikenSlow.Load("chicken_slow_{0}.png", 8);
            atlasExplosion.Load("explosion_{0}.png", 5);

            musicBgm = Content.Load<Song>("bgm.ogg");
            musicLoss = Content.Load<Song>("loss.ogg");

            soundHurt = Content.Load<SoundEffect>("hurt.wav");
            soundFire1 = Content.Load<SoundEffect>("fire_1.wav");
            soundFire2 = Content.Load<SoundEffect>("fire_2.wav");
            soundFire3 = Content.Load<SoundEffect>("fire_3.wav");
            soundExplosion = Content.Load<SoundEffect>("explosion.wav");

            fontSystem = new();
            fontSystem.AddFont(File.ReadAllBytes(Path.Combine("Content", "IPix.ttf")));
            font = fontSystem.GetFont(18);

            ChickenFast.AtlasChickenFast = atlasChickenFast;
            ChickenMedium.AtlasChickenMedium = atlasChikenMedium;
            ChickenSlow.AtlasChickenSlow = atlasChikenSlow;
            Chicken.AtlasExplosion = atlasExplosion;

            Chicken.SoundExplosion = soundExplosion;
        }

        protected override void UnloadContent()
        {
            textureHeart.Dispose();
            textureBullet.Dispose();
            textureBattery.Dispose();
            textureCrosshair.Dispose();
            textureBackground.Dispose();
            textureBarrelIdle.Dispose();

            atlasBarrelFire.Dispose();
            atlasChickenFast.Dispose();
            atlasChikenMedium.Dispose();
            atlasChikenSlow.Dispose();
            atlasExplosion.Dispose();

            musicBgm.Dispose();
            musicLoss.Dispose();

            soundHurt.Dispose();
            soundFire1.Dispose();
            soundFire2.Dispose();
            soundFire3.Dispose();
            soundExplosion.Dispose();
        }

        protected override void Update(GameTime gameTime)
        {
            if (!IsActive) return;

            var curMouseState = Mouse.GetState();
            posCrosshair.X = curMouseState.X;
            posCrosshair.Y = curMouseState.Y;
            var barrelDirection = posCrosshair - posBarrel;
            angleBarrel = MathHelper.ToDegrees(MathF.Atan2(barrelDirection.Y, barrelDirection.X));

            isFireKeyDown = curMouseState.LeftButton == ButtonState.Pressed;

            var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            timerGenerate.Update(delta);
            timerIncreaseNumPerGen.Update(delta);

            foreach (var bullet in bulletList)
            {
                bullet.Update(delta);
            }

            foreach (var chicken in chickenList)
            {
                chicken.Update(delta);

                foreach (var bullet in bulletList)
                {
                    if (!chicken.IsAlive || bullet.CanRemove())
                    {
                        continue;
                    }

                    var posBullet = bullet.Position;
                    var posChicken = chicken.Position;
                    var sizeChicken = new Vector2(30, 40);

                    if (posBullet.X >= (posChicken.X - sizeChicken.X / 2)
                        && posBullet.X <= (posChicken.X + sizeChicken.X / 2)
                        && posBullet.Y >= (posChicken.Y - sizeChicken.Y / 2)
                        && posBullet.Y <= (posChicken.Y + sizeChicken.Y / 2))
                    {
                        score += 1;
                        bullet.Hit();
                        chicken.Hurt();
                    }
                }

                if (!chicken.IsAlive)
                {
                    continue;
                }

                // 漏网之鸡将减少剩余生命值
                if (chicken.Position.Y >= 720)
                {
                    chicken.MakeInvalid();
                    soundHurt.Play();

                    hp -= 1;
                }
            }

            // 移除无效的子弹和僵尸鸡
            bulletList.RemoveAll(b => b.CanRemove());
            chickenList.RemoveAll(c => c.CanRemove());

            // 根据Y坐标排序，保证靠前的鸡先被绘制
            chickenList.Sort((c1, c2) => (c1.Position.Y - c2.Position.Y) switch
            {
                > 0 => 1,
                < 0 => -1,
                _ => 0
            });

            if (!isCoolDown)
            {
                camera.Shake(3.0f, 0.1f);
                animationBarrelFire.Update(delta);
            }

            if (isCoolDown && isFireKeyDown)
            {
                animationBarrelFire.Reset();
                isCoolDown = false;

                float lengthBarrel = 105;
                Vector2 posBarrelCenter = new(640, 610);

                var newBullet = new Bullet(textureBullet, angleBarrel);
                bulletList.Add(newBullet);
                var angleBullet = angleBarrel + (Random.Shared.Next(30) - 15);
                var radians = MathHelper.ToRadians(angleBarrel);
                var bulletDirection = new Vector2(MathF.Cos(radians), MathF.Sin(radians));
                newBullet.Position = posBarrelCenter + bulletDirection * lengthBarrel;

                switch (Random.Shared.Next(3))
                {
                    case 0: soundFire1.Play(); break;
                    case 1: soundFire2.Play(); break;
                    case 2: soundFire3.Play(); break;
                }
            }

            camera.Update(delta);

            if (hp <= 0)
            {
                MediaPlayer.Stop();
                MediaPlayer.Play(musicLoss);
                string msg = $"最终游戏得分: {score}";
                SDL3.SDL.SDL_ShowSimpleMessageBox(SDL3.SDL.SDL_MessageBoxFlags.SDL_MESSAGEBOX_INFORMATION, "游戏结束", msg, this.Window.Handle);
                Exit();
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            if (!IsActive) return;

            GraphicsDevice.Clear(Color.CornflowerBlue);

            camera.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            camera.BeginRender();
            int widthBg = textureBackground.Width;
            int heightBg = textureBackground.Height;
            var rectBgackground = new Rectangle(
                (1280 - widthBg) / 2,
                (720 - heightBg) / 2,
                widthBg,
                heightBg);
            camera.RenderTexture(textureBackground, null, rectBgackground, 0, Vector2.Zero);

            foreach (var chicken in chickenList)
            {
                chicken.Render(camera);
            }

            foreach (var bullet in bulletList)
            {
                bullet.Render(camera);
            }

            int widthBattery = textureBattery.Width;
            int heightBattery = textureBattery.Height;
            var rectBattery = new Rectangle(
                (int)(posBattery.X - widthBattery / 2.0f),
                (int)(posBattery.Y - heightBattery / 2.0f),
                widthBattery,
                heightBattery);
            camera.RenderTexture(textureBattery, null, rectBattery, 0, Vector2.Zero);

            int widthBarrel = textureBarrelIdle.Width;
            int heightBarrel = textureBarrelIdle.Height;
            var rectBarrel = new Rectangle(
                (int)posBarrel.X,
                (int)posBarrel.Y,
                widthBarrel,
                heightBarrel);

            if (isCoolDown)
            {
                camera.RenderTexture(textureBarrelIdle, null, rectBarrel, angleBarrel, centerBarrel);
            }
            else
            {
                animationBarrelFire.Angle = angleBarrel;
                animationBarrelFire.Render(camera);
            }

            int widthCrosshair = textureCrosshair.Width;
            int heightCrosshair = textureCrosshair.Height;
            var rectCrosshair = new Rectangle(
                (int)(posCrosshair.X - widthCrosshair / 2.0f),
                (int)(posCrosshair.Y - heightCrosshair / 2.0f),
                widthCrosshair,
                heightCrosshair);
            camera.RenderTexture(textureCrosshair, null, rectCrosshair, 0, Vector2.Zero);
            camera.EndRender();

            spriteBatch.Begin();
            int widthHeart = textureHeart.Width;
            int heightHeart = textureHeart.Height;
            for (int i = 0; i < hp; i++)
            {
                var rectDst = new Rectangle(15 + (widthHeart + 10) * i, 15, widthHeart, heightHeart);
                spriteBatch.Draw(textureHeart, rectDst, Color.White);
            }
            var scoreStr = $"SCORE: {score}";
            spriteBatch.DrawString(font, scoreStr, new Vector2(1280 - (scoreStr.Length - 1) * 15, 8), Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
