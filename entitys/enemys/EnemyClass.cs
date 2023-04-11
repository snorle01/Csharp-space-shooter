namespace space_shooter_game.entitys;

public abstract class EnemyClass {
    protected Texture2D bullet_texture;
    protected Texture2D ship_texture;
    protected int speed = 1;
    protected int cooldown = 0;
    public Rectangle rect;
    public int health = 10;

    public EnemyClass(Texture2D ship_texture, Texture2D bullet_texture, Vector2 pos) {
        this.ship_texture = ship_texture;
        this.bullet_texture = bullet_texture;
        rect = new Rectangle((int)pos.X, (int)pos.Y, ship_texture.Width, ship_texture.Height);
    }

    protected Vector2 get_center() {
        return new Vector2(rect.X + ship_texture.Width / 2, rect.Y + ship_texture.Height / 2);
    }

    public void move() {
        rect.X -= speed;
    }

    public abstract void shoot(Vector2 target, List<BulletClass> bullet_list);

    public void update(GameTime game_time) {
        if (cooldown > 0) {
            cooldown--;
        }
    }

    public void draw(SpriteBatch sprite_batch) {
        sprite_batch.Draw(ship_texture, rect, Color.White);
    }
}