using System.Collections;

public class PlayerModel : System.Object {

	private uint _score;
	private string _name;
	private uint scoreMultiplier = 1;
	private bool _active = false;

	public PlayerModel()
	{

	}
	internal void Init(string name)
	{
		this._active = true;
		this._name = name;
	}
	internal bool Active
	{
		get{return _active;}
	}
	internal void IncreaseScore()
	{
		_score += 1 * scoreMultiplier;
	}
	internal string Name
	{
		get{return _name;}
		set{_name = value;}
	}

	internal uint Score
	{
		get{ return _score; }
	}
	internal void Deactivate()
	{
		_name = null;
		_active = false;
	}
}
