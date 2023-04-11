using space_shooter_game.states.game;

namespace space_shooter_game.states;

public class GameOver : StateClass {
    private SpriteFont font;
    private Vector2 text_size; 
    private List<ButtonClass> buttons = new List<ButtonClass>();
    public GameOver(Game1 game, ContentManager content, GraphicsDevice graphics_device) : base(game, content, graphics_device) {
        font = content.Load<SpriteFont>("font");
        text_size = font.MeasureString("Game over");

        ButtonClass button = new ButtonClass("Retry", new Vector2(game.screen_width / 2, 300), font, graphics_device, true);
        button.click_event += reset_button;
        buttons.Add(button);

        button = new ButtonClass("Title", new Vector2(game.screen_width / 2, 350), font, graphics_device, true);
        button.click_event += title_button;
        buttons.Add(button);
    }

    public override void draw(SpriteBatch sprite_batch) {
        graphics_device.Clear(Color.Red);

        sprite_batch.DrawString(font, "Game over", new Vector2(game.screen_width / 2 - text_size.X / 2, 100), Color.Black);

        foreach (ButtonClass button in buttons) {
            button.draw(sprite_batch);
        }
    }

    public override void update(GameTime game_time) {
        foreach (ButtonClass button in buttons) {
            button.update();
        }
    }

    // button methods
    private void reset_button(object sender, EventArgs e) {
        game.change_state(new GameClass(game, content, graphics_device), 60);
    }
    private void title_button(object sender, EventArgs e) {
        game.change_state(new MenuClass(game, content, graphics_device), 60);
    }
}