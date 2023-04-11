namespace space_shooter_game.states;

public abstract class StateClass {
    protected Game1 game;
    protected ContentManager content;
    protected GraphicsDevice graphics_device;
    protected KeyboardState prev_keyboard;
    protected StateClass(Game1 game, ContentManager content, GraphicsDevice graphics_device) {
        this.game = game;
        this.content = content;
        this.graphics_device = graphics_device;
    }
    public abstract void draw(SpriteBatch sprite_batch);
        
    public virtual void update(GameTime game_time) {
        // put at end of update()
        prev_keyboard = Keyboard.GetState();
    }
}