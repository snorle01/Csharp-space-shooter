namespace space_shooter_game.entitys;

public abstract class BossClass {
    protected Texture2D ship_texture;
    protected Texture2D bullet_texture;
    protected int cooldown = 0;
    protected int speed = 5;
    protected Random random = new Random();
    public Rectangle rect;
    public int max_health;
    public int health;
    public int lives = 3;
    public bool ready = false;

    public BossClass(Texture2D ship_texture, Texture2D bullet_texture, Vector2 position) {
        this.ship_texture = ship_texture;
        this.bullet_texture = bullet_texture;
        rect = new Rectangle((int)position.X, (int)position.Y, ship_texture.Width, ship_texture.Height);
        set_health();
    }

    protected Vector2 get_center() {
        return new Vector2(rect.X + ship_texture.Width / 2, rect.Y + ship_texture.Height / 2);
    }

    // used to set varables to default after the boss dies and starts new attack 
    public abstract void reset();

    public void move() {
        rect.X -= speed;
    }

    public void update() {
        if (cooldown > 0) {
            cooldown--;
        }
    }

    public abstract void shoot(List<BulletClass> bullet_list);

    public abstract void set_health();

    public void draw(SpriteBatch sprite_batch) {
        sprite_batch.Draw(ship_texture, rect, Color.White);
    }
}