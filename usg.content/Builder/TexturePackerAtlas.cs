using System.Text.Json;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using MonoGame.Extended.Content.ContentReaders;
using MonoGame.Extended.Content.TexturePacker;
using MonoGame.Extended.Graphics;

// TexturePacker JSON -> Texture2DAtlas, built against MonoGame 3.8.6.
// Writes the exact layout MonoGame.Extended's Texture2DAtlasReader expects
// (see MonoGame.Extended v6.1.1 TexturePackerWriter.cs / Texture2DAtlasReader.cs).

public sealed class AtlasContent
{
    public required string ImageAssetName { get; init; }
    public required List<AtlasRegion> Regions { get; init; }
}

public sealed record AtlasRegion(int X, int Y, int Width, int Height, string Name, int Rotated,
    TexturePackerSize? Size, TexturePackerPoint? Offset, TexturePackerPointF? Pivot);

[ContentImporter(".json", DefaultProcessor = nameof(TexturePackerAtlasProcessor), DisplayName = "TexturePacker JSON Importer - usg")]
public class TexturePackerAtlasImporter : ContentImporter<TexturePackerFileContent>
{
    public override TexturePackerFileContent Import(string filename, ContentImporterContext context)
    {
        using var stream = File.OpenRead(filename);
        return JsonSerializer.Deserialize<TexturePackerFileContent>(stream)
               ?? throw new InvalidContentException($"Could not parse TexturePacker JSON: {filename}");
    }
}

[ContentProcessor(DisplayName = "TexturePacker Atlas Processor - usg")]
public class TexturePackerAtlasProcessor : ContentProcessor<TexturePackerFileContent, AtlasContent>
{
    public override AtlasContent Process(TexturePackerFileContent input, ContentProcessorContext context)
    {
        // "monogame-extended" data format: sheet + frames live under "textures".
        if (input.Textures is { Count: > 0 })
        {
            var tex = input.Textures[0];
            var regions = (tex.Frames ?? new()).Select(kv => new AtlasRegion(
                kv.Value.Frame.X, kv.Value.Frame.Y, kv.Value.Frame.Width, kv.Value.Frame.Height,
                kv.Key, kv.Value.Rotated, kv.Value.Size, kv.Value.Offset, kv.Value.Pivot)).ToList();
            return new AtlasContent { ImageAssetName = Path.ChangeExtension(tex.FileName, null), Regions = regions };
        }

        // Generic TexturePacker "JSON (Array)" format: top-level "frames" + "meta.image".
        if (input.Regions is { Count: > 0 } && input.Meta?.Image != null)
        {
            var regions = input.Regions.Select(r => new AtlasRegion(
                r.Frame.X, r.Frame.Y, r.Frame.Width, r.Frame.Height,
                Path.ChangeExtension(r.FileName, null), 0, null, null, null)).ToList();
            return new AtlasContent { ImageAssetName = Path.ChangeExtension(input.Meta.Image, null), Regions = regions };
        }

        throw new InvalidContentException("TexturePacker JSON contains no textures or frames.");
    }
}

[ContentTypeWriter]
public class TexturePackerAtlasWriter : ContentTypeWriter<AtlasContent>
{
    protected override void Write(ContentWriter output, AtlasContent value)
    {
        output.Write(value.ImageAssetName);
        output.Write(value.Regions.Count);
        foreach (var r in value.Regions)
        {
            output.Write(r.X);
            output.Write(r.Y);
            output.Write(r.Width);
            output.Write(r.Height);
            output.Write(r.Name);
            output.Write(r.Rotated);
            output.Write(r.Size != null);          // trimmed?
            if (r.Size != null)
            {
                output.Write(r.Size.Width);
                output.Write(r.Size.Height);
                output.Write(r.Offset?.X ?? 0);   // missing offset = no trim offset
                output.Write(r.Offset?.Y ?? 0);
            }
            output.Write(r.Pivot != null);
            if (r.Pivot != null)
            {
                output.Write(r.Pivot.X);
                output.Write(r.Pivot.Y);
            }
        }
    }

    public override string GetRuntimeType(TargetPlatform targetPlatform) => typeof(Texture2DAtlas).AssemblyQualifiedName!;
    public override string GetRuntimeReader(TargetPlatform targetPlatform) => typeof(Texture2DAtlasReader).AssemblyQualifiedName!;
}
