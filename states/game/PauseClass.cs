namespace space_shooter_game.states;
public class PauseClass : StateClass {
    private Texture2D fade_texture;
    private StateClass prev_state;
    private SpriteFont font;
    private List<ButtonClass> buttons = new List<ButtonClass>(); 
    public PauseClass(Game1 game, ContentManager content, GraphicsDevice graphics_device, StateClass state) : base(game, content, graphics_device) {
        fade_texture = Func.make_rectangle(game.screen_width, game.screen_height, new Color(0, 0, 0, 100), graphics_device);
        font = content.Load<SpriteFont>("font");
        this.prev_state = state;

        ButtonClass button = new ButtonClass("Resume game", new Vector2(10, 50), font, graphics_device);
        button.click_event += resume_button;
        buttons.Add(button);

        button = new ButtonClass("Title", new Vector2(10, 100), font, graphics_device);
        button.click_event += title_button;
        buttons.Add(button);
    }

    public override void draw(SpriteBatch sprite_batch) {
        prev_state.draw(sprite_batch);
        sprite_batch.Draw(fade_texture, new Vector2(0, 0), Color.White);
        sprite_batch.DrawString(font, "Paused", new Vector2(10, 10), Color.White);
        foreach (ButtonClass button in buttons) {
            button.draw(sprite_batch);
        }
    }

    public override void update(GameTime game_time) {
        // pause
        if (prev_keyboard.IsKeyDown(Keys.Escape) && Keyboard.GetState().IsKeyUp(Keys.Escape)) {
            game.change_state(prev_state);
        }
        foreach (ButtonClass button in buttons) {
            button.update();
        }

        base.update(game_time);
    }

    private void resume_button(object sender, EventArgs e) {
        game.change_state(prev_state);
    }
    private void title_button(object sender, EventArgs e) {
        game.change_state(new MenuClass(game, content, graphics_device));
    }
}