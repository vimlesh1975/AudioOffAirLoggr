# AudioOffAirLoggr
<img width="1881" height="923" alt="image" src="https://github.com/user-attachments/assets/7f3eb976-1f1e-470c-b606-d246239500ee" />


AudioPlayer is a small Windows desktop player inspired by the DecklinkPlayer workflow, but simplified for full audio-file playback through the default system audio device.

## Features

- Folder browser, clip grid, and playlist grid for operator-style playback.
- Open and play common audio files through PC/system audio.
- Full-file waveform preview.
- Click or drag the waveform scrubber to seek.
- Play, pause, stop, position, and duration display.
- Save and open simple JSON playlists.
- No DeckLink dependency, no in/out points, and no trimming.
- Default media folder is `C:\casparcg\_media\audio` when it exists.

## Build

```powershell
dotnet build .\audioplayer.csproj -p:Platform=x64
```

The executable is written to:

```text
bin\x64\Debug\AudioPlayer
```

The build closes running `audioplayer*` processes, removes old player executables, creates a timestamped executable such as `audioplayer_160526_142000.exe`, deletes the plain `audioplayer.exe`, and opens the new timestamped build.

## Run

```powershell
.\bin\x64\Debug\AudioPlayer\audioplayer_DDMMYY_HHMMSS.exe
```
