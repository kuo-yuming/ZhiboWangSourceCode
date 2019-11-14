using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using GameCore.Manager.Jurassic;
using System.Linq;

public class SlotManager : MonoBehaviour {

	public List<Line> Lines = new List<Line> ();
	public List<Reel> Reels = new List<Reel> ();
	public List<GameObject> Icons = new List<GameObject> ();
	public List<UISprite> FreeNums = new List<UISprite> ();
	public GameObject freeLogo;
	public GameObject logo;
	public GameObject combosprite;
	//public static List<int> slotLines = new List<int>();
	//public static List<int> slotSymbol = new List<int>();
	public static SlotData slotdata = new SlotData();
	//public static CPACK_Jurassic_BetResult RewardData = null;
	public static int RewardMoney = 0;
	public static UInt64 playerMoney;
	public ExcitedAnim comboAnim;
	public UISprite BackGround;
	public static Action startSpin;
	public static Action OnChange;
	bool buttonset = false;
	public AudioSource palyOnceSound;
	public List<AudioClip> audio = new List<AudioClip>();
	//public static Action stopSpin;
	//bool slotset;
	private static SlotManager _instance;
	public static SlotManager instance{
		get{
			return _instance;
		}
	}
	float waitTime = 2f;
	/// <summary>
	/// Slot data.
	/// </summary>
	public class SlotData{
		public  List<int> slotLines = new List<int>();
		public  List<int> slotSymbol = new List<int>();
		public int slotCombo = 0;
		public bool excited = false;
		public bool fakeExcited = false;
		public bool slotSet = false;
		public bool JackPot = false;
		public bool isUse = false;
		public bool UseItem = false;
		public SlotData(List<int> line = null,List<int> symbol = null)
		{
			this.slotLines = line;
			this.slotSymbol = symbol;
			this.slotSet = false;
			this.slotCombo = 0;
			this.excited = false;
			this.UseItem = false;
		}
		/// <summary>
		/// SetData(lineList,symbolList,ComboNum)
		/// </summary>
		/// <param name="line">Line.</param>
		/// <param name="symbol">Symbol.</param>
		/// <param name="combo">Combo.</param>
		public void SetData(List<int> line,List<int> symbol, int combo)
		{
			this.slotLines = line;
			this.slotSymbol = symbol;
			this.slotSet = true;
			this.slotCombo = combo;
			Debug.Log ("getpackage");

		}
		/// <summary>
		/// SetExcited(justplayanimation,playexcited) just chose one.
		/// </summary>
		/// <param name="fake">If set to <c>true</c> fake.</param>
		/// <param name="excit">If set to <c>true</c> excit.</param>
		public void SetExcited(bool fake,bool excit)
		{
			this.excited = excit;
			this.fakeExcited = fake;
		}

		public void ClearData()
		{
			this.slotLines.Clear();
			this.slotSymbol.Clear();
			this.slotSet = false;
			this.slotCombo = 0;
			this.excited = false;
			this.fakeExcited = false;
			this.JackPot = false;
			this.UseItem = false;
			Debug.Log ("clearpackage");
		}

	}
		
	float _countDown = 0;

	float countDown {
		get{
			return _countDown;
		}
		set{
			_countDown = value;
			if (_countDown < 0f) {
				instance.StartCoroutine (CheckSlot());
				_countDown = 0f;
			}
		}
	}

	public enum State
	{
		Idle,
		spining,
		animationing,
		JackPot,
		excited,
		freeGame,
	}

	private static State _state;

	public static State lastState = State.spining;

	public static State slotState{
		get{
			return _state;
		}
		set{
			if (_state == value)
				return;
			_state = value;
			switch(value)
			{
			case State.Idle:
				break;
			case State.spining:
				lastState = value;
				break;
			case State.excited:
				lastState = value;
				break;
			case State.JackPot:
				lastState = value;
				break;
			case State.freeGame:
				lastState = value;
				break;
			}
			//Debug.LogWarning (value);
			//BugUse.AddMessage("Change State to " + value.ToString());
				JurassicUIManager.instance.stopBut.SetState (UIButtonColor.State.Normal, false);
				JurassicUIManager.instance.startButton.SetActive (value == State.Idle);
				JurassicUIManager.instance.StopButton.SetActive (value != State.Idle);	
		}
	}

