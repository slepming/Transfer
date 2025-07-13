# Video loading
Video is loaded using FFmpeg. More precisely, the program receives the path to the video. 
Then, using FFMpeg, the video is split into audio and video streams.
# FFmpeg arguments
FFmpeg arguments are collect in the FFmpegArgument class. You can change the arguments and their values.
# Config
The config file is named app.ini. IT contains settings for the UI and FFMpeg, such as codecs and bitrate. 

For optimal performance, you don't need to change config.

However, you can change the video codec. See the enums below.

```csharp
public enum AudioCodecs
{
    libmp3lame,
    eac3,
    ac3,
    libfdk_aac,
    libvorbis,
    aac
}

public enum VideoCodecs
{
    libx264,
    libx265,
    /* libvpx,
    libtheora,
    png,
    mpegts Будут удалены*/
}

public enum Profiles
{
    baseline,
    main,
    high
}

public enum Presets
{
    ultrafast,
    superfast,
    veryfast,
    faster,
    fast,
    medium,
    slow,
    slower,
    veryslow
}

```
