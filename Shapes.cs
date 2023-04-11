using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace space_shooter_game;

public class LineClass {
    private Texture2D texture;
    private Rectangle rect;
    private float angle;
    private bool centered;
    private int width;

    public LineClass(GraphicsDevice graphics_device, Color color, Rectangle rect, float angle) {
        this.centered = false;
        this.width = rect.Height;

        this.rect = rect;
        this.angle = angle;
        texture = new Texture2D(graphics_device, 1, 1);
        texture.SetData(new[] {color});
    }

    public LineClass(GraphicsDevice graphics_device, Color color, Vector2 pos0, Vector2 pos1, int width = 1, bool centered = true) {
        this.centered = centered;
        this.width = width;

        texture = new Texture2D(graphics_device, 1, 1);
        texture.SetData(new[] {color});

        // the width of the line can't be less then 1
        if (width < 1) {
            throw new ArgumentException("width cannot be less then 1");
        }

        set_line_with_points(pos0, pos1, centered, width);
    }

    private void set_line_with_points(Vector2 pos0, Vector2 pos1, bool centered, int width) {
        // gets the angle
        this.angle = (float)Math.Atan2(pos1.Y - pos0.Y, pos1.X - pos0.X);

        // get offset
        Vector2 offset;
        if (centered) {
            float new_angle = angle - (float)(Math.PI / 2);
            offset = new Vector2((float)Math.Cos(new_angle), (float)Math.Sin(new_angle)) * (width / 2);
        } else {
            offset = Vector2.Zero;
        }

        // find lenght of rect
        float lenght_x = pos0.X - pos1.X;
        float lenght_y = pos0.Y - pos1.Y;
        int lenght = (int)Math.Sqrt(lenght_x * lenght_x + lenght_y * lenght_y);

        // sets the rect and lenght of the rect
        this.rect = new Rectangle((int)pos0.X + (int)offset.X, (int)pos0.Y + (int)offset.Y, lenght, width);
    }

    public void draw(SpriteBatch sprite_batch) {
        sprite_batch.Draw(texture, rect, null, Color.White, angle, Vector2.Zero, SpriteEffects.None, 0);
    }

    public void change_angle(float radians) {
        angle += radians;
        if (angle > Math.Tau) {
            angle -= (float)Math.Tau * (int)(angle / Math.Tau);
        } else if (angle < -Math.Tau) {
            angle += (float)Math.Tau * (int)(Math.Abs(angle) / Math.Tau);
        }
    }

    public void set_angle(float radians) {
        angle = radians;
        if (angle > Math.Tau) {
            angle -= (float)Math.Tau * (int)(angle / Math.Tau);
        } else if (angle < -Math.Tau) {
            angle += (float)Math.Tau * (int)(Math.Abs(angle) / Math.Tau);
        }
    }

    public void set_points(Vector2 pos0, Vector2 pos1) {
        set_line_with_points(pos0, pos1, centered, width);
    }
}

public class CircleClass {
    private LineClass[] lines;
    private Vector2 position;
    private int radius;

    public CircleClass(GraphicsDevice graphics_device, Color color, Vector2 position, int radius, int num_of_points, int width = 1) {
        // if the width is less then 1 then make the circle full
        if (width < 1) {
            width = radius;
        }

        this.position = position;
        this.radius = radius;

        Vector2[] points;
        points = new Vector2[num_of_points];

        float incroment = (float)Math.Tau / num_of_points;
        for (int i = 0; i < num_of_points; i++) {
            float angle = incroment * i;
            points[i] = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * radius;
        }

        lines = new LineClass[num_of_points];
        for (int index = 0; index < num_of_points; index++) {
            int next_index = index + 1;
            if (next_index >= num_of_points) {
                next_index = 0;
            }
            lines[index] = new LineClass(graphics_device, color, position + points[index], position + points[next_index], width, false);
        }
    }


    public void draw(SpriteBatch sprite_batch) {
        foreach(LineClass line in lines) {
            line.draw(sprite_batch);
        }
    }
}