	IEnumerator Start()
	{
		if (_instance == null)
			_instance = this.gameObject.GetComponent<SlotManager> ();
		while (Sound_Control.Instance.Jurassic_Sound.SlotMoving == null)
			yield return null;
		audio [0] = Sound_Control.Instance.Jurassic_Sound.SlotMoving;
		audio [1] = Sound_Control.Instance.Jurassic_Sound.SlotStop;
		audio [2] = Sound_Control.Instance.Jurassic_Sound.NoAward;
		audio [3] = Sound_Control.Instance.Jurassic_Sound.LineAward;
		audio [4] = Sound_Control.Instance.Jurassic_Sound.FourLineAward;
		audio [5] = Sound_Control.Instance.Jurassic_Sound.FiveLineAward;
		audio [6] = Sound_Control.Instance.Jurassic_Sound.JP;
		audio [7] = Sound_Control.Instance.Jurassic_Sound.InCombo;
		audio [8] = Sound_Control.Instance.Jurassic_Sound.ComboBGM;
		audio [9] = Sound_Control.Instance.Jurassic_Sound.AwardCombo;
		audio [10] = Sound_Control.Instance.Jurassic_Sound.EndCombo;
		AllScenceLoad.LoadScence = false;

	}
	public int testcombo;
	public bool testexcited;
	public bool textexcit;
	public bool JP;
	void Update()
	{
		if (countDown > 0)
			countDown -= Time.deltaTime;
		combosprite.SetActive (lastState != State.spining);
	}

	/// <summary>
	/// set reward anmiation
	/// </summary>
	/// <returns>The line.</returns>
	/// <param name="level">Level.</param>
	public IEnumerator SetLine(List<int> level)
	{
		while (Reels [Reels.Count - 1].state != Reel.State.Idle)
			yield return null;
		yield return new WaitForSeconds (0.1f);
		int i = 0;
		foreach (Line line in Lines) {
			line.ShowLevel (slotdata.slotLines[i]);
			i++;
		}
		if (RewardMoney > 0 && !slotdata.JackPot) {
			bool four = false;
			bool five = false;
			foreach (int num in level) {
				if (num == 4)
					four = true;
				if (num == 5)
					five = true;
			}

			if (five)
				AudioSource.PlayClipAtPoint (instance.audio [5], transform.position);
			else if (four && five == false)
				AudioSource.PlayClipAtPoint (instance.audio [4], transform.position);
			else
				AudioSource.PlayClipAtPoint (instance.audio [3], transform.position);
		} else if (slotdata.JackPot) {
			AudioSource.PlayClipAtPoint (instance.audio [6], transform.position);
		} else {
			AudioSource.PlayClipAtPoint (instance.audio[2],transform.position);
		}


		if (slotdata.JackPot) {
			JackPot.instance.SetJP (0);
			JackPotAnim.instance.play ();

		}else if (RewardMoney > 0) {
			Jurassic_WinMoney.instance.WinMoney = (long)RewardMoney;
			Jurassic_WinMoney.instance.StartPlay = true;
			canClearData = true;
		}

		slotState = State.Idle;
		if(slotdata.UseItem)
			instance.SetItem ();
		//JurassicUIManager.instance.RewardMoney = playerMoney;
	}


	public void SetItem()
	{
		if (slotdata.isUse && lastState == State.spining) {
			Jurassic_GameUIBag.ItemOnClick = false;
			Jurassic_GameUIItem.m_ItemClose = true;
			Jurassic_GameUIItem.instance.CloseItem ();
		}
	}

	public bool canClearData = false;
	public void SetMoney()
	{
		if (!canClearData) {
			//BugUse.AddMessage ("Can't Clear");
			return;
		}
		if(RewardMoney > 0)
			JurassicUIManager.instance.RewardMoney = playerMoney;
		instance.StopLine ();
		if (slotdata.slotSet) {
			//BugUse.AddMessage ("Clear package");
			slotdata.ClearData ();
		}
		Jurassic_WinMoney.instance.Reseat = true;
		canClearData = false;
	}

	public void StopLine()
	{
		Lines.ForEach(x => x.LightStop());
	}

	public void SetSlot(List<int> symbol)
	{
		int i = 0;
		if(lastState == State.freeGame)
			FreeGameNumber = slotdata.slotCombo;
		foreach (Reel reel in Reels) {
			reel.target = symbol[i] ;
			i++;
		}
	}

