namespace space_shooter_game.entitys;

public class EnemyCircle : EnemyClass {
    public EnemyCircle(Texture2D ship_texture, Texture2D bullet_texture, Vector2 pos) : base(ship_texture, bullet_texture, pos) {
    }

    public override void shoot(Vector2 target, List<BulletClass> bullet_list) {
        if (cooldown == 0) {
            float incroment = (float)Math.Tau / 10;
            for (int i = 0; i < 10; i++) {
                bullet_list.Add(new BulletClass(bullet_texture, new Vector2(rect.X + ship_texture.Width / 2, rect.Y + ship_texture.Height / 2), incroment * i, 3));
            }
            cooldown = 120;
        }
    }
}