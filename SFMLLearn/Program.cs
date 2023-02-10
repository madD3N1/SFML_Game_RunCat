using System;
using SFML.Learning;
using SFML.Graphics;
using SFML.Window;

class Program : Game
{
    static string backgroundTexture = LoadTexture("background.png");
    static string playerTexture = LoadTexture("player.png");
    static string foodTexture = LoadTexture("food.png");

    static string meowSound = LoadSound("meow_sound.wav");
    static string crashSound = LoadSound("cat_crash_sound.wav");
    static string bgMusic = LoadMusic("bg_music.wav");

    static float playerX = 100;
    static float playerY = 220;
    static int playerSize = 56;
    static float playerSpeed = 400;
    static int playerDirection = 1;

    static int playerScore = 0;

    static float foodX;
    static float foodY;
    static int foodSize = 32;

    static void PlayerMove()
    {
        if (GetKey(Keyboard.Key.W) == true) playerDirection = 0;
        if (GetKey(Keyboard.Key.D) == true) playerDirection = 1;
        if (GetKey(Keyboard.Key.S) == true) playerDirection = 2;
        if (GetKey(Keyboard.Key.A) == true) playerDirection = 3;

        if (playerDirection == 0) playerY -= playerSpeed * DeltaTime;
        if (playerDirection == 1) playerX += playerSpeed * DeltaTime; 
        if (playerDirection == 2) playerY += playerSpeed * DeltaTime; 
        if (playerDirection == 3) playerX -= playerSpeed * DeltaTime;
    }

    static void DrawPlayer()
    {
        if (playerDirection == 0) DrawSprite(playerTexture, playerX, playerY, 64, 64, playerSize, playerSize);
        if (playerDirection == 1) DrawSprite(playerTexture, playerX, playerY, 0, 0, playerSize, playerSize);
        if (playerDirection == 2) DrawSprite(playerTexture, playerX, playerY, 0, 64, playerSize, playerSize);
        if (playerDirection == 3) DrawSprite(playerTexture, playerX, playerY, 64, 0, playerSize, playerSize);
    }

    static void Main(string[] args)
    {
        InitWindow(800, 600, "Test");

        SetFont("comic.ttf");

        Random rnd = new Random();

        foodX = rnd.Next(0, 800 - foodSize);
        foodY = rnd.Next(200, 600 - foodSize);

        bool isLose = false;

        PlayMusic(bgMusic, 10);

        while (true)
        {
            // 1. Расчет
            DispatchEvents();

            if (isLose == false)
            {
                PlayerMove();

                if (playerX + playerSize >= foodX && playerX <= foodX + foodSize
                    && playerY + playerSize >= foodY && playerY <= foodY + foodSize)
                {
                    foodX = rnd.Next(0, 800 - foodSize);
                    foodY = rnd.Next(200, 600 - foodSize);

                    playerScore += 1;
                    playerSpeed += 10;

                    PlaySound(meowSound, 10);
                }

                if (playerX + playerSize > 800 || playerX < 0 || playerY + playerSize > 600 || playerY < 150)
                {
                    isLose = true;

                    PlaySound(crashSound, 50);
                }
            }

            if(isLose == true)
            {
                if(GetKeyDown(Keyboard.Key.R) == true)
                {
                    SetFillColor(Color.White);
                    isLose = false;
                    playerX = 100;
                    playerY = 220;
                    playerSpeed = 400;
                    playerDirection = 1;
                    playerScore = 0;
                }
            }
            // Игровая логика

            // 2. Очистка буфера и окно
            ClearWindow();

            // 3. Отрисовка буфера на окне

            DrawSprite(backgroundTexture, 0, 0);

            if(isLose == true)
            {
                SetFillColor(50, 50, 50);
                DrawText(150, 220, "Ну и чего ты носишься по кухне?!", 32);

                SetFillColor(70, 70, 70);
                DrawText(170, 270, "Нажми \"R\" чтобы перезапустить игру!", 26);
            }

            DrawPlayer();

            DrawSprite(foodTexture, foodX, foodY);

            SetFillColor(70, 70, 70);
            DrawText(20, 8, "Съедино корма: " + playerScore.ToString(), 18);

            DisplayWindow();

            // 4. Ожидание
            Delay(1);
        }
    }
}
