using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZombieShooter
{
    class Bullet
    {
        public string direction;
        public int bulletLeft;
        public int bulletTop;

        private int speed = 15;
        private PictureBox bullet = new PictureBox();
        private Timer BulletTimer = new Timer();

        public void MakeBullet(Form form)
        {
            bullet.BackColor = Color.White;
            bullet.Size = new Size(5, 5);
            bullet.Tag = "bullet";
            bullet.Left = bulletLeft;
            bullet.Top = bulletTop;
            bullet.BringToFront();

            form.Controls.Add(bullet);

            BulletTimer.Interval = speed;
            BulletTimer.Tick += new EventHandler(BulletTimerEvent);
            BulletTimer.Start();
        }

        private void BulletTimerEvent(object sender, EventArgs e)
        {
            if(direction == "left")
            {
                bullet.Left -= speed;
            }
            if(direction == "right")
            {
                bullet.Left += speed;
            }
            if (direction == "up")
            {
                bullet.Top -= speed;
            }
            if(direction == "down")
            {
                bullet.Top += speed;
            }

            if(bullet.Left < 10 || bullet.Left > 810 || bullet.Top < -10 || bullet.Top > 810) //bullet goes out of bounds
            {
                BulletTimer.Stop();
                BulletTimer.Dispose();
                bullet.Dispose();
                BulletTimer = null;
                bullet = null;
            }
        }
    }
}
