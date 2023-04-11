namespace space_shooter_game.entitys.bosses;

public class Boss00 : BossClass {
    private float breaking = 0f;
    private float angle = 0;

    public Boss00(Texture2D ship_texture, Texture2D bullet_texture, Vector2 position) : base(ship_texture, bullet_texture, position) {
    }

    public override void reset() {
        angle = 0;
    }

    public override void set_health() {
        switch (lives) {
        case 3:
            max_health = 200;
            break;
        case 2:
            max_health = 200;
            break;
        case 1:
            max_health = 400;
            break;
        }
        health = max_health;
    }

    public override void shoot(List<BulletClass> bullet_list) {
        if (cooldown == 0) {
            switch (lives) {
            case 3:
                bullet_list.Add(new BulletClass(bullet_texture, get_center(), angle, 5));
                bullet_list.Add(new BulletClass(bullet_texture, get_center(), angle + (float)Math.PI, 5));
                angle += 0.1f;
                if (angle > Math.Tau) {
                    double prosent = random.NextDouble();
                    angle = 0.1f * (float)prosent;
                }
                break;

            case 2:
                angle = (float)Math.Tau / 25 * (float)random.NextDouble();
                float speed = 1f;
                for (int i = 0; i < 60; i++) {
                    bullet_list.Add(new BulletClass(bullet_texture, get_center(), angle, speed));
                    angle += (float)Math.Tau / 20;
                    speed += 0.1f;
                }
                cooldown = 30;
                break;

            case 1:
                for (int i = 0; i < 5; i++) {
                    // break line
                    float broken_angle;
                    double breaking_random = random.NextDouble();
                    if (breaking_random < 0.5) {
                        broken_angle = angle - (breaking * (float)breaking_random);
                    } else {
                        broken_angle = angle + (breaking * (float)breaking_random);
                    }

                    Vector2 spawn = new Vector2((float)Math.Cos(broken_angle), (float)Math.Sin(broken_angle)) * (i * 20);
                    bullet_list.Add(new BulletClass(bullet_texture, get_center() + spawn, broken_angle, 4));
                    spawn = new Vector2((float)Math.Cos(broken_angle + Math.PI), (float)Math.Sin(broken_angle + Math.PI)) * (i * 20);
                    bullet_list.Add(new BulletClass(bullet_texture, get_center() + spawn, broken_angle + (float)Math.PI, 4));
                }
                    
                angle += 0.1f;
                if (angle > Math.PI) {
                    angle = 0f;
                    breaking += 0.01f;
                }
                cooldown = 5;
                break;
            }
        }
    }
}