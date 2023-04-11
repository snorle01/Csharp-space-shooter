using System.Linq;

namespace space_shooter_game.entitys;

public class PlayerClass {
    private Dictionary<string, Texture2D[]> ship_textures;
    private Texture2D rect_texture;
    private int speed = 6;
    private int cooldown = 0;
    private float shoot_angle = 0;
    public List<BulletClass> bullets = new List<BulletClass>();
    public Vector2 position;
    public int health = 10;

    // animation
    private int current_frame = 0;
    private int animation_speed = 10;
    private int animation_time = 0;
    private int max_frames;
    private string current_motion;

    // hurt animation
    public int hurt_timer = 0;

    public PlayerClass(Dictionary<string, Texture2D[]> ship_textures, Texture2D bullet_texture, Vector2 pos) {
        this.ship_textures = ship_textures;
        this.rect_texture = bullet_texture;
        position = pos;
        max_frames = ship_textures["straight"].Count();
        current_motion = "straight";
    }

    public void move(int screen_width, int screen_height) {
        int speed;
        if (Keyboard.GetState().IsKeyDown(Keys.LeftShift)) {
            speed = this.speed / 2;
        } else {
            speed = this.speed;
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Up) && Keyboard.GetState().IsKeyUp(Keys.Down)) {
            position.Y -= speed;
            if (position.Y < 0 + get_current_texture().Height / 2) {
                position.Y = 0 + get_current_texture().Height / 2;
            }
            if (!Keyboard.GetState().IsKeyDown(Keys.LeftShift)) {
                shoot_angle -= 0.04f;
                if (shoot_angle < -Math.PI / 4) {
                    shoot_angle = (float)-Math.PI / 4;
                }
            }
        } else if (Keyboard.GetState().IsKeyDown(Keys.Down) && Keyboard.GetState().IsKeyUp(Keys.Up)) {
            position.Y += speed;
            if (position.Y > screen_height - get_current_texture().Height / 2) {
                position.Y = screen_height - get_current_texture().Height / 2;
            }
            if (!Keyboard.GetState().IsKeyDown(Keys.LeftShift)) {
                shoot_angle += 0.04f;
                if (shoot_angle > Math.PI / 4) {
                    shoot_angle = (float)Math.PI / 4;
                }
            }
        } else if (!Keyboard.GetState().IsKeyDown(Keys.LeftShift)) {
            if (shoot_angle < 0.0f) {
                shoot_angle += 0.01f;
            }
            if (shoot_angle > 0.0f) {
                shoot_angle -= 0.01f;
            }
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Left)) {
            position.X -= speed;
            if (position.X < 0 + get_current_texture().Width / 2) {
                position.X = 0 + get_current_texture().Width / 2;
            }
        }
        if (Keyboard.GetState().IsKeyDown(Keys.Right)) {
            position.X += speed;
            if (position.X > screen_width - get_current_texture().Width / 2) {
                position.X = screen_width - get_current_texture().Width / 2;
            }
        }

        // shoot
        if (Keyboard.GetState().IsKeyDown(Keys.Z)  && cooldown == 0) {
            cooldown = 5;
            bullets.Add(new BulletClass(rect_texture, position, shoot_angle, 10));
        }
    }

    public void animate() {
        animation_time++;
        if (animation_time == animation_speed) {
            animation_time = 0;
            current_frame++;
            if (current_frame == max_frames) {
                current_frame = 0;
            }
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Up) && Keyboard.GetState().IsKeyUp(Keys.Down)) {
            current_motion = "up";
        } else if (Keyboard.GetState().IsKeyDown(Keys.Down) && Keyboard.GetState().IsKeyUp(Keys.Up)) {
            current_motion = "down";
        } else {
            current_motion = "straight";
        }
    }

    private Texture2D get_current_texture() {
        return ship_textures[current_motion][current_frame];
    }

    public void hurt(int time = 10) {
        if (hurt_timer == 0) {
            hurt_timer = time;
            health--;
        }
    }

    public void draw(SpriteBatch sprite_batch) {
        Vector2 draw_pos = new Vector2(position.X - get_current_texture().Width / 2, position.Y - get_current_texture().Height / 2);
        if (hurt_timer > 0) {
            sprite_batch.Draw(get_current_texture(), draw_pos, Color.Red);
        } else {
            sprite_batch.Draw(get_current_texture(), draw_pos, Color.White);
        }
        if (Keyboard.GetState().IsKeyDown(Keys.LeftShift)) {
            sprite_batch.Draw(rect_texture, new Vector2(position.X - rect_texture.Width / 2, position.Y - rect_texture.Height / 2), Color.Red);
        }
    }

    public void update(GameTime game_time) {
        if (cooldown > 0) {
            cooldown--;
        }
        if (hurt_timer > 0) {
            hurt_timer--;
        }
    }
}