using space_shooter_game.entitys;
using space_shooter_game.entitys.bosses;

namespace space_shooter_game.states.game;

public class GameClass : StateClass {
    private PlayerClass player;
    private BossClass boss;
    private List<EnemyClass> enemys = new List<EnemyClass>();
    private List<BulletClass> enemybullet_list = new List<BulletClass>();
    private List<ExplotionClass> explotion_list = new List<ExplotionClass>();
    private Random random = new Random();
    private int enemy_counter;
    private int level = 0;
    private int boss_timer_max = 60;
    private double boss_timer_seconds;

    // textures
    private SpriteFont font;
    private Dictionary<string, Texture2D> enemy_textures;
    private Texture2D exsplotion_texture; 
    private Texture2D boss_texture;
    private Texture2D boss_health_bar;
    private Texture2D enemy_bullet;

    public GameClass(Game1 game, ContentManager content, GraphicsDevice graphics_device) : base(game, content, graphics_device) {
        font = content.Load<SpriteFont>("font");
        enemy_counter = random.Next(60, 180);
        boss_timer_seconds = boss_timer_max;

        // get player textures
        Texture2D[] straight_textures = {content.Load<Texture2D>("ship/straight/ship1"),
                                         content.Load<Texture2D>("ship/straight/ship2"),
                                         content.Load<Texture2D>("ship/straight/ship3")};
        Texture2D[] up_textures = {content.Load<Texture2D>("ship/up/ship1"),
                                   content.Load<Texture2D>("ship/up/ship2"),
                                   content.Load<Texture2D>("ship/up/ship3")};
        Texture2D[] down_textures = {content.Load<Texture2D>("ship/down/ship1"),
                                     content.Load<Texture2D>("ship/down/ship2"),
                                     content.Load<Texture2D>("ship/down/ship3")};
        Dictionary<string, Texture2D[]> player_textures = new Dictionary<string, Texture2D[]>(){
            {"straight", straight_textures},
            {"up", up_textures},
            {"down", down_textures}
        };

        // enemy textures
        enemy_textures = new Dictionary<string, Texture2D>(){
            {"blue", content.Load<Texture2D>("blue_enemy")},
            {"red", content.Load<Texture2D>("red_enemy")}
        };
        exsplotion_texture = Func.make_circle(25, Color.White, graphics_device);
        // boss texture
        boss_texture = content.Load<Texture2D>("red_boss");

        enemy_bullet = Func.make_circle(5, Color.White, graphics_device);

        player = new PlayerClass(player_textures, Func.make_circle(5, Color.White, graphics_device), new Vector2(100, game.screen_height / 2));
    }

