using Godot;
using Rusty.ISA;
using System.Collections.Generic;

public static class IconAtlasser
{
    public static void ToFile(InstructionSet set, string path)
    {
        List<Texture2D> icons = new();
        foreach (var def in set.Definitions)
        {
            if (def.Icon == null)
                continue;
            icons.Add(def.Icon);
        }
        Texture2D tex = CombineTextures(icons);
        tex.GetImage().SavePng(path);
    }

    public static Texture2D CombineTextures(List<Texture2D> textures, int textureSize = 100, int texturesPerRow = 10)
    {
        if (textures == null || textures.Count == 0)
        {
            GD.PrintErr("No textures provided!");
            return null;
        }

        int totalTextures = textures.Count;
        int rows = Mathf.CeilToInt((float)totalTextures / texturesPerRow);

        int outputWidth = texturesPerRow * textureSize;
        int outputHeight = rows * textureSize;

        Image combinedImage = Image.CreateEmpty(outputWidth, outputHeight, false, Image.Format.Rgba8);

        for (int i = 0; i < totalTextures; i++)
        {
            Texture2D texture = textures[i];
            Image img = texture.GetImage();

            int x = (i % texturesPerRow) * textureSize;
            int y = (i / texturesPerRow) * textureSize;

            combinedImage.BlitRect(img, new Rect2I(0, 0, textureSize, textureSize), new Vector2I(x, y));
        }

        ImageTexture resultTexture = new ImageTexture();
        resultTexture.SetImage(combinedImage);

        return resultTexture;
    }
}
