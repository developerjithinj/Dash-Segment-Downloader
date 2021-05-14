# Dash-Segment-Downloader
C#.NET app to download dash segments from MPD file.

**HOW TO USE?**
![guide](https://user-images.githubusercontent.com/18112992/118232995-fa34b700-b4ae-11eb-8dfb-f35b44946c44.png)

1. Paste the link to a valid mpd file and press **Fetch** to list the tracks.
2. Click **Browse** to select output location.
3. Select the tracks to download.
4. Adjust the parallel download thread count if necessary.
5. Start Download by clicking **Download Segments**.
6. Once download is completed, start playback from you local server.


**IMPORTANT**
1. Not all mpd files are supported including the live streams.
2. If you are facing issues in downloads, reduce the download threads.
3. If you have only selected some tracks to download, manually edit the downloaded manifest file to remove the non-downloaded tracks or handled that in the player to avoid playback failures.
4. If you face CORS issues in playback, ensure that video source and player are using the same source. Or, adjust server CORS configuration if supported.