	/// <summary>
	/// shose stop control
	/// </summary>
	/// <returns>The slot.</returns>
	public static IEnumerator CheckSlot()
	{
		while (!slotdata.slotSet && !JurassicUIManager.instance.Test) {
			//BugUse.AddMessageError ("Not Get Package");
			Debug.LogWarning (!slotdata.slotSet && !JurassicUIManager.instance.Test);
			yield return null;
		}
		instance.buttonset = true;
		JurassicUIManager.instance.stopCollider.enabled = !instance.buttonset;
		JurassicUIManager.instance.stopBut.SetState (UIButtonColor.State.Disabled,false);
		//SlotManager.slotdata.slotSet = false;
		instance.SetSlot (SlotManager.slotdata.slotSymbol);
		//Debug.LogWarning ("CheckSlot=" + slotState);
		switch(slotState)
		{
		case State.freeGame:
			instance.StartCoroutine (instance.StopFreeGameSpin());
			SlotManager.instance.palyOnceSound.Stop ();
			break;
		case State.spining:
			instance.StopRandSpin ();
			SlotManager.instance.palyOnceSound.Stop ();
			break;
		case State.excited:
			instance.StartCoroutine (instance.StopExcitedSpin());
			SlotManager.instance.palyOnceSound.Stop ();
			break;
		}
	}
		
	public void StartRandSpin()
	{
		instance.StopLine ();
		if (JackPot.isSet)
			JackPot.instance.ReSetJP ();
		Lines [9].LineIcon.ForEach (x => x.SetActive (false));
		if (startSpin != null)
			startSpin ();
		//BugUse.AddMessage ("StartSpin");
		if(lastState != State.excited)
			SlotManager.instance.palyOnceSound.Play();
		slotState = lastState;
		instance.countDown = instance.waitTime;
	}

	/// <summary>
	/// Check Excited mod
	/// </summary>
	public void StopRandSpin()
	{
		_countDown = 0f;
		if (slotdata.excited == true || slotdata.fakeExcited == true) {
			instance.StartCoroutine (FakeExcitedStop());
		} else {
			instance.StartCoroutine (stopSpin ());
		}
	}


	IEnumerator FakeExcitedStop()
	{

		for (int i = 0; i < (Reels.Count - 1); i++) {
			Reels [i].StopSpin ();
			yield return new WaitForSeconds (0.01f);
		}
		AudioSource.PlayClipAtPoint (instance.audio[7],transform.position);
		for (int i = 0; i < 4; i++) {
			Lines [9].LineIcon [i].SetActive (true);	
			Lines [9].icon [i].LockPlay ();
		}

		yield return new WaitForSeconds (2f);

		if (slotdata.excited) {
			Reels [Reels.Count - 1].StopSpin ();
			//set last symbol
			Lines [9].LineIcon [4].SetActive (true);
			Lines [9].icon [4].LockPlay ();
			//
			yield return new WaitForSeconds (2f);
			Lines [9].LineIcon.ForEach (x => x.SetActive (false));
			comboAnim.playExcited ();
			slotState = State.excited;
			if(!AutoSpin.instance.AutoSet)
				AutoSpin.instance.AutoSpins ();
//			while (slotState != State.Idle)
//				yield return null;
//			comboAnim.StopView (combo,total);
//			comboAnim.Stop ();

		} else {
			Reels [Reels.Count - 1].StopSpin ();
			yield return new WaitForSeconds (1.5f);
			slotState = State.Idle;
		}
		//slotState = State.Idle;
	}
		
	/// <summary>
	/// checkfreegame.
	/// </summary>
	/// <returns>The spin.</returns>
	public IEnumerator stopSpin()
	{
		foreach (Reel reel in instance.Reels) {
			reel.StopSpin ();
			yield return new WaitForSeconds (0.01f);
		}


		if (slotdata.slotCombo > 0) {
			//Debug.LogWarning ("InfreeGame");
			yield return new WaitForSeconds (0.5f);

			for (int i = 0; i < Reels.Count; i++) {
				if (Reels [i].target == 9) {
					Reels [i].PlayDiamondAnimation ();
					Icons [i].SetActive (true);
				}
			}

			yield return new WaitForSeconds (2.0f);

			foreach (var obj in Icons) {
				obj.SetActive (false);
			}

			foreach (var obj in Reels) {
				obj.StopSymbolAnimation ();
			}
			slotState = State.freeGame;
			if (OnChange != null)
				OnChange ();
			SlotManager.instance.freeLogo.SetActive (true);
			SlotManager.instance.logo.SetActive (false);
			FreeGameNumber = slotdata.slotCombo;
			slotState = State.Idle;
			if (!AutoSpin.instance.AutoSet)
				AutoSpin.instance.AutoSpins ();
		} 
		else {
			instance.StartCoroutine (SetLine (slotdata.slotLines));
		}
	}
		
	int combo = 0;
	int total = 0;

