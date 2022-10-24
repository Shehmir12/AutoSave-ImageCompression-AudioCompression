using System;
using UnityEditor;
using UnityEngine;

public static class SetGame
{
    [MenuItem("Shehmir/Texture Optimization/Compression for Android #%_.")]
    static void ImageCompression()
    {

        var numChanges = 0;

        foreach (var guid in AssetDatabase.FindAssets("t:Texture", new String[] { "Assets" }))
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var importer = TextureImporter.GetAtPath(path) as TextureImporter; if (importer == null) continue;

            if (importer.npotScale != TextureImporterNPOTScale.None)
            {
                var def = importer.GetDefaultPlatformTextureSettings();
                var changed = false;
                var TextureFormate = TextureImporterFormat.ETC2_RGBA8Crunched;

                Action<TextureImporterPlatformSettings> maybeChange = (platSettings) =>
                {
                    if (
                        platSettings.format != TextureFormate ||
                        platSettings.compressionQuality != def.compressionQuality ||
                        platSettings.maxTextureSize != 1024 ||
                        !platSettings.overridden
                    )
                    {
                        platSettings.format = TextureFormate;
                        platSettings.compressionQuality = def.compressionQuality;
                        platSettings.maxTextureSize = 1024;
                        platSettings.overridden = true;

                        changed = true;
                        importer.SetPlatformTextureSettings(platSettings);
                    }
                };

                maybeChange(importer.GetPlatformTextureSettings("iPhone"));
                maybeChange(importer.GetPlatformTextureSettings("Android"));

                if (changed)
                {
                    importer.SaveAndReimport();
                    ++numChanges;
                }
            }
            else
            {
                var def = importer.GetDefaultPlatformTextureSettings();
                var changed = false;
                var TextureFormate = TextureImporterFormat.ASTC_6x6;
                Action<TextureImporterPlatformSettings> maybeChange = (platSettings) =>
                {
                    if (
                        platSettings.format != TextureFormate ||
                        platSettings.compressionQuality != def.compressionQuality ||
                        platSettings.maxTextureSize != 1024 ||
                        !platSettings.overridden
                    )
                    {
                        platSettings.format = TextureFormate;
                        platSettings.compressionQuality = def.compressionQuality;
                        platSettings.maxTextureSize = 1024;
                        platSettings.overridden = true;

                        changed = true;
                        importer.SetPlatformTextureSettings(platSettings);
                    }
                };

                maybeChange(importer.GetPlatformTextureSettings("iPhone"));
                maybeChange(importer.GetPlatformTextureSettings("Android"));

                if (changed)
                {
                    importer.SaveAndReimport();
                    ++numChanges;
                }
            }



        }

        Debug.Log(String.Format("Image Compression: {0} images updated", numChanges));
    }
    [MenuItem("Shehmir/Audio Optimization/Compression for Android")]
    static void SoundCompressor()
    {
        var numChanges = 0;
        foreach (var guid in AssetDatabase.FindAssets("t:AudioClip", new String[] { "Assets" }))
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var importer = AudioImporter.GetAtPath(path) as AudioImporter;
            if (importer == null) continue;
            var def = importer.defaultSampleSettings;
            var changed = false;
            var AudioFormate = AudioClipLoadType.DecompressOnLoad;
            importer.preloadAudioData = true;
            //var AudioCompressionFormate = AudioType.OGGVORBIS;

            Action<AudioImporterSampleSettings> maybeChange = (platSettings) =>
            {
                if (platSettings.quality != def.quality || platSettings.conversionMode != 60)
                {
                    platSettings.compressionFormat = AudioCompressionFormat.Vorbis;
                    platSettings.quality = def.quality;
                    platSettings.conversionMode = AudioSettings.outputSampleRate;


                    changed = true;
                    importer.SetOverrideSampleSettings("", platSettings);
                }
            };

            maybeChange(importer.GetOverrideSampleSettings("iPhone"));
            maybeChange(importer.GetOverrideSampleSettings("Android"));

            if (changed)
            {
                importer.SaveAndReimport();
                ++numChanges;
            }
        }
    }
}