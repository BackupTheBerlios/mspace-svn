// created on 08/31/2004 at 13:24
namespace Muine.Sdk.Player
{

	public enum RepetitionType {
				    RepeatSong,
				    RepeatPlaylist
				    }
	
	public delegate void EOSEventHandler ();
	public delegate void TickEventHandler (int pos);
	public delegate void StateEventHandler (bool playing);
						
	public interface IPlayer {
	
	    bool Playing {get; set;}
	    int Position {get; set;}
	    int Volume {get; set;}
	    void Play ();
	    void Pause ();
	    void Stop ();
	    void Next ();
	    void Previous ();
	    void Repeat (RepetitionType type);
	    
	    event EOSEventHandler EOSEvent;
	    event TickEventHandler TickEvent;
	    event StateEventHandler StateEvent;
		
	}
}
