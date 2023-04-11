using space_shooter_game.states;

namespace space_shooter_game;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private StateClass current_state;

    public int screen_width;
    public int screen_height;

    // screen transition
    private Texture2D fade_texture;
    private StateClass change_state_to;
    private bool changing_state = false;
    private bool fade_out = false;
    private bool fade_in = false;
    private int change_time;
    private int max_change_time;

    public Game1() {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize() {
        current_state = new MenuClass(this, Content, GraphicsDevice);

        screen_width = _graphics.PreferredBackBufferWidth;
        screen_height = _graphics.PreferredBackBufferHeight;

        base.Initialize();
    }

    protected override void LoadContent() {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
    }

    protected override void Update(GameTime gameTime) {
        // changing screen with fade in and out
        if (changing_state) {
            change_time--;
            if (fade_out) {
                float alpha = 255 - 255 * change_time / max_change_time;
                make_fade_texture((int)alpha);
                if (change_time <= 0) {
                    current_state = change_state_to;
                    change_time = max_change_time;
                    fade_out = false;
                    fade_in = true;
                }
            } else if (fade_in) {
                float alpha = 255 * change_time / max_change_time;
                make_fade_texture((int)alpha);
                if (change_time <= 0) {
                    current_state = change_state_to;
                    fade_in = false;
                    changing_state = false;
                }
            }
        }
        current_state.update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
        _spriteBatch.Begin();
        current_state.draw(_spriteBatch);
        
        if (changing_state) {
            _spriteBatch.Draw(fade_texture, new Vector2(0, 0), Color.White);
        }
        _spriteBatch.End();

        base.Draw(gameTime);
    }

    public void change_state(StateClass state, int time = 0) {
        if (time > 0) {
            changing_state = true;
            fade_out = true;
            make_fade_texture(0);
            change_state_to = state;
            max_change_time = time / 2;
            change_time = max_change_time;
        } else {
            current_state = state;
        }
    }

    private void make_fade_texture(int alpha) {
        fade_texture = Func.make_rectangle(new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), new Color(0,0,0,alpha), GraphicsDevice);
    }
}
