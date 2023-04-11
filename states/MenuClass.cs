using space_shooter_game.states.game;

namespace space_shooter_game.states;

public class MenuClass : StateClass {
    private List<ButtonClass> buttons = new List<ButtonClass>();

    public MenuClass(Game1 game, ContentManager content, GraphicsDevice graphics_device) : base(game, content, graphics_device){
        ButtonClass button = new ButtonClass("Start game", new Vector2(100, 100), content.Load<SpriteFont>("font"), graphics_device);
        button.click_event += start_game;
        buttons.Add(button);

        button = new ButtonClass("End game", new Vector2(100, 150), content.Load<SpriteFont>("font"), graphics_device);
        button.click_event += quit_button;
        buttons.Add(button);
    }

    public override void draw(SpriteBatch sprite_batch) {
        graphics_device.Clear(Color.SkyBlue);

        foreach (ButtonClass button in buttons) {
            button.draw(sprite_batch);
        }
    }

    public override void update(GameTime game_time) {
        foreach (ButtonClass button in buttons) {
            button.update();
        }
        base.update(game_time);
    }

     // button methods
     private void start_game(object sender, EventArgs e) {
         game.change_state(new GameClass(game, content, graphics_device), 60);
     }

    private void quit_button(object sender, EventArgs e) {
        game.Exit();
    }
}