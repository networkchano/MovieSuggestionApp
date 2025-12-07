using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MovieSuggestionApp
{
    public static class AnimationManager
    {
        // Fade animation
        public static async void Fade(Control ctrl, float from, float to, int duration)
        {
            float step = (to - from) / 30f;

            for (int i = 0; i < 30; i++)
            {
                ctrl.BackColor = Color.FromArgb(
                    (int)(from + step * i),
                    ctrl.BackColor.R,
                    ctrl.BackColor.G,
                    ctrl.BackColor.B
                );

                await Task.Delay(duration / 30);
            }
        }

        // Move animation
        public static async void Move(Control ctrl, int targetX, int targetY, int duration)
        {
            int startX = ctrl.Left;
            int startY = ctrl.Top;

            int stepX = (targetX - startX) / 30;
            int stepY = (targetY - startY) / 30;

            for (int i = 0; i < 30; i++)
            {
                ctrl.Left += stepX;
                ctrl.Top += stepY;
                await Task.Delay(duration / 30);
            }
        }

        // Scale animation (fixed, only ONE version)
        public static async void Scale(Control ctrl, float scaleFrom, float scaleTo, int duration)
        {
            float step = (scaleTo - scaleFrom) / 30f;

            for (int i = 0; i < 30; i++)
            {
                float scale = scaleFrom + (step * i);

                ctrl.Width = (int)(ctrl.Width * scale);
                ctrl.Height = (int)(ctrl.Height * scale);

                await Task.Delay(duration / 30);
               

            }
        }
    }
}
