using System.Linq;

public static class Func {
    // makes a rectangle with a spesified color
    public static Texture2D make_rectangle(Rectangle rect, Color color, GraphicsDevice graphics_device) {
        Color[] colors = new Color[rect.Width * rect.Height];
        for(int pixel = 0; pixel < colors.Count(); pixel++) {
            colors[pixel] = color;
        }

        Texture2D texture = new Texture2D(graphics_device, rect.Width, rect.Height);
        texture.SetData(colors);
        return texture;
    }
    public static Texture2D make_rectangle(int width, int height, Color color, GraphicsDevice graphics_device) {
        Color[] colors = new Color[width * height];
        for(int pixel = 0; pixel < colors.Count(); pixel++) {
            colors[pixel] = color;
        }

        Texture2D texture = new Texture2D(graphics_device, width, height);
        texture.SetData(colors);
        return texture;
    }

    // makes a circle with a spesified color
    public static Texture2D make_circle(int radius, Color color, GraphicsDevice graphics_device) {
        int diamater = radius * 2;
        Color[] colors = new Color[diamater * diamater];
        for (int pixel = 0; pixel < colors.Count(); pixel++) {
            int y = pixel / diamater;
            int x = (pixel - y * diamater);
            if (get_distance(new Vector2(x, y), new Vector2(radius, radius)) < radius) {
                colors[pixel] = color;
            } else {
                colors[pixel] = new Color(0, 0, 0, 0);
            }
        }

        Texture2D texture = new Texture2D(graphics_device, diamater, diamater);
        texture.SetData(colors);
        return texture;
    }

    // gets the angle between 2 points
    public static double get_angle(Vector2 pos0, Vector2 pos1) {
        return Math.Atan2(pos1.Y - pos0.Y, pos1.X - pos0.X);
    }

    // gets the distance between 2 points
    public static double get_distance(Vector2 pos0, Vector2 pos1) {
        float lenght_x = pos0.X - pos1.X;
        float lenght_y = pos0.Y - pos1.Y;
        return Math.Sqrt(lenght_x * lenght_x + lenght_y * lenght_y);
    }
}