    public override void update(GameTime game_time) {
        // pause
        if (prev_keyboard.IsKeyDown(Keys.Escape) && Keyboard.GetState().IsKeyUp(Keys.Escape)) {
            game.change_state(new PauseClass(game, content, graphics_device, this));
        }

        // spawn boss
        if (boss == null) {
            boss_timer_seconds -= game_time.ElapsedGameTime.TotalSeconds;
            if (boss_timer_seconds <= 0) {
                boss = new Boss00(boss_texture, enemy_bullet, new Vector2(game.screen_width, game.screen_height / 2 - boss_texture.Height / 2));
                make_boss_health_bar();
                enemybullet_list.Clear();
                enemys.Clear();
            }
        }


        // spawn new enemy
        if (boss == null) {
            enemy_counter--;
            if (enemy_counter == 0) {
                enemy_counter = random.Next(60, 180);
                int choise = random.Next(0, level + 1);
                switch (choise) {
                case 0:
                    enemys.Add(new EnemyAim(enemy_textures["blue"], enemy_bullet, new Vector2(game.screen_width, random.Next(0, game.screen_height - enemy_textures["blue"].Height))));
                    break;
                case 1:
                    enemys.Add(new EnemyCircle(enemy_textures["red"], enemy_bullet, new Vector2(game.screen_width, random.Next(0, game.screen_height - enemy_textures["red"].Height))));
                    break;
                }
            }
        }

        // player bullet
        for (int bullet_index = 0; bullet_index < player.bullets.Count; bullet_index++) {
            BulletClass bullet = player.bullets[bullet_index];
            bullet.move();
            // bullet out of screen
            if (bullet.rect.X > game.screen_width || 
                bullet.rect.Y > game.screen_height || 
                bullet.rect.Bottom < 0) {
                    player.bullets.RemoveAt(bullet_index);
                    bullet_index--;
                    continue;
            }
            // checks if bullet should collide with enemys or boss
            if (boss == null) {
                // bullet collides with enemy
                for (int enemy_index = 0; enemy_index < enemys.Count; enemy_index++) {
                    EnemyClass enemy = enemys[enemy_index];
                    if (bullet.rect.Intersects(enemy.rect)) {
                        player.bullets.RemoveAt(bullet_index);
                        bullet_index--;
                        enemy.health--;
                        if (enemy.health == 0) {
                            explotion_list.Add(new ExplotionClass(graphics_device, new Vector2(enemy.rect.X + enemy.rect.Width / 2, enemy.rect.Y + enemy.rect.Height / 2), 25, 3f));
                            enemys.RemoveAt(enemy_index);
                            enemy_index--;
                        }
                        break;
                    }
                }
            } else if (boss != null) {
                // bullets collide with boss
                if (bullet.rect.Intersects(boss.rect)) {
                    player.bullets.RemoveAt(bullet_index);
                    bullet_index--;
                    boss.health--;
                    // check if the boss dies
                    if (boss.health == 0) {
                        boss.lives--;
                        enemybullet_list = new List<BulletClass>();
                        boss.set_health();
                        boss.reset();
                        // boss truly dies
                        if (boss.lives == 0) {
                            explotion_list.Add(new ExplotionClass(graphics_device, new Vector2(boss.rect.X + boss.rect.Width / 2, boss.rect.Y + boss.rect.Height / 2), 500, 10));
                            boss = null;
                            boss_timer_seconds = boss_timer_max;
                            level += 1;
                            enemybullet_list.Clear();
                        }
                    }
                    continue;
                }
            }
        }

        // player
        player.move(game.screen_width, game.screen_height);
        player.animate();
        player.update(game_time);
        if (player.health <= 0) {
            game.change_state(new GameOver(game, content, graphics_device));
        }

        // enemys bullet
        for (int bullet_index = 0; bullet_index < enemybullet_list.Count; bullet_index++) {
            BulletClass bullet = enemybullet_list[bullet_index];
            bullet.move();
            // bullet out of screen
            if (bullet.rect.Right < 0 ||
                bullet.rect.Bottom < 0 ||
                bullet.rect.X > game.screen_width ||
                bullet.rect.Y > game.screen_height) {
                    enemybullet_list.RemoveAt(bullet_index);
                    bullet_index--;
                    continue;
            }
            if (Func.get_distance(player.position, bullet.pos) < 5) {
                enemybullet_list.RemoveAt(bullet_index);
                bullet_index--;
                player.hurt();
                continue;
            }
        }

        // enemys
        for (int enemy_index = 0; enemy_index < enemys.Count; enemy_index++) {
            EnemyClass enemy = enemys[enemy_index];
            enemy.update(game_time);
            enemy.move();
            // enemy out off screen
            if (enemy.rect.X < 0 - enemy.rect.Width) {
                enemys.RemoveAt(enemy_index);
                enemy_index--;
                continue;
            } 
            enemy.shoot(player.position, enemybullet_list);
            // enemy collide with player
            if (enemy.rect.Contains(player.position)) {
                enemys.RemoveAt(enemy_index);
                enemy_index--;
                player.hurt();
            }
        }

        // boss
        if (boss != null) {
            make_boss_health_bar();
            if (boss.rect.Contains(player.position)) {
                player.hurt();
            }
            // boss is ready
            if (boss.ready) {
                boss.shoot(enemybullet_list);
                boss.update();
            // boss is not ready
            } else {
                boss.move();
                if (boss.rect.X < game.screen_width - boss.rect.Width) {
                    boss.ready = true;
                    boss.rect.X = game.screen_width - boss.rect.Width;
                }
            }
        }


        // exsplotions
        for (int index = 0; index < explotion_list.Count; index++) {
            ExplotionClass explotion = explotion_list[index];
            explotion.update(game_time);
            if (explotion.size > explotion.max_size) {
                explotion_list.RemoveAt(index);
                index--;
            }
        }

        base.update(game_time);
    }

    public override void draw(SpriteBatch sprite_batch) {
        graphics_device.Clear(Color.Black);

        // explotions
        foreach (ExplotionClass explotion in explotion_list) {
            explotion.draw(sprite_batch);
        }

        // player bullets
        foreach (BulletClass bullet in player.bullets) {
            bullet.draw(sprite_batch);
        }

        player.draw(sprite_batch);

        // enemys
        foreach (EnemyClass enemy in enemys) {
            enemy.draw(sprite_batch);
        }

        if (boss != null) {
            boss.draw(sprite_batch);
        }

        // enemy bullets
        foreach (BulletClass bullet in enemybullet_list) {
            bullet.draw(sprite_batch);
        }

        sprite_batch.DrawString(font, $"health {player.health}", new Vector2(game.screen_width - 100, 10), Color.White);
        sprite_batch.DrawString(font, $"Time {(int)boss_timer_seconds}", new Vector2(game.screen_width - 100, 30), Color.White);
        if (boss != null) {
            sprite_batch.Draw(boss_health_bar, new Vector2(5, game.screen_height - 15), Color.White);
        }
    }


    private void make_boss_health_bar() {
        float prosent_left = (float)boss.health / (float)boss.max_health;
        float health_bar_width = (game.screen_width - 10) * prosent_left;
        // color
        Color boss_color;
        if ((float)boss.health / (float)boss.max_health < 0.5) {
            float color_prosent = (float)boss.health / ((float)boss.max_health / 2f);
            boss_color = new Color(255, (int)(255 * color_prosent), (int)(255 * color_prosent));
        } else {
            boss_color = Color.White;
        }
        boss_health_bar = Func.make_rectangle(new Rectangle(0, 0, (int)health_bar_width, 10), boss_color, graphics_device);
    }
}

