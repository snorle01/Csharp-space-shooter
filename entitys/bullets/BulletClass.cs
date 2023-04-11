namespace space_shooter_game.entitys;

public class BulletClass {
    private Texture2D texture;
    private float speed;
    private Vector2 movement;
    public Rectangle rect;
    public Vector2 pos; 
    public BulletClass(Texture2D texture, Vector2 pos, float angle, float speed) {
        this.texture = texture;
        this.pos = pos;
        this.speed = speed;
        rect = new Rectangle((int)pos.X, (int)pos.Y, texture.Width, texture.Height);
        movement = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
    }

    public void draw(SpriteBatch sprite_batch) {
        sprite_batch.Draw(texture, new Vector2(pos.X - texture.Width / 2, pos.Y - texture.Height / 2), Color.White);
    }

    public void move() {
        pos += movement * speed;
        rect.X = (int)pos.X;
        rect.Y = (int)pos.Y;
    }
}