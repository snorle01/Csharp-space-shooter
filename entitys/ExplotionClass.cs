namespace space_shooter_game.entitys;

public class ExplotionClass {
    public Vector2 position;
    public int max_size = 50;
    public float size = 0;
    private GraphicsDevice graphics_device;
    private Texture2D texture;
    private float grow_rate;

    public ExplotionClass(GraphicsDevice graphics_device, Vector2 position, int max_size, float grow_rate) {
        this.graphics_device = graphics_device;
        this.position = position;
        this.max_size = max_size;
        this.grow_rate = grow_rate;
    }

    public void update(GameTime game_time) {
        size += grow_rate;
        if (size > 1) {
            texture = Func.make_circle((int)size, Color.White, graphics_device);
        } else {
            texture = Func.make_rectangle(1, 1, Color.White, graphics_device);
        }
    }

    public void draw(SpriteBatch sprite_batch) {
        Vector2 new_pos = new Vector2(position.X - texture.Width / 2, position.Y - texture.Height / 2);
        sprite_batch.Draw(texture, new_pos, Color.White);
    }
}