using System.ComponentModel;
using System.Diagnostics;
using System.Media;
using System.Runtime.Serialization;
using System.Runtime.Versioning;
using TextBlade.Core.Services;

namespace TextBlade.Plateform.Windows;

[SupportedOSPlatform("windows")]

public class WindowsSoundPlayer : ISoundPlayer
{
    private readonly SoundPlayer _soundPlayer = new();

    public void Load() => _soundPlayer.Load();

    // Wrap the method into a real async call
    public async Task LoadAsync(CancellationToken cancellationToken=default)
    {
        var tcs = new TaskCompletionSource<bool>(); // Used to signal the end of the loading operation

        AsyncCompletedEventHandler loadCompletedHandler = null!;
        loadCompletedHandler = (_, args) =>
        {
            if (args.Error != null)
            {
                tcs.TrySetException(args.Error); // Handle errors
            }
            else if (args.Cancelled)
            {
                tcs.TrySetCanceled(); // Handle cancellation
            }
            else
            {
                tcs.TrySetResult(true); // Signal completion
            }

            _soundPlayer.LoadCompleted -= loadCompletedHandler; // Unsubscribe
        };

        _soundPlayer.LoadCompleted += loadCompletedHandler;

        // Start the synchronous load operation
        _soundPlayer.LoadAsync();

        // Await until the completion source is set
        await using (cancellationToken.Register(() => tcs.TrySetCanceled(), useSynchronizationContext: false))
        {
            await tcs.Task; // Await the completion of the load operation
        }
    }

    public void Play() => _soundPlayer.Play();

    public void PlayLooping()
    {
        _soundPlayer.PlayLooping();
    }

    public void PlaySync()
    {
        _soundPlayer.PlaySync();
    }

    public void Stop()
    {
        _soundPlayer.Stop();
    }

    public bool IsLoadCompleted => _soundPlayer.IsLoadCompleted;

    public int LoadTimeout
    {
        get => _soundPlayer.LoadTimeout;
        set => _soundPlayer.LoadTimeout = value;
    }

    public string SoundLocation
    {
        get => _soundPlayer.SoundLocation;
        set => _soundPlayer.SoundLocation = value;
    }

    public Stream? Stream
    {
        get => _soundPlayer.Stream;
        set => _soundPlayer.Stream = value;
    }

    public object? Tag
    {
        get => _soundPlayer.Tag;
        set => _soundPlayer.Tag = value;
    }

    public event AsyncCompletedEventHandler? LoadCompleted
    {
        add => _soundPlayer.LoadCompleted += value;
        remove => _soundPlayer.LoadCompleted -= value;
    }

    public event EventHandler? SoundLocationChanged
    {
        add => _soundPlayer.SoundLocationChanged += value;
        remove => _soundPlayer.SoundLocationChanged -= value;
    }

    public event EventHandler? StreamChanged
    {
        add => _soundPlayer.StreamChanged += value;
        remove => _soundPlayer.StreamChanged -= value;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _soundPlayer.Dispose();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
