using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZombieShooter
{
    public partial class ZombieGame : Form
    {
        bool goLeft, goRight, goUp, goDown, gameOver;
        string facing = "up";
        int playerHealth = 100;
        int speed = 10;
        int ammo = 10;
        int zombieSpeed = 3;
        int score = 0;
        Random randNum = new Random();

        List<PictureBox> zombiesList = new List<PictureBox>();


        public ZombieGame()
        {
            InitializeComponent();
            RestartGame();
        }

        private void MainTimerEvent(object sender, EventArgs e)
        {
            if (playerHealth > 1)
            {
                HealthBar.Value = playerHealth;
            }
            else
            {
                gameOver = true;
                Player.Image = Properties.Resources.dead;
                GameTimer.Stop();
            }

            lblAmmo.Text = "Ammo: " + ammo;
            lblScore.Text = "Score: " + score;

            if (goLeft == true && Player.Left > 0)
            {
                Player.Left -= speed;
            }
            if (goRight == true && Player.Left + Player.Width < this.ClientSize.Width)
            {
                Player.Left += speed;
            }
            if (goUp == true && Player.Top > 40)
            {
                Player.Top -= speed;
            }
            if (goDown == true && Player.Top + Player.Height < this.ClientSize.Height)
            {
                Player.Top += speed;
            }

            foreach (Control x in this.Controls)
            {


                if(x is PictureBox && (string)x.Tag == "ammo")
                {
                    if (Player.Bounds.IntersectsWith(x.Bounds))
                    {
                        this.Controls.Remove(x);
                        //((PictureBox)x).Dispose();
                        x.Dispose();
                        ammo += 5;
                    }
                }

                if(x is PictureBox && (string)x.Tag == "zombie")
                {

                    if (Player.Bounds.IntersectsWith(x.Bounds))
                    {
                        playerHealth -= 1;
                    }

                    if (x.Left > Player.Left)
                    {
                        x.Left -= zombieSpeed;
                        ((PictureBox)x).Image = Properties.Resources.zleft;
                    }
                    if (x.Left < Player.Left)
                    {
                        x.Left += zombieSpeed;
                        ((PictureBox)x).Image = Properties.Resources.zright;
                    }

                    if(x.Top > Player.Top)
                    {
                        x.Top -= zombieSpeed;
                        ((PictureBox)x).Image = Properties.Resources.zup;
                    }
                    if (x.Top < Player.Top)
                    {
                        x.Top += zombieSpeed;
                        ((PictureBox)x).Image = Properties.Resources.zdown;
                    }
                }

                foreach(Control y in this.Controls)
                {
                    if(y is PictureBox && (string)y.Tag == "bullet" && x is PictureBox && (string)x.Tag == "zombie")
                    {
                        if (x.Bounds.IntersectsWith(y.Bounds))
                        {
                            score++;
                            this.Controls.Remove(y);
                            ((PictureBox)y).Dispose();
                            this.Controls.Remove(x);
                            ((PictureBox)x).Dispose();
                            zombiesList.Remove((PictureBox)x);
                            MakeZombies();
                        }
                    }
                }
            }
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (gameOver)
            {
                return;
            }

            if (e.KeyCode == Keys.Left)
            {
                goLeft = true;
                facing = "left";
                Player.Image = Properties.Resources.left; //Switching image shown
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = true;
                facing = "right";
                Player.Image = Properties.Resources.right;
            }
            if (e.KeyCode == Keys.Up)
            {
                goUp = true;
                facing = "up";
                Player.Image = Properties.Resources.up;
            }
            if (e.KeyCode == Keys.Down)
            {
                goDown = true;
                facing = "down";
                Player.Image = Properties.Resources.down;
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = false;
            }
            if (e.KeyCode == Keys.Up)
            {
                goUp = false;
            }
            if (e.KeyCode == Keys.Down)
            {
                goDown = false;
            }

            if (e.KeyCode == Keys.Space && ammo > 0 && !gameOver) //Don't like how this drops ammo based on being at 0, then you'll always replensish 5. Maybe base it off a timer?
            {
                ammo --;
                ShootBullet(facing);

                if(ammo < 1)
                {
                    DropAmmo();
                }
            }

            if(e.KeyCode == Keys.Enter && gameOver == true)
            {
                RestartGame();
            }
        }

        private void ShootBullet(string direction)
        {
            Bullet shot = new Bullet();
            shot.direction = direction;
            shot.bulletLeft = Player.Left + (Player.Width / 2);
            shot.bulletTop = Player.Top + (Player.Height / 2);
            shot.MakeBullet(this);
        }

        private void MakeZombies()
        {
            PictureBox zombie = new PictureBox();
            zombie.Tag = "zombie";
            zombie.Image = Properties.Resources.zdown;
            zombie.Left = randNum.Next(-50, 850);
            zombie.Top = randNum.Next(-50, 850);
            zombie.SizeMode = PictureBoxSizeMode.AutoSize;
            zombiesList.Add(zombie);
            this.Controls.Add(zombie);
            Player.BringToFront();
        }

        private void DropAmmo()
        {
            PictureBox ammo = new PictureBox();
            ammo.Tag = "ammo";
            ammo.Image = Properties.Resources.ammo_Image;
            ammo.Left = randNum.Next(10, this.ClientSize.Width - ammo.Width);
            ammo.Top = randNum.Next(60, this.ClientSize.Height - ammo.Height);
            this.Controls.Add(ammo);
            ammo.BringToFront();
            Player.BringToFront();
        }

        private void RestartGame()
        {
            Player.Image = Properties.Resources.up;
            foreach(PictureBox x in zombiesList)
            {
                this.Controls.Remove(x);
            }

            foreach(Control y in this.Controls)
            {
                if(y is PictureBox && (string)y.Tag == "ammo")
                {
                    this.Controls.Remove(y);
                    ((PictureBox)y).Dispose();
                }
            }

            zombiesList.Clear();

            for(int i = 0; i < 3; i++)
            {
                MakeZombies();
            }

            goUp = false;
            goDown = false;
            goLeft = false;
            goRight = false;

            gameOver = false;

            playerHealth = 100;
            score = 0;
            ammo = 10;

            GameTimer.Start();
        }
    }
}
