public class ButtonClass {
    private string text;
    private SpriteFont font;
    private Texture2D rect_texture;
    private Color color = Color.White;
    private MouseState current_mouse;
    private MouseState prev_mouse;
    private int padding = 10;
    public Rectangle rect;
    public event EventHandler click_event;

    public ButtonClass(string text, Vector2 pos, SpriteFont font, GraphicsDevice graphics_device, bool center = false) {
        this.text = text;
        this.font = font;
        rect = new Rectangle((int)pos.X, (int)pos.Y, (int)font.MeasureString(text).X + padding, (int)font.MeasureString(text).Y + padding);
        rect_texture = Func.make_rectangle(rect, color, graphics_device);
        if (center) {
            rect.X -= rect.Width / 2;
            rect.Y -= rect.Height / 2;
        }
    }

    public void update() {
        prev_mouse = current_mouse;
        current_mouse = Mouse.GetState();

        if (rect.Contains(current_mouse.X, current_mouse.Y)) {
            color = Color.Gray;
            if (current_mouse.LeftButton == ButtonState.Released && prev_mouse.LeftButton == ButtonState.Pressed) {
                click_event.Invoke(this, new EventArgs());
            }
        } else {
            color = Color.White;
        }
    }

    public void draw(SpriteBatch sprite_batch) {
        sprite_batch.Draw(rect_texture, new Vector2(rect.X, rect.Y), color);
        sprite_batch.DrawString(font, text, new Vector2(rect.X + padding / 2, rect.Y + padding / 2), Color.Black);
    } 
}