	/// <summary>
	/// ComboAnmiation Control
	/// </summary>
	/// <returns>The excited spin.</returns>
	public IEnumerator StopExcitedSpin()
	{
		foreach (Reel reel in instance.Reels) {
			reel.StopSpin ();
			yield return new WaitForSeconds (0.01f);
		}

		if (!slotdata.excited) {
			lastState = State.spining;
			yield return new WaitForSeconds (1.0f);
			comboAnim.AminSource.Stop ();
			comboAnim.StopView (combo,total);
			combo = total = 0;
			instance.SetItem ();
		} else {
			total += (int)RewardMoney;
			foreach (Reel reel in instance.Reels) {
				reel.PlaySymbolAnimation ();
			}
			AudioSource.PlayClipAtPoint (SlotManager.instance.audio [9], this.transform.position);
			comboAnim.SetInfo (++combo,total);
			yield return new WaitForSeconds (2.0f);
			foreach (Reel reel in instance.Reels) {
				reel.StopSymbolAnimation ();
			JurassicUIManager.instance.RewardMoney = playerMoney;
			slotState = State.Idle;
			}
		}

	}

	/// <summary>
	/// freegame number show
	/// </summary>
	/// <value>The free game number.</value>
	public int FreeGameNumber{
		get{return _FreeGameNumber; }
		set{
			_FreeGameNumber = value;
			string number = value.ToString ();
			List<char> buff = number.ToList(); 
			buff.Reverse ();
			FreeNums [0].spriteName = "freenum_" + buff[0];
			if (buff.Count > 1) {
				FreeNums [1].enabled = true;
				FreeNums [1].spriteName = "freenum_" + buff[1];
			}
			else {
				FreeNums [1].enabled = false;
			}
		}
	}
	int _FreeGameNumber = 0;

	/// <summary>
	/// freegame anmiation control
	/// </summary>
	/// <returns>The free game spin.</returns>
	public IEnumerator StopFreeGameSpin()
	{
		//FreeGameNumber = slotdata.slotCombo;
		if (slotdata.slotCombo == 0){
			lastState = State.spining;
		}
		foreach (Reel reel in instance.Reels) {
			reel.StopSpin ();
			yield return new WaitForSeconds (0.01f);
		}

			instance.StartCoroutine (SetLine (slotdata.slotLines));
		
		yield return new WaitForSeconds (2.0f);
		if (OnChange != null)
			OnChange ();
		SlotManager.instance.freeLogo.SetActive (FreeGameNumber > 0);
		SlotManager.instance.logo.SetActive (FreeGameNumber == 0);
		//JurassicUIManager.instance.RewardMoney = playerMoney;
		if (slotdata.slotCombo == 0){
			yield return new WaitForSeconds (1.0f);
		}
		//slotState = State.Idle;
	}

	/// <summary>
	/// start UIbutton (autospin).
	/// </summary>
	public static void StartButton()
	{
		if (slotState != State.Idle) {
			//BugUse.AddMessageError ("Not Idle");
			return;
		}
		instance.buttonset = false;
		JurassicUIManager.instance.stopCollider.enabled = !instance.buttonset;
		instance.StartRandSpin ();
	}
		
	/// <summary>
	/// Stops UIbutton (autospin).
	/// </summary>
	public static void StopButton()
	{
		if (!slotdata.slotSet || instance.buttonset) {
			//if(!slotdata.slotSet)
				//BugUse.AddMessageError ("Not Get Package");
			//if(instance.buttonset)
				//BugUse.AddMessageError ("Button is Click");
			return;
		}
		instance.buttonset = true;
		JurassicUIManager.instance.stopCollider.enabled = !instance.buttonset;
		JurassicUIManager.instance.stopBut.SetState (UIButtonColor.State.Disabled,false);
		instance.countDown = 0;
		//SlotManager.slotdata.slotSet = false;
		instance.SetSlot (SlotManager.slotdata.slotSymbol);
		switch(slotState)
		{
		case State.freeGame:
			instance.StartCoroutine (instance.StopFreeGameSpin());
			SlotManager.instance.palyOnceSound.Stop ();
			break;
		case State.spining:
			instance.StopRandSpin ();
			SlotManager.instance.palyOnceSound.Stop ();
			break;
		case State.excited:
			instance.StartCoroutine (instance.StopExcitedSpin());
			SlotManager.instance.palyOnceSound.Stop ();
			break;
		}
	}

	static IEnumerator Delay(float delay = 1.5f)
	{
		yield return new WaitForSeconds (delay);
	}
}
