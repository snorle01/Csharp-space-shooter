namespace space_shooter_game.entitys;

public class EnemyAim : EnemyClass {
    public EnemyAim(Texture2D ship_texture, Texture2D bullet_texture, Vector2 pos) : base(ship_texture, bullet_texture, pos) {}

    public override void shoot(Vector2 target, List<BulletClass> bullet_list) {
        if (cooldown == 0) {
            float angle = (float)Func.get_angle(get_center(), target);
            bullet_list.Add(new BulletClass(bullet_texture, new Vector2(rect.X + ship_texture.Width / 2, rect.Y + ship_texture.Height / 2), angle, 3));
            cooldown = 120;
        }
    